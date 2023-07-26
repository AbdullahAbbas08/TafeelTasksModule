import {
  Component,
  OnInit,
  ViewChild,
  Output,
  EventEmitter,
  Input
} from "@angular/core";
import {
  NgbDateStruct,
  NgbCalendar,
  NgbDatepickerI18n,
  NgbDatepicker
} from "@ng-bootstrap/ng-bootstrap";

// Import Calendars
import { HijriCalendar, IslamicI18n } from "../../_services/calendar.service";
import * as moment from "moment-hijri";

@Component({
  selector: "ngbd-datepicker-islamicumalqura",
  templateUrl: "./datepicker-islamicumalqura.html",
  providers: [
    { provide: NgbCalendar, useClass: HijriCalendar },
    { provide: NgbDatepickerI18n, useClass: IslamicI18n }
  ]
})
export class NgbdDatepickerIslamicumalqura implements OnInit {
  terTemplate;
  @Input('foo') foo;
  @Input('footerTemplate') footerTemplate;
  @Input('minDate') minDate: { year: number; month: number; day: number };

  @Output("onMonthChanged") onMonthChanged = new EventEmitter();
  @Output("emitToParent") emitToParent = new EventEmitter();
  @Output("select") select = new EventEmitter();
  @Output("navigate") navigate = new EventEmitter();

  model: NgbDateStruct;
  @ViewChild("dp", { static: false }) dp: NgbDatepicker;

  constructor(private calendar: NgbCalendar) {}

  ngOnInit() {
    this.selectToday();
  }

  selectToday() {
    let hijriDate = this.calendar.getToday();
    hijriDate.day = hijriDate.day + (localStorage["hijriDaysValue"] ? JSON.parse(localStorage["hijriDaysValue"]) : 0);
    this.model = hijriDate;
  }

  getDateWithHMSInHijri(hijriDate) {
    this.select.emit(hijriDate);
  }

  getGregorianDate() {
    let { year, month, day } = this.calendar.getToday(),
      second =
        new Date().getSeconds().toString().length == 1
          ? "0" + new Date().getSeconds().toString()
          : new Date().getSeconds().toString(),
      minute =
        new Date().getMinutes().toString().length == 1
          ? "0" + new Date().getMinutes().toString()
          : new Date().getMinutes().toString(),
      hour =
        new Date().getHours().toString().length == 1
          ? "0" + new Date().getHours().toString()
          : new Date().getHours().toString(),
      gregorianDate: string = moment(
        `${day}/${month}/${year}`,
        "iYYYY/iM/iD"
      ).format("YYYY/M/D");
    return { second, minute, hour, year, month, day, gregorianDate };
  }
}
