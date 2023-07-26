import {
  Component,
  OnInit,
  Input,
  ViewChild,
  AfterViewInit,
  OnDestroy
} from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { BehaviorSubject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import {
  ComiteeTaskCategoryDTO,
  CommiteeMemberDTO,
  CommiteeTaskDTO,
  CommiteetaskMultiMissionDTO,
  CommiteeTaskMultiMissionUserDTO,
  CommitteeTaskAttachmentDTO,
  GroupDto,
  LookUpDTO,
  UserDetailsDTO,
  UserTaskDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker';
import { AuthService } from 'src/app/auth/auth.service';
import { CommitteeService } from 'src/app/committees/committee.service';
import { TasksService } from '../../tasks.service';
import { Subscription } from 'rxjs';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
@Component({
  selector: 'app-create-task',
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.scss'],
})
export class CreateTaskComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() task: CommiteeTaskDTO;
  @ViewChild('datePicker') datePicker;
  taskForm: FormGroup;
  options: string[];
  toggleFromCalendar: boolean = false;
  toggleToCalendar: boolean = false;
  selectedEndDate: Date;
  taskSelectedStartDate: Date;
  isLoading = false;
  users: CommiteeMemberDTO[] | LookUpDTO[] = [];
  assistantUsers: LookUpDTO[] = []
  lookupTypes = SettingControllers;
  committeeId: string;
  meetingId: any
  usersChanged$ = new BehaviorSubject('');
  usersAssistantChanged$ = new BehaviorSubject('');
  groupsChanged$ = new BehaviorSubject('');
  selectedDateType = this.translateService.currentLang === 'ar' ? DateType.Hijri : DateType.Gregorian;
  currentLang: string;
  minHijri: NgbDateStruct;
  minGreg: NgbDateStruct;
  minEndHijri: NgbDateStruct;
  minEndGreg: NgbDateStruct;
  MinSubTaskEndHijri: NgbDateStruct;
  MinSubTaskEndGreg: NgbDateStruct;
  MaxSubTaskEndHijri: NgbDateStruct;
  MaxSubTaskEndGreg: NgbDateStruct;
  saving = false;
  selectedStartDate: NgbDateStruct;
  selectedDate: NgbDateStruct;
  subTaskSelectedDate: NgbDateStruct[] = [];
  taskCategories: ComiteeTaskCategoryDTO[] = [];
  mainAssigendUser: string
  files: File[] = [];
  subscription: Subscription;
  @ViewChild('fileInput') fileInput: any;
  existingAttachments?: CommitteeTaskAttachmentDTO[] = [];
  groupList: GroupDto[] = [];
  checkedDate: boolean = true;
  disabledBtn: boolean = false;
  hideSaveButton: boolean = true
  constructor(
    public formBuilder: FormBuilder,
    public lookupService: LookupService,
    public dateService: NgbDateService,
    private dateFormatterService: DateFormatterService,
    public storeService: StoreService,
    public modalService: SharedModalService,
    public translateService: TranslateService,
    public committeeService: CommitteeService,
    public message: NzMessageService,
    private taskService: TasksService,
    private authService: AuthService,
    private browserService: BrowserStorageService,
    private notificationService: NzNotificationService,
  ) { }

  ngAfterViewInit(): void {
    this.editMode();
  }

  ngOnInit(): void {
    this.initTaskForm();
    if (this.committeeId) {
      this.initUsersForCommittee();
    } else if (this.meetingId) {
      this.initGroups();
      this.initMeetingUsers();
    }
    else {
      this.initUsers();
      this.initAssistantUsers();
      this.initGroups()
    }

    this.currentLang = this.translateService.currentLang;
    this.setMinAllowed();
    this.getTaskCategories();
  }
  ngOnDestroy() {
    if (this.subscription) this.subscription.unsubscribe();
  }
  isNotGroupSelected(value) {
    if (this.formControls['groups'].value !== null) {
      return this.formControls['groups'].value.indexOf(value) === -1
    }
  }
  isNotAssisstantSelected(value, index): boolean {
    if ((<FormArray>this.taskForm.get('multiTasks')).controls[index].value['subTaskAssistantUsers'] !== null) {
      return (<FormArray>this.taskForm.get('multiTasks')).controls[index].value['subTaskAssistantUsers'].indexOf(value) === -1
    }
  }

  initTaskForm() {
    this.taskForm = this.formBuilder.group({
      title: [null, [Validators.required]],
      details: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      startDate: [null, [Validators.required]],
      endTime: new FormControl(null, [Validators.required]),
      category: [null, [Validators.required]],
      mainAssinedUserId: [null, [Validators.required]],
      // assistantUsers: [[], []],
      groups: [[], []],
      isShared: [false, []],
      files: [null, []],
      emailNotify: [false, []],
      appNotify: [false, []],
      smsNotify: [false, []],
      multiTasks: new FormArray([], []),
    });
  }

  onAddOption() {
    this.hideSaveButton = false
    if (this.task) {
      (<FormArray>this.taskForm.get('multiTasks')).push(
        new FormGroup({
          name: new FormControl(null, [Validators.required]),
          state: new FormControl(false),
          subTaskEnd: new FormControl(null, [Validators.required]),
          subTaskAssistantUsers: new FormControl([], [Validators.required]),
          missionId: new FormControl(null, []),
        })
      )
    } else {
      (<FormArray>this.taskForm.get('multiTasks')).push(
        new FormGroup({
          name: new FormControl(null, [Validators.required]),
          state: new FormControl(false),
          subTaskEnd: new FormControl(null, [Validators.required]),
          subTaskAssistantUsers: new FormControl([], [Validators.required]),
        })
      )
    }
  }

  onDeleteOption(index: number) {
    this.hideSaveButton = true;
    (<FormArray>this.taskForm.get('multiTasks')).removeAt(index);
  }

  get multiTasksControls() {
    return (this.taskForm.get('multiTasks') as FormArray).controls;
  }
  editMode() {
    if (this.task) {
      if (this.task.taskGroups.length > 0) {
        this.task.taskGroups.map((val) => {
          this.groupList.push(new GroupDto({
            groupId: val.groupId,
            groupNameAr: val.group.groupNameAr,
            groupNameEn: val.group.groupNameEn
          }))
        })
      }
      this.formControls['endDate'].patchValue(this.task.endDate);
      this.formControls['startDate'].patchValue(this.task.startDate);
      this.formControls['endTime'].patchValue(this.dateService.getTimeZoneOffset(this.task.endDate));
      this.formControls['category'].patchValue(
        this.task.comiteeTaskCategory?.comiteeTaskCategoryId
      );
      this.formControls['details'].patchValue(this.task.taskDetails)
      this.formControls['title'].patchValue(this.task.title);
      this.formControls['mainAssinedUserId'].patchValue(
        this.task.mainAssinedUserId
      );
      if (!this.users.some(x => x.id == this.task.mainAssinedUser.userId)) {
        this.users.push(new LookUpDTO({ id: this.task.mainAssinedUser.userId, name: this.task.mainAssinedUser.fullNameAr }))
      }
      // this.formControls['assistantUsers'].patchValue(
      //   this.task.assistantUsers.map((user) => user.userId)
      // );
      this.formControls['groups'].patchValue(
        this.task.taskGroups.map((group) => group.groupId)
      );
      this.task.multiMission.map((y, index) => {
        // if(!this.assistantUsers.some(x => x.id == y.userId)){
        //   this.assistantUsers.push(new LookUpDTO({id:y.userId,name:y.user.fullNameAr}))
        // }
        y.commiteeTaskMultiMissionUserDTOs.map((z) => {
          if (!this.assistantUsers.some(x => x.id == z.userId)) {
            this.assistantUsers.push(new LookUpDTO({ id: z.userId, name: z.userDetailsDto.fullNameAr }))
          }
        })
      })
      this.selectedDate = this.translateService.currentLang === 'ar' ? this.dateFormatterService.ToHijri(this.getNgbDate(this.dateService.getTimeZoneOffset(this.task.endDate))) : this.getNgbDate(this.task.endDate);
      this.selectedStartDate = this.translateService.currentLang === 'ar' ? this.dateFormatterService.ToHijri(this.getNgbDate(this.dateService.getTimeZoneOffset(this.task.startDate))) : this.getNgbDate(this.dateService.getTimeZoneOffset(this.task.startDate))
      this.selectedEndDate = this.dateService.getTimeZoneOffset(this.task.endDate);
      this.taskSelectedStartDate = this.dateService.getTimeZoneOffset(this.task.startDate)
      this.existingAttachments = this.task.taskAttachments ?? [];
      this.formControls['isShared'].patchValue(this.task.isShared);
      this.updateMultiTasks();
    }
  }
  updateMultiTasks() {
    this.subTaskSelectedDate = []
    this.task.multiMission.forEach((mission, index) => {
      (<FormArray>this.taskForm.get('multiTasks')).push(
        new FormGroup({
          name: new FormControl(mission.name, [Validators.required]),
          state: new FormControl(mission.state),
          subTaskEnd: new FormControl(this.dateService.fromDate(this.dateService.getTimeZoneOffset(mission.endDateMultiMission)), [Validators.required]),
          subTaskAssistantUsers: new FormControl(mission.commiteeTaskMultiMissionUserDTOs.map((user) => user.userId), [Validators.required]),
          missionId: new FormControl(mission.commiteeTaskMultiMissionId),
        })
      );
      this.subTaskSelectedDate.push(this.translateService.currentLang === 'ar' ? this.dateFormatterService.ToHijri(this.getNgbDate(this.dateService.getTimeZoneOffset(mission.endDateMultiMission))) : this.getNgbDate(this.dateService.getTimeZoneOffset(mission.endDateMultiMission)));
      this.MinSubTaskEndGreg = this.dateService.fromDate(this.task.startDate);
      this.MinSubTaskEndHijri = this.dateFormatterService.ToHijri(this.dateService.fromDate(this.task.startDate))
      this.MaxSubTaskEndGreg = this.dateService.fromDate(this.task.endDate)
      this.MaxSubTaskEndHijri = this.dateFormatterService.ToHijri(this.dateService.fromDate(this.task.endDate))
    });
  }

  // getting form controls
  get formControls() {
    return this.taskForm.controls;
  }

  initUsersForCommittee() {
    this.usersChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.lookupService.getMembersLookup(
            30,
            0,
            this.committeeId,
            text ? text : undefined,
            []
          );
        })
      )
      .subscribe((res: CommiteeMemberDTO[]) => {
        this.users = res.filter((user) => {
          return user.active;
        });
        this.isLoading = false;
      });
  }

  initUsers() {
    this.usersChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.lookupService.getUsersLookup(
            20,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,
            undefined
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.users = data;
        this.isLoading = false;
      });
  }
  initAssistantUsers() {
    this.usersAssistantChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.lookupService.getUsersLookup(
            20,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,
            undefined
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.assistantUsers = data;
        this.isLoading = false;
      });
  }
  initMeetingUsers() {
    this.taskService.getAllMeetingUsers(this.meetingId).subscribe((res) => {
      res.forEach((x) => {
        this.users.push(new LookUpDTO({
          id: x.userId,
          name: this.translateService.currentLang === 'ar' ? x.fullNameAr : x.fullNameEn
        }))
      })
    })
  }
  initGroups() {
    this.groupsChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.taskService.getAllGroups(
            20,
            0,
            false,
            text
              ? [{ field: 'groupNameAr', operator: 'contains', value: text },
              { field: 'groupNameEn', operator: 'contains', value: text }]
              : undefined,
            this.authService.getUser().userId
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.groupList = data;
        this.isLoading = false;
      });
  }

  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
      case SettingControllers.ASSISTANTUSERS:
        this.usersAssistantChanged$.next(value);
        break;
      case SettingControllers.GROUPS:
        this.groupsChanged$.next(value);
        break;
      default:
        break;
    }
  }
  onAddSubTask() {
    if ((<FormArray>this.taskForm.get('multiTasks')).invalid) {
      this.translateService
        .get('pleasefillallinputs')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
    } else {
      var multiTasks = new CommiteetaskMultiMissionDTO({
        name: this.taskForm.value.multiTasks[this.taskForm.value.multiTasks.length - 1].name,
        state: this.taskForm.value.multiTasks[this.taskForm.value.multiTasks.length - 1].state,
        endDateMultiMission: this.dateService.toDate(this.taskForm.value.multiTasks[this.taskForm.value.multiTasks.length - 1].subTaskEnd),
        commiteeTaskMultiMissionUserDTOs: this.taskForm.value.multiTasks[this.taskForm.value.multiTasks.length - 1]['subTaskAssistantUsers'].map((id, i) => { return new CommiteeTaskMultiMissionUserDTO({ userId: id }) })
      })
      this.taskService.insertNewSubTask(this.browserService.encrypteString(this.task.commiteeTaskId), multiTasks).subscribe((res) => {
        if (res) {
          this.hideSaveButton = true;
          this.multiTasksControls.map((el: FormGroup, index: number) => {
            if (index == this.multiTasksControls.length - 1) {
              el.controls.missionId.patchValue(res.commiteeTaskMultiMissionId)
            }
          })
        }
      })
    }
  }
  onFocus() { }

  submitTaskData() {
    if (this.taskForm && !this.taskForm.valid) return;

    this.saving = true;

    if (this.task) {
      var multiTasks = this.taskForm.value.multiTasks.map((task) => {
        return new CommiteetaskMultiMissionDTO({ name: task.name, state: task.state, endDateMultiMission: this.dateService.toDate(task.subTaskEnd), commiteeTaskMultiMissionId: task.missionId, commiteeTaskMultiMissionUserDTOs: task['subTaskAssistantUsers'].map((id, i) => { return new CommiteeTaskMultiMissionUserDTO({ userId: id, commiteeTaskMultiMissionId: task.missionId }) }) });
      });
    } else {
      var multiTasks = this.taskForm.value.multiTasks.map((task) => {
        return new CommiteetaskMultiMissionDTO({ name: task.name, state: task.state, endDateMultiMission: this.dateService.toDate(task.subTaskEnd), commiteeTaskMultiMissionUserDTOs: task['subTaskAssistantUsers'].map((id, i) => { return new CommiteeTaskMultiMissionUserDTO({ userId: id }) }) });
      });
    }
    this.subTaskSelectedDate = []
    this.taskService
      .saveCommitteeTask(
        this.taskForm.value,
        this.getStartDateTime(),
        this.getEndDateTime(),
        this.committeeId,
        this.task?.commiteeTaskId,
        this.existingAttachments,
        multiTasks,
        this.meetingId
      )
      .subscribe((res) => {
        if (res && res.length) {
          this.task = res[0];

          if (this.taskForm.value.files?.length > 0) {
            this.taskService
              .postAttachments(
                this.taskForm.value.files,
                this.task.commiteeTaskId
              )
              .subscribe(() => {
                this.storeService.refresh$.next(res[0]);
                this.saving = false;
                this.close();
              });
          } else {
            this.storeService.refresh$.next(res[0]);
            this.saving = false;
            this.close();
          }
        }
      });
  }

  close() {
    this.taskForm.reset();
    this.modalService.destroyModal();
  }

  dateSelected(selectedDate: NgbDateStruct) {
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.selectedEndDate = new Date(
        Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
      );
      this.formControls['endDate'].patchValue(selectedDate);
      this.checkedDate = !this.checkedDate;
    } else {
      this.formControls['endDate'].reset();
      this.multiTasksControls.map((el: FormGroup, index: number) => {
        el.controls.subTaskEnd.reset()
      })
    }
    this.subTaskSelectedDate = [];
    this.selectedDate = undefined
    this.MaxSubTaskEndGreg = selectedDate
    this.MaxSubTaskEndHijri = this.dateFormatterService.ToHijri(selectedDate)
  }
  subTaskDateSelected(selectedDate: NgbDateStruct, i) {
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.multiTasksControls.map((el: FormGroup, index: number) => {
        if (index === i) {
          el.controls.subTaskEnd.patchValue(selectedDate)
        }
      })
    } else {
      this.multiTasksControls.map((el: FormGroup, index: number) => {
        if (index === i) {
          el.controls.subTaskEnd.reset()
        }
      })
    }
  }
  dateStartSelected(selectedDate: NgbDateStruct) {
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.taskSelectedStartDate = new Date(
        Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
      );

      this.formControls['startDate'].patchValue(selectedDate);
    }
    this.selectedDate = undefined
    this.subTaskSelectedDate = []
    this.minEndGreg = selectedDate;
    this.minEndHijri = this.dateFormatterService.ToHijri(selectedDate);
    this.MinSubTaskEndGreg = selectedDate;
    this.MinSubTaskEndHijri = this.dateFormatterService.ToHijri(selectedDate);
  }
  setMinAllowed() {
    this.minGreg = this.dateFormatterService.GetTodayGregorian();
    this.minHijri = this.dateFormatterService.GetTodayHijri();
    this.minEndGreg = this.dateFormatterService.GetTodayGregorian();
    this.minEndHijri = this.dateFormatterService.GetTodayHijri();
    this.MinSubTaskEndGreg = this.dateFormatterService.GetTodayGregorian();
    this.MinSubTaskEndHijri = this.dateFormatterService.GetTodayHijri()
  }

  onSelectFile(event: Event) {
    const selectedFiles = (event.target as HTMLInputElement).files;

    for (let i = 0; i < selectedFiles.length; i++) {
      this.files.push(selectedFiles[`${i}`]);
    }

    this.taskForm.patchValue({ files: this.files });
    this.fileInput.nativeElement.value = '';
  }

  removeSelectedFile(index) {
    this.files.splice(index, 1);
    this.taskForm.patchValue({ files: this.files });
  }

  removeExistingFile(index) {
    this.existingAttachments.splice(index, 1);
  }

  getEndDateTime() {
    let endTime = this.taskForm.value.endTime;

    let taskEndDateTime: Date = new Date(Date.UTC(this.selectedEndDate.getFullYear(),
      this.selectedEndDate.getMonth(),
      this.selectedEndDate.getDate(),
      endTime.getHours(),
      endTime.getMinutes(),
      0)
    );

    return taskEndDateTime;
  }
  getStartDateTime() {
    let startDate = this.taskForm.value.startDate;
    let taskStartDate: Date = new Date(Date.UTC(this.taskSelectedStartDate.getFullYear(), this.taskSelectedStartDate.getMonth(), this.taskSelectedStartDate.getDate()));
    return taskStartDate
  }
  getTaskCategories() {
    this.taskService.getTaskCategories().subscribe((res) => {
      this.taskCategories = res?.data;
    });
  }

  addScannedFile(file: File) {
    this.files.push(file);
    this.taskForm.patchValue({ files: this.files });
  }
  getNgbDate(date: Date) {
    return {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate(),
    };
  }
}
