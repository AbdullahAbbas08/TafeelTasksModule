import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/auth/auth.service';
import {
  MeetingAttendeeDTO,
  AttendeeState,
  MeetingCoordinatorDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { SingleMeetingService } from '../single-meeting.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-invited-users',
  templateUrl: './invited-users.component.html',
  styleUrls: ['./invited-users.component.scss'],
})
export class InvitedUsersComponent implements OnInit, OnDestroy {
  createdUserName;
  currentLang;
  createdUserImage;
  meetingParticipants = [];
  meetingId: number;
  subscription_1: Subscription;
  subscription_2: Subscription;
  subscription_3: Subscription;
  constructor(
    private authService: AuthService,
    public translateService: TranslateService,
    private _meetingService: SingleMeetingService,
    private storeService: StoreService,
    private router: Router
  ) {
    let routeArr: string[] = this.router.url.split('/');
    this.meetingId = +routeArr[routeArr.length - 1];
  }
  ngOnInit(): void {
    this.langChange();
    this.setCreatedUserDetails();
    if (this.meetingId) {
      this.getInvitedUsers();
    }
    this.subscription_1 = this.storeService.refreshAttendees$.subscribe(
      (val) => {
        if (val) {
          val.map((res) => {
            this.meetingParticipants.push(
              new MeetingAttendeeDTO({
                attendee: res.user,
                state: AttendeeState._1,
                available: res.available,
                attendeeId: res.userId,
              })
            );
          });
        }
      }
    );
    this.subscription_2 = this.storeService.onDeleteUser$.subscribe((val) => {
      if (val) {
        this.meetingParticipants.forEach((ele, index) => {
          if (ele.coordinatorId == val || ele.attendeeId == val) {
            this.meetingParticipants.splice(index, 1);
          }
        });
      }
    });
    this.subscription_3 = this.storeService.refreshCoordinator$.subscribe(
      (val) => {
        if (val) {
          val.map((res) => {
            this.meetingParticipants.push(
              new MeetingCoordinatorDTO({
                coordinator: res.user,
                state: AttendeeState._1,
                available: res.available,
                coordinatorId: res.userId,
              })
            );
          });
        }
      }
    );
  }
  ngOnDestroy() {
    this.subscription_1.unsubscribe();
    this.subscription_2.unsubscribe();
    this.subscription_3.unsubscribe();
  }
  setCreatedUserDetails() {
    let user = this.authService.getUser();
    this.createdUserName =
      this.currentLang === 'ar' ? user.fullNameAr : user.fullNameEn;
    this.createdUserImage = user.userImage;
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedUserDetails();
    });
  }
  getInvitedUsers() {
    if (this._meetingService.meeting) {
      let meetingAttendees = this._meetingService.meeting?.meetingAttendees;
      let meetingCoordinators =
        this._meetingService.meeting?.meetingCoordinators.filter((res) => {
          return !res.isCreator;
        });
      this.meetingParticipants = [...meetingCoordinators, ...meetingAttendees];
    }
  }
}
