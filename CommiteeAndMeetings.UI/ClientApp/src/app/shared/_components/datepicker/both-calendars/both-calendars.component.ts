import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy
} from "@angular/core";
import { NgbdDatepickerIslamicumalqura } from "./../datepicker-islamicumalqura";
import {
  NgbDatepickerI18n,
  NgbCalendar,
  NgbDatepicker,
  NgbDateStruct
} from "@ng-bootstrap/ng-bootstrap";
import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";
import { ArabicCalendarGeorgian } from 'src/app/shared/_services/calendar.service';
import { StoreService } from "src/app/shared/_services/store.service";
import * as moment from "moment-hijri";

@Component({
  selector: "app-both-calendars",
  templateUrl: "./both-calendars.component.html",
  styleUrls: ["./both-calendars.component.scss"],
  providers: [{ provide: NgbDatepickerI18n, useClass: ArabicCalendarGeorgian }]
})
export class BothCalendarsComponent
  implements OnInit, AfterViewInit, OnDestroy {
  destroyObs$ = new Subject();

  hijri: boolean = false;
  gregorian: boolean = false;

  @Output() select = new EventEmitter();
  @Output() navigate = new EventEmitter();

  @Input() datepickerCloseFooter: any;
  @Input() minDate: { year: number; month: number; day: number };

  georgianModel;

  @ViewChild("Geodp", { static: false }) georgianCalendar: NgbDatepicker;
  @ViewChild("Hijdp", { static: false })
  HijriCalendar: NgbdDatepickerIslamicumalqura;

  constructor(
    private calendar: NgbCalendar,
    private store: StoreService,
    private elRef: ElementRef
  ) {}

  ngOnInit() {
    this.hijri = localStorage["isHijriDateCalendar"] == "1";
    this.gregorian = localStorage["isHijriDateCalendar"] != "1";

    this.store.calendarType$.subscribe((oneOrZero: string) => {
      if (oneOrZero != "1") {
        this.georgianModel = this.calendar.getToday();
      }
    });
  }

  ngAfterViewInit() {
    // get i element [Calendar ICON].
    const i = (this.elRef
      .nativeElement as HTMLElement).previousElementSibling.querySelector(
      "i"
    ) as any;
    // Subscribe on calendar date selection, onSelect
    this.select
      .pipe(takeUntil(this.destroyObs$))
      .subscribe(selectEventResult => {
        // if calendar is open [i.checked == true], after select date we must reassign to be [i.checked == false].
        if (i.checked) {
          i.checked = false;
        }
      });
  }

  ngOnDestroy() {
    this.destroyObs$.next(false);
    this.destroyObs$.complete();
  }

  makeDateObject(dateObj) {
    let { year, month, day } = dateObj;
    day = day.toString().length < 2 ? "0" + day : day;
    month = month.toString().length < 2 ? "0" + month : month;

    let date = {
      dateInString: `${day}/${month}/${year}`,
      dateObj: dateObj
    };
    if (this.hijri) {
      let geogrianDate: string = moment(
          date.dateInString,
          "iD/iM/iYYYY"
        )._i.slice(0, 10),
        dateArr = geogrianDate.split("-"); // Array [year, month, day] index 0=> year 1=> month 2=> day
      let dateToEmit: any = {
        dateInString: `${day}/${month}/${year}`,
        dateObj: {
          year: +dateArr[0],
          month: +dateArr[1],
          day: +dateArr[2]
        }
      };
      this.handleMaskOnselect(dateToEmit);
      this.select.emit(dateToEmit);
      return;
    }
    this.select.emit(date);
  }

  handleMaskOnselect(event) {
    if (
      event &&
      this.hijri &&
      (event.dateInString.includes("29/02") ||
        event.dateInString.includes("30/02"))
    ) {
      this.store.removeMaskFromInput$.next({
        state: true,
        value: event.dateInString
      });
    } else {
      this.store.removeMaskFromInput$.next({
        state: false,
        value: event.dateInString
      });
    }
  }

  navigateToSpecificDay(dateObj: NgbDateStruct): void {
    if (localStorage['isHijriDateCalendar'] && localStorage['isHijriDateCalendar'] === '1' && this.HijriCalendar) {
      this.HijriCalendar.model = dateObj;
      this.HijriCalendar.dp.navigateTo(dateObj);
    } else if (this.georgianCalendar) {
      this.georgianModel = dateObj;
      this.georgianCalendar.navigateTo(dateObj);
    }
  }
}
