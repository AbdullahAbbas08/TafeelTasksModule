import { StoreService } from 'src/app/shared/_services/store.service';
import { MOMService } from './mom.service';
import {
  API_BASE_URL,
  MeetingCommentDTO,
  MeetingSummaryDTO,
  MeetingTopicDTO,
  MinuteOfMeetingDTO,
  SurveyDTO,
  SwaggerClient,
} from './../../../core/_services/swagger/SwaggerClient.service';
import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { TopicType } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { SingleMeetingService } from '../single-meeting.service';
import { ActivatedRoute } from '@angular/router';
import {
  CommitteeActions,
  GerogorianMonthsAr,
  GerogorianMonthsEn,
  MeetingActions,
} from 'src/app/shared/_enums/AppEnums';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { Subscription } from 'rxjs';
import { NzModalService } from 'ng-zorro-antd/modal';
import { CloseMeetingModalComponent } from 'src/app/shared/_components/close-meeting-modal/close-meeting-modal.component';

@Component({
  selector: 'app-minutes-of-meeting',
  templateUrl: './minutes-of-meeting.component.html',
  styleUrls: ['./minutes-of-meeting.component.scss'],
})
export class MinutesOfMeetingComponent implements OnInit ,OnDestroy {
  currentLang: string;
  topicType = TopicType;

  meetingDate: Date;
  meetingDay: number;
  meetingMonth: string;
  meetingYear: number;

  meetingId: number;
  addMinOfMeeting: boolean = false;

  minutesOfMeeting: MinuteOfMeetingDTO[] = [];
  count: number = 0;

  take: number = 20;
  skip: number = 0;
  actionTypes = CommitteeActions;
  loading = false;
  isCoordinator = false;
  isCreator = false;
  meetingFinished = false;
  meetingCanceled:boolean = false;
  meetingClosed = false;

