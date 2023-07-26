import { ReletedMeetingState } from './../../../core/_services/swagger/SwaggerClient.service';
import { MultipleMeetingService } from './../multiple-meeting.service';

import { Component, OnInit } from '@angular/core';
import { ReletedMeetingDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-meeting-list',
  templateUrl: './meeting-list.component.html',
  styleUrls: ['./meeting-list.component.scss'],
})
export class MeetingListComponent implements OnInit {
  refId: number;
  meetingList: ReletedMeetingDTO[] = [];
  meetingState = ReletedMeetingState;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getMeetingList();
  }

  getMeetingList() {
    this.activatedRoute.data.subscribe((data) => {
      this.meetingList = data['meetingList']?.reletedMeetings;
    });
  }

  navigateToMeeting(id: number) {
    this.router.navigateByUrl('/meetings/schedule-meeting/' + id);
  }
}
