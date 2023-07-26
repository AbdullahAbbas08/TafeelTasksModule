import { Router } from '@angular/router';
import {
  AttendeeState,
  ListOfMeetingUserDTO,
  MeetingAvailabilityDTO,
  MeetingUserDTO,
  UserType,
} from './../../core/_services/swagger/SwaggerClient.service';
import { tap } from 'rxjs/operators';
import {
  MeetingDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SingleMeetingService {
  currentMeeting: MeetingDTO;
  attendeeChange = new Subject<MeetingAvailabilityDTO>();
  coordinatorChange = new Subject<MeetingAvailabilityDTO>();
  selectedUserChange = new Subject<number>();

  updateMeeting: Subject<MeetingDTO> = new Subject();

  constructor(private swagger: SwaggerClient, private router: Router) {}

  scheduleSingleMeeting(meetingInfo: MeetingDTO[]) {
    return this.swagger.apiMeetingsInsertCustomPost(meetingInfo);
  }

  editSingleMeeting(meetingInfo: MeetingDTO[]) {
    return this.swagger.apiMeetingsUpdatePut(meetingInfo);
  }

  getMeetingDetails(id: number) {
    return this.swagger.apiMeetingsGetByIdGet(`${id}`).pipe(
      tap((meeting) => {
        if (!meeting) {
          this.router.navigate(['/meetings']);
          return;
        }
        this.meeting = meeting;
        this.meetingFinished = meeting.isFinished;
        this.meetingCanceled = meeting.isCanceled
        this.meetingClosed = meeting.colsed;
        this.currentMeeting = meeting;
        
      })
    );
  }

  set meeting(meeting: MeetingDTO) {
    this.currentMeeting = meeting;
  }

  get meeting(): MeetingDTO {
    return this.currentMeeting;
  }

  set meetingCanceled(value){
    this.currentMeeting.isCanceled = value
  }
  get meetingCanceled(){
    return this.currentMeeting.isCanceled
  }

  set meetingFinished(value) {
    this.currentMeeting.isFinished = value;
  }

  get meetingFinished() {
    return this.currentMeeting?.isFinished;
  }

  set meetingClosed(value) {
    this.currentMeeting.colsed = value;
  }

  get meetingClosed() {
    return this.currentMeeting?.colsed;
  }

  addCoordinator(userIds) {
    const coordinators = new ListOfMeetingUserDTO({
      userDTO: userIds,
    });
    return this.swagger.apiMeetingsInsertMeetingMultiAttendeesOrCoordinatorsPost(
      coordinators
    );
  }

  addAttendee(userIds) {
    const Attendees = new ListOfMeetingUserDTO({
      userDTO: userIds,
    });
    return this.swagger.apiMeetingsInsertMeetingMultiAttendeesOrCoordinatorsPost(
      Attendees
    );
  }

  deleteParticipant(userId: number, meetingId: number, userType: number) {
    return this.swagger.apiMeetingsDeleteMeetingAttendeesOrCoordinatorPost(
      userId,
      meetingId,
      userType
    );
  }
  deleteMOM(momId: number) {
    return this.swagger.apiMinuteOfMeetingsDeleteDelete(momId);
  }
  getUserMeetings(userId: number, userType: UserType) {
    return this.swagger.apiMeetingsGetMeetingUserAvailabilityGet(
      userId,
      this.meeting.id,
      userType
    );
  }

  sendInvitationOrReplyInvitation(
    userId: number,
    userType: UserType,
    state: AttendeeState
  ) {
    return this.swagger.apiMeetingsChangeMeetingAttendeesOrCoordinatorStatePost(
      userId,
      this.meeting.id,
      userType,
      state
    );
  }

  changeMeetingApprovalState(state: boolean) {
    return this.swagger.apiMeetingsChangeApproveManualGet(
      this.meeting.id,
      state
    );
  }

  changeCoordinatorRequiredToAttend(
    userId: number,
    meetingId: number,
    type: UserType
  ) {
    return this.swagger.apiMeetingsToogleCoordinatorOrAttendeeConfirmMeetingAttendancePut(
      userId,
      meetingId,
      type
    );
  }

  getCommitteeName(committeeId: number) {
    return this.swagger.apiCommiteesGetCommitteeNamesByIdGet(committeeId);
  }

  isAttendeeOrCoordinator(id: number) {

    return [
      ...this.meeting.meetingAttendees,
    ].find((x) => x.attendeeId === id) || [
      ...this.meeting.meetingCoordinators,
    ].find((x) => x.coordinatorId === id);
  }
}
