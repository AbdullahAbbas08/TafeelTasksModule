import { Component, OnInit ,ChangeDetectionStrategy, AfterViewChecked,  ChangeDetectorRef, ViewChild,OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import {CalendarEvent,CalendarEventAction,CalendarMonthViewBeforeRenderEvent,CalendarView, CalendarViewPeriod,CalendarWeekViewBeforeRenderEvent, DateAdapter} from 'angular-calendar';
import { DashboardService } from '../dashboard.service';
import { DisplayMeetingCallType, MeetingDetailsDTO} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import {
  isSameDay,
  isSameMonth,
} from 'date-fns';
import { Router } from '@angular/router';
import { WeekViewHour, WeekViewHourColumn } from 'calendar-utils';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { MeetingActions } from 'src/app/shared/_enums/AppEnums';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { element } from 'protractor';
import { AuthService } from 'src/app/auth/auth.service';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
@Component({
  selector: 'app-meetings-calender',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './meetings-calender.component.html',
  styleUrls: ['./meetings-calender.component.scss']
})
export class MeetingsCalenderComponent implements OnInit , AfterViewChecked,OnDestroy {

  view: CalendarView = CalendarView.Week;
  CalendarView = CalendarView;
  viewDate: Date = new Date();
  refresh: Subject<any> = new Subject();
  events: CalendarEvent<{meetingDetails:MeetingDetailsDTO}>[] = [];
  activeDayIsOpen: boolean = false;
  activeDay: Date = new Date();
  period: CalendarViewPeriod
  dateInHijri:any;
  actions: CalendarEventAction[];
  hourColumns: WeekViewHourColumn[];
  selectedDayViewDate: any;
  constructor(public dateService: NgbDateService,private authService:AuthService,private modal: NzModalService,private modalService: SharedModalService, private notificationService: NzNotificationService,private router:Router,private _cdr: ChangeDetectorRef,public translate: TranslateService,private _dashboardService:DashboardService) { }

  ngOnInit(): void {
    const newDate = new Date(this.viewDate.getFullYear(),this.viewDate.getMonth(),this.viewDate.getDate())
  this.dateInHijri = new Intl.DateTimeFormat('ar-FR-u-ca-islamic', { month: 'long',year : 'numeric'}).format(newDate);
  this._dashboardService.changeMeetingState.subscribe((res) => {
    if(res){
      const indexNumber = this.events.findIndex(x => x.meta.meetingDetails?.id === res);
      this.events.splice(indexNumber,1);
      this.refresh.next()
    }
  })
  }
  ngOnDestroy() {
    this._dashboardService.changeMeetingState.next(null)
  }
  ngAfterViewChecked() {
  }
  getDateInterval(){
    var currentDate = new Date; 
    var first = currentDate.getDate() - currentDate.getDay(); 
    var last = first + 6; 
    var firstday = new Date(currentDate.setDate(first))
    var lastday = new Date(currentDate.setDate(last))
    this.getAllMeetings(firstday,lastday,DisplayMeetingCallType._0)
  }
  getAllMeetings(dateFrom:Date,dateTo:Date,fromDashboard:DisplayMeetingCallType){
   this._dashboardService.getAllMeetings(dateFrom,dateTo,fromDashboard).subscribe((meeting:MeetingDetailsDTO[]) => {
    this.events = [];
    meeting.map((res) => {
      if(res.meetingState === 1 && this.authService.isAuthUserHasPermissions(['exitMeeting'])){
        this.events.push({
          start:this.dateService.getTimeZoneOffset(res.meetingFromTime),
          end:this.dateService.getTimeZoneOffset(res.meetingToTime),
          title:res.title,
          id:res.id,
          meta:{
           meetingDetails:res
          },
          actions: [
           {
             label: this.translate.currentLang === 'ar' ? '  التفاصيل': 'Details  ',
             onClick: ({ event }: { event: CalendarEvent }): void => {
              this.router.navigateByUrl(`/meetings/schedule-meeting/${event.id}`)
              },
              
          },
          {
            label: this.translate.currentLang === 'ar' ? '  إلغاء ': 'Exit  ',
            onClick: ({ event }: { event: CalendarEvent }): void => {
             this.showDeleteMeeting(res.id)
             },
          }
        ],
        })
        this.refresh.next()
      } else if(res.meetingState !==4) {
        this.events.push({
          start:this.dateService.getTimeZoneOffset(res.meetingFromTime),
          end:this.dateService.getTimeZoneOffset(res.meetingToTime),
          title:res.title,
          id:res.id,
          meta:{
           meetingDetails:res
          },
          actions: [
           {
             label: this.translate.currentLang === 'ar' ? 'التفاصيل': 'Details',
             onClick: ({ event }: { event: CalendarEvent }): void => {
              this.router.navigateByUrl(`/meetings/schedule-meeting/${event.id}`)
              },
             },
        ],
        })
        this.refresh.next()
      }
      else {
        this.events.push({
          start:this.dateService.getTimeZoneOffset(res.meetingFromTime),
          end:this.dateService.getTimeZoneOffset(res.meetingToTime),
          title:res.title,
          id:res.id,
          meta:{
           meetingDetails:res
          },
          actions: [
            {
              label: this.translate.currentLang === 'ar' ? '  تم إلغاء الأجتماع': 'meetingCanceled',
              onClick: ({ event }: { event: CalendarEvent }): void => {},
             }
         ],
        })
        this.refresh.next()
      }
     })
   })
  }
  beforeWeeklyViewRender(event:CalendarWeekViewBeforeRenderEvent) {
    if (!this.period || this.period.start.getTime() !== event.period.start.getTime() || this.period.end.getTime() !== event.period.end.getTime()) {
      this.period = event.period;
      this._cdr.detectChanges();
      this.hourColumns = event.hourColumns;
      this.getAllMeetings(this.period.start,this.period.end,DisplayMeetingCallType._0)
    }
  }
  beforeMonthlyViewRender(event: CalendarMonthViewBeforeRenderEvent){
    if (!this.period || this.period.start.getTime() !== event.period.start.getTime() || this.period.end.getTime() !== event.period.end.getTime()){
      this.period = event.period;
      this._cdr.detectChanges();
      this.getAllMeetings(this.period.start,this.period.end,DisplayMeetingCallType._0)
    }
  }
  hourSegmentClicked(date: Date) {
    if(date < new Date()){
      this.translate
      .get('canotcreatemeetingInThisTime')
      .subscribe((translateValue) =>
        this.notificationService.error(translateValue, '')
      );
    } else {
      this.selectedDayViewDate = date;
      sessionStorage.setItem('meetind Date',this.selectedDayViewDate);
      this.modalService.openDrawerModal(MeetingActions.CreateNewMeeting,undefined,undefined,undefined,undefined,undefined,undefined,undefined,this.selectedDayViewDate);
    }
  }
  showDeleteMeeting(meetingId):void{
    this.modal.confirm({
      nzTitle: this.translate.currentLang === 'ar' ?  'هل أنت متاكد من إلغاء هذا الأجتماع ؟ ' : "Are You Sure you want to delete this meeting ? ",
      nzOkText: this.translate.currentLang === 'ar' ? 'نعم': 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.confirmDeleteMeeting(meetingId),
      nzCancelText: this.translate.currentLang === 'ar' ? 'لا': 'No',
    });
  }
  confirmDeleteMeeting(meetingId){
   const meeting =  this.events.find((meeting) => meeting.meta.meetingDetails.id === meetingId);
   if(meeting.meta.meetingDetails.meetingState === 1){
      this._dashboardService.cancelMeeting(meetingId).subscribe((res) => {
        if(res){
          this._dashboardService.cancelMeetingState.next(meetingId)
          meeting.meta.meetingDetails.meetingState = 4;
          meeting.actions = [
           {
            label: this.translate.currentLang === 'ar' ? '  تم إلغاء الأجتماع': 'meetingCanceled',
            onClick: ({ event }: { event: CalendarEvent }): void => {},
           }
          ]
          this.refresh.next();
          this.translate
          .get('MeetingDeleted')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
        }
      })
   }
  }
  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
    this.dateInHijri = new Intl.DateTimeFormat('ar-FR-u-ca-islamic', {month: 'long',year : 'numeric'}).format(this.viewDate);
  }
  setView(view: CalendarView) {
    this.view = view;
    this.dateInHijri = new Intl.DateTimeFormat('ar-FR-u-ca-islamic', { month: 'long',year : 'numeric'}).format(this.viewDate);
  }
  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
      this.activeDay = date;
 
      if ((isSameDay(this.activeDay, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
  }

}
