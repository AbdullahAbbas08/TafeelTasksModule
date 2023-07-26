import { Component, Input, OnInit } from '@angular/core';
import { SurveyDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DashboardService } from '../../dashboard.service';
import {BehaviorSubject} from 'rxjs'
import { AuthService } from 'src/app/auth/auth.service';
@Component({
  selector: 'app-topic-voting',
  templateUrl: './topic-voting.component.html',
  styleUrls: ['./topic-voting.component.scss']
})
export class TopicVotingComponent implements OnInit {
   surveyId:any;
   userId: number;
   topicVoting$: BehaviorSubject<SurveyDTO> = new BehaviorSubject<SurveyDTO>(null);
   isCoordinator:boolean;
   isCreator:boolean;
   isMeetingClosed:boolean;
   byPassStartForActivities:boolean;
   userParticipated = false;
  constructor(private _dashBoardService:DashboardService,private authService: AuthService) { }

  ngOnInit(): void {
    this.getSingleSurvey();
    this.userId = this.authService.getUser().userId;
  }
  getSingleSurvey(){
    this._dashBoardService.getSingleTopicVoting(this.surveyId).subscribe((survey) => {
      this.topicVoting$.next(survey);
      survey.surveyAnswers.forEach((answer) => {
        answer.surveyAnswerUsers.forEach((user) => {
          if (user.userId == this.userId) {
            this.userParticipated = true;
          }
        });
      });
    })
  }


}
