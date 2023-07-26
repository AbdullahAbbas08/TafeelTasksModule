import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BehaviorSubject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { CommitteeService, ExtendedCommiteeDTODataSourceResult, ExtendedCommitteeDTO } from '../committee.service';
import {
  ExtendedCategoryDTO,
  ExtendedCommiteeTypeDTO,
  LookupService,
} from './../../core/_services/lookup.service';
import {
  OrganizationDetailsDTO,
  LookUpDTO,
  CommiteeDTO,
} from './../../core/_services/swagger/SwaggerClient.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-create-committee',
  templateUrl: './create-committee.component.html',
  styleUrls: ['./create-committee.component.scss'],
})
export class CreateCommitteeComponent implements OnInit {
  committeeForm: FormGroup;
  committeeTypes: ExtendedCommiteeTypeDTO[] = [];
  committeeCategories: ExtendedCategoryDTO[] = [];
  parentCommittees: ExtendedCommitteeDTO[] = [];
  users: LookUpDTO[] = [];
  committeSecrtary:LookUpDTO[]=[]
  organizations: OrganizationDetailsDTO[] = [];

  isLoading: boolean = false;
  selectedDateType = DateType.Hijri;
  typesChanged$ = new BehaviorSubject('');
  categoriesChanged$ = new BehaviorSubject('');
  parentCommitteesChanged$ = new BehaviorSubject('');
  organizationsChanged$ = new BehaviorSubject('');
  usersChanged$ = new BehaviorSubject('');
  CommitteeSecretaryChanged$ = new BehaviorSubject('')
  lookupTypes = SettingControllers;

  toggleFromCalendar: boolean = false;
  toggleToCalendar: boolean = false;
  selectedFromDate: Date;
  selectedToDate:Date;

  committeeId:any;
  currentLang: string;
  editableCommittee : CommiteeDTO = new CommiteeDTO();
  currentHeadNameAr:string;
  currentHeadNameEn:string;
  committeSecretaryNameAr:string;
  committeSecretaryNameEn:string;
  organizationNameAr:string;
  organizationNameEn:string;
  maxdateHijiri:NgbDateStruct;
  maxDateGorge:NgbDateStruct;
  mindateHijiri:NgbDateStruct;
  minDateGorge:NgbDateStruct;
  isChecked:boolean = false;
  selectedHeadUnit:number;
  selectedCommitteSecertary:number;
  constructor(
    private committeeService: CommitteeService,
    private formBuilder: FormBuilder,
    private lookupService: LookupService,
    private message: NzMessageService,
    public translateService: TranslateService,
    public dateService: NgbDateService,
    private storeService: StoreService,
    private modalService: SharedModalService,
    private notificationService: NzNotificationService,
    private router: Router,
    private dateFormatterService: DateFormatterService,
    private browserService: BrowserStorageService,
  ) {}

  ngOnInit(): void {
    this.getCommittee()
    this.initCommitteeForm();
    this.setValidity()
    // init lookups ...
    this.initTypes();
    this.initCategories();
    this.initParentCommittees();
    this.initOrganizations();
    this.initUsers();
    this.initCommitteSecatary();
    this.currentLang = this.translateService.currentLang;
    this.getMinMaxDate();
    this.storeService.refreshCommittees$.next(null);

  }

