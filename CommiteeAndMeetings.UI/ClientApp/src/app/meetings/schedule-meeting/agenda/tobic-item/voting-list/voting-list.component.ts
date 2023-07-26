import { distinctUntilKeyChanged } from 'rxjs/operators';
import { StoreService } from 'src/app/shared/_services/store.service';
import { SurveyDTO } from './../../../../../core/_services/swagger/SwaggerClient.service';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { MeetingTopicDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-voting-list',
  templateUrl: './voting-list.component.html',
  styleUrls: ['./voting-list.component.scss'],
})
export class VotingListComponent implements OnInit, OnDestroy {
  @Input() topic: MeetingTopicDTO;
  @Input() isCoordinator;
  @Input() isCreator;
  @Input() isMeetingClosed;

  topicVotings: SurveyDTO[] = [];
  currentLang: string;
  subscriptionVoting: Subscription;

  constructor(
    private translateService: TranslateService,
    private storeService: StoreService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.topicVotings = this.topic.topicSurveies;
    this.subscriptionVoting = this.storeService.refreshTopicVotings$
      .pipe(distinctUntilKeyChanged('surveyId'))
      .subscribe((voting: SurveyDTO) => {
        if (voting && voting.meetingTopicId === this.topic.id)
          this.topicVotings.push(
            new SurveyDTO({
              surveyId: voting.surveyId,
              subject: voting.subject,
              multi: voting.multi,
              surveyAnswers: voting.surveyAnswers,
              attachments: voting.attachments,
              comments: voting.comments,
              surveyUsers: voting.surveyUsers,
              meetingTopicId: voting.meetingTopicId,
              meetingId: voting.meetingId,
              createdByUser: voting.createdByUser,
              createdOn: voting.createdOn,
              surveyEndDate:voting.surveyEndDate
            })
          );
      });
  }

  ngOnDestroy() {
    this.subscriptionVoting.unsubscribe();
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
}
