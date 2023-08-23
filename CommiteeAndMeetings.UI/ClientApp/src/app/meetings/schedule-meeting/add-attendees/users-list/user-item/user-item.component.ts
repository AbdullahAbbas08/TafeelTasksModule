import { SingleMeetingService } from './../../../single-meeting.service';
import {
  UserDetailsDTO,
  AttendeeState,
  MeetingAttendeeDTO,
  MeetingCoordinatorDTO,
  UserType,
} from './../../../../../core/_services/swagger/SwaggerClient.service';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { StoreService } from 'src/app/shared/_services/store.service';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.scss'],
})
export class UserItemComponent implements OnInit {
  @Input() userType: UserType;
  @Input() coordinator: MeetingCoordinatorDTO;
  @Input() attendee: MeetingAttendeeDTO;
  @Input() first: boolean;
  @Output() deleted: EventEmitter<boolean> = new EventEmitter<boolean>();

  currentLang;
  userName;
  userImage;
  userTitle;
  available: boolean;
  state: AttendeeState;
  externalUser: boolean;
  sendDate: Date;
  replyDate: Date;

  stateType = AttendeeState;
  meetingUserType = UserType;
  userId: number;

  selected: boolean = false;
  visible = false;
  isAttendee = false;

  constructor(
    public translateService: TranslateService,
    public singleMeeting: SingleMeetingService,
    private storeService: StoreService
  ) { }

  ngOnInit(): void {
    this.langChange();
    this.setUserDetails();
    this.listenToSelectedUser();
  }

  setUserDetails() {
    switch (this.userType) {
      case UserType._1:
        this.userName =
          this.currentLang === 'ar'
            ? this.coordinator.coordinator.fullNameAr
            : this.coordinator.coordinator.fullNameEn;
        this.userImage = this.coordinator.coordinator.profileImage;
        this.userTitle = this.coordinator.coordinator.jobTitleName;
        this.state = this.coordinator.state;
        this.available = this.coordinator.available;
        this.externalUser = this.coordinator.coordinator.externalUser;
        this.userId = this.coordinator.coordinatorId;
        this.sendDate = this.coordinator.sendingDate;
        this.replyDate =
          this.coordinator.state > 2 ? new Date(this.coordinator.updatedOn.getFullYear(),
            this.coordinator.updatedOn.getMonth(),
            this.coordinator.updatedOn.getDate(),
            this.coordinator.updatedOn.getHours() - 1,
            this.coordinator.updatedOn.getMinutes(),
            this.coordinator.updatedOn.getSeconds()) : null;
        this.isAttendee = this.coordinator.confirmeAttendance;
        break;
      case UserType._2:
        this.userName =
          this.currentLang === 'ar'
            ? this.attendee.attendee.fullNameAr
            : this.attendee.attendee.fullNameEn;
        this.userImage = this.attendee.attendee.profileImage;
        this.userTitle = this.attendee.attendee.jobTitleName;
        this.state = this.attendee.state;
        this.available = this.attendee.available;
        this.externalUser = this.attendee.attendee.externalUser;
        this.userId = this.attendee.attendeeId;
        this.sendDate = this.attendee.sendingDate;
        this.replyDate =
          this.attendee.state > 2 ? new Date(this.attendee.updatedOn.getFullYear(),
            this.attendee.updatedOn.getMonth(),
            this.attendee.updatedOn.getDate(),
            this.attendee.updatedOn.getHours() - 1,
            this.attendee.updatedOn.getMinutes(),
            this.attendee.updatedOn.getSeconds()) : null;

        break;
      default:
        break;
    }
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setUserDetails();
    });
  }

  changeIsAttendde() {
    this.singleMeeting
      .changeCoordinatorRequiredToAttend(
        this.userId,
        this.singleMeeting.meeting.id,
        UserType._1
      )
      .subscribe((res) => {
        this.isAttendee = res?.confirmeAttendance;
      });
  }

  sendInvitationRequest(event: Event) {
    event.stopPropagation();

    let requiredState = AttendeeState._2;

    switch (this.userType) {
      case UserType._1:
        this.singleMeeting
          .sendInvitationOrReplyInvitation(
            this.coordinator.coordinatorId,
            this.userType,
            requiredState
          )
          .subscribe((res) => {
            if (res) {
              this.state = requiredState;
              this.sendDate = res.sendingDate;
              this.coordinator.sendingDate = this.sendDate;
            }
          });
        break;
      case UserType._2:
        this.singleMeeting
          .sendInvitationOrReplyInvitation(
            this.attendee.attendeeId,
            this.userType,
            requiredState
          )
          .subscribe((res) => {
            if (res) {
              this.state = requiredState;
              this.sendDate = res.sendingDate;
              this.attendee.sendingDate = this.sendDate;
              this.singleMeeting.meeting?.meetingAttendees.map((attende) => {
                if (attende.attendeeId === this.attendee.attendeeId) {
                  attende.state = this.state
                }
              })
            }
          });
        break;
      default:
        break;
    }
  }

  getUserMeetings() {
    switch (this.userType) {
      case UserType._1:
        this.singleMeeting
          .getUserMeetings(this.coordinator.coordinatorId, this.userType)
          .subscribe((res) => {
            if (res) {
              this.singleMeeting.coordinatorChange.next(res);
              this.singleMeeting.selectedUserChange.next(
                this.coordinator.coordinatorId
              );
              this.selected = true;
            }
          });
        break;
      case UserType._2:
        this.singleMeeting
          .getUserMeetings(this.attendee.attendeeId, this.userType)
          .subscribe((res) => {
            if (res) {
              this.singleMeeting.attendeeChange.next(res);
              this.singleMeeting.selectedUserChange.next(
                this.attendee.attendeeId
              );
              this.selected = true;
            }
          });
        break;
      default:
        break;
    }
  }

  listenToSelectedUser() {
    this.singleMeeting.selectedUserChange.subscribe((id) => {
      if (this.userId !== id) {
        this.selected = false;
      }
    });
  }

  onDelete() {
    this.singleMeeting
      .deleteParticipant(
        this.userId,
        this.singleMeeting.meeting.id,
        this.userType
      )
      .subscribe((res) => {
        if (res) {
          this.deleted.emit(true);
          this.storeService.onDeleteUser$.next(this.userId);
        }
      });
  }
}
