import { SingleMeetingService } from './../../single-meeting.service';
import {
  UserType,
  MeetingAttendeeDTO,
  MeetingCoordinatorDTO,
} from './../../../../core/_services/swagger/SwaggerClient.service';
import { Component, Input, OnInit } from '@angular/core';
import { StoreService } from 'src/app/shared/_services/store.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
})
export class UsersListComponent implements OnInit {
  @Input() userType: UserType;
  @Input() coordinators: MeetingCoordinatorDTO[] = [];
  @Input() attendees: MeetingAttendeeDTO[] = [];

  checkType = UserType;
  constructor(
    private _store: StoreService,
    private singleMeeting: SingleMeetingService
  ) {}

  ngOnInit(): void {
  }

  removeParticipant(index: number) {
    switch (this.userType) {
      case UserType._1:
        this.coordinators.splice(index, 1);
        this.singleMeeting.meeting.meetingCoordinators.splice(index, 1);
        break;
      case UserType._2:
        this.attendees.splice(index, 1);
        this.singleMeeting.meeting.meetingAttendees.splice(index, 1);
        break;
    }
  }
}
