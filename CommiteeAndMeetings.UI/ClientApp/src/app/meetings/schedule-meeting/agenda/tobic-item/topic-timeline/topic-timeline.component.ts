import { Subscription } from 'rxjs';
import {
  MeetingTopicDTO,
  SurveyDTO,
  TopicState,
} from './../../../../../core/_services/swagger/SwaggerClient.service';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import {
  CommentDTO,
  CommentType,
  TopicCommentDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { AgendaService } from '../../agenda.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { distinctUntilKeyChanged } from 'rxjs/operators';

@Component({
  selector: 'app-topic-timeline',
  templateUrl: './topic-timeline.component.html',
  styleUrls: ['./topic-timeline.component.scss'],
})
export class TopicTimelineComponent implements OnInit, OnDestroy {
  @Input() topic: MeetingTopicDTO;
  @Input() isCoordinator;
  @Input() isCreator;
  @Input() isMeetingClosed;
  @Input() isMeetingCanceled;
  commentCount: number;
  topicTimelineItems: (SurveyDTO | TopicCommentDTO)[] = [];
  currentLang: string;
  commentType = CommentType;
  topicState = TopicState;
  subscriptionComments: Subscription;
  subscriptionVoting: Subscription;

  constructor(
    private agendaService: AgendaService,
    public translateService: TranslateService,
    private storeService: StoreService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.getTopicTimeline();
    this.subscriptionVoting = this.storeService.refreshTopicVotings$
      .pipe(distinctUntilKeyChanged('surveyId'))
      .subscribe((voting) => {
        if (voting && voting.meetingTopicId === this.topic.id)
          this.topicTimelineItems.push(
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
    this.subscriptionComments =
      this.storeService.refreshTopicCommentsRecommendations$
        .pipe(distinctUntilKeyChanged('id'))
        .subscribe((item: TopicCommentDTO) => {
          if (item && item.topicId === this.topic.id) {
            this.topicTimelineItems.push(item);
            this.commentCount += 1;
          }
        });
  }

  ngOnDestroy() {
    this.subscriptionComments.unsubscribe();
    this.subscriptionVoting.unsubscribe();
  }

  addComment(commentObj: { comment: CommentDTO; id: number }) {
    this.agendaService
      .addComment(commentObj.comment, commentObj.id, CommentType._1)
      .subscribe((res) => {
        this.storeService.refreshTopicCommentsRecommendations$.next(res[0]);
      });
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }

  getTopicTimeline() {
    this.agendaService.getTopicWallItems(this.topic.id).subscribe((res) => {
      if (res) {
        this.topicTimelineItems = [...res];
        this.commentCount = this.topicTimelineItems.filter((item) => {
          return item instanceof TopicCommentDTO;
        }).length;
      }
    });
  }
}
