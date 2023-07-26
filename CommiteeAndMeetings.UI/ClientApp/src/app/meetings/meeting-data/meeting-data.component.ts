import { AgendaService } from './../schedule-meeting/agenda/agenda.service';
import {
  CommiteeDTO,
  MeetingTopicDTO,
  MeetingUserDTO,
} from './../../core/_services/swagger/SwaggerClient.service';
import {
  Period,
  UserType,
} from '../../core/_services/swagger/SwaggerClient.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SingleMeetingService } from '../schedule-meeting/single-meeting.service';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  Component,
  OnInit,
  AfterViewInit,
  Output,
  EventEmitter,
  Input,
  OnDestroy,
} from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateType, DateFormatterService } from 'ngx-hijri-gregorian-datepicker';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import {
  AttendeeState,
  LookUpDTO,
  MeetingCoordinatorDTO,
  MeetingDTO,
  MeetingProjectDTO,
  MeetingURlDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { debounceTime, skipUntil, switchMap,skipWhile } from 'rxjs/operators';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { StoreService } from 'src/app/shared/_services/store.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
let StartFromHours:number[] = [];
let StartFromMins:number[] =[];
let currStartDate: Date;
let startHours: number[] = [];
let startMins: number[] = [];
let currEndDate: Date;
@Component({
  selector: 'app-meeting-data',
  templateUrl: './meeting-data.component.html',
  styleUrls: ['./meeting-data.component.scss'],
})
export class MeetingDataComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() repeated;
  @Output() meetingApprovedSwitch: EventEmitter<boolean> = new EventEmitter();
  @Output() meetingInProgress: EventEmitter<MeetingTopicDTO> =
    new EventEmitter();
  meetingForm: FormGroup;
  selectedDateType = DateType.Gregorian;
  currentLang: string;
  minHijri: NgbDateStruct;
  minGreg: NgbDateStruct;
  lookupTypes = SettingControllers;
  selectedMeetingDate: Date;
  time: Date | null = null;
  projectsChange$ = new BehaviorSubject('');
  committeeChange$ = new BehaviorSubject('');
  usersChange$ = new BehaviorSubject('');
  onlineAddresses: string[] = [];
  actualLocation:string[] = [];
  projects: LookUpDTO[] = [];
  committees: LookUpDTO[] = [];
  selectedProjects = [];
  selectedCommittees = [];
  meetingId: number;
  isLoading = false;
  isSecret = false;
  isPublic = true;
  isConfirm = true;
  loadingData = false;
  userType = UserType;
  coordinators: MeetingCoordinatorDTO[] = [];
  meetingDate: Date;
  meetingDateNgb: NgbDateStruct;
  approved: boolean = false;
  clearInput: any;

  isCoordinator = false;
  isCreator = true;
  meetingClosed = false;

  period = Period;
  repeatedTimes;
  repeatedType;
  referenceId: number;
  selectedEndDate: Date;
  newMeetingDate:string
  subscribtion: Subscription;
  listOfcoordinators: MeetingUserDTO[] = [];
  subscribution:Subscription
  committeeId: string;
  constructor(
    private dateFormatterService: DateFormatterService,
    public translateService: TranslateService,
    private lookupService: LookupService,
    public singleMeetingService: SingleMeetingService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private storeService: StoreService,
    private notificationService: NzNotificationService,
    private agendaService: AgendaService,
    private browserService:BrowserStorageService,
    public dateService: NgbDateService,

  ) {}

  ngAfterViewInit(): void {
    this.subscribeToFormChanges();
  }

  ngOnInit(): void {
    this.initForm();
    this.langChange();
    this.checkMode();
    this.initProjects();
    this.initCommittees();
    this.setMinAllowed();
    this.checkEndDate();
    this.subscribtion = this.agendaService.meetingEnded.subscribe(() =>
    this.checkPreviewMode(this.isCoordinator, this.isCreator)
  );

   this.storeService.onDeleteUser$.asObservable().subscribe((val) => {
     if(val){
       this.listOfcoordinators.map(((ele, index) => {
         if(ele.userId == val){
           this.listOfcoordinators.splice(index,1)
         }
       }))
     }
   })
  this.checkNavigationFromCommittee();
    this.newMeetingDate = sessionStorage.getItem('meetind Date');
    if(this.newMeetingDate){
       let meetingDate = new Date(this.newMeetingDate)
      this.checkStartDate(meetingDate)
      this.formControls['startTime'].patchValue(meetingDate);
      if(this.formControls['startTime'].value){
        currEndDate = meetingDate
        this.changeHours(currEndDate)
      }
      /* patch data from session storage */
       this.meetingDateNgb = this.getNgbDate(meetingDate);
      
          this.selectedMeetingDate = new Date(
              Date.UTC(this.meetingDateNgb.year, this.meetingDateNgb.month -1, this.meetingDateNgb.day)
            );
           this.formControls['meetingDate'].patchValue(this.selectedMeetingDate);
    }
  }

  ngOnDestroy(): void {
    this.storeService.refreshCoordinator$.next(undefined);
    sessionStorage.clear();
  }
  checkEndDate(){
    this.meetingForm.get('endTime').valueChanges.pipe(skipWhile(x=>!x)).subscribe((val: Date) => {
      if (!val){
        this.meetingForm.patchValue({ endTime: this.selectedEndDate },{emitEvent:false});
      } else {
        let startDate = this.meetingForm.controls['startTime'].value;
        let date1 = new Date();
        date1.setHours(val.getHours()); date1.setMinutes(val.getMinutes()); date1.setMilliseconds(val.getMilliseconds());
        let date2 = new Date();
        date2.setHours(startDate.getHours()); date2.setMinutes(startDate.getMinutes()); date2.setMilliseconds(startDate.getMilliseconds());
        if (
          date1.getTime() < date2.getTime()
        ) {
          this.meetingForm.patchValue({ endTime: undefined },{emitEvent:false});
        }else {
          val.setDate(this.meetingForm.controls['startTime'].value.getDate());
        }
      }
    });
  }
  changeHours(date: Date) {
    if(date){
      this.formControls['endTime'].reset();
      currEndDate = date;
      let hours = (date as Date).getHours();
      startHours = [];
      for (let i = 0; i < hours; i++) {
        startHours.push(i);
      }
      let mins = (date as Date).getMinutes();
      startMins = [];
      for (let i = 0; i < mins; i++) {
        startMins.push(i);
      }
    }
  }
  checkStartDate(date: Date){
    currStartDate = date;
    let hours = (date as Date).getHours();
    StartFromHours = [];
    for (let i = 0; i < hours; i++) {
      StartFromHours.push(i);
    }
    let mins = (date as Date).getMinutes();
    StartFromMins = [];
    for (let i = 0; i < mins; i++) {
      StartFromMins.push(i);
    }
  }
  getDisabledStartHours(): number[] {
    let arr = new Array<number>();
    StartFromHours.map((i) => {
      arr.push(i);
    });
    return arr;
  }

  getdisabledStartMinutes(hour: number): number[] {
    if (hour === currStartDate.getHours()) {
      let arr = new Array<number>();
      StartFromMins.map((i) => {
        arr.push(i);
      });
      return arr;
    } else {
      return [];
    }
  }
  changeValid(date: Date) {
    this.selectedEndDate = date;
  }

  getDisabledHours(): number[] {
    let arr = new Array<number>();
    startHours.map((i) => {
      arr.push(i);
    });
    return arr;
  }

  getdisabledMinutes(hour: number): number[] {
    if (hour === currEndDate.getHours()) {
      let arr = new Array<number>();
      startMins.map((i) => {
        arr.push(i);
      });
      return arr;
    } else {
      return [];
    }
  }

  initForm() {
    this.meetingForm = new FormGroup({
      title: new FormControl('', [Validators.required]),
      subject: new FormControl(''),
      meetingDate: new FormControl(null, [Validators.required]),
      startTime: new FormControl(null, [Validators.required]),
      endTime: new FormControl(null, [Validators.required]),
      remindBefore: new FormControl(null),
      location: new FormControl(null,[Validators.required]),
      isSecret: new FormControl(false),
      isPublic: new FormControl(true),
      isConfirm: new FormControl(true),
      approveMeeting: new FormControl(false),
      committee: new FormControl(null),
      meetingUrls: new FormControl(null),
      meetingProjects: new FormControl(null),
      ActualLocation:new FormControl(''),
      repeatedType: new FormControl(undefined),
      repeatedTimes: new FormControl(undefined),
    });
  }

  initEditForm() {
    this.loadingData = true;
    this.singleMeetingService
      .getMeetingDetails(this.meetingId)
      .subscribe((res: MeetingDTO) => {
        this.meetingForm.patchValue({
          title: res.title,
          subject: res.subject,
          meetingDate: res.date,
          startTime: this.dateService.getTimeZoneOffset(res.meetingFromTime),
          endTime: this.dateService.getTimeZoneOffset(res.meetingToTime),
          remindBefore: new Date(2000, 11, 1, 1, res.reminderBeforeMinutes),
          location: res.physicalLocation,
          isSecret: res.isSecret,
          isPublic: res.permitedToEnterMeeting,
          isConfirm: res.memberConfirmation,
          approveMeeting: res.approveManual,
          committee: res.committeId || res.commitee?.commiteeId,
        });
        this.onlineAddresses = res.meetingURls.map((urlObj) => {
          return urlObj.onlineUrl;
        });
       if(res.actualLocation !== null){
        this.actualLocation[0] = res.actualLocation
       }
        if (res.committeId || res.commitee?.commiteeId) {
          this.committees.push(
            new LookUpDTO({ id: res.committeId, name: res.commitee.name })
          );
        }

        this.selectedProjects = res.meetingProjects.map((project) => {
          return {
            id: project.project.id,
            name:
              this.currentLang === 'ar'
                ? project.project.projectNameAr
                : project.project.projectNameEn,
          };
        });
        this.coordinators = [...res.meetingCoordinators];
        this.meetingDate = res.date;
        this.meetingDateNgb = this.getNgbDate(this.meetingDate);
        this.approved = res.approveManual;

        this.meetingApprovedSwitch.emit(res.approveManual);
        this.storeService.meetingDetails$.next(res);
        this.isCoordinator = res.isCoordinator;
        this.isCreator = res.isCreator;
        this.meetingClosed = this.singleMeetingService.meetingClosed;
        this.checkPreviewMode(this.isCoordinator, this.isCreator);
        this.repeated = res.repated;
        this.repeatedTimes = res.repatedTimes;
        this.repeatedType = res.periodByDays;
        this.referenceId = res.referenceNumber;

        // Check if Meeting is in progress
        if (res.meetingTopics.length > 0) {
          this.meetingInProgress.emit(res.meetingTopics[0]);
        }

        this.singleMeetingService.meeting = res;
        this.loadingData = false;
      });
  }

  onSubmit() {
    if (this.meetingForm.valid) {
      this.loadingData = true;
      let title: string = this.meetingForm.value.title;
      let subject: string = this.meetingForm.value.subject;

      let date: Date = new Date(
        Date.UTC(
          this.meetingForm.value.meetingDate.getFullYear(),
          this.meetingForm.value.meetingDate.getMonth(),
          this.meetingForm.value.meetingDate.getDate()
        )
      );
 
      let fromTime = this.meetingForm.value.startTime;
      let endTime = this.meetingForm.value.endTime;

      let meetingFromTime: Date = new Date(
        Date.UTC(        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        fromTime.getHours(),
        fromTime.getMinutes(),
        0)
      );

      let meetingToTime: Date = new Date(
        Date.UTC(
          date.getFullYear(),
          date.getMonth(),
          date.getDate(),
          endTime.getHours(),
          endTime.getMinutes(),
          0
        )
      );
      let reminderBeforeMinutes: number;

      if (this.meetingForm.value.remindBefore) {
        reminderBeforeMinutes =
          this.meetingForm.value.remindBefore.getMinutes();
      }

      let meetingURls: MeetingURlDTO[] = this.onlineAddresses.map((link) => {
        return new MeetingURlDTO({
          onlineUrl: link,
        });
      });

      let physicalLocation: string = this.meetingForm.value.location;

      let meetingProjects: MeetingProjectDTO[] = this.selectedProjects.map(
        (project) => {
          return new MeetingProjectDTO({
            projectId: project.id,
          });
        }
      );

      let isSecret: boolean = this.meetingForm.value.isSecret;
      let permitedToEnterMeeting: boolean = this.meetingForm.value.isPublic;
      let memberConfirmation: boolean = this.meetingForm.value.isConfirm;
      let actualLocation : string = this.actualLocation[0]
      let meetingInfo: MeetingDTO = new MeetingDTO({
        title,
        subject,
        repated: this.repeated,
        date,
        meetingFromTime,
        meetingToTime,
        reminderBeforeMinutes,
        meetingURls,
        physicalLocation,
        meetingProjects,
        isSecret,
        permitedToEnterMeeting,
        memberConfirmation,
        id: this.meetingId,
        approveManual: this.approved,
        committeId: this.meetingForm.value.committee,
        actualLocation
      });

      /// Save Create Single Meeting
      if (!this.meetingId && !this.repeated) {
        this.singleMeetingService
          .scheduleSingleMeeting([meetingInfo])
          .subscribe((res: MeetingDTO[]) => {
            if (!this.committeeId) {
              this.singleMeetingService.meeting = res[0];
              res[0].id
                ? this.router.navigate([res[0].id], {
                    relativeTo: this.activatedRoute,
                  })
                : null;
            } else {
              res[0].id
                ? this.router.navigateByUrl(
                    '/meetings/schedule-meeting/' + res[0].id
                  )
                : null;
            }
          });
      }
      /// Save Edit Single Meeting
      else if (this.meetingId && !this.repeated) {
        this.singleMeetingService
          .editSingleMeeting([meetingInfo])
          .subscribe((res) => {
            this.singleMeetingService.meeting = res[0];
            this.singleMeetingService.updateMeeting.next(res[0]);
            this.coordinators = res[0].meetingCoordinators;
            this.singleMeetingService.meeting.meetingAttendees = [
              ...res[0]?.meetingAttendees,
            ];
            this.meetingDate = res[0].date;
            this.approved = res[0].approveManual;
            this.meetingDateNgb = this.getNgbDate(this.meetingDate);
            this.loadingData = false;
          });
      }
      /// Save Create Multi Meeting
      else if (this.repeated && !this.meetingId) {
        meetingInfo.repatedTimes = this.meetingForm.value.repeatedTimes;
        meetingInfo.periodByDays = this.meetingForm.value.repeatedType;
        this.singleMeetingService
          .scheduleSingleMeeting([meetingInfo])
          .subscribe((res) => {
            if (!this.committeeId) {
              this.router.navigate([res[0].referenceNumber], {
                relativeTo: this.activatedRoute,
              });
            } else {
              this.router.navigateByUrl(
                '/meetings/multiple-meeting/' + res[0].referenceNumber
              );
            }
          });
      }
      /// Save Edit Multi Meeting
      else if (this.meetingId && this.repeated) {
        meetingInfo.repatedTimes = this.repeatedTimes;
        meetingInfo.periodByDays = this.repeatedType;
        meetingInfo.referenceNumber = this.referenceId;
        this.singleMeetingService
          .editSingleMeeting([meetingInfo])
          .subscribe((res) => {
            this.singleMeetingService.meeting = res[0];
            this.singleMeetingService.updateMeeting.next(res[0]);
            this.meetingDate = res[0].date;
            this.approved = res[0].approveManual;
            this.coordinators = res[0].meetingCoordinators;
            this.singleMeetingService.meeting.meetingAttendees = [
              ...res[0]?.meetingAttendees,
            ];
            this.meetingDateNgb = this.getNgbDate(this.meetingDate);
            this.loadingData = false;
          });
      }
    }
  }

  get formControls() {
    return this.meetingForm.controls;
  }

  dateSelected(selectedDate: NgbDateStruct) {
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.selectedMeetingDate = new Date(
        Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
      );
      this.formControls['meetingDate'].patchValue(this.selectedMeetingDate);
    }
  }

  setMinAllowed() {
    this.minGreg = this.dateFormatterService.GetTodayGregorian();
    this.minHijri = this.dateFormatterService.GetTodayHijri();
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }

  addAddress(link: string) {
    this.onlineAddresses.push(link);
    this.formControls.meetingUrls.patchValue(undefined);
  }
  addActualLocation(link:string){
    this.actualLocation.push(link)
    this.formControls.ActualLocation.patchValue(undefined)
  }
  removeSelectedAddress(i) {
    this.onlineAddresses.splice(i, 1);
  }
  removeSelectedLocation(i){
    this.actualLocation.splice(i,1);
  }
  addProject(projectId: number) {
    if (!this.selectedProjects.some((el) => el.id === projectId)) {
      this.selectedProjects.push({
        id: projectId,
        name: this.projects.find((p) => p.id === projectId).name,
      });
    }
    this.clearInput = undefined;
  }
  removeSelectedProject(i) {
    this.selectedProjects.splice(i, 1);
  }
  addCommittee(id: number) {
    this.selectedCommittees.push({
      id,
      name: this.committees.find((comm) => comm.id === id).name,
    });
  }

  removeSelectedCommittee(i) {
    this.selectedCommittees.splice(i, 1);
  }

  initProjects() {
    this.projectsChange$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getLookupProjects(25, 0, text ? text : undefined)
        )
      )
      .subscribe((res: LookUpDTO[]) => {
        this.projects = res;

        this.isLoading = false;
      });
  }

  initCommittees() {
    const encryptedId:string = this.browserService.encrypteString(`${this.committeeId}_${this.browserService.getUserRoleId()}`)
    this.committeeChange$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getParentCommittees(
            20,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,
              encryptedId
          )
        )
      )
      .subscribe((res: LookUpDTO[]) => {
        this.committees = [...res];
        this.isLoading = false;
      });
  }

  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case this.lookupTypes.PROJECTS:
        this.projectsChange$.next(value);
        break;
      case this.lookupTypes.COMMITTEE:
        this.committeeChange$.next(value);
        break;
    }
  }

  subscribeToFormChanges() {
    this.meetingForm
      .get('isSecret')
      .valueChanges.subscribe((val) => (this.isSecret = val));

    this.meetingForm
      .get('isPublic')
      .valueChanges.subscribe((val) => (this.isPublic = val));

    this.meetingForm
      .get('isConfirm')
      .valueChanges.subscribe((val) => (this.isConfirm = val));
  }

  checkMode() {
    if (this.repeated) {
      this.meetingForm.get('repeatedType').setValidators(Validators.required);
      this.meetingForm.get('repeatedType').updateValueAndValidity();
      this.meetingForm.get('repeatedTimes').setValidators(Validators.required);
      this.meetingForm.get('repeatedTimes').updateValueAndValidity();
    }

    this.activatedRoute.paramMap.subscribe((params) => {
      this.meetingId = +params.get('id');

      if (this.meetingId) {
        this.initEditForm();
      }
    });
  }

  checkNavigationFromCommittee() {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.committeeId = params.get('committeeId');
      if (this.committeeId) {
        const committeId = this.browserService.decrypteString(this.committeeId);
        this.singleMeetingService
          .getCommitteeName(committeId)
          .subscribe((com: CommiteeDTO) => {
            this.committees.push(
              new LookUpDTO({ id:committeId, name: com.name })
            );
            this.meetingForm.patchValue({ committee: committeId });
          });
      }
    });
  }

  onAddCoordinator(userId) {
    if (userId) {
      userId.map((res) => {
        this.listOfcoordinators.push(
          new MeetingUserDTO({
            userId: res,
            userType: UserType._1,
            meetingId: this.meetingId,
          })
        );
      });
      this.singleMeetingService
        .addCoordinator(this.listOfcoordinators)
        .subscribe((res) => {
          if (res) {
            res.forEach((val) => {
              this.coordinators.push(
                new MeetingCoordinatorDTO({
                  coordinator: val.user,
                  state: AttendeeState._1,
                  available: val.available,
                  coordinatorId: val.userId,
                })
              );
              // this.singleMeetingService.meeting.meetingCoordinators.push(
              //   new MeetingCoordinatorDTO({
              //     coordinator: val.user,
              //     state: AttendeeState._1,
              //     available: val.available,
              //     coordinatorId: val.userId,
              //   })
              // );
            });
            this.singleMeetingService.meeting.meetingCoordinators = [...this.coordinators];
            this.storeService.refreshCoordinator$.next(res);
          }
        });
    }
  }

  meetingApprovalStateChange(approvedState: boolean) {
    this.singleMeetingService
      .changeMeetingApprovalState(approvedState)
      .subscribe(() => {
        this.approved = approvedState;
        this.meetingApprovedSwitch.emit(approvedState);
      });
  }

  getNgbDate(date: Date) {
    return {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate(),
    };
  }

  checkPreviewMode(isCoordinator: boolean, isCreator: boolean) {
    if (
      !(isCoordinator || isCreator) ||
      this.singleMeetingService.meeting?.isFinished ||
      this.singleMeetingService.meeting?.isCanceled
    ) {
      this.meetingForm.get('title').disable();
      this.meetingForm.get('subject').disable();
      this.meetingForm.get('meetingDate').disable();
      this.meetingForm.get('startTime').disable();
      this.meetingForm.get('endTime').disable();
      this.meetingForm.get('remindBefore').disable();
      this.meetingForm.get('location').disable();
      this.meetingForm.get('isSecret').disable();
      this.meetingForm.get('isPublic').disable();
      this.meetingForm.get('isConfirm').disable();
      this.meetingForm.get('approveMeeting').disable();
      this.meetingForm.get('committee').disable();
      this.meetingForm.get('meetingUrls').disable();
      this.meetingForm.get('meetingProjects').disable();
      this.meetingForm.get('ActualLocation').disable()
    }
  }
  goToLink(url: string){
    window.open(url, "_blank");
}
}