  initCommitteeForm() {
    this.committeeForm = this.formBuilder.group({
      name: ['' , [Validators.required]],
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      commiteeTypeId: ['', [Validators.required]],
      categoryId: ['', [Validators.required]],
      parentCommiteeId: [''],
      departmentLinkId: [''], // required when enableTransactions or enableDecisions is true
      dateFrom: ['',[Validators.required]],
      dateTo: ['',[Validators.required]],
      enableTransactions: [false],
      enableDecisions: [false],
      isSecrete: [false],
      currenHeadUnitId: ['', [Validators.required]],
      committeeSecretary:['',[Validators.required]]
    });
  }
  setValidity(){
    if(this.committeeId){
      this.form_controls['dateFrom'].clearValidators();
      this.form_controls['dateTo'].clearValidators();
    }
  }
  populateFormData(){
    this.committeeForm.patchValue({
      name: (this.editableCommittee.name ? this.editableCommittee.name : ''),
      title: (this.editableCommittee.title ? this.editableCommittee.title :''),
      description: (this.editableCommittee.description ? this.editableCommittee.description :''),
      commiteeTypeId: (this.editableCommittee.commiteeTypeId ? this.editableCommittee.commiteeTypeId :''),
      categoryId: (this.editableCommittee.categoryId ? this.editableCommittee.categoryId :''),
      parentCommiteeId: (this.editableCommittee.parentCommiteeId ? this.editableCommittee.parentCommiteeId :''),
      departmentLinkId: (this.editableCommittee.departmentLinkId ? this.editableCommittee.departmentLinkId :''), // required when enableTransactions or enableDecisions is true
      enableTransactions: (this.editableCommittee.enableTransactions == true? this.editableCommittee.enableTransactions :false),
      enableDecisions: (this.editableCommittee.enableDecisions == true ? this.editableCommittee.enableDecisions :false),
      isSecrete: (this.editableCommittee.isSecrete == true ? this.editableCommittee.isSecrete :false),
      currenHeadUnitId: (this.editableCommittee.currenHeadUnitId ? this.editableCommittee.currenHeadUnitId : ''),
      committeeSecretary:(this.editableCommittee.committeeSecretaryId ? this.editableCommittee.committeeSecretaryId : '')
    });
   this.currentHeadNameAr = this.editableCommittee.currenHeadUnit?.fullNameAr;
   this.currentHeadNameEn = this.editableCommittee.currenHeadUnit?.fullNameEn;
   this.committeSecretaryNameAr = this.editableCommittee.committeeSecretary.fullNameAr;
   this.committeSecretaryNameEn  = this.editableCommittee.committeeSecretary.fullNameEn;
   this.organizationNameAr = this.editableCommittee.departmentLink?.organizationNameAr;
   this.organizationNameEn = this.editableCommittee.departmentLink?.organizationNameEn;
  }

  // getting form controls
  get form_controls() {
    return this.committeeForm.controls;
  }

  getCommittee(){
   if (this.committeeId){
    this.committeeService.getCommitteeDetails(this.browserService.encrypteString(this.committeeId)).subscribe(res =>{
      this.editableCommittee = res;
      this.selectedHeadUnit = res.currenHeadUnitId;
      this.selectedCommitteSecertary = res.committeeSecretaryId
      this.populateFormData()
    })
   }
  }

