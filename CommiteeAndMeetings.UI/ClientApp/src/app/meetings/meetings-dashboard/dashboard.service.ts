import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';
import { SwaggerClient,LookUpDTO,MeetingDetailsDTO, DataSourceRequest, MeetingDetailsDTODataSourceResult, MeetingUserAvailabilityDTO, MeetingActivityLookup, SurveyDTO, DisplayMeetingCallType} from 'src/app/core/_services/swagger/SwaggerClient.service';
export interface changeState {
  userId: any;
  meetingId: any;
  userType:any;
  userState:any;
}
@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  changeAttendeCordinatorState: BehaviorSubject<changeState> = new BehaviorSubject<changeState>(null);
  changeMeetingState: BehaviorSubject<number> = new BehaviorSubject<number>(null);
  cancelMeetingState:BehaviorSubject<number> = new BehaviorSubject<number>(null);
  constructor(private swaggerServce: SwaggerClient) { }

  getAllMeetings(dateFrom?:Date,dateTo?:Date,fromDashboard?:DisplayMeetingCallType):Observable<MeetingDetailsDTO[]>{
    return this.swaggerServce.apiMeetingsDisplayMeetingsGet(dateFrom,dateTo,fromDashboard)
  }
  getClosedMeetings(body: DataSourceRequest):Observable<MeetingDetailsDTODataSourceResult>{
    return this.swaggerServce.apiMeetingsDisplayClosedMeetingPost(body)
  }
  getAllActivities(body:DataSourceRequest):Observable<MeetingActivityLookup[]>{
    return this.swaggerServce.apiMeetingsGetAllActivitiesPost(body)
  }
  getSingleTopicVoting(id:string):Observable<SurveyDTO>{
    return this.swaggerServce.apiSurveysGetByIdGet(id)
  }
  getFinishedMeetings(body: DataSourceRequest):Observable<MeetingDetailsDTODataSourceResult>{
    return this.swaggerServce.apiMeetingsDisplayFinishedMeetingPost(body)
  }
  getMeetingsNameList(take:number,skip:number,text:string):Observable<LookUpDTO[]>{
     return this.swaggerServce.apiMeetingHeaderAndFootersGetListOfMeetingLookupGet(take,skip,text);
  }
  changeUserState(userId: number, meetingId: number,userType:number,state:number):Observable<MeetingUserAvailabilityDTO>{
    return this.swaggerServce.apiMeetingsChangeMeetingAttendeesOrCoordinatorStatePost(userId,meetingId,userType,state)
  }
  cancelMeeting(meetingId:number):Observable<Boolean>{
    return this.swaggerServce.apiMeetingsCanceledMeetingPost(meetingId)
  }
}
