import { Component, OnInit } from '@angular/core';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
import { UserService } from '../user.service';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-delgate-user',
  templateUrl: './delgate-user.component.html',
  styleUrls: ['./delgate-user.component.scss']
})
export class DelgateUserComponent implements OnInit {
  delegateForm: FormGroup;
  committeId:string;
  userId:number;
  committeMemberId:number
  selectedEndDate: Date;
  selectedDateType = DateType.Hijri;
  mindateHijiri:NgbDateStruct;
  minDateGorge:NgbDateStruct;
  isChecked:boolean = false;
  currentLang: string;
  constructor(private modalService: SharedModalService,
    private formBuilder: FormBuilder,
    private dateFormatterService: DateFormatterService,
    public dateService: NgbDateService,
    private _UserServices:UserService,
    public translate: TranslateService,
    private notificationService: NzNotificationService,
    private browserStorage:BrowserStorageService
    ) { }

  ngOnInit(): void {
    this.initdelegateForm()
    this.currentLang = this.translate.currentLang;
    this.getMinDate();
  }
  initdelegateForm(){
    this.delegateForm = this.formBuilder.group({
      delegateReason: ['' , [Validators.required]],
    })
  }
  close(){
    this.modalService.destroyModal();
   }
  delegateUser(){
    this._UserServices.delegateUser(
      this.userId,
      this.browserStorage.encrypteString(`${this.committeId}_${this.browserStorage.getUserRoleId()}`),
      this.committeMemberId,
      this.delegateForm.controls['delegateReason'].value,
      this.selectedEndDate).subscribe((res)=> {
      if(res){
        this._UserServices.userDelegate.next(res);
        this.translate
        .get('DelegationDone')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
        this.close();
      }
    })
    
  }
  dateSelected(selectedDate: NgbDateStruct){
    
    if (selectedDate.year < 1900)
    selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
    this.selectedEndDate = new Date(
    Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
  );
  }
  getMinDate(){
    this.mindateHijiri = {
      year:this.dateFormatterService.GetTodayHijri().year,
      month:this.dateFormatterService.GetTodayHijri().month,
      day:this.dateFormatterService.GetTodayHijri().day + 1
    }
    this.minDateGorge = {
      year:this.dateFormatterService.GetTodayGregorian().year,
      month:this.dateFormatterService.GetTodayGregorian().month,
      day:this.dateFormatterService.GetTodayGregorian().day + 1
    }
  }
}
