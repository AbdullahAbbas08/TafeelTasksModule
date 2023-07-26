import { Directive, OnInit, ElementRef, Renderer2, Output, EventEmitter } from '@angular/core';

import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Directive({
  selector: '[PasswordCriteria]'
})
export class PasswordCriteriaDirective implements OnInit {

  constructor(private swagger: SwaggerClient,
    private route: ActivatedRoute, 
    private Element: ElementRef,
    private rendrer: Renderer2) { }

  @Output() PasswordFlag = new EventEmitter();
  validate: boolean = false
  ngOnInit() {
    this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet('EnablePasswordCheck').subscribe(value => {
      if(value.systemSettingValue === '2'){
        this.validate = true;
      }
    })
    const input = this.Element.nativeElement as HTMLInputElement;
    const pathValue = this.route.snapshot.url[this.route.snapshot.url.length - 1];
    const routeValue = window.location.href;
    let userName: string;
    if (routeValue.includes('Users') && !routeValue.includes('add')) {
      userName = pathValue['path'];
    } else if(routeValue.includes('Users') && routeValue.includes('add')) {
      userName = '';
    }
    fromEvent(input, 'input')
    .pipe(debounceTime(500),
    distinctUntilChanged()
    ).subscribe((res:any) => {
      const PassInputValue = res.target.value;
        if(!this.validate) {
          if(PassInputValue.length<8){
            this.rendrer.addClass(input,'ng-invalid')
          }
         else{this.rendrer.removeClass(input,'ng-invalid')}
         this.PasswordFlag.emit(!this.validate);
        }
    });

  }


}
