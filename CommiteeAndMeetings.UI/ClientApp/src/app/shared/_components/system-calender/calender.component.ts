import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ViewChild,
  TemplateRef,
  Input,
  OnDestroy
} from '@angular/core';
import { NgbDateStruct, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import {
  startOfDay,
  endOfDay,
  subDays,
  addDays,
  endOfMonth,
  isSameDay,
  isSameMonth,
  addHours,
} from 'date-fns';
import {
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent,
  CalendarView,
} from 'angular-calendar';
import { Subject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { CommiteeTaskDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SearchService } from '../../_services/search.service';
import { Subscription } from 'rxjs';
import { collapseAnimation } from 'angular-calendar';
import { TasksFilterEnum } from '../../_enums/AppEnums';
import { BrowserStorageService } from '../../_services/browser-storage.service';
import { TasksService } from 'src/app/tasks/tasks.service';
@Component({
  selector: 'app-calender',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './calender.component.html',
  styleUrls: ['./calender.component.scss'],
  animations: [collapseAnimation]
})
export class CalenderComponent implements OnInit, OnDestroy  {
  @ViewChild('modalContent', { static: true }) modalContent: TemplateRef<any>;
  @Input() items:CommiteeTaskDTO[];
  model: NgbDateStruct;
  activeDayIsOpen: boolean = false;
  activeDay: Date = new Date();
  date: { year: number; month: number };
  viewDate: Date = new Date();
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  toggle: boolean = false;
  refresh: Subject<any> = new Subject();
  committeeId:any;
  subscription: Subscription;
  currentLang: string;
  modalData: {
    action: string;
    event: CalendarEvent;
  };
  events: CalendarEvent<{completed:boolean,endDate:Date}>[] = [];
  dateNow:Date = new Date();
  TasksFilterEnum = TasksFilterEnum;
  isFullScrren: boolean = false;
  status: boolean = false;
  dateInHijri:any;
  constructor(
    private calendar: NgbCalendar,
    public translate: TranslateService,
    private searchService: SearchService,
    private translateService: TranslateService,
    private browserService:BrowserStorageService,
    private route: ActivatedRoute,
    private swagger:SwaggerClient
  ) {
  }

  ngOnInit(): void {
    this.committeeId = this.route.snapshot.paramMap.get('id');
    this.selectToday();
    this.setEventInCalender();
    this.langChange();
    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
         if(!word){
          this.setEventInCalender()
         }
    });
    this.dateInHijri = new Intl.DateTimeFormat('ar-FR-u-ca-islamic', { month: 'long',year : 'numeric'}).format(this.viewDate);

  }
  ngOnDestroy() {
    if (this.subscription) this.subscription.unsubscribe();
  }
  setEventInCalender(committeClassificationId?){
    this.events = [];
    
    if(this.committeeId){
     this.swagger.apiCommiteeTasksGetAllCalenderGet(this.browserService.encryptCommitteId(this.committeeId),undefined).subscribe((result) => {
        if(result){
           result.map((res) => {
            this.events.push({
              start:res.endDate,
              title:res.title,
              id:res.commiteeTaskId,
              meta:{
                completed:res.completed,
                endDate:res.endDate
               },
            })
            this.refresh.next()
          })
        }
      })
    } else {
      this.swagger.apiCommiteeTasksGetAllCalenderGet(undefined,undefined).subscribe((result) => {
        if(result){
           result.map((res) => {
            this.events.push({
              start:res.endDate,
              title:res.title,
              id:res.commiteeTaskId,
              meta:{
                completed:res.completed,
                endDate:res.endDate
               },
            })
            this.refresh.next()
          })
        }
      })
    }
  }
  selectToday() {
    this.model = this.calendar.getToday();
  }
  setView(view: CalendarView) {
    this.view = view;
    this.dateInHijri = new Intl.DateTimeFormat('ar-FR-u-ca-islamic', { month: 'long',year : 'numeric'}).format(this.viewDate);
  }
  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
    this.dateInHijri = new Intl.DateTimeFormat('ar-FR-u-ca-islamic', {month: 'long',year : 'numeric'}).format(this.viewDate);
  }
  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    this.activeDay = date;
      if (
        (isSameDay(this.activeDay, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
  }
  frameStyles(day) : string{
     const eventClass = day.events.map((event) => {
       if(event.meta.completed === false && event.meta.endDate < this.dateNow){
         return 'cal-cell-top cal-out-month hasevent'
       } else {
         return 'cal-cell-top'
       }
     })
     return eventClass
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
  toggleFullScreen() {
    this.isFullScrren = !this.isFullScrren;
    this.status = !this.status; 
    if (this.isFullScrren){
       this.openFullscreen()
    }else {
       this.closeFullscreen()
    }
  }
  openFullscreen(){
    let systemCalender = document.getElementById("caledner");
    systemCalender.style.height = '100% !important';
    systemCalender.style.position = 'fixed';
    systemCalender.style.top = '0';
    systemCalender.style.left = '0';
    systemCalender.style.width = '100%';
    systemCalender.style['z-index'] = '10000';
    let ribbonbar = document.getElementById('systemCalender');
    ribbonbar.style.width = '100vw';
    ribbonbar.style.height = '100vh' // all content
  }
  closeFullscreen(){
    let elem = document.getElementById("caledner");
    elem.style.height = '100% !important';
    elem.style.position = 'initial';
    elem.style.width = '100%';
    let ribbonbar = document.getElementById('systemCalender');
    ribbonbar.style.width = '100%'
    ribbonbar.style.height = '100%' // all content
  }
}
