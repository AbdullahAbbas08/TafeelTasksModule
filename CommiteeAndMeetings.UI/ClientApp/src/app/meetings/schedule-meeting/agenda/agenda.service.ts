import {
  CommentDTO,
  CommentType,
  MeetingTopicDTO,
  PauseResume,
  StartStop,
  SurveyDTO,
  SwaggerClient,
  TopicActivitityDTO,
  TopicCommentDTO,
  TopicState,
  AttendeesList,
  AttendeeDTO,
} from './../../../core/_services/swagger/SwaggerClient.service';

import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Subject, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AgendaService {
  cancelledTopic: Subject<number> = new Subject<number>();
  meetingEnded: Subject<boolean> = new Subject<boolean>();
  meetingStarted: BehaviorSubject<boolean> = new BehaviorSubject(null);

  constructor(private swagger: SwaggerClient) {}

  addTopic(topic: MeetingTopicDTO) {
    return this.swagger.apiMeetingTopicsInsertPost([topic]);
  }

  getMeetingTopics(meetingId) {
    return this.swagger.apiMeetingTopicsGetAllTopicsGet(meetingId);
  }

  // Post Topic Comment
  addComment(comment: CommentDTO, topicId: number, commentType: CommentType) {
    let topicComment = new TopicCommentDTO({
      comment,
      topicId,
      commentType,
    });
    return this.swagger.apiTopicCommentsInsertPost([topicComment]);
  }

  // Get Topic Timeline
  getTopicWallItems(topicId: number) {
    let topicVotings: SurveyDTO[] = [];
    let topicComments: TopicCommentDTO[] = [];
    let timelineItems: (TopicCommentDTO | SurveyDTO)[];
    return this.swagger.apiMeetingTopicsGetTopicActivitiesGet(topicId).pipe(
      map((res: TopicActivitityDTO) => {
        if (res?.surveys) topicVotings = [...res.surveys];
        if (res?.comments) topicComments = [...res.comments];
        timelineItems = [...topicVotings, ...topicComments];
        return timelineItems.sort(
          (a: TopicCommentDTO | SurveyDTO, b: TopicCommentDTO | SurveyDTO) => {
            return a.createdOn.getTime() - b.createdOn.getTime();
          }
        );
      })
    );
  }

  // Begin Topic
  beginTopic(id: number) {
    return this.swagger.apiMeetingTopicsTopicStartEndPost(id, StartStop._1);
  }

  // End Topic
  endTopic(id: number) {
    return this.swagger.apiMeetingTopicsTopicStartEndPost(id, StartStop._2);
  }

  // Next Topic
  nextTopic(currentId: number, nextId: number, currentIndex: number) {
    return this.swagger.apiMeetingTopicsNextTopicPost(
      currentId,
      nextId,
      currentIndex
    );
  }

  // Pause Topic
  pauseTopic(id: number) {
    return this.swagger.apiMeetingTopicsTopicPauseResumePost(
      id,
      PauseResume._1
    );
  }

  // Resume Topic
  resumeTopic(id: number) {
    return this.swagger.apiMeetingTopicsTopicPauseResumePost(
      id,
      PauseResume._2
    );
  }

  // Cancel Topic
  cancelTopic(id: number) {
    return this.swagger.apiMeetingTopicsChangeTopicStateGet(id, TopicState._5);
  }

  // Get Attendance List
  getMeetingAttendanceList(meetingId: number) {
    return this.swagger.apiMeetingsUsersForMeetingGet(meetingId);
  }

  // Save Attendee List
  saveAttendeeList(meetingId: number, attendanceList: AttendeeDTO[]) {
    return this.swagger.apiMeetingsPostMeetingAttendeesPost(
      meetingId,
      new AttendeesList({ attendees: attendanceList })
    );
  }
}