  initTypes() {
    this.typesChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getTypes(
            20,
            0,
            false,
            text
              ? [
                  {
                    field: 'commiteeTypeNameAr',
                    operator: 'contains',
                    value: text,
                  },
                  {
                    field: 'commiteeTypeNameEn',
                    operator: 'contains',
                    value: text,
                  },
                ]
              : undefined
          )
        )
      )
      .subscribe((res: ExtendedCommiteeTypeDTO[]) => {
        this.committeeTypes = res;
        this.isLoading = false;
      });
  }

  initCategories() {
    this.categoriesChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getCategories(
            20,
            0,
            false,
            text
              ? [
                  {
                    field: 'categoryNameAr',
                    operator: 'contains',
                    value: text,
                  },
                  {
                    field: 'categoryNameEn',
                    operator: 'contains',
                    value: text,
                  },
                ]
              : undefined
          )
        )
      )
      .subscribe((res: ExtendedCategoryDTO[]) => {
        this.committeeCategories = res;
        this.isLoading = false;
      });
  }
  // initParentCommittees() {
  //   const encryptedId:string = this.browserService.encrypteString(`${this.committeeId}_${this.browserService.getUserRoleId()}`)
  //   this.parentCommitteesChanged$
  //     .asObservable()
  //     .pipe(
  //       debounceTime(500),
  //       switchMap((text: string) =>
  //         this.lookupService.getParentCommittees(
  //           20,
  //           0,
  //           false,
  //           text
  //             ? [{ field: 'name', operator: 'contains', value: text }]
  //             : undefined,
  //             encryptedId
  //         )
  //       )
  //     )
  //     .subscribe((res: LookUpDTO[]) => {
  //       this.parentCommittees = res;
  //       if(this.committeeId){
  //       this.parentCommittees =  res.filter((commitee) => {
  //          return commitee.id != this.editableCommittee.commiteeId
  //         })
  //       }
  //       this.isLoading = false;
  //     });
  // }
  initParentCommittees(){
    this.parentCommitteesChanged$
    .asObservable()
    .pipe(
      debounceTime(500),
      switchMap((text:string) => {
        return this.committeeService.getCommittees(10,0,text
          ? [{ field: 'name', operator: 'contains', value: text }]
          : undefined,
          undefined)
      })
    ).subscribe((res: ExtendedCommiteeDTODataSourceResult) => {
        this.parentCommittees = res.data;
        this.isLoading = false;
    })
  }
  initOrganizations() {
    this.organizationsChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getOrganizations(
            20,
            0,
            false,
            text
              ? [
                  {
                    field: 'organizationNameAr',
                    operator: 'contains',
                    value: text,
                  },
                  {
                    field: 'organizationNameEn',
                    operator: 'contains',
                    value: text,
                  },
                ]
              : undefined
          )
        )
      )
      .subscribe((res: OrganizationDetailsDTO[]) => {
        this.organizations = res;
        this.isLoading = false;
      });
  }

  initUsers() {
    this.usersChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getUsersLookup(
            20,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,undefined
          )
        )
      )
      .subscribe((res: LookUpDTO[]) => {
        this.users = res;
        this.isLoading = false;
      });
  }
  initCommitteSecatary() {
    this.CommitteeSecretaryChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getUsersLookup(
            20,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,undefined
          )
        )
      )
      .subscribe((res: LookUpDTO[]) => {
        this.committeSecrtary = res
        this.isLoading = false;
      });
  }
  getSelectedHeadUnit(event){
    this.selectedHeadUnit = event ? this.editableCommittee.currenHeadUnitId : event;
    if(this.committeeForm.get('currenHeadUnitId').value === this.committeeForm.get('committeeSecretary').value){
      this.committeeForm.get('currenHeadUnitId').reset();
      this.translateService
      .get('ThisUserAlreadyExistAscommitteeSecretary')
      .subscribe((translateValue) =>
        this.notificationService.error(translateValue, '')
      );
      this.currentHeadNameAr = undefined;
      this.currentHeadNameEn = undefined;
      this.selectedHeadUnit = undefined
    }
 }
 removeSelectedSecertary(event){
  this.selectedCommitteSecertary = event ? this.editableCommittee.committeeSecretaryId : event
   if(this.committeeForm.get('currenHeadUnitId').value === this.committeeForm.get('committeeSecretary').value) {
     this.committeeForm.get('committeeSecretary').reset();
     this.translateService
     .get('ThisUserAlreadyExistAssCommitteHeadUnit')
     .subscribe((translateValue) =>
       this.notificationService.error(translateValue, '')
     );
     this.committeSecretaryNameAr = undefined;
     this.committeSecretaryNameEn = undefined;
     this.selectedCommitteSecertary = undefined
   }
 }
  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.TYPE:
        this.typesChanged$.next(value);
        break;
      case SettingControllers.CATEGORY:
        this.categoriesChanged$.next(value);
        break;
      case SettingControllers.PARENTCOMMITTEE:
        this.parentCommitteesChanged$.next(value);
        break;
      case SettingControllers.ORGANIZATION:
        this.organizationsChanged$.next(value);
        break;
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
      case SettingControllers.COMMITTEESECRETARY:
        this.CommitteeSecretaryChanged$.next(value)
        break;
      default:
        break;
    }
  }

  close() {
    // this.committeeForm.reset();
    this.modalService.closeDrawer();
  }

  changeCheckStatus() {
    if (
      this.form_controls['enableDecisions'].value ||
      this.form_controls['enableTransactions'].value
    ) {
      this.form_controls['departmentLinkId'].setValidators(Validators.required);
    } else {
      this.form_controls['departmentLinkId'].clearValidators();
    }
    this.form_controls['departmentLinkId'].updateValueAndValidity();
  }

  submitCommitteeData() {
    if (this.committeeForm && !this.committeeForm.valid) {
      this.translateService
        .get('FormNotValid')
        .subscribe((text) => this.message.error(text));
      return;
    }
    if (this.committeeId){
      this.committeeService
        .editCommitteeData(
          this.committeeForm.value,
          this.browserService.encrypteString(`${this.committeeId}_${this.browserService.getUserRoleId()}`)
        )
        .subscribe((res) => {
          if (res && res.length) {
            //edit new committee to committee list then reset and close ..
            this.translateService.get('committeUpdated').subscribe((translateValue) => {
              this.notificationService.success(translateValue,'');
            })
            this.editableCommittee = res[0];
            this.populateFormData()
            this.committeeService.editedCommittee.next(res[0])
            this.close();
          }else {
            this.translateService
            .get('CommitteUpdatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
          }
        });
    }else {
      this.committeeService
        .saveCommitteeData(
          this.committeeForm.value,
          this.selectedFromDate,
          this.selectedToDate
          )
        .subscribe((res) => {
          if (res && res.length) {
            //insert new committee to committee list then reset and close ..
            this.storeService.refreshCommittees$.next(res[0]);
            this.close();
          }
        });
    }
  }

  dateFromSelected(selectedDate: NgbDateStruct) {
    if (selectedDate?.year < 1900)
      selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
    this.selectedFromDate = new Date(
      Date.UTC(selectedDate?.year, selectedDate?.month - 1, selectedDate?.day)
    );

    this.form_controls['dateFrom'].patchValue(selectedDate);
  }

  dateToSelected(selectedDate: NgbDateStruct){
    if (selectedDate?.year < 1900)
    selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
  this.selectedToDate = new Date(
    Date.UTC(selectedDate?.year, selectedDate?.month - 1, selectedDate?.day)
  );
    this.form_controls['dateTo'].patchValue(selectedDate);
  }
  getMinMaxDate(){
    this.maxdateHijiri = this.dateFormatterService.GetTodayHijri();
    this.maxDateGorge = this.dateFormatterService.GetTodayGregorian();
    this.mindateHijiri= {
      year:this.dateFormatterService.GetTodayHijri()?.year,
      month:this.dateFormatterService.GetTodayHijri()?.month,
      day:this.dateFormatterService.GetTodayHijri()?.day + 1
    }
    this.minDateGorge = {
      year:this.dateFormatterService.GetTodayGregorian()?.year,
      month:this.dateFormatterService.GetTodayGregorian()?.month,
      day:this.dateFormatterService.GetTodayGregorian()?.day + 1
    }
  }
  checkDateValue(event){
     if(event === true){
    this.isChecked != this.isChecked;
    this.selectedFromDate = undefined;
    this.selectedToDate  = undefined;
    this.committeeForm.get("dateFrom").clearValidators();
    this.committeeForm.get('dateFrom').updateValueAndValidity();
    this.committeeForm.get("dateTo").clearValidators();
    this.committeeForm.get('dateTo').updateValueAndValidity();
     } else  {
      this.isChecked != this.isChecked;
      this.committeeForm.get("dateFrom").setValidators(Validators.required);
      this.committeeForm.get('dateFrom').updateValueAndValidity();
      this.committeeForm.get("dateTo").setValidators(Validators.required);
      this.committeeForm.get('dateTo').updateValueAndValidity();
     }
  }
}
