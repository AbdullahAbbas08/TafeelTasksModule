import { SingleMeetingService } from './../../../../single-meeting.service';
import { Subscription } from 'rxjs/index';
import {
  distinctUntilChanged,
  map,
  distinctUntilKeyChanged,
} from 'rxjs/operators';
import { StoreService } from 'src/app/shared/_services/store.service';
import {
  MeetingCoordinatorDTO,
  SavedAttachmentDTO,
  SurveyAnswerDTO,
  SurveyAnswerUserDTO,
  SurveyDTO,
  SwaggerClient,
} from './../../../../../../core/_services/swagger/SwaggerClient.service';
import {
  Component,
  Input,
  OnInit,
  OnDestroy,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/auth/auth.service';
import { VotingService } from 'src/app/committees/committee-details/votes/voting.service';
import { DateFormatterService } from 'ngx-hijri-gregorian-datepicker';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AgendaService } from '../../../agenda.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';

@Component({
  selector: 'app-voting-item-topic',
  templateUrl: './voting-item.component.html',
  styleUrls: ['./voting-item.component.scss'],
})
export class TopicVotingItemComponent implements OnInit, OnDestroy, OnChanges {
  @Input('item') item: SurveyDTO;
  @Input() topicId: number;
  @Input() meetingId: number;
  @Input() isCoordinator;
  @Input() isCreator;
  @Input() isMeetingClosed;
  @Input() byPassStartForActivities;
  @Input() recommendationSurvary;
  attachments: SavedAttachmentDTO[] = [];
  voteAnswer: string[] = [];
  targetIndex = 0;
  loadingData = false;
  userId: number;
  createdUserName: string;
  createdUserImage: string;
  currentLang: string;
  userParticipated = false;
  createdOnDate;
  createdOnTime;
  started = false;

  subscription: Subscription;
  subscribtion: Subscription;
  voteEndDate:Date;
  checkEndDate:boolean = false;
  ifCoordinatorAttendee:boolean = false;
  meetingCoordinateAttende:MeetingCoordinatorDTO[]=[]
  constructor(
    private votingService: VotingService,
    private authService: AuthService,
    public translateService: TranslateService,
    private store: StoreService,
    private dateService: DateFormatterService,
    private agendaService: AgendaService,
    private modalService: SharedModalService,
    private singleMeetingService:SingleMeetingService,
    private swagger:SwaggerClient
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    this.isMeetingClosed = changes['isMeetingClosed'].currentValue;
  }

  ngOnInit(): void {
    this.langChange();
    this.item?.attachments.forEach((attachment) =>
      this.attachments.push(attachment.attachment)
    );
    this.recommendationSurvary?.map((survey) => {
      this.item.surveyAnswers.push(new SurveyAnswerDTO({
        surveyAnswerId:survey.surveyAnswerId,
        answer:survey.answer,
        surveyId:survey.surveyId,
        surveyAnswerUsers:survey.surveyAnswerUsers
      }))
    })
    this.userId = this.authService.getUser().userId;
    this.currentLang = this.translateService.currentLang;
    this.setCreatedByUserDetails();
    this.checkIfUserVoted();

    this.store.refreshAnswerUsers$
      .pipe(distinctUntilKeyChanged('firstId'))
      .subscribe((res) => {
        if (res && res.topicId === this.topicId) {
          res.answerUsers?.forEach((item) => {
            this.item.surveyAnswers
              .filter((ans) => ans.surveyAnswerId === item.surveyAnswerId)
              .forEach((answer) => {
                if (!answer.surveyAnswerUsers?.includes(item)) {
                  answer.surveyAnswerUsers.unshift(item);
                }
              });
          });
        }
      });

    this.store.refreshAnswerUsersforCurrentUser$.subscribe((res) => {
      if (res.topicId === this.topicId) {
        res.answerUsers?.forEach((item) => {
          this.item.surveyAnswers
            .filter((ans) => ans.surveyAnswerId === item.surveyAnswerId)
            .forEach((answer) => {
              if (!answer.surveyAnswerUsers?.includes(item)) {
                answer.surveyAnswerUsers.unshift(item);
              }
            });
        });
      }
    });

    this.subscription = this.store.votingParticipation$.subscribe((id) => {
      this.item.surveyId === id && (this.userParticipated = true);
    });

    this.getCreatedOnDate();
    this.getCreatedOnTime();
   this.checkIsCoordinatorAttendee()
    this.subscribtion = this.agendaService.meetingStarted.subscribe(
      (state) => {
        this.started = state;
        if(this.meetingCoordinateAttende){
          //  && !this.meetingCoordinateAttende[0]?.isCreator
          if(this.meetingCoordinateAttende[0]?.confirmeAttendance=== false){
            this.started = false
            this.isCoordinator = false
          }
        }
      }
    );
  }
  checkIsCoordinatorAttendee(){
    this.meetingCoordinateAttende = this.singleMeetingService.meeting.meetingCoordinators.filter((coor) => {
      return coor.coordinator.userId == +this.userId
    });
    if(this.meetingCoordinateAttende ){
      if(this.meetingCoordinateAttende[0]?.confirmeAttendance=== false){
        this.isCoordinator = false;
        this.isMeetingClosed = true;
      }
    }
  }
  ngOnDestroy() {
    
    this.subscription.unsubscribe();
    this.subscribtion.unsubscribe();
    this.store.refreshAnswerUsersforCurrentUser$.next(null)
  }

