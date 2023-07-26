import { SingleMeetingService } from './../single-meeting.service';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  TemplateRef,
} from '@angular/core';
import {
  AttendeeState,
  CommiteeMemberDTO,
  LookUpDTO,
  MeetingAttendeeDTO,
  MeetingCoordinatorDTO,
  SwaggerClient,
  UserType,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { debounceTime, switchMap } from 'rxjs/operators';
import { UserService } from 'src/app/committees/committee-details/users/user.service';
import { NzMarks } from 'ng-zorro-antd/slider';
import { StoreService } from 'src/app/shared/_services/store.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';

@Component({
  selector: 'app-search-users',
  templateUrl: './search-users.component.html',
  styleUrls: ['./search-users.component.scss'],
})
export class SearchUsersComponent implements OnInit, OnDestroy {
  @Output() userIdEmitter: EventEmitter<number[]> = new EventEmitter();
  @Input() userType: UserType;
  @Input() coordinators: MeetingCoordinatorDTO[] = [];
  @Input() attendee: MeetingAttendeeDTO[] = [];
  listOfUsers = [];
  checkType = UserType;
  lookupTypes = SettingControllers;
  externalUserChanged$ = new BehaviorSubject('');
  internalUserChanged$ = new BehaviorSubject('');
  isLoading = false;
  take: number = 10;
  skip: number = 0;
  users: LookUpDTO[] = [];
  selectedUsers: LookUpDTO[] = [];
  searchInternal = true;
  searchExternal = false;
  clearInput: any;
  searchValue: number = 100;
  committeeMembers: CommiteeMemberDTO[] = [];
  selectedCommitteeMembersIds = [];
  listOfSelectedValue: number[] = [];
  subscription3: Subscription;
  orgChanged$ = new BehaviorSubject('');
  organizations: LookUpDTO[] = [];
  selectedOrganization:number[]=[]
  marks: NzMarks = {
    0: {
      style: {
        color: '#117a8b',
      },
      label: this.translate.currentLang == 'ar' ? 'خارجي' : 'External',
    },
    50: {
      style: {
        color: '#117a8b',
      },
      label: this.translate.currentLang == 'ar' ? 'داخلي' : 'Internal',
    },
    100: {
      style: {
        color: '#117a8b',
      },
      label: this.translate.currentLang == 'ar' ? 'الكل' : 'All',
    },
  };
  bsubject: any;

  constructor(
    private userService: UserService,
    private translate: TranslateService,
    private store: StoreService,
    public singleMeeting: SingleMeetingService,
    private modal: NzModalService,
    private notificationService: NzNotificationService,
    public singleMeetingService: SingleMeetingService,
    private browserService:BrowserStorageService,
    public lookupService: LookupService,
    private swagger:SwaggerClient,
    private layoutService: LayoutService,
  ) {}

  ngOnInit(): void {
    this.initInternalUsers();
    this.initExternalUsers();
    this.onChange(100);
    this.getMeetingUserList();
    this.bsubject = this.store.onDeleteUser$.asObservable();
    this.subscription3 = this.store.onDeleteUser$.asObservable().subscribe((val) => {
      if (val) {
        this.singleMeetingService.currentMeeting.meetingAttendees.map((ele, index) => {
          if (ele.attendeeId == val) {
            this.singleMeetingService.meeting.meetingAttendees.splice(index, 1);
          }
        });
        this.singleMeetingService.currentMeeting.meetingCoordinators.map((ele, index) => {
          if (ele.coordinatorId == val) {
            this.singleMeetingService.meeting.meetingCoordinators.splice(index, 1);
          }
        });
        this.listOfSelectedValue = []
        this.getListOfUsers(this.singleMeetingService.meeting.meetingCoordinators,this.singleMeetingService.meeting.meetingAttendees);
      }
    });
  }
  ngOnDestroy() {
  }
  getMeetingUserList() {
    let attendees = this.singleMeetingService.meeting.meetingAttendees;
    let coordinators = this.singleMeetingService.meeting.meetingCoordinators;
    this.getListOfUsers(coordinators,attendees);
  }
  getListOfUsers(coordinators,attendees){
    this.listOfUsers = [];
    coordinators.map((res) => {
      this.listOfUsers.push(
        new MeetingCoordinatorDTO({
          coordinator: res.coordinator,
          state: AttendeeState._1,
          available: res.available,
          coordinatorId: res.coordinatorId,
        })
      );
    });
    attendees.map((res) => {
      this.listOfUsers.push(
        new MeetingAttendeeDTO({
          attendee: res.attendee,
          state: AttendeeState._1,
          available: res.available,
          attendeeId: res.attendeeId,
        })
      );
    });
  }
  initExternalUsers() {
    this.externalUserChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.userService.getExternalUser(
            this.take,
            this.skip,
            text ? text : undefined
          )
        )
      )
      .subscribe((res: LookUpDTO[]) => {
        this.users = [...this.users, ...res];
        this.isLoading = false;
      });
  }

  initInternalUsers() {
    this.internalUserChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.userService.getInternalUser(
            this.take,
            this.skip,
            text ? text : undefined
          )
        )
      )
      .subscribe((res: LookUpDTO[]) => {
        this.users = [...this.users, ...res];
        this.isLoading = false;
      });
  }

  onSearch(value: string): void {
    this.users = [];

    this.isLoading = true;

    this.searchExternal ? this.externalUserChanged$.next(value) : null;

    this.searchInternal ? this.internalUserChanged$.next(value) : null;

    !(this.searchInternal || this.searchExternal)
      ? (this.isLoading = false)
      : null;
  }

  addUsers(usersIds: number[]) {
    this.userIdEmitter.emit(usersIds);
    this.listOfSelectedValue = [];
    this.selectedCommitteeMembersIds = [];
     usersIds.map((x) => this.listOfUsers.push({"userId":x}));
  }
  checkUser($event: []) {
    let res = $event[$event.length - 1];
    if (res === undefined) {
      return;
    } else {
      setTimeout(() => {
        if (
          this.listOfUsers.find(
            (x) => x.coordinatorId === res || x.attendeeId === res || x.userId === res
          )
        ) {
          this.translate
            .get('Thismemberalreadyexists')
            .subscribe((translateValue) =>
              this.notificationService.warning(translateValue, '')
            );
            $event.pop();
          
        }
      });
    }
  }
  switchUsers() {
    this.users = [];
    this.onSearch('');
  }

  onChange(value: number) {
    switch (value) {
      case 0:
        this.searchInternal = false;
        this.searchExternal = true;
        this.switchUsers();
        break;
      case 50:
        this.searchInternal = true;
        this.searchExternal = false;
        this.switchUsers();
        break;
      case 100:
        this.searchInternal = true;
        this.searchExternal = true;
        this.switchUsers();
        break;
    }
  }

  openCommiteeMembersPopup(
    tplTitle: TemplateRef<{}>,
    tplContent: TemplateRef<{}>
  ): void {
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
  openOrgMembersPopup(
    orgTitle: TemplateRef<{}>,
    orgContent: TemplateRef<{}>
  ): void {
    this.modal.create({
      nzTitle: orgTitle,
      nzContent: orgContent,
      nzFooter: null,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzClosable: true,
      nzWidth: 500,
      nzOnCancel: () => this.selectedOrganization = []
    });
  }

  getCommitteeMembers() {
    this.userService
      .getCommitteUsers(
        100,
        0,
        this.browserService.encrypteString(`${this.singleMeeting.meeting.committeId}`),
        [],
        undefined,
        undefined
      )
      .subscribe((response) => {
        this.committeeMembers = [...response.data].filter(
          (member) => !this.singleMeeting.isAttendeeOrCoordinator(member.userId)
        );
      });
  }
  orgSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
       case SettingControllers.ALLORGANIZATION:
        this.orgChanged$.next(value)
        break;
      default:
        break;
    }
  }
  getAllOrganizations(){
    this.orgChanged$
    .asObservable()
    .pipe(
      debounceTime(500),
      switchMap((text: string) => {
        return this.lookupService.getOrgsLookup(
          25,
          0,
          false,
          text
            ? [{ field: 'name', operator: 'contains', value: text }]
            : undefined,
            true
        );
      })
    )
    .subscribe((data: LookUpDTO[]) => {
      this.organizations = data;
      this.isLoading = false;
  
    });
  }
  isNotSelected(value: number): boolean {
    return this.listOfSelectedValue.indexOf(value) === -1;
  }
  isNotOrgSelected(value: number): boolean{
    return this.selectedOrganization.indexOf(value) === -1
  }
  addRemoveCommitteeUsers(added: boolean, id: number) {
    if (added) {
      this.selectedCommitteeMembersIds.push(id);
    } else {
      let index = this.selectedCommitteeMembersIds.findIndex(
        (addedId) => addedId === id
      );
      if (index > -1) {
        this.selectedCommitteeMembersIds.splice(id, 1);
      }
    }
  }
  addOrgUsers(selectedOrg){
    this.layoutService.toggleSpinner(true);
    this.swagger.apiCommiteesGetMeetingHeadUnitLookupUserAndOrganizationPost(selectedOrg).subscribe((res) => {
     if(res){
     let resAfterFilter = [...res].filter(
      (member) => !this.singleMeeting.isAttendeeOrCoordinator(member.id)
    );
      this.selectedOrganization = [];
      let usersIds:number[] = []
      resAfterFilter.map((x) => {
        usersIds.push(x.id)
      })
      this.addUsers(usersIds);
     }
    })
}
  
}
