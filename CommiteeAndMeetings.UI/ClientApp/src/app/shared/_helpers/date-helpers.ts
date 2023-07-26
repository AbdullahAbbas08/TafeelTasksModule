import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment-hijri';
import * as staticEnums from './../_enums/AppEnums';
export interface DateInStringObj {
  dateInString?: string;
  convertedDate?: { year: number; month: number; day: number };
}
export const isHijriCalendar =
  localStorage['isHijriDateCalendar'] &&
  localStorage['isHijriDateCalendar'] == '1'
    ? true
    : localStorage['isHijriDateCalendar'] &&
      localStorage['isHijriDateCalendar'] == '0'
    ? false
    : false;
export const isArabic = localStorage['culture'] === 'ar' ? true : false;

type GeorgianDateMMYYYYFeomHijri = [number, number | undefined];

export const convertToGeorgianYear = (
  hijriYear: number | string,
  hijriMonth?: number | string
): GeorgianDateMMYYYYFeomHijri => {
  if (!hijriMonth) {
    let georgianYear: number;
    if (typeof hijriYear === 'string') {
      georgianYear = +moment(hijriYear, 'iYYYY').format('YYYY');
    } else if (typeof hijriYear === 'number') {
      georgianYear = +moment(`${hijriYear}`, 'iYYYY').format('YYYY');
    }
    // console.log(georgianYear);
    return [georgianYear, undefined];
  } else {
    return moment(`${hijriYear} ${hijriMonth}`, 'iYYYY iMM')
      .format('YYYY MM')
      .split(' ')
      .map((x: string) => +x);
  }
};

export const parseArabic = (str) => {
  return str
    .replace(/[٠١٢٣٤٥٦٧٨٩]/g, function (d) {
      return d.charCodeAt(0) - 1632; // Convert Arabic numbers
    })
    .replace(/[۰۱۲۳۴۵۶۷۸۹]/g, function (d) {
      return d.charCodeAt(0) - 1776; // Convert Persian numbers
    });
};

export const parseEnglish = (str) => {
  str = str.toString();
  return str.replace(/[0-9]/g, function (d) {
    return String.fromCharCode(d.charCodeAt(0) + 1584); // Convert To Arabic Numbers
  });
};

export const isValidDate = (d: any): boolean =>
  d instanceof Date && !isNaN(<any>d);

// get georgian date for view as ObjDate {year,month,day}, convert string date [georgian] to {year,month,day}
export const getDateObjForView = (
  date: Date | string = new Date()
): { year: number; month: number; day: number } => {
  const dateObj = new Date(date),
    year = dateObj.getFullYear(),
    month = dateObj.getMonth() + 1,
    day = dateObj.getDate();
  return { year, month, day };
};

// get Current Day UmAlqura as {year,month,day}.
export const getHijriDateObjForView = (): {
  year: number;
  month: number;
  day: number;
} => {
  const momentDate = moment(),
    year = momentDate.iYear(),
    month = momentDate.iMonth() + 1,
    day = momentDate.iDate() + +localStorage['hijriDaysValue'];
  return { year, month, day };
};

// Get Current Day Date depends on user settings.
export const getDateObjForViewAsUserSettings = (): {
  year: number;
  month: number;
  day: number;
} => {
  if (
    localStorage.getItem('isHijriDateCalendar') &&
    localStorage.getItem('isHijriDateCalendar') === '1'
  ) {
    return getHijriDateObjForView();
  } else {
    return getDateObjForView();
  }
};

// Convert Hijri Date to Georgian Date.
export const georgianFromHijri = ({ year, month, day }) => {
  const dateFromHijriToG = moment(
    `${year} ${month} ${day}`,
    'iYYYY iM iD'
  ).format('DD/MM/YYYY');
  const date = parseArabic(dateFromHijriToG).split('/');
  return {
    year: +date[2],
    month: +date[1],
    day: +date[0],
  };
};

