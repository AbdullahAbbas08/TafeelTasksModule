import { Subject } from 'rxjs';
import {
  getDateObjForViewAsUserSettings,
  parseArabic,
  formatDateObj,
} from './../_helpers/date-helpers';
import {
  Directive,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ElementRef,
  Input,
  Output,
  EventEmitter,
  HostListener,
  forwardRef,
  HostBinding,
  OnChanges,
} from '@angular/core';
import Inputmask from 'inputmask';

import * as moment from 'moment-hijri';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { StoreService } from '../_services/store.service';

@Directive({
  selector: '[dateInputValidation]',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DateInputValidationDirective),
      multi: true,
    },
  ],
})
export class DateInputValidationDirective
  implements OnInit, OnDestroy, AfterViewInit, ControlValueAccessor, OnChanges {
  @Input() togglerReference: boolean;
  @Input() inputreference: HTMLInputElement;
  @Input() minDate: { year: number; month: number; day: number };
  @Input() maxDate: { year: number; month: number; day: number };
  @Output() toggle = new EventEmitter();
  @Output() selectDate = new EventEmitter();
  @Output() isValidDate = new EventEmitter();

  @Input()
  @HostBinding('disabled')
  disabled: boolean;
  private minMaxYear: number;
  inputElm;
  inputMask;
  dateValue: string = '';
  destroyObs$ = new Subject();
  counterId = 0;
  isHijriCalendar: boolean =
    localStorage['isHijriDateCalendar'] &&
    localStorage['isHijriDateCalendar'] === '1';

  constructor(public el: ElementRef, private store: StoreService) {
    this.inputElm = this.el.nativeElement as HTMLInputElement;
    this.inputMask = new Inputmask();
  }

  public onChange: any = (value) => {};
  public onTouched: any = () => {};
  writeValue(val: string): void {
    if (val) this.el.nativeElement.value = val;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean) {
    this.disabled = isDisabled;
  }

  ngOnChanges(changes): void {
    // console.log(this.inputreference ,  ' this.inputreference ');
  }

  ngOnInit() {}

  ngAfterViewInit() {
    this.minMaxYear = +localStorage.getItem('minMax') || 50;

    document.addEventListener('keypress', this.disableEnterBehavior);
    this.validateDateInputs(true, this.inputElm);
    const parent = this.inputElm.parentElement;

    // Calendar Icon
    // Create i tag
    const i = document.createElement('i');
    const clear = document.createElement('div');

    i.classList.add('fas');
    i.classList.add('fa-calendar');
    i.classList.add('d-inline-block');
    i.classList.add('position-absolute');
    i.classList.add('__calendar-icon');
    parent.appendChild(i);

    i.addEventListener('click', (e) => {
      const target = e.target as any;
      if (!target.checked) {
        target.checked = true;
        this.toggle.emit(target.checked);
      } else {
        target.checked = false;
        this.toggle.emit(target.checked);
      }
    });

    // Clear button
    clear.classList.add('fas');
    clear.classList.add('fa-times');
    clear.classList.add('__clear_date_input_value');
    parent.appendChild(clear);
    if (!this.disabled) {
      clear.addEventListener('click', (e) => {
        this.inputElm.value = null;
        this.onChange(null);
        this.store.removeMaskFromInput$.next({ state: false, value: null });
        this.selectDate.emit({ dateObj: {}, dateInString: '' });
      });
    }

    this.store.removeMaskFromInput$
      .pipe(takeUntil(this.destroyObs$))
      .subscribe((x) => {
        if (x) this.dateValue = x.value;
        if (x && x.state) {
          if (this.inputreference) {
            this.validateDateInputs(false, this.inputreference);
          } else {
            this.validateDateInputs(false);
          }
        } else if (x && !x.state) {
          this.validateDateInputs(true);
        }
      });
  }

  ngOnDestroy() {
    this.store.removeMaskFromInput$.next({ state: false, value: null });
    this.destroyObs$.next(false);
    this.destroyObs$.complete();
  }

  private disableEnterBehavior(e: KeyboardEvent) {
    if (e.keyCode === 13) {
      e.preventDefault();
    }
  }

  // Validate Date Inputs
  validateDateInputs(isActiveMask, input = this.inputElm) {
    const { year, month, day } = getDateObjForViewAsUserSettings();
    if (!this.dateValue) this.dateValue = `${day}/0${month}/${year}`;
    //console.log(this.minDate, ' date ')
    //console.log('min date', formatDateObj(this.minDate))
    this.inputMask = new Inputmask('datetime', {
      oncomplete: (e: any) => {
        this.selectDate.emit(this.convertStringDateToDateObj(e.target.value));
        this.onChange(
          this.convertStringDateToDateObj(e.target.value).dateInString
        );
        this.writeValue(
          this.convertStringDateToDateObj(e.target.value).dateInString
        );
      },
      inputFormat: 'dd/mm/yyyy',
      min: this.minDate
        ? formatDateObj(this.minDate)
        : 'dd/mm/' + (year - this.minMaxYear).toString(),
      max: 'dd/mm/' + (year + this.minMaxYear).toString(),
      leapday: this.isHijriCalendar ? '31/02/' : '29/02',
    });
    if (isActiveMask) {
      if (
        this.isHijriCalendar &&
        (this.dateValue.includes('29/02') || this.dateValue.includes('30/02'))
      ) {
        if (input && input.inputmask) {
          input.inputmask.remove();
        }
      } else {
        this.inputMask.mask(input);
      }
    } else {
      if (input && input.inputmask) {
        input.inputmask.remove();
      }
    }
    return this.inputMask;
  }

  private convertStringDateToDateObj(dateInString: string) {
    let dateArr;
    if (this.isHijriCalendar) {
      dateArr = moment(dateInString, 'iDD/iMM/iYYYY')
        .format('DD/MM/YYYY')
        .split('/');
      dateArr = dateArr.map((x) => parseArabic(x));
      const year = +dateArr[dateArr.length - 1],
        month = +dateArr[1],
        day = +dateArr[0],
        dateObj = { year, month, day };
      return {
        dateInString,
        dateObj,
      };
    } else {
      const date = new Date(dateInString.split('/').reverse().join('/'));
      const year = date.getFullYear(),
        month = date.getMonth() + 1,
        day = date.getDate(),
        dateObj = { year, month, day };

      return { dateInString, dateObj };
    }
  }

  @HostListener('blur')
  validateDate() {
    const isValid = Inputmask.isValid(this.el.nativeElement.value, {
      alias: 'datetime',
      inputFormat: 'dd/mm/yyyy',
    });
    if (!isValid) {
      this.isValidDate.emit({ invalidDate: true });
      this.el.nativeElement.setCustomValidity('date');
      this.onChange(this.el.nativeElement.value);
    } else {
      this.isValidDate.emit(null);
      this.el.nativeElement.setCustomValidity('');
    }
  }
}
