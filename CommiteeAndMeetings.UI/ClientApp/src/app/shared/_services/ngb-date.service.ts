import { Injectable } from "@angular/core";
import { NgbDateParserFormatter, NgbDateStruct } from "@ng-bootstrap/ng-bootstrap";
import * as moment from "moment-hijri";
import { BrowserStorageService } from "./browser-storage.service";
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker'; 

export type dateType = "noHours" | "withHours";

@Injectable({
  providedIn: "root"
})
export class NgbDateService {
  constructor(private browserService: BrowserStorageService) {}

  toDate(Model: NgbDateStruct, dateType: dateType = "noHours"): Date {
    if (!Model) {
      return null;
    }
    const date = new Date();
    switch (dateType) {
      case "withHours": {
        return new Date(
          Model.year,
          Model.month - 1,
          Model.day,
          date.getHours(),
          date.getMinutes(),
          date.getSeconds()
        );
        break;
      }
      case "noHours": {
        return new Date(Model.year, Model.month - 1, Model.day);
        break;
      }
    }
  }

  toShortDateString(Model: NgbDateStruct): string {
    if (!Model) {
      return null;
    }
    const date = new Date(Model.year, Model.month - 1, Model.day);
    return `${
      date.getDate() < 10 ? "0" + date.getDate() : date.getDate()
    }-${date.getMonth() + 1}-${date.getFullYear()}`;
  }

  toFullDate(Model: NgbDateStruct, dateType: dateType): Date {
    if (!Model) {
      return null;
    }
    const date = new Date(Model.year, Model.month - 1, Model.day);
    const now = new Date();
    let n;
    switch (dateType) {
      case "noHours": {
        n = `${date.getFullYear()}-${
          date.getMonth() + 1 < 10
            ? "0" + (date.getMonth() + 1)
            : date.getMonth() + 1
        }-${date.getDate() < 10 ? "0" + date.getDate() : date.getDate()}`;
        break;
      }
      case "withHours": {
        n = `${date.getFullYear()}-${
          date.getMonth() + 1 < 10
            ? "0" + (date.getMonth() + 1)
            : date.getMonth() + 1
        }-${date.getDate() < 10 ? "0" + date.getDate() : date.getDate()}T${
          now.getHours() < 10 ? "0" + now.getHours() : now.getHours()
        }:${
          now.getMinutes() < 10 ? "0" + now.getMinutes() : now.getMinutes()
        }:00`;
        break;
      }
    }
    return new Date(n);
  }

  fromDate(date: Date): NgbDateStruct {
    if (!date) {
      return null;
    }
    return <NgbDateStruct>{
      day: date.getDate(),
      month: date.getMonth() + 1,
      year: date.getFullYear()
    };
  }

  getTime(Date: Date): String {
    if (!Date) {
      return null;
    }
    return `${Date.getHours() < 10 ? "0" + Date.getHours() : Date.getHours()}:${
      Date.getMinutes() < 10 ? "0" + Date.getMinutes() : Date.getMinutes()
    }`;
  }

  /* create gregorian date object from date string like 2019/02/14 if greogrian and 1440/06/19 if hijri */
  createDateFromStructObject(
    dateString: string,
    hours: number = new Date().getHours(),
    minutes: number = new Date().getMinutes(),
    includeTime: boolean = false
  ): Date {
    if (dateString && dateString.length > 10) {
      dateString = dateString.slice(0, 10);
    }
    // console.log('[input] Date createDateFromStructObject() :', dateString);
    let dateArray: string[];
    if (dateString) {
      dateArray = dateString.split("/").reverse();
    }
    // console.log(dateArray, 'date array');
    let outputDate: Date;

    const isHijriCalendar: boolean =
      localStorage.getItem("isHijriDateCalendar") &&
      localStorage.getItem("isHijriDateCalendar") === "1";
    if (isHijriCalendar && dateString) {
      moment.locale("en-US");
      const idateAr: string[] = moment(dateString, "iDD/iMM/iYYYY")
        .format("DD/MM/YYYY")
        .split("/")
        .reverse();
      outputDate = includeTime
        ? new Date(
            +idateAr[0],
            +idateAr[1] - 1,
            +idateAr[2],
            hours,
            minutes,
            new Date().getSeconds()
          )
        : new Date(+idateAr[0], +idateAr[1] - 1, +idateAr[2], 2); // hours is 2 so toJSON method still save the selected day
    } else {
      if (dateArray && dateArray.length) {
        outputDate = includeTime
          ? new Date(
              +dateArray[0],
              +dateArray[1] - 1,
              +dateArray[2],
              hours,
              minutes,
              new Date().getSeconds()
            )
          : new Date(+dateArray[0], +dateArray[1] - 1, +dateArray[2], 2); // hours is 2 so toJSON method still save the selected day
      }
    }
    // console.log('[output] Date createDateFromStructObject() :', outputDate);
    return outputDate;
  }

