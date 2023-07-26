import { Directive, OnInit, Input } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, ValidatorFn, Validator, FormControl } from '@angular/forms';
import { LocalizationService } from '../_services/localization.service';
import { TranslateService } from '@ngx-translate/core';

let isArabic = 'ar';
let Message1, Message2;

function validateContactFactory(length: number): ValidatorFn {
  return (c: AbstractControl) => {
    const isValid = validateMinLength(c.value, length);
    if (isValid) { return null; } else {
      // TODO: try to display message dynamicly from here
      return {
        valid: false,
        message: Message1 + ' (' + length + ') ' + Message2,
      };
    }
  };
}

function validateMinLength(value: string, length: number) {
  return value.length >= length ? true : false;
}


@Directive({
  selector: '[app-min-length][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: MinLengthDirective, multi: true }
  ]
})
export class MinLengthDirective implements OnInit, Validator {
  // tslint:disable-next-line:no-input-rename
  @Input('app-min-length') length: number;
  validator: ValidatorFn;

  constructor(private localization: LocalizationService, private translate: TranslateService) {
    localization.isArabic$.subscribe((res) => {
      this.translate.get('MinLength').subscribe(value => { Message1 = value; });
      this.translate.get('Character').subscribe(value => { Message2 = value; });
    });
  }

  ngOnInit() {
    this.validator = validateContactFactory(this.length);
  }

  validate(c: FormControl) {
    if (!c.value) { return null; }
    return this.validator(c);
  }
}
