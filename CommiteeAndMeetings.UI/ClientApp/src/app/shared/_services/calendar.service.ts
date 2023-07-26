import { TranslateService } from "@ngx-translate/core";
import {
  NgbDateStruct,
  NgbDatepickerI18n,
  NgbCalendarIslamicUmalqura,
  NgbDate
} from "@ng-bootstrap/ng-bootstrap";
import { Injectable } from "@angular/core";
import * as moment from "moment-hijri";

@Injectable({
  providedIn: "root"
})
export class CalendarService {
  constructor() {}
}

@Injectable({
  providedIn: "root"
})
export class ArabicCalendarGeorgian extends NgbDatepickerI18n {
  MONTHS =
    localStorage["culture"] && JSON.parse(localStorage["culture"]) == "ar"
      ? [
          "يناير",
          "فبراير",
          "مارس",
          "أبريل",
          "مايو",
          "يونيو",
          "يوليو",
          "أغسطس",
          "سبتمبر",
          "أكتوبر",
          "نوفمبر",
          "ديسمبر"
        ]
      : [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June",
          "July",
          "August",
          "September",
          "October",
          "November",
          "December"
        ];
  WEEKDAYS =
    localStorage["culture"] && JSON.parse(localStorage["culture"]) == "ar"
      ? ["ن", "ث", "ر", "خ", "ج", "س", "ح"]
      : ["Mon", "Thu", "Wed", "Tue", "Fri", "Sat", "Sun"];

  constructor(private translate: TranslateService) {
    super();
    this.translate.onLangChange.subscribe(langObj => {
      let { lang } = langObj;
      if (lang == "en") {
        this.MONTHS = [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June",
          "July",
          "August",
          "September",
          "October",
          "November",
          "December"
        ];

        this.WEEKDAYS = ["Mon", "Thu", "Wed", "Tue", "Fri", "Sat", "Sun"];
      } else {
        this.MONTHS = [
          "يناير",
          "فبراير",
          "مارس",
          "أبريل",
          "مايو",
          "يونيو",
          "يوليو",
          "أغسطس",
          "سبتمبر",
          "أكتوبر",
          "نوفمبر",
          "ديسمبر"
        ];
        this.WEEKDAYS = ["ن", "ث", "ر", "خ", "ج", "س", "ح"];
      }
    });
  }

  getWeekdayShortName(weekday: number) {
    return this.WEEKDAYS[weekday - 1];
  }

  getMonthShortName(month: number) {
    return this.MONTHS[month - 1];
  }

  getMonthFullName(month: number) {
    return this.MONTHS[month - 1];
  }

  getDayAriaLabel(date: NgbDateStruct): string {
    return `${date.day}-${date.month}-${date.year}`;
  }
}

@Injectable({
  providedIn: "root"
})
export class IslamicI18n extends NgbDatepickerI18n {
  WEEKDAYS =
  localStorage["culture"]&& JSON.parse(localStorage["culture"]) == "ar"
      ? ["ن", "ث", "ر", "خ", "ج", "س", "ح"]
      : ["Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"];
  MONTHS =
  localStorage["culture"]&& JSON.parse(localStorage["culture"]) == "ar"
      ? [
          "محرم",
          "صفر",
          "ربيع الأول",
          "ربيع الآخر",
          "جمادى الأولى",
          "جمادى الآخرة",
          "رجب",
          "شعبان",
          "رمضان",
          "شوال",
          "ذو القعدة",
          "ذو الحجة"
        ]
      : [
          "Muharram",
          "Safar",
          "Rabi'ul Awwal",
          "Rabi'ul Akhir",
          "Jumadal Ula",
          "Jumadal Akhira",
          "Rajab",
          "Sha'ban",
          "Ramadan",
          "Shawwal",
          "Dhul Qa'ada",
          "Dhul Hijja"
        ];

  constructor(private translate: TranslateService) {
    super();
    this.translate.onLangChange.subscribe(langObj => {
      const { lang } = langObj;
      if (lang == "en") {
        this.WEEKDAYS = ["Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"];
        this.MONTHS = [
          "Muharram",
          "Safar",
          "Rabi'ul Awwal",
          "Rabi'ul Akhir",
          "Jumadal Ula",
          "Jumadal Akhira",
          "Rajab",
          "Sha'ban",
          "Ramadan",
          "Shawwal",
          "Dhul Qa'ada",
          "Dhul Hijja"
        ];
      } else {
        this.WEEKDAYS = ["ن", "ث", "ر", "خ", "ج", "س", "ح"];
        this.MONTHS = [
          "محرم",
          "صفر",
          "ربيع الأول",
          "ربيع الآخر",
          "جمادى الأولى",
          "جمادى الآخرة",
          "رجب",
          "شعبان",
          "رمضان",
          "شوال",
          "ذو القعدة",
          "ذو الحجة"
        ];
      }
    });
  }
  getWeekdayShortName(weekday: number) {
    return this.WEEKDAYS[weekday - 1];
  }

  getMonthShortName(month: number) {
    return this.MONTHS[month - 1];
  }

  getMonthFullName(month: number) {
    return this.MONTHS[month - 1];
  }

  getDayAriaLabel(date: NgbDateStruct): string {
    return `${date.day}-${date.month}-${date.year}`;
  }
}

@Injectable({
  providedIn: "root"
})
export class HijriCalendar extends NgbCalendarIslamicUmalqura {
  fromGregorian() {
    let h = moment(),
      f = h.format("iYYYY/iM/iD"),
      d = h.iDate(),
      m = h.iMonth() + 1,
      y = h.iYear();
    return new NgbDate(y, m, d);
  }
}
