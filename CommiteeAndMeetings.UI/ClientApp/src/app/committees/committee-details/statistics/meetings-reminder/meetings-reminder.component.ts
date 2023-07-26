import { Component, OnInit,ChangeDetectionStrategy,ViewChild,
  TemplateRef, } from '@angular/core';
import {NgbDateStruct, NgbCalendar} from '@ng-bootstrap/ng-bootstrap';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
const colors: any = {
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA',
  },
};
import { Subject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StatisticsService } from '../statistics.service';
import { MeetingSummaryDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-meetings-reminder',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './meetings-reminder.component.html',
  styleUrls: ['./meetings-reminder.component.scss']
})
export class MeetingsReminderComponent implements OnInit {
  @ViewChild('modalContent', { static: true }) modalContent: TemplateRef<any>;
  model: NgbDateStruct;
  committeeId:string
  activeDayIsOpen: boolean = false;
  date: {year: number, month: number};
  viewDate: Date = new Date();
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  toggle:boolean = false;
  refresh: Subject<any> = new Subject();
  modalData: {
    action: string;
    event: CalendarEvent;
  };
  events: CalendarEvent<{meetingSummary:MeetingSummaryDTO}>[] = [];

  constructor(private route:ActivatedRoute,private calendar: NgbCalendar,private _statistics:StatisticsService,public translate: TranslateService,private router:Router,private browserStorage:BrowserStorageService) {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
   }

  ngOnInit(): void {
    this.selectToday();
    this.getCommitteesMeeting();
  }
  selectToday() {
    this.model = this.calendar.getToday();
  }
  setView(view: CalendarView) {
    this.view = view;
  }
  getCommitteesMeeting(){
    this._statistics.getCommitteesMeetings(this.browserStorage.encryptCommitteId(this.committeeId)).subscribe((meeting:MeetingSummaryDTO[]) => {
      this.events = [];
      meeting.map((res)=> {
        this.events.push({
          start:res.meetingFromTime,
          title:res.title,
          id:res.id,
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
      })
    })
  }
  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
  }
  handleEvent( event: CalendarEvent): void {
  }
  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
      this.viewDate = date;
    }
  }
}
