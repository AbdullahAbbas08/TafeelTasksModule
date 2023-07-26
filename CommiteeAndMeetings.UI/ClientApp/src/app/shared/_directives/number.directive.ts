import { Directive, ElementRef, HostListener } from '@angular/core';

var numericValue = 0;

@Directive({
  selector: '[app-number][ngModel]'
})
export class NumberDirective {

  constructor(private el: ElementRef) { }

  @HostListener('keydown', ['$event']) onkeydown(event) {
    numericValue = Number((<HTMLInputElement>event.target).value);
    const e = <KeyboardEvent>event;
    const text = numericValue.toString();
    if ([46, 8, 9, 27, 13].indexOf(e.keyCode) !== -1 || //110 , 190 => BTN
      // Allow: Ctrl+A
      (e.keyCode === 65 && (e.ctrlKey || e.metaKey)) ||
      // Allow: Ctrl+C
      (e.keyCode === 67 && (e.ctrlKey || e.metaKey)) ||
      // Allow: Ctrl+V
      (e.keyCode === 86 && (e.ctrlKey || e.metaKey)) ||
      // Allow: Ctrl+X
      (e.keyCode === 88 && (e.ctrlKey || e.metaKey)) ||
      // Allow: home, end, left, right
      (e.keyCode >= 35 && e.keyCode <= 39)) {
      // let it happen, don't do anything
      return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) ) {
      e.preventDefault();
    }
  }

  // copy-paste case
  @HostListener('change', ['$event']) onchange(event) {
    const text = (<HTMLInputElement>event.target).value;
    if (this.isText(text)) {
      (<HTMLInputElement>event.target).value = numericValue.toString();
    }
  }

  isText(text): boolean {
    const rgex1 = /^[0-9 ]*[a-zA-Z_]+[a-zA-Z0-9 ]*$/;
    const rgex2 = /^[0-9 ]*[\u0600-\u06FF_]+[\u0600-\u06FF0-9 ]*$/;
    return (rgex1.test(String(text).toLowerCase()) || rgex2.test(String(text).toLowerCase()));
  }

}
