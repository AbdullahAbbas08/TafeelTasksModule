import {
  AllSurveyDTO,
  SurveyAnswerUserDTO,
  SurveyDTODataSourceResult,
} from './../../../core/_services/swagger/SwaggerClient.service';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {
  API_BASE_URL,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VotingService {
  baseUrl: string;
  commiteeId: number;
  updateVotings = new Subject<any>();

  constructor(
    private http: HttpClient,
    private swaggerService: SwaggerClient,
    @Inject(API_BASE_URL) baseUrl: string
  ) {
    this.baseUrl = baseUrl;
  }

  getVotings(
    take: number,
    skip: number,
    committeeId: string,
    dateFrom:Date,
    dateTo:Date,
    searchText:string
    // filters: any[] = [],
    // filteLogic = undefined
  ): Observable<AllSurveyDTO> {
    return this.swaggerService.apiSurveysGetAllServeyGet(
      take,
      skip,
      committeeId,
      dateFrom,
      dateTo,
      searchText
    );
  }

  postVoting(
    files: File[],
    subject: string,
    multi: boolean,
    isShared: boolean,
    selectedUsers,
    votingOptions,
    committeeId,
    meetingTopicId,
    meetingId,
    selectedDate
  ) {
    let surveyUsers = undefined;
    let surveyAnswers = undefined;

    if (selectedUsers) {
      surveyUsers = this.processDataString(selectedUsers);
    }

    if (votingOptions) {
      surveyAnswers = this.processDataString(votingOptions);
    }

    const postData = new FormData();
    if (files) files.forEach((file) => postData.append(file.name, file));

    postData.append('subject', subject);
    postData.append('multi', multi ? 'true' : 'false');
    postData.append('isShared', isShared ? 'true' : 'false');
    postData.append('surveyUsers', surveyUsers);
    postData.append('surveyAnswers', surveyAnswers);
    postData.append('selectedDate',selectedDate)
    if (committeeId) postData.append('commiteeId', committeeId);
    if (meetingTopicId) postData.append('meetingTopicId', meetingTopicId);
    if (meetingId){
      postData.delete('commiteeId');
      postData.append('meetingId', meetingId);
    } 
   
    return this.http.post(
      `${this.baseUrl}/api/Document/UploadAttachmentToSurvey`,
      postData
    );
  }

  processDataString(array: any[]): string {
    let result = '';

    array.forEach((element) => {
      result += `${element},`;
    });
    result = result.slice(0, -1);
    return result;
  }

  sendVoteAnswer(answers: SurveyAnswerUserDTO[]) {
    return this.swaggerService.apiSurveyAnswerUsersInsertCustomePost(answers);
  }

  getVoteAnswerUsers(take: number, skip: number, answerId: number) {
    return this.swaggerService.apiSurveyAnswerUsersGetAllGet(
      take,
      skip,
      undefined,
      'surveyAnswerId',
      'eq',
      `${answerId}`,
      undefined,
      [],
      false
    );
  }
}
