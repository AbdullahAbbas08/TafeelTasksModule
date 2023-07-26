import { Directive, OnInit, Input } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, ValidatorFn, Validator, FormControl } from '@angular/forms';
import { LocalizationService } from '../_services/localization.service';
import { TranslateService } from '@ngx-translate/core';

function validateLanguageFactory(lang: string): ValidatorFn {
  return (c: AbstractControl) => {
    const isValid = validateLanguage(c.value, lang);
    if (isValid) { return null; } else {
      return {
        valid: false,
        message: generateMessage(lang),
      };
    }
  };
}

function validateLanguage(text: string, lang: string) {
  let isValid = true;
  switch (lang) {
    case 'ar': isValid = validateArabicText(text); break;
    case 'en': isValid = validateEnglishText(text); break;
    case 'fn': isValid = validateFrenchText(text); break;
  }
  return isValid;
}

function validateArabicText(text) {
  // const rgex = /^[0-9 ]*[\u0600-\u06FF_]+[\u0600-\u06FF0-9 .]*$/;
  const rgex = /^[0-9 ]+|[\u0600-\u06FF_.\-& ]+[\u0600-\u06FF0-9 ._@./#$,()!^*%&+-]*$/;
  return rgex.test(String(text).toLowerCase());
}

function validateEnglishText(text) {
  // const rgex = /^[0-9 ]*[a-zA-Z_]+[a-zA-Z0-9 .]*$/;
  const rgex = /^[a-zA-Z0-9\-_&. _@./#$()!,^*%&+-]+$/;
  return rgex.test(String(text).toLowerCase());
}

function validateFrenchText(text) {
  // const rgex = /^[0-9 ]*[a-zA-Z_]+[a-zA-Z0-9 .]*$/;
  const rgex = /^[a-zàâçéèêëîïôûùüÿñæœ .-_@./^,!#$()*%&+-]+$/i;
  return rgex.test(String(text).toLowerCase());
}

function generateMessage(lang: string) {
  return 'Not' + (lang === 'en' ? 'English' : lang === 'fn' ? 'French' :'Arabic') + 'Text';
}


@Directive({
  selector: '[app-lang][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: LanguageDirective, multi: true }
  ]
})
export class LanguageDirective implements OnInit, Validator {
  // tslint:disable-next-line:no-input-rename
  @Input('app-lang') lang: string;
  validator: ValidatorFn;

  constructor(private localization: LocalizationService) {
  }

  ngOnInit() {
    this.validator = validateLanguageFactory(this.lang);
  }

  validate(c: FormControl) {
    if (!c.value) { return null; }
    return this.validator(c);
  }
}
