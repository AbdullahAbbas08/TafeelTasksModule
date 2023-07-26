import {
  AttendeeDTO,
  AttendeesList,
  CommentDTO,
  CommentType,
  IAttendeesList,
  MeetingCommentDTO,
  MeetingSummaryDTO,
  MinuteOfMeetingDTO,
  MOMCommentDTO,
} from './../../../core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MOMService {
  constructor(private swagger: SwaggerClient) {}

  addMOM(mom: MinuteOfMeetingDTO) {
    return this.swagger.apiMinuteOfMeetingsInsertPost([mom]);
  }

  getMeetingTopicsLookup(meetingId: number) {
    return this.swagger.apiMeetingTopicsGetMeettingTopicLookupGet(meetingId);
  }

  getMinutesOfMeetings(meetingId: number, take: number, skip: number) {
    return this.swagger.apiMinuteOfMeetingsGetAllGet(
      take,
      skip,
      undefined,
      'meetingId',
      'eq',
      `${meetingId}`,
      'or',
      undefined,
      false
    );
  }

  // Post Topic Comment
  addComment(
    comment: CommentDTO,
    minuteOfMeetingId: number,
    commentType: CommentType
  ) {
    let momComment = new MOMCommentDTO({
      comment,
      minuteOfMeetingId,
      commentType,
    });
    return this.swagger.apiMOMCommentsInsertPost([momComment]);
  }

  getMeetingVoting(meetingId: number) {
    return this.swagger.apiMeetingsGetSurviesByMeetingIdPost(meetingId);
  }
  checkMeetingInvitaion(meetindId:number):Observable<boolean>{
   return  this.swagger.apiMinuteOfMeetingsSendMailMoMInvitationGet(meetindId)
  }
  addMeetingRecommendation(newRecommendation: MeetingCommentDTO) {
    return this.swagger.apiMeetingCommentsInsertMeetingCommentPost(newRecommendation);
  }

  updateMeetingRecommendation(updatedRecommendation: MeetingCommentDTO) {
    return this.swagger.apiMeetingCommentsUpdatePut([updatedRecommendation]);
  }

  deleteMeetingRecommendation(id: number) {
    return this.swagger.apiMeetingCommentsDeleteMeetingCommentDelete(id);
  }

  getMeetingRecommendations(meetingId: number) {
    return this.swagger.apiMeetingCommentsGetMeetingCommentsByMeetingIdGet(
      meetingId
    );
  }

  sendMOMforApproval(meetingId: number) {
    return this.swagger.apiMinuteOfMeetingsMOMApprovalGet(meetingId);
  }

  closeMeeting(meetingId: number) {
    //  var x = {} as AttendeesList;
    return this.swagger.apiMeetingsColseMeetingPost(meetingId);
  }

  getMeetingSummary(meetindId: number): Observable<MeetingSummaryDTO> {
    return this.swagger.apiMeetingsGetMeetingSummaryGet(meetindId);
  }
}