  voting: SurveyDTO;
  recommendations: MeetingCommentDTO[] = [];
  meetingSummary: MeetingSummaryDTO;
  editedRecommendation: MeetingCommentDTO = new MeetingCommentDTO();
  subscribtion:Subscription;
  allmeetingTopics:MeetingTopicDTO[]=[];
  checkMomInvitation:boolean = false;
  momOfMeetingSurveyCheck:boolean = false;
  displayMom:boolean = false
  constructor(
    public translateService: TranslateService,
    private momService: MOMService,
    private singleMeeting: SingleMeetingService,
    private activatedRoute: ActivatedRoute,
    private modalService: SharedModalService,
    private modelService: NzModalService,
    private storeService: StoreService,
    private swagger:SwaggerClient,
    @Inject(API_BASE_URL) public baseUrl: string
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.checkMode();
    this.getMeetingVoting();
    this.getMeetingRecommendations();
    this.getMinutesOfMeetings();
    this.getSummary();
    this.checkMinutesOFMeeting();
    this.checkMinutesDisabled()
   this.subscribtion = this.storeService.refreshRecommendation$.subscribe((val) => {
      if(val){
        this.getSummary()
      }
    });
    if (!this.voting) {
      this.storeService.refreshMeetingVoting$.subscribe(
        (voting) => {
          this.voting = voting;
          this.momOfMeetingSurveyCheck = true
        }
      );
    }
    this.isCoordinator = this.singleMeeting.meeting.isCoordinator;
    this.meetingFinished = this.singleMeeting.meeting.isFinished;
    this.meetingCanceled = this.singleMeeting.meeting.isCanceled;
    this.isCreator = this.singleMeeting.meeting.isCreator;
    this.meetingClosed = this.singleMeeting.meetingClosed;
  }
  ngOnDestroy(){
    this.subscribtion.unsubscribe();
    this.storeService.refreshMeetingVoting$.next(null)
  }
  checkMinutesDisabled(){
    this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet('DisplayMintuesOfMeeting').subscribe((res) => {
        if(res.systemSettingValue === "1"){
            this.displayMom = true
        } else {
          this.displayMom = false
        }
    })
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setMeetingData();
    });
  }

  getMinutesOfMeetings() {
    this.loading = true;
    this.momService
      .getMinutesOfMeetings(this.meetingId, this.take, this.skip)
      .subscribe((res) => {
        if (res.data && res.count) {
          this.minutesOfMeeting = res.data;
          this.count = res.count;
        }
        this.loading = false;
      });
  }

  addMeetingVoting() {
    this.modalService.openDrawerModal(
      MeetingActions.CreateMeetingVoting,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      this.meetingId,
      undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,true
    );
  }

  setMeetingData() {
    this.meetingDate = this.singleMeeting.meeting.meetingFromTime;
    this.meetingYear = this.meetingDate.getFullYear();
    this.meetingMonth =
      this.currentLang === 'ar'
        ? GerogorianMonthsAr[this.meetingDate.getMonth() + 1]
        : GerogorianMonthsEn[this.meetingDate.getMonth() + 1];

    this.meetingDay = this.meetingDate.getDate();
  }

  checkMode() {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.meetingId = +params.get('id');

      if (this.meetingId) {
        this.setMeetingData();
      }
    });
  }

  addToMinutesOfMeeting(MOM: MinuteOfMeetingDTO) {
    this.minutesOfMeeting.push(MOM);
  }

  getMeetingVoting() {
    this.momService.getMeetingVoting(this.meetingId).subscribe((res) => {
      if (res.length !== 0) {
        this.voting = res[0];
        this.momOfMeetingSurveyCheck = true
      } else if (res.length === 0) {
        this.voting = res[0];
        this.momOfMeetingSurveyCheck = false
      }
    });
  }
  checkMinutesOFMeeting(){
    this.momService.checkMeetingInvitaion(this.meetingId).subscribe((res) => {
          this.checkMomInvitation = res
    })
  }
  getMeetingRecommendations() {
    this.momService
      .getMeetingRecommendations(this.meetingId)
      .subscribe((res) => {
        if (res) {
          this.recommendations = res;
        }
      });
  }

  insertRecommendation(recommendation: MeetingCommentDTO) {
    this.recommendations.push(recommendation);
  }

  editRecommendation(recommendation: MeetingCommentDTO) {
    this.editedRecommendation = { ...recommendation } as MeetingCommentDTO;
  }

  deleteRecommendation(recommendation: MeetingCommentDTO, index: number) {
    this.momService
      .deleteMeetingRecommendation(recommendation.id)
      .subscribe((res) => {
        this.recommendations.splice(index, 1);
        this.getSummary();
      });
  }

  print() {
    this.getSummary();
    setTimeout(()=>{
      let direction = this.currentLang == 'ar' ? 'rtl' : 'ltr';
      let popupWinindow;
      let innerContents = document.getElementById(
        'main-print-section-id'
      ).innerHTML;
      popupWinindow = window.open(
        '',
        '_blank',
        'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no'
      );
      popupWinindow.document.open();
      popupWinindow.document.write(
        `<html dir="${direction}" lang="${this.currentLang}"><head><link rel="stylesheet" type="text/css" href="${this.baseUrl}/assets/css/print-referral.css" /></head><body onload="setTimeout(() => {window.print()},100);">` +
          innerContents +
          '</html>'
      );
      popupWinindow.document.close();
    },500)
  }
  printRecommendations(){
    setTimeout(()=>{
      let direction = this.currentLang == 'ar' ? 'rtl' : 'ltr';
      let popupWinindow;
      let innerContents = document.getElementById(
        'main-print-recommendation-id'
      ).innerHTML;
      popupWinindow = window.open(
        '',
        '_blank',
        'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no'
      );
      popupWinindow.document.open();
      popupWinindow.document.write(
        `<html dir="${direction}" lang="${this.currentLang}"><head><link rel="stylesheet" type="text/css" href="${this.baseUrl}/assets/css/print-referral.css" /></head><body onload="setTimeout(() => {window.print()},100);">` +
          innerContents +
          '</html>'
      );
      popupWinindow.document.close();
    },500)
  }
  getSummary() {
    this.momService.getMeetingSummary(this.meetingId).subscribe((result) => {
      this.meetingSummary = result;
      this.allmeetingTopics =   result.meetingTopicDTOs.filter((res) => res.topicTitle !== '');
    });
  }

  onSendMinuetsOfMeeting() {
    this.momService.sendMOMforApproval(this.meetingId).subscribe((res) => {
      if (res) {
        this.modalService.createMessage('success', 'MOMHasBeenSent');
        this.checkMomInvitation = true
      }
    });
  }

  onCloseMeeting() {
    this.modelService.confirm({
      nzTitle: this.translateService.instant('CloseMeetingConfirmation'),
      nzOkText: this.translateService.instant('Yes'),
      nzCancelText: this.translateService.instant('No'),
      nzOkType: 'primary',
      nzOnOk: () => this.closeMeeting(),
      nzContent:CloseMeetingModalComponent,
      nzComponentParams: {
        meetingId:this.meetingId
      },
      nzClassName:'closeMeetingConfirmationModal'
    });
  }

  closeMeeting(){
    this.momService.closeMeeting(this.meetingId).subscribe((res) => {
      if (res) {
        this.modalService.createMessage('success', 'MeetingHasBeenClosed');
        this.singleMeeting.meetingClosed = true;
        this.meetingClosed = true;
      }
    });
  }

  refresh() {
    this.getMinutesOfMeetings();
    this.getMeetingRecommendations();
    this.getMeetingVoting();
    this.getSummary();
  }

  removeMOM(index: number) {
    this.minutesOfMeeting.splice(index, 1);
  }
  addNewTimeLineItem() {
    this.modalService.openDrawerModal(
      8,undefined,undefined,undefined,undefined,undefined,this.meetingId
      
    );
  }
}
