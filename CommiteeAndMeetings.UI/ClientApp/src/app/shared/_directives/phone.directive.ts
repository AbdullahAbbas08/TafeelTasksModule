import { Directive } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, Validator, ValidatorFn, FormControl } from '@angular/forms';

let phoneLength;
function validatePhoneFactory() {
  return (c: AbstractControl) => {
    const isValid = validatePhone(c.value, phoneLength);
    if (isValid) { return null; } else {
      return {
        valid: false,
        message: 'ThisIsNotValidPhone',
      };
    }
  };
}

function validatePhone(phone, phoneLen) {
  let regx;
  // regx = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,9}[1-9]{1,}$/im;
  // regx = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,9}$/im;
  // regx = /^(?:\+(?:[0-9]{10})|(?:[0-9]{10}))$/im;
  if (phoneLen) {
    regx = new RegExp("^(?:\\+(?:[0-9]{"+(phoneLen-1)+"})|(?:[0-9]{"+phoneLen+"}))$", "gmi");
  }
  return regx.test(String(phone).toLowerCase());
}

@Directive({
  selector: '[app-phone][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: PhoneDirective, multi: true }
  ]
})
export class PhoneDirective implements Validator {
  validator: ValidatorFn;

  constructor() {
    phoneLength = JSON.parse(localStorage.getItem('MobileNoValidation'));
    this.validator = validatePhoneFactory();
  }

  validate(c: FormControl) {
    if (!c.value) { return null; }
    return this.validator(c);
  }
}
