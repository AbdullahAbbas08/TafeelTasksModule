import { StoreService } from 'src/app/shared/_services/store.service';
import { TranslateService } from '@ngx-translate/core';
import { SingleMeetingService } from './../../single-meeting.service';
import { addHours } from 'date-fns';
import { Component, Input, OnInit } from '@angular/core';
import { CalendarEvent } from 'angular-calendar';
import {
  AttendeeState,
  MeetingAttendeeDTO,
  MeetingAvailabilityDTO,
  MeetingCoordinatorDTO,
  UserType,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-user-schedule',
  templateUrl: './user-schedule.component.html',
  styleUrls: ['./user-schedule.component.scss'],
})
export class UserScheduleComponent implements OnInit {
  @Input() userType: UserType;
  @Input() coordinators: MeetingCoordinatorDTO[] = [];
  @Input() attendees: MeetingAttendeeDTO[] = [];
  @Input() viewDate: Date;
  userSchedule: MeetingAvailabilityDTO;

  availableUsersCount: number;
  unAvailableUsersCount: number;
  currentLang;
  refresh: Subject<any> = new Subject();
  stateType = AttendeeState;

  events: CalendarEvent<{
    id?: number;
    attendees?: MeetingAttendeeDTO[];
    subject?: string;
    remindBefore?: number;
  }>[] = [];

  constructor(
    private singleMeetingService: SingleMeetingService,
    public translate: TranslateService,
    private storeService: StoreService
  ) {}

  ngOnInit(): void {
    this.calculateNumberOfAvailableUsers();
    this.userChangeListener();
    this.langChange();
    this.refreshParticipants();
  }

  refreshParticipants() {
    if (this.userType === UserType._2) {
      this.storeService.refreshAttendees$.subscribe((attndee) => {
        if (attndee) {
          this.calculateNumberOfAvailableUsers();
        }
      });
    } else {
      this.storeService.refreshCoordinator$.subscribe((coordinator) => {
        if (coordinator) {
          this.calculateNumberOfAvailableUsers();
        }
      });
    }
  }

  calculateNumberOfAvailableUsers() {
    switch (this.userType) {
      case UserType._1:
        let availableCoordinators = this.coordinators?.filter(
          (coo) => coo.available === true
        );
        this.availableUsersCount = availableCoordinators?.length;
        this.unAvailableUsersCount =
          this.coordinators?.length - this.availableUsersCount;
        break;
      case UserType._2:
        let availableAttendees = this.attendees?.filter(
          (att) => att.available === true
        );
        this.availableUsersCount = availableAttendees?.length;
        this.unAvailableUsersCount =
          this.attendees?.length - this.availableUsersCount;
        break;
      default:
        break;
    }
  }

  userChangeListener() {
    switch (this.userType) {
      case UserType._1:
        this.singleMeetingService.coordinatorChange.subscribe(
          (userSchedule) => {
            this.userSchedule = userSchedule;
            this.events = userSchedule.meetings.map((meeting) => {
              return {
                title: meeting.title,
                start: meeting.meetingFromTime,
                end: meeting.meetingToTime,
                meta: {
                  id: meeting.id,
                  attendees: meeting.meetingAttendees,
                  subject: meeting.subject,
                  remindBefore: meeting.reminderBeforeMinutes,
                },
              };
            });
          }
        );
        break;
      case UserType._2:
        this.singleMeetingService.attendeeChange.subscribe((userSchedule) => {
          this.userSchedule = userSchedule;
          this.events = userSchedule.meetings.map((meeting) => {
            return {
              title: meeting.title,
              start: meeting.meetingFromTime,
              end: meeting.meetingToTime,
              meta: {
                id: meeting.id,
                attendees: meeting.meetingAttendees,
                subject: meeting.subject,
                remindBefore: meeting.reminderBeforeMinutes,
              },
            };
          });
        });
        break;
      default:
        break;
    }
    this.refresh.next();
  }

  langChange() {
    this.currentLang = this.translate.currentLang;
    this.translate.onLangChange.subscribe(() => {
      this.currentLang = this.translate.currentLang;
    });
  }
}
