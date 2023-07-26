import { BrowserStorageService } from './../_services/browser-storage.service';
import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment-hijri';

@Pipe({
  name: 'customDate'
})
export class CustomDatePipe implements PipeTransform {

  constructor(
    private browserStorageService: BrowserStorageService
    ) {
  }

  transform(value: Date, args?: any): any {
    // const d = new Date(`${value.month}-${value.day}-${value.year}`).toString().split(' ');
    if (!value) {
      return;
    }

    if (!(value instanceof Date)) {
      if ((value as string).match(/[\u0600-\u06FF]/)) {
        let v = value as string
        if (v.split('/')[v.split('/').length - 1].length === 4) {
          v = v.split('/').reverse().join('/')
        }
        return v
      }
    }
    if (new Date(value).getTime() <= 0) { // * it will be invalid date or Hijri Date.
      let dateInString;
      const d = new Date(value),
      year = d.getFullYear(),
      month = d.getMonth() + 1,
      day = (d.getDate()) + (+localStorage['hijriDaysValue']);
      dateInString = `${day}/${month}/${year}`;
      return dateInString;
    };
    // console.log('customDate() @Pipe', value)
    let dateString: string,
      date = new Date(value),
      daysValueFROMAPI = localStorage['hijriDaysValue'] ? +localStorage['hijriDaysValue'] : 0;

    moment.locale('en-US');
    let hijriFormat = this.browserStorageService.getLocal('culture') ? 'iYYYY/iMM/iDD' : 'iDD/iMM/iYYYY';
    if (localStorage.getItem('isHijriDateCalendar') === '1') {
      const dateParts: string = moment(date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + (date.getDate() + daysValueFROMAPI), 'YYYY-M-D')
        .format(hijriFormat);
      // .split('/');
      dateString = dateParts;
    } else {
      const m = (date.getMonth() + 1).toString().length === 2 ? (date.getMonth() + 1).toString() : '0' + (date.getMonth() + 1).toString();
      const d = (date.getDate().toString().length === 2) ? date.getDate().toString() : ('0' + date.getDate().toString());
      const y = date.getFullYear().toString();

      dateString = this.browserStorageService.getLocal('culture') === 'ar' ? `${y}/${m}/${d}` : `${d}/${m}/${y}`;
    }

    if (!(Array.isArray(args)) && args == 'd/MMM/y') {
      let monthFormatted = date.toDateString().split(' ')[1];
      return `${monthFormatted}/${date.getFullYear()}/${date.getDate()}`
    }
    dateString = this.browserStorageService.getLocal('culture') === 'ar' ? this.parseEnglish(dateString) : this.parseArabic(dateString);
    return dateString; // `${d.day}/${d.month}/${d.year}`;
  }

  parseEnglish(str) {
    str = str.toString();
    return str.replace(/[0-9]/g, function (d) {
      return String.fromCharCode(d.charCodeAt(0) + 1584) // Convert To Arabic Numbers
    });
  }

  parseArabic(str) {
    return str.replace(/[٠١٢٣٤٥٦٧٨٩]/g, function (d) {
      return d.charCodeAt(0) - 1632; // Convert Arabic numbers
    }).replace(/[۰۱۲۳۴۵۶۷۸۹]/g, function (d) {
      return d.charCodeAt(0) - 1776; // Convert Persian numbers
    });
  }

}
