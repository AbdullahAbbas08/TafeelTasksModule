import { SurveyDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TopicCommentDTO, SurveyAnswerUserDTO } from './../../core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

interface CalendarType {
  isHijri: boolean;
  isGeorgian: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  userSettingsState$ = new BehaviorSubject({});
  loggedUser$ = new BehaviorSubject(null);
  calendarType$: BehaviorSubject<string> = new BehaviorSubject(
    localStorage['isHijriDateCalendar']
  );
  langType$ = new BehaviorSubject(localStorage['culture']);
  dashboardStatistics$ = new BehaviorSubject(null); // value of statistics of dashboard
  attachmentUploadAuthState$ = new BehaviorSubject(null); // store state of uploading attachment auth directive.
  isIeFlag$ = new BehaviorSubject(null); // share flag of ie browser state
  removeMaskFromInput$ = new BehaviorSubject(null);
  refreshCommittees$ = new BehaviorSubject(null);
  refreshTasks$ = new BehaviorSubject(null);
  refreshDocuments$ = new BehaviorSubject(null);
  refreshVotings$ = new BehaviorSubject(null);
  refreshUsers$ = new BehaviorSubject(null);
  refreshAttendees$ = new BehaviorSubject(null);
  refreshRecommendation$ = new BehaviorSubject(null);
  refreshUpdatingRecommendation$ = new BehaviorSubject(null);
  refreshMinutesOfMeeting$ = new BehaviorSubject(null);
  refreshCoordinator$ = new BehaviorSubject(null);
  onDeleteUser$ = new Subject();
  refreshTimelineItems$ = new BehaviorSubject(null);
  allCommittes$ = new BehaviorSubject(null);
  commiteeId: number;
  meetingDetails$ = new BehaviorSubject(null);
  // Signal-R Subjects for Meetings
  refreshTopicVotings$ = new Subject<any>();
  refreshTopicTimeline$ = new BehaviorSubject(null);
  refreshAnswerUsers$ = new Subject<any>();
  refreshAnswerUsersTimeline$ = new Subject<any>();
  refreshAnswerUsersforCurrentUser$ = new Subject<any>();
  refreshTopicCommentsRecommendations$ = new Subject<TopicCommentDTO>();
  votingParticipation$ = new Subject<number>();
  refreshMeetingVoting$ = new BehaviorSubject(null);
 
  refresh$ = new BehaviorSubject(null);
  constructor(private _swagger:SwaggerClient) {}

  setCommitteeId(id: number) {
    this.commiteeId = id;
  }

  getCommitteeId() {
    return this.commiteeId;
  }

}