  /* conmvert gregorian date object to hijri or gregorian date string based on current ui culture */
  creatStructObjectFromDate(date: Date): string {
    if (!date) {
      return;
    }

    let dateString: string;
    let separator = "/";
    moment.locale("en-US");
    if (
      localStorage.getItem("isHijriDateCalendar") &&
      localStorage.getItem("isHijriDateCalendar") === "1"
    ) {
      const dateParts: string = moment(
        date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate(),
        "YYYY-M-D"
      ).format("iDD/iMM/iYYYY");
      // .split('/');
      let dateArr = dateParts.split(separator);
      let day = dateArr[0];
      let month = dateArr[1];
      let year = dateArr[2];
      const d = day.toString().length < 2 ? "0" + day : day;
      const m = month.toString().length < 2 ? "0" + month : month;
      dateString = `${day}${separator}${month}${separator}${year}`;
    } else {
      dateString =
        (date.getDate().toString().length < 2
          ? "0" + date.getDate()
          : date.getDate()) +
        separator +
        ((date.getMonth() + 1).toString().length < 2
          ? "0" + (date.getMonth() + 1)
          : date.getMonth() + 1) +
        separator +
        date.getFullYear();
    }

    return dateString;
  }

  createStructObjFromString(data: string): NgbDateStruct {
    let dateArr = data.split('/');
    let day = dateArr[0];
    let month = dateArr[1];
    let year = dateArr[2];
    const d = day.toString().length < 2 ? "0" + day : day;
    const m = month.toString().length < 2 ? "0" + month : month;
    let dateModel = {year: +year, month: +m, day: +d} as NgbDateStruct;
    return dateModel;
  }

  formatDate(Model: NgbDateStruct): string {
    const date = new Date(Model.year, Model.month - 1, Model.day);
    return (
      date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear()
    );
  }

  parseEnglish(str) {
    str = str.toString();
    return str.replace(/[0-9]/g, function(d) {
      return String.fromCharCode(d.charCodeAt(0) + 1584); // Convert To Arabic Numbers
    });
  }

  parseArabic(str) {
    return str
      .replace(/[٠١٢٣٤٥٦٧٨٩]/g, function(d) {
        return d.charCodeAt(0) - 1632; // Convert Arabic numbers
      })
      .replace(/[۰۱۲۳۴۵۶۷۸۹]/g, function(d) {
        return d.charCodeAt(0) - 1776; // Convert Persian numbers
      });
  }

  changeCalendar() {
    const newValue =
      this.browserService.getLocal("isHijriDateCalendar") === 0 ? 1 : 0;
    this.browserService.setLocal("isHijriDateCalendar", newValue);
    setTimeout(() => {
      window.location.reload();
    });
  }
  fromGorgianToHijri(date:string):NgbDateStruct{
    if ( localStorage.getItem("isHijriDateCalendar") &&
    localStorage.getItem("isHijriDateCalendar") === "1"){
      let x :DateFormatterService;
      return x.ToHijriDateStruct(date,"DD/MM/YYYY")
    }
    return this.createStructObjFromString(date);
  }
  // function to get the time in any zone egy or KSA
  getTimeZoneOffset(dateTimeZone:Date){
    var targetTime = new Date(dateTimeZone),
    tzDifference = targetTime.getTimezoneOffset(),
    offsetTime = new Date(targetTime.getTime() + tzDifference * 60 * 1000)
    return offsetTime
  }
  // convert gorgian date number to arabic number
  getTasksDateInArabic(date){
    var datelang = this.getTimeZoneOffset(date);
    const m = (datelang.getMonth() + 1).toString().length === 2 ? (datelang.getMonth() + 1).toString() : '0' + (datelang.getMonth() + 1).toString(),
          d = (datelang.getDate().toString().length === 2) ? datelang.getDate().toString() : ('0' + datelang.getDate().toString()),
          y = datelang.getFullYear().toString();
   var dateInString = `${y}/${m}/${d}`;
   var newDate = this.parseEnglish(dateInString)
   return  newDate
 }
}
