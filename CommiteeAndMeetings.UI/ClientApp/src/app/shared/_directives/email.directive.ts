import { Directive } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, ValidatorFn, Validator, FormControl } from '@angular/forms';

function validateEmailFactory(): ValidatorFn {
  return (c: AbstractControl) => {
    const isValid = validateEmail(c.value);
    if (isValid) { return null; } else {
      return {
        valid: false,
        message: 'ThisIsNotValidEmail',
      };
    }
  };
}

function validateEmail(email) {
  const rgex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return rgex.test(String(email).toLowerCase());
}


@Directive({
  selector: '[app-email][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: EmailDirective, multi: true }
  ]
})
export class EmailDirective implements Validator {
  validator: ValidatorFn;

  constructor() {
    this.validator = validateEmailFactory();
  }

  validate(c: FormControl) {
    if (!c.value) { return null; }
    return this.validator(c);
  }
}
