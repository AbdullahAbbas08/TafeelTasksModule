import { Component, OnInit } from '@angular/core';
import { AttendeeState, LookUpDTO, MeetingAttendeeDTO, MeetingCoordinatorDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { NzMarks } from 'ng-zorro-antd/slider';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { UserService } from 'src/app/committees/committee-details/users/user.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { changeState, DashboardService } from '../../dashboard.service';
@Component({
  selector: 'app-recomend-user',
  templateUrl: './recomend-user.component.html',
  styleUrls: ['./recomend-user.component.scss']
})
export class RecomendUserComponent implements OnInit {
  id:number;
  attendees:MeetingAttendeeDTO[];
  coordiantors:MeetingCoordinatorDTO[];
  userType:number;
  userState:number;
  userId:any;
  searchInternal = true;
  searchExternal = false;
  searchValue: number = 100;
  users: LookUpDTO[] = [];
  isLoading = false;
  externalUserChanged$ = new BehaviorSubject('');
  internalUserChanged$ = new BehaviorSubject('');
  take: number = 10;
  skip: number = 0;
  refusedReason:string;
  saving:boolean = false
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
  listOfUsers = [];
  listOfSelectedValue: number;
  constructor(private _dashboardService:DashboardService,private browserStorage:BrowserStorageService,private swagger:SwaggerClient,private modalService: SharedModalService,private notificationService: NzNotificationService,private translate: TranslateService,private userService: UserService) { }

  ngOnInit(): void {
    this.onChange(100);
    this.initExternalUsers();
    this.initInternalUsers();
    this.getListOfUsers();
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
  switchUsers() {
    this.users = [];
    this.onSearch('');
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
  checkUser($event) {
    let res = $event;
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
            this.listOfSelectedValue = undefined
        }
      });
    }
  }
  getListOfUsers(){
    this.listOfUsers = [];
    this.coordiantors.map((res) => {
      this.listOfUsers.push(
        new MeetingCoordinatorDTO({
          coordinator: res.coordinator,
          state: AttendeeState._1,
          available: res.available,
          coordinatorId: res.coordinatorId,
        })
      );
    });
    this.attendees.map((res) => {
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
  close() {
    this.modalService.destroyModal();
    this.listOfSelectedValue = undefined;
    this.refusedReason = undefined
  }
  refuesedSubmit(){
    this.saving = true;
    this.swagger.apiMeetingsGettingReplacingCoordinateOrAttendeePost(this.browserStorage.encrypteString(this.id),this.refusedReason,this.listOfSelectedValue ? this.browserStorage.encrypteString(this.listOfSelectedValue) : undefined ).subscribe((res) => {
      if(res){
        let stateObj:changeState = {
          userId:this.userId,
          meetingId:this.id,
          userType:this.userType,
          userState:this.userState,
        }
        this.saving = false;
        this._dashboardService.changeAttendeCordinatorState.next(stateObj);
        this.modalService.destroyModal()
      }
    })
  }
}
