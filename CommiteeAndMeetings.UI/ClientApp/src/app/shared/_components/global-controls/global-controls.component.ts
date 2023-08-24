import { TranslateService } from '@ngx-translate/core';
import {
  CommitteeActions,
  MeetingActions,
  SettingControllers,
  TasksFilterEnum,
} from './../../../shared/_enums/AppEnums';
import { SearchService } from './../../../shared/_services/search.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import {
  Component,
  OnInit,
  OnDestroy,
  Input,
  TemplateRef,
  Inject,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { Subscription } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import {
  CommiteeUsersRoleDTO,
  SwaggerClient,
  ValidityPeriodDTO,
  API_BASE_URL,
  CommiteeTaskDTO,
  ComiteeTaskCategoryDTO,
  CommiteeMemberDTO,
  LookUpDTO,
  CommiteeDTO,
  UserTaskCountDTO,
  CountResultDTO,
  MeetingDetailsDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { ThemeService } from 'src/app/shared/_services/theme.service';
import { TasksService } from 'src/app/tasks/tasks.service';
import { CommitteeService, ExtendedCommiteeDTODataSourceResult, ExtendedCommitteeDTO } from 'src/app/committees/committee.service';
import { LayoutService } from '../../_services/layout.service';
import { BehaviorSubject } from 'rxjs';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker';
import { BrowserStorageService } from '../../_services/browser-storage.service';
import { StatisticsService } from 'src/app/committees/committee-details/statistics/statistics.service';
import { AuthService } from 'src/app/auth/auth.service';
import { DashboardService } from 'src/app/meetings/meetings-dashboard/dashboard.service';
import { ExtentedCommiteeMemberDTO, UserService } from 'src/app/committees/committee-details/users/user.service';

@Component({
  selector: 'app-global-controls',
  templateUrl: './global-controls.component.html',
  styleUrls: ['./global-controls.component.scss'],
})
export class GlobalControlsComponent implements OnInit, OnDestroy {
  @Input() committeDetails:CommiteeDTO
  @Input() committeeEndDate: Date;
  @Input() enableTransaction: Boolean;
  @Input() enableDescision: any;
  @Input() isForGeneralTasks = false;
  usersChanged$ = new BehaviorSubject('');
  committeChanged$ = new BehaviorSubject('');
  meetingsChanged$ = new BehaviorSubject('');
  users: CommiteeMemberDTO[] | LookUpDTO[] = [];
  lookupTypes = SettingControllers;
  tasksSummary: CommiteeTaskDTO[] = [];
  delegatedRole: CommiteeUsersRoleDTO;
  dashboardSearch: FormControl = new FormControl('');
  actionTypes = CommitteeActions;
  showGlobalSearch: boolean = false;
  searchword: string;
  closeFlag: boolean = false;
  committeeId: any;
  stateSubscription: Subscription;
  editSubscription: Subscription;
  filterCaseSubscribtion:Subscription;
  validityPeriod: ValidityPeriodDTO;
  visible;
  isModalVisible;
  committeeActive;
  urlLink = '';
  tasksCategory: ComiteeTaskCategoryDTO[] = [];
  permittedToAddAttachment = false;
  permittedToAddTask = false;
  permittedToAddVote = false;
  permittedToAddTransaction = false;
  permittedToArchive = false;
  permittedToExtendCommittee = false;
  permittedToDisableMember = false;
  permittedToDelegateMember = false;
  permittedToAddMember = false;
  permittedToAllTasks:boolean = false;
  permittedToLateTasks:boolean = false;
  permittedToClosedTasks:boolean = false;
  permittedToAddMeeting:boolean = false;
  permittedToCommitteMinutes:boolean = false;
  permittedToNotHeadAndMember:boolean = false
  currentUrl: string;
  printIconFlag: boolean = false;
  currentLang: string;
  isLoading: boolean = false;
  selectedType: any;
  selectedClassification: number;
  loadingData:boolean = false;
  TasksFilterEnum = TasksFilterEnum;
  filterTaskValues = Object.keys;
  selectedDateType = DateType.Gregorian;
  selectedFromDate: Date;
  selectedToDate:Date;
  selectedMainUser:any;
  selectedAssistantUser:any;
  selectedCommitte:any;
  selectedMeeting:any
  tasksStats:UserTaskCountDTO[] =[];
  userId: number;
  commiteeMinutsCount:CountResultDTO[]=[];
  skip: number = 0;
  take: number = 12;
  filter_Logic: string = 'or';
  filters: any[] = [];
  allCommittees: ExtendedCommitteeDTO[] = [];
  allMeetings:MeetingDetailsDTO[];
  searchtext:string;
  AllUsers: ExtentedCommiteeMemberDTO[] = [];
  constructor(
    public committeeService: CommitteeService,
    private modalService: SharedModalService,
    private route: ActivatedRoute,
    private storeService: StoreService,
    private searchService: SearchService,
    private modal: NzModalService,
    public translate: TranslateService,
    public swagger: SwaggerClient,
    private router: Router,
    @Inject(API_BASE_URL) public baseUrl: string,
    private _taskservice: TasksService,
    public themeService: ThemeService,
    private layoutService: LayoutService,
    public lookupService: LookupService,
    private dateFormatterService: DateFormatterService,
    private browserService: BrowserStorageService,
    private _statistics:StatisticsService,
    private authService: AuthService,
    private _dashboardService:DashboardService,
    private _userService:UserService
  ) {
    this.checkPermissions();
  }

  ngOnInit(): void {
    this.userId = this.authService.getUser().userId;
    this.currentUrl = this.router.routerState.snapshot.url;
    if (this.currentUrl.includes('tasks')) {
      this.printIconFlag = true;
    } else {
      this.printIconFlag = false;
    }
    this.langChange();
    this.listenToEditedTask();
    this.getTasksCategores();
    this.route.params.subscribe((params) => {
      this.committeeId = params.id;

      if (this.committeeId) {
        this.storeService.setCommitteeId(this.committeeId);
        this.committeeActive = this.committeeService.getCommitteeCurrentState();
        // Validity Period
        this.stateSubscription =
          this.committeeService.committeePeriodChange$.subscribe((period) => {
            this.validityPeriod = period;
          });

        this.checkCurrentComponent();
        this.getMasarUrl();
        this.initUsersForCommittee();
      } else {
        this.initUsers();
      }
    });
    this.committeeService.filterFlag.subscribe((res) => {
      if(res){
        this.showGlobalSearch = false 
      }
    });
 this.filterCaseSubscribtion =   this._taskservice.filterWithClick.subscribe((res) => {
      if(res) {
        switch (res) {
          case 1:
            this.selectedType = this.TasksFilterEnum.all
          break;
          case 2:
            this.selectedType = this.TasksFilterEnum.late
          break;
          case 3:
            this.selectedType = this.TasksFilterEnum.Underprocedure
          break;
          case 4:
            this.selectedType = 9    // enum value for AssistantUserTasks
          break;
          case 5:
            this.selectedType = 8   // enum value for ClosedTasks
          break;
          case 6:
            this.selectedType = 10   // enum value for TasksToView
          break;
          default:
          break;
        }
      }
    })

  }
  filterWithCommitteAndMeeting(){
    this.initAllCommitte();
    this.initAllMeetings()
  }
  checkCurrentComponent() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = this.router.routerState.snapshot.url;
        if (this.currentUrl.includes('tasks')) {
          this.printIconFlag = true;
        } else {
          this.printIconFlag = false;
        }
      }
    });
  }
  ngOnDestroy() {
    if (this.stateSubscription) this.stateSubscription.unsubscribe();
    if (this.editSubscription) this.editSubscription.unsubscribe();
    this._taskservice.filterWithClick.next(null)
  }

  addNewTimeLineItem(type: number) {
    this.modalService.openDrawerModal(
      type,
      this.committeeId,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      this.enableDescision,
      undefined,undefined,undefined,undefined,undefined,this.committeDetails?.departmentLinkId
      
    );
  }
  clearFields() {
    this.selectedType = undefined;
    this.selectedClassification = undefined;
  }
  searchThis() {
    if(this.printIconFlag){
      if(!this.selectedType){
        this.selectedType = TasksFilterEnum.all
      }
      if(this.selectedAssistantUser === null){
        this.selectedAssistantUser = undefined
      }
      if(this.selectedMainUser === null){
        this.selectedMainUser = undefined
      }
      if(this.selectedCommitte === null){
        this.selectedCommitte = undefined
      }
      if(this.selectedMeeting === null){
        this.selectedMeeting = undefined
      }
      this._taskservice.tasksFilters.next({
        typeId: this.selectedType,
        classificationId: this.selectedClassification,
        searchtxt:this.searchword,
        body:{
          fromDate:this.selectedFromDate,
          toDate:this.selectedToDate,
          mainUserId:this.selectedMainUser,
          mainAssinedUserId:this.selectedAssistantUser,
          committeId:this.selectedCommitte,
          meetingId:this.selectedMeeting
        }
      });
      this._taskservice.toggleTasksFilter.next(false)
    } else {
      this.searchService.search(this.searchword);
    }

  }

  // cancelSearch() {
  //   this.searchword = undefined;
  //   this.showGlobalSearch = !this.showGlobalSearch;
  //   // this.searchThis();
  // }

  onArchivePeriod() {
    this.modalService.initModalConfirmArchiving(
      this.committeeId,
      this.validityPeriod?.validityPeriodId,
      this.validityPeriod.validityPeriodFrom,
      this.committeDetails.createdOn
    );
  }

  onExtendCommittee() {
    this.modalService.openDrawerModal(
      CommitteeActions.ExtendCommittee,
      this.committeeId,
      null,
      null,
      this.committeeEndDate
    );
  }

  refresh() {
    this.searchService.search(undefined);
    this._taskservice.tasksFilters.next(undefined)
    this.selectedType = undefined;
    this.selectedClassification = undefined;
    this._taskservice.toggleTasksFilter.next(false);
    this.selectedMainUser = undefined;
    this.selectedAssistantUser = undefined;
    this.selectedFromDate = undefined;
    this.selectedToDate = undefined;
    this.selectedCommitte = undefined;
    this.selectedMeeting = undefined
  }
  closeSearch(){
    this.showGlobalSearch = !this.showGlobalSearch;
     this.searchService.search(undefined);
    this._taskservice.tasksFilters.next(undefined)
    this.selectedType = undefined;
    this.selectedClassification = undefined;
    this._taskservice.toggleTasksFilter.next(false);
    this.searchword = undefined;
    this.selectedMainUser = undefined;
    this.selectedAssistantUser = undefined;
    this.selectedFromDate = undefined;
    this.selectedToDate = undefined;
    this.selectedCommitte = undefined;
    this.selectedMeeting = undefined
  }
  checkPermissions() {
      setTimeout(() => {
        this.permittedToAddAttachment =
        this.committeeService.checkPermission('ADDATTACHMENT');
      this.permittedToAddTask = 
        this.committeeService.checkPermission('ADDTASK');
      this.permittedToAddVote = 
        this.committeeService.checkPermission('ADDVOTE');
      this.permittedToAddTransaction =
        this.committeeService.checkPermission('ADDTRANSACTION');
      this.permittedToArchive =
        this.committeeService.checkPermission('ARCHEIVECOMMITTEE');
      this.permittedToExtendCommittee =
        this.committeeService.checkPermission('EXTENDCOMMITTEE');
      this.permittedToAllTasks = 
        this.committeeService.checkPermission('allTasks');
      this.permittedToLateTasks = 
        this.committeeService.checkPermission('lateTasks');
      this.permittedToClosedTasks = 
        this.committeeService.checkPermission('closedTasks');
      this.permittedToAddMeeting = 
        this.committeeService.checkPermission('createMeeting');
      this.permittedToCommitteMinutes = 
         this.committeeService.checkPermission('committeMinutes')
        if(this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers?.some((el) => el.userId === +this.userId)){
          this.permittedToNotHeadAndMember = true
        }
      },500)
  }

  createTplModal(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>): void {
    this.modal.create({
      nzTitle: tplTitle,
      nzContent: tplContent,
      nzFooter: null,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzClosable: true,
      nzWidth: 450,
    });
  }

  getMasarUrl() {
    this.swagger
      .apiCommiteesGeMasarUrlGet()
      .subscribe((url) => (this.urlLink = url));
  }

  addNewMeeting() {
    this.modalService.openDrawerModal(
      MeetingActions.CreateNewMeeting,
      this.committeeId
    );
  }

  listenToEditedTask() {
    this.editSubscription = this.storeService.refresh$.subscribe((value) =>
      this.refresh()
    );
  }

  print() {
    if(this.selectedAssistantUser === null){
      this.selectedAssistantUser = undefined
    }
    if(this.selectedMainUser === null){
      this.selectedMainUser = undefined
    }
    if(this.selectedClassification === null){
      this.selectedClassification = undefined
    }
    if(this.selectedCommitte === null){
      this.selectedCommitte = undefined
    }
    if(this.selectedMeeting === null){
      this.selectedMeeting = undefined
    }
    this.getAllTasksForPrint(this.selectedType,this.selectedClassification,this.searchword,this.selectedFromDate,this.selectedToDate,this.selectedMainUser,this.selectedAssistantUser,this.selectedMeeting)
  }
  langChange() {
    this.currentLang = this.translate.currentLang;
    this.translate.onLangChange.subscribe(() => {
      this.currentLang = this.translate.currentLang;
    });
  }
  getAllTasksForPrint(selectedTask: number, selectedClassification?,searchword?:string,fromDate?:Date,ToDate?:Date,mainUser?:number,assistantUser?:number,meetingId?:number) {
    if (!selectedTask) {
      selectedTask = 7;
    } else {
      selectedTask = selectedTask;
    }
    this.loadingData = true;
    this.layoutService.toggleSpinner(true);
    if(this.committeeId){
      this.swagger
      .apiCommiteeTasksGetAllForPrintGet(
        selectedTask,
        this.browserService.encryptCommitteId(this.committeeId),
        selectedClassification,
        searchword,
        undefined,
        fromDate,
        ToDate,
        mainUser,
        assistantUser,
        undefined,
        undefined,
        undefined,
        undefined
      )
      .subscribe((result) => {
        if(result){
          this.tasksSummary = result;
          this.loadingData = false;
          this.layoutService.toggleSpinner(false);
          this.drawPrintTable()
        }
      });
    } else {
      this.swagger
      .apiCommiteeTasksGetAllForPrintGet(
        selectedTask,
        this.committeeId,
        selectedClassification,
        searchword,
        undefined,
        fromDate,
        ToDate,
        mainUser,
        assistantUser,
        undefined,
        undefined,
        meetingId,
        undefined,
      )
      .subscribe((result) => {
        if(result){
          this.tasksSummary = result;
          this.loadingData = false;
          this.layoutService.toggleSpinner(false);
          this.drawPrintTable()
        }
      });
    }
  }
  getTasksCategores() {
    this._taskservice.getTaskCategories().subscribe((result) => {
      this.tasksCategory = result.data;
      this.isLoading = false;
    });
  }
  drawPrintTable(){
    setTimeout(() => {
      let direction = this.currentLang == 'ar' ? 'rtl' : 'ltr';
      let popupWinindow;
      let innerContents = document.getElementById(
        'main-print-section-id'
      ).innerHTML;
      popupWinindow = window.open(
        '',
        '_blank',
        'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no'
      );
      popupWinindow.document.open();
      popupWinindow.document.write(
        `<html dir="${direction}" lang="${this.currentLang}"><head><link rel="stylesheet" type="text/css" href="${this.baseUrl}/assets/css/print-referral.css" /></head><body onload="setTimeout(() => {window.print()},100);">` +
          innerContents +
          '</html>'
      );
      popupWinindow.document.close();
    },1000)
  }
  
  showCommitteMinutes(){
    this.loadingData = true;
    this.layoutService.toggleSpinner(true);
    // this._statistics.getStatsPerUser(this.browserService.encryptCommitteId(this.committeeId)).subscribe((res) => {
    //   this.tasksStats = res;
    // })
    const id = this.browserService.decrypteString(this.committeeId);
    this._userService.getCommitteUsers(50,0,this.browserService.encrypteString(id),[],'or',this.searchtext).subscribe((res) => {
        this.AllUsers = res.data
    })
    this._statistics.getCommiteeStatsCount(this.browserService.encrypteString(id)).subscribe((res:CountResultDTO[]) => {
       if(res){
        this.commiteeMinutsCount =res;
        this.loadingData = false;
        this.layoutService.toggleSpinner(false);
        this.printCommitteMinutes();
       }
    })
  }
  printCommitteMinutes(){
    setTimeout(() => {
      let direction = this.currentLang == 'ar' ? 'rtl' : 'ltr';
      let popupWinindow;
      let innerContents = document.getElementById(
        'main-committe-minutes-id'
      ).innerHTML;
      popupWinindow = window.open(
        '',
        '_blank',
        'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no'
      );
      popupWinindow.document.open();
      popupWinindow.document.write(
        `<html dir="${direction}" lang="${this.currentLang}"><head><link rel="stylesheet" type="text/css" href="${this.baseUrl}/assets/css/committe-print.css" /></head><body onload="setTimeout(() => {window.print()},100);">` +
          innerContents +
          '</html>'
      );
      popupWinindow.document.close();
    },1000)
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
  initAllCommitte(){
    this.committeChanged$
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
        this.allCommittees = res.data;
        this.isLoading = false;
    })
  }
  initAllMeetings(){
    this.meetingsChanged$
    .asObservable()
    .pipe(
      debounceTime(500),
      switchMap((text:string) => {
        return  this._dashboardService.getAllMeetings()
      })
    ).subscribe((meeting:MeetingDetailsDTO[]) => {
      this.allMeetings = meeting
      this.isLoading = false
    })
  }
  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
      case SettingControllers.COMMITTEE:
        this.committeChanged$.next(value)
        break;
      case SettingControllers.MeetingsList:
        this.meetingsChanged$.next(value);
        break;
      default:
        break;
    }
  }
  dateFromSelected(selectedDate: NgbDateStruct) {
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.selectedFromDate = new Date(
        Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
      );
    } else{
      this.selectedFromDate = undefined
    }
  }
  dateSelected(selectedDate: NgbDateStruct){
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.selectedToDate = new Date(
        Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
      );
    }  else{
      this.selectedToDate = undefined
    }
  }
}