  sendVote() {
    if (this.voteAnswer.length < 1) return;
    let surveyAnswer: SurveyAnswerUserDTO[] = this.voteAnswer.map(
      (answerId) => {
        return new SurveyAnswerUserDTO({
          userId: this.userId,
          surveyAnswerId: +answerId,
          meetingId:this.meetingId
        });
      }
    );

    this.votingService.sendVoteAnswer(surveyAnswer).subscribe((res) => {
      if (res) {
        res.forEach((item) => {
          this.item.surveyAnswers
            .filter((ans) => ans.surveyAnswerId === item.surveyAnswerId)
            .forEach((answer) => answer.surveyAnswerUsers.unshift(item));
        });

        this.userParticipated = true;
        this.store.votingParticipation$.next(this.item.surveyId);
        this.store.refreshAnswerUsersforCurrentUser$.next({
          answerUsers: res,
          topicId: this.topicId,
        });
      }
    });
    this.modalService.destroyModal()
  }

  setCreatedByUserDetails() {
    if (this.item?.createdByUser) {
      this.createdUserName =
        this.currentLang === 'ar'
          ? this.item?.createdByUser.fullNameAr
          : this.item?.createdByUser.fullNameEn;
      this.createdUserImage = this.item?.createdByUser.profileImage;
    }
  }

  checkIfUserVoted() {
    this.item?.surveyAnswers.forEach((answer) => {
      answer.surveyAnswerUsers.forEach((user) => {
        if (user.userId == this.userId) {
          this.userParticipated = true;
        }
      });
    });
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedByUserDetails();
    });
  }

  getCreatedOnDate() {
    let date = new Date(this.item?.createdOn);

    let ngbHijri: NgbDateStruct = this.dateService.ToHijri({
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate(),
    });

    let hijriDateString = `${ngbHijri.day}/${ngbHijri.month}/${ngbHijri.year}`;
    let gregDateString = `${date.getDate()}/${date.getMonth()}/${date.getFullYear()}`;

    this.createdOnDate =
      this.translateService.currentLang == 'ar'
        ? hijriDateString
        : gregDateString;
  }

  getCreatedOnTime() {
    let hours, minutes, seconds;
    if (this.item?.createdOn) {
      hours = new Date(this.item?.createdOn).getHours();
      minutes = new Date(this.item?.createdOn).getMinutes();
      seconds = new Date(this.item?.createdOn).getSeconds();
    } else {
      hours = new Date().getHours();
      minutes = new Date().getMinutes();
      seconds = new Date().getSeconds();
    }

    this.createdOnTime = `${hours}:${minutes}:${seconds}`;
  }
}
