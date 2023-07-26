import { StoreService } from 'src/app/shared/_services/store.service';
import { TranslateService } from '@ngx-translate/core';
import { VotingService } from '../../../committees/committee-details/votes/voting.service';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { debounceTime, switchMap, map } from 'rxjs/operators';
import { LookupService } from 'src/app/core/_services/lookup.service';
import {
  CommiteeMemberDTO,
  LookUpDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { mimeTypeValidator } from 'src/app/shared/mime-type';
import { AuthService } from 'src/app/auth/auth.service';
import { BrowserStorageService } from '../../_services/browser-storage.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker';
import { NgbDateService } from '../../_services/ngb-date.service';
@Component({
  selector: 'app-create-voting',
  templateUrl: './create-voting.component.html',
  styleUrls: ['./create-voting.component.scss'],
})
export class CreateVotingComponent implements OnInit {
  votingForm: FormGroup;
  currentLang;
  fromMom:boolean
  imagePreview: string;
  isVisible = false;
  isLoading = false;
  privacy = 'public';
  usersChanged$ = new BehaviorSubject('');
  users: LookUpDTO[] = [];
  lookupTypes = SettingControllers;
  files: File[] = [];
  multi = false;
  committeeId: string;
  meetingTopicId: number;
  meetingId: number;
  saving = false;
  @ViewChild('fileInput') fileInput: any;
  selectedDateType = DateType.Hijri;
  mindateHijiri:NgbDateStruct;
  minDateGorge:NgbDateStruct;
  maxdateHijiri:NgbDateStruct;
  maxDateGorge:NgbDateStruct;
  selectedDate: Date;
  constructor(
    private translateService: TranslateService,
    public lookupService: LookupService,
    private votingService: VotingService,
    private modalService: SharedModalService,
    private modal: NzModalService,
    private storeService: StoreService,
    private authService: AuthService,
    private browserService:BrowserStorageService,
    private dateFormatterService: DateFormatterService,
    private swagger:SwaggerClient,
    public dateService: NgbDateService,
  ) {}

  ngOnInit(): void {
    this.currentLang = this.translateService.currentLang;
   
    this.initForm();
    if (this.committeeId) this.initUsers();

    this.votingForm.controls['privacy'].valueChanges.subscribe((type) => {
      this.privacy = type;
    });
    this.votingForm.controls['multi'].valueChanges.subscribe((multi) => {
      this.multi = multi;
    });
    this.optionDefualtValue();
    this.getMinMaxDate();
  }

  initForm() {
    this.votingForm = new FormGroup({
      subject: new FormControl('', [Validators.required]),
      multi: new FormControl(false, [Validators.required]),
      options: new FormArray([], [Validators.required]),
      privacy: new FormControl('public'),
      selectedUsers: new FormControl([]),
      files: new FormControl(null, {}),
      dateselected: new FormControl( null,[Validators.required])
    });
  }

  get optionsControls() {
    return (this.votingForm.get('options') as FormArray).controls;
  }

  onAddOption() {
    (<FormArray>this.votingForm.get('options')).push(
      new FormGroup({
        name: new FormControl(null, Validators.required),
      })
    );
  }
 optionDefualtValue(){
  (<FormArray>this.votingForm.get('options')).push(
    new FormGroup({
      name: new FormControl('موافق', Validators.required),
    })
  );
  (<FormArray>this.votingForm.get('options')).push(
    new FormGroup({
      name: new FormControl('غير موافق', Validators.required),
    })
  );
  (<FormArray>this.votingForm.get('options')).push(
    new FormGroup({
      name: new FormControl('متحفظ', Validators.required),
    })
  );
 }
  onDeleteOption(index: number) {
    (<FormArray>this.votingForm.get('options')).removeAt(index);
  }

  onSelectFile(event: Event) {
    const selectedFiles = (event.target as HTMLInputElement).files;

    for (let i = 0; i < selectedFiles.length; i++) {
      this.files.push(selectedFiles[`${i}`]);
    }

    this.votingForm.patchValue({ files: this.files });
    this.fileInput.nativeElement.value = '';
    // this.votingForm.get('files').updateValueAndValidity();
  }

  removeSelectedFile(index) {
    this.files.splice(index, 1);
    this.votingForm.patchValue({ files: this.files });
    // this.votingForm.get('files').updateValueAndValidity();
  }

  onSave() {
    if (!this.votingForm.valid) return;
    this.saving = true;
    const subject = this.votingForm.value.subject;
    const multi = this.votingForm.value.multi;
    const uploadedFiles = this.votingForm.value.files;
    const options = this.votingForm.value.options.map((opt) => {
      return opt.name;
    });
    const selectedUsers = this.votingForm.value.selectedUsers;
    const isShared = this.privacy === 'public' ? true : false;

    this.saveVoting(
      uploadedFiles,
      subject,
      multi,
      isShared,
      selectedUsers,
      options
    );
  }

  initUsers() {
    this.usersChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getMembersLookup(
            20,
            0,
            this.committeeId,
            text ? text : undefined,
            []
          )
        )
      )
      .subscribe((res: CommiteeMemberDTO[]) => {
        this.users = res.filter(
          (user) => user.userId !== +this.authService.getUser().userId
        );

        this.isLoading = false;
      });
  }

  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
      default:
        break;
    }
  }

  close() {
    this.votingForm.reset();
    this.modalService.destroyModal();
  }

  createTplModal(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>): void {
    this.modal.create({
      nzTitle: tplTitle,
      nzContent: tplContent,
      nzClassName: 'my-modal',
      nzFooter: null,
      nzMaskClosable: true,
      nzClosable: true,
      nzWidth: 450,
    });
  }

  saveVoting(uploadedFiles, subject, multi, isShared, selectedUsers, options) {
    this.votingService
      .postVoting(
        uploadedFiles,
        subject,
        multi,
        isShared,
        selectedUsers,
        options,
        this.browserService.encryptCommitteId(this.committeeId),
        this.meetingTopicId,
        this.meetingId,
        this.selectedDate.toJSON()
      )
      .subscribe((res) => {
        if (res) {
          //insert new voting to voting list of Committee
          if (this.committeeId) {
            this.storeService.refreshVotings$.next({
              ...res,
              justInserted: true,
            });
            this.storeService.refreshTimelineItems$.next({
              ...res,
              type: 'Voting',
              justInserted: true,
            });
          }
          //insert new voting to voting list of Topic
          else if (this.meetingTopicId) {
            this.storeService.refreshTopicVotings$.next(res);
          }
          //insert voting to Meeting
          else if (this.meetingId) {
            this.storeService.refreshMeetingVoting$.next(res);
          }
        }
        this.saving = false;
        this.close();
      });
  }
  dateSelected(selectedDate: NgbDateStruct) {
    if (selectedDate?.year < 1900)
      selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
    this.selectedDate = new Date(
      Date.UTC(selectedDate?.year, selectedDate?.month - 1, selectedDate?.day)
    );
    this.votingForm.controls['dateselected'].patchValue(selectedDate);
  }
  getMinMaxDate(){
    this.mindateHijiri= {
      year:this.dateFormatterService.GetTodayHijri()?.year,
      month:this.dateFormatterService.GetTodayHijri()?.month,
      day:this.dateFormatterService.GetTodayHijri()?.day
    }
    this.minDateGorge = {
      year:this.dateFormatterService.GetTodayGregorian()?.year,
      month:this.dateFormatterService.GetTodayGregorian()?.month,
      day:this.dateFormatterService.GetTodayGregorian()?.day
    }
    if(this.fromMom){
      this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet('EndPeriodFormintuesOfMeeting').subscribe((res)=> {
        const systemValue = +res.systemSettingValue;
        const today = new Date();
        today.setDate(today.getDate() + systemValue)
        this.maxDateGorge = this.dateService.fromDate(today)
        this.maxdateHijiri = this.dateFormatterService.ToHijri(this.dateService.fromDate(today))
      })
    }
  }
}
