import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AttachemntTypeCountDTO, CountResultDTO, LineChartDTO, MeetingSummaryDTO, SwaggerClient, UserTaskCountDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  constructor(private swaggerServce: SwaggerClient) { }

  getActivitiesPerMonth(committeId:string):Observable<LineChartDTO[]>{
    return this.swaggerServce.apiCommiteesGetActiviteyPerMonthGet(committeId)
  }
  getStatsPerUser(committeeId:string):Observable<UserTaskCountDTO[]>{
    return this.swaggerServce.apiCommiteesGetTasksPerUserGet(committeeId)
  }
  getAttacemntsPerType(committeeId:string):Observable<AttachemntTypeCountDTO[]>{
    return this.swaggerServce.apiCommiteesGetAttachemntPerTypeGet(committeeId)
  }
  getCommitteesMeetings(committeeId:string):Observable<MeetingSummaryDTO[]>{
    return this.swaggerServce.apiCommiteesGetMeetingsByCommitteIdGet(committeeId);
  }
  getCommiteeStatsCount(committeeId:string): Observable<CountResultDTO[]>{
    return this.swaggerServce.apiCommiteesCommitteStatisticGet(committeeId)
  }
}