// Convert Georgian Date to Hijri Date.
export const getHijriFromGeorgian = (date?: {
  year: number;
  month: number;
  day: number;
}): NgbDateStruct => {
  // const myDate = new Date(date.year, date.month, date.day);
  if (date) {
    const [d, m, y] = parseArabic(
      moment(`${date.day} ${date.month} ${date.year}`, `D M YYYY`).format(
        `iDD-iMM-iYYYY`
      )
    )
      .split('-')
      .map((x) => parseInt(x));
    return { year: y, month: m, day: d };
  } else {
    const [day, month, year] = parseArabic(moment().format('iDD-iMM-iYYYY'))
      .split('-')
      .map((x) => parseInt(x));
    return { year, month, day };
  }
};

// Get toDay dateInString [georgian or hijri]
export const dateInStringTODAY = () => {
  let { year, month, day } = getDateObjForViewAsUserSettings();
  const d = day.toString().length < 2 ? '0' + day : day;
  const m = month.toString().length < 2 ? '0' + month : month;
  if (isArabic) {
    return parseEnglish(`${year}/${m}/${d}`);
    //return `${d}/${m}/${year}`; // 05/04/2019/ or 25/08/1441 as user settings says.
  } else {
    return parseArabic(`${d}/${m}/${year}`); // 05/04/2019/ or 25/08/1441 as user settings says.
  }
};

export const dateInString = (date: Date | any): DateInStringObj => {
  if (date === '') {
    return date;
  }

  if (!date) {
    console.error(
      new Error('Input[date]: Invalid Date - You Have To Pass Valid Date Value')
    );
    date = 'NoValidDateProvided';
    return date;
  }

  if (!(date instanceof Date)) {
    return {
      dateInString: date,
    };
  }

  if (
    localStorage.getItem('isHijriDateCalendar') &&
    localStorage.getItem('isHijriDateCalendar') === '1'
  ) {
    const dateArr = parseArabic(moment(date).format('iYYYY iMM iDD')).split(
        ' '
      ),
      h = date.getHours().toString(),
      m = date.getMinutes().toString(),
      s = date.getSeconds().toString();
    const convertedDate = {
      year: dateArr[0].toString(),
      month: dateArr[1].length < 2 ? '0' + dateArr[1] : dateArr[1],
      day: dateArr[2].length < 2 ? '0' + dateArr[2] : dateArr[2],
      h: h.length < 2 ? '0' + h : h,
      m: m.length < 2 ? '0' + m : m,
      s: s.length < 2 ? '0' + s : s,
      format: +(h.length < 2 ? '0' + h : h) >= 12 ? 'ص' : 'م',
    };
    return {
      convertedDate,
      dateInString: isArabic
        ? parseEnglish(
            `${convertedDate.year}/${convertedDate.month}/${convertedDate.day} ${convertedDate.s}:${convertedDate.m}:${convertedDate.h} ${convertedDate.format}`
          )
        : parseArabic(
            `${convertedDate.h}:${convertedDate.m}:${convertedDate.s} ${convertedDate.format} ${convertedDate.day}/${convertedDate.month}/${convertedDate.year}`
          ),
    };
  }
  const convertedDate = {
    year: date.getFullYear(),
    month: date.getMonth() + 1,
    day: date.getDate(),
    h:
      date.getHours().toString().length < 2
        ? '0' + date.getHours()
        : date.getHours(),
    m:
      date.getMinutes().toString().length < 2
        ? '0' + date.getMinutes()
        : date.getMinutes(),
    s:
      date.getSeconds().toString().length < 2
        ? '0' + date.getSeconds()
        : date.getSeconds(),
    format:
      +(date.getHours().toString().length < 2
        ? '0' + date.getHours()
        : date.getHours()) >= 12
        ? 'pm'
        : 'am',
  };
  const day =
    convertedDate.day.toString().length < 2
      ? '0' + convertedDate.day.toString()
      : convertedDate.day.toString();
  const month =
    convertedDate.month.toString().length < 2
      ? '0' + convertedDate.month.toString()
      : convertedDate.month.toString();
  const year =
    convertedDate.year.toString().length < 2
      ? '0' + convertedDate.year.toString()
      : convertedDate.year.toString();
  return {
    convertedDate,
    dateInString: `${day}/${month}/${year} ${convertedDate.h}:${convertedDate.m}:${convertedDate.s} ${convertedDate.format}`,
  };
};

