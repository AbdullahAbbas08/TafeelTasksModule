import { Directive, AfterViewInit, ElementRef, Input, HostListener, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Directive({
  selector: '[appTrimStartEndSpace]',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TrimStartEndSpaceDirective),
      multi: true
    }
  ]
})
export class TrimStartEndSpaceDirective implements AfterViewInit, ControlValueAccessor {
  @Input() enableTrim: boolean = true;
  constructor(public el: ElementRef) { 
  }

  public onChange: any = (value) => { }
  public onTouched: any = () => { }
  writeValue(val: any): void {
    if (val) this.el.nativeElement.value = val;
  }

  registerOnChange(fn: any): void { this.onChange = fn; }
  registerOnTouched(fn: any): void { this.onTouched = fn; }

  ngAfterViewInit() {
    this.el.nativeElement.addEventListener('keypress', this.disableBehavior);
  }

  private disableBehavior(e: any) {
    if (e.keyCode === 32) {
      if (!e.target.value) e.preventDefault();
    }
  }

  @HostListener("blur")
  validateInputData() {
    let value = this.el.nativeElement.value;
    if (value) {
      value = value.trim();
    }
    this.onChange(value);
    this.writeValue(value);
  }
}