export const getCurrentFullDateObj = () => {
  const dateInString = dateInStringTODAY(),
    dateObj =
      localStorage.getItem('isHijriDateCalendar') &&
      localStorage.getItem('isHijriDateCalendar') === '1'
        ? georgianFromHijri(getHijriDateObjForView())
        : getDateObjForView();
  return { dateInString, dateObj };
  /*
    dateInString: 05/08/1440,
    dateObj: { day: 8, month: 4, year: 2019 } // call ngbDateService.toDate(dateObj) and pass to it dateObj to
    return Date Instance to be passed to API.
    */
};

// convert dateObj { year; month; day} to string like 05-12-1440 or 03-07-2019
export const formatDateObj = (
  dateObj: { year: number; month: number; day: number },
  seprator = '/'
) => {
  if (!seprator.match(/\/|-/)) {
    return new Error('Seprator Must be [ / or - ].');
  }
  const { year, month, day } = dateObj;
  const m = month.toString().length < 2 ? '0' + month : month.toString();
  const d = day.toString().length < 2 ? '0' + day : day.toString();
  return `${d}${seprator}${m}${seprator}${year}`;
};

export interface NewDateFormate {
  yearNum: number;
  monthName: string;
  dayNum: number;
}

export const GetMonthName = (date: Date) => {
  const greogorianView = getDateObjForView(date);
  const hijrIView = getHijriFromGeorgian(greogorianView);
  let yearNum;
  let dayNum;
  let monthName;
  if (
    localStorage.getItem('isHijriDateCalendar') &&
    localStorage.getItem('isHijriDateCalendar') === '1'
  ) {
    monthName = isArabic
      ? staticEnums.hijriMonthsAr[hijrIView.month]
      : staticEnums.hijriMonthsEn[hijrIView.month];
    yearNum = isArabic
      ? parseArabic(hijrIView.year.toString())
      : parseEnglish(hijrIView.year.toString());
    dayNum = isArabic
      ? parseArabic(hijrIView.day.toString())
      : parseEnglish(hijrIView.day.toString());
  } else {
    monthName = isArabic
      ? staticEnums.GerogorianMonthsAr[greogorianView.month]
      : staticEnums.GerogorianMonthsEn[greogorianView.month];
    yearNum = isArabic
      ? parseArabic(greogorianView.year.toString())
      : parseEnglish(greogorianView.year.toString());
    dayNum = isArabic
      ? parseArabic(greogorianView.day.toString())
      : parseEnglish(greogorianView.day.toString());
  }
  const NewDate: NewDateFormate = { yearNum, monthName, dayNum };
  return NewDate;
};

export class MasarDate {
  constructor(public date: Date) {}
  static convertDateInstanceToCurrentCulture(date: Date) {
    const d = new Date(date);
    if (!(d instanceof Date)) {
      return;
    }
    // slice from 0 to 10 to remove hours, minutes, seconds and [PM _ AM].
    console.log(d, dateInString(d).dateInString.slice(0, 10));
    return dateInString(d).dateInString.slice(0, 10);
  }

  getTimeFormatted() {
    let hours = this.date.getHours();
    const minutes = this.date.getMinutes();
    const seconds = this.date.getSeconds();
    let pmORam = '';

    if (hours <= 12) {
      pmORam = 'AM';
    } else {
      pmORam = 'PM';
    }

    hours = this.formatHours(hours);

    const h = this.formatDigits(hours);
    const m = this.formatDigits(minutes);
    const s = this.formatDigits(seconds);

    return this.formatDate(h, m, s, pmORam);
  }

  private formatDigits(digit) {
    if (digit.toString().length == 2) {
      return digit;
    } else {
      return '0' + digit.toString();
    }
  }

  private formatHours(hours: number) {
    if (hours > 12) {
      return hours - 12;
    } else {
      return hours;
    }
  }

  private formatDate(h, m, s, pmORam: string) {
    return h + ':' + m + ' ' + pmORam;
  }
}
