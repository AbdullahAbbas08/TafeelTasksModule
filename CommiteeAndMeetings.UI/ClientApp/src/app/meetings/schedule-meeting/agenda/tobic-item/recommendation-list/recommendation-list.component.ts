import { SingleMeetingService } from './../../../single-meeting.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { TranslateService } from '@ngx-translate/core';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import {
  CommentDTO,
  CommentType,
  MeetingTopicDTO,
  TopicCommentDTO,
  TopicState,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { AgendaService } from '../../agenda.service';
import { Subscription } from 'rxjs';
import { distinctUntilKeyChanged } from 'rxjs/operators';

@Component({
  selector: 'app-recommendation-list',
  templateUrl: './recommendation-list.component.html',
  styleUrls: ['./recommendation-list.component.scss'],
})
export class RecommendationListComponent implements OnInit, OnDestroy {
  @Input() topic: MeetingTopicDTO;
  @Input() isCoordinator;
  @Input() isCreator;

  count: number;
  recommendations: TopicCommentDTO[] = [];
  currentLang: string;

  meetingClosed = false;
  topicState = TopicState;
  subscriptionRecommendations: Subscription;

  constructor(
    private agendaService: AgendaService,
    public translateService: TranslateService,
    private storeService: StoreService,
    private singleMeetingService: SingleMeetingService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.recommendations = this.topic.topicComments.filter(
      (comment) => comment.commentType === CommentType._2
    );
    this.count = this.recommendations.length;

    this.subscriptionRecommendations =
      this.storeService.refreshTopicCommentsRecommendations$
        .pipe(distinctUntilKeyChanged('id'))
        .subscribe((item: TopicCommentDTO) => {
          if (
            item &&
            item.commentType === CommentType._2 &&
            item.topicId === this.topic.id
          ) {
            this.recommendations.push(item);
            this.count += 1;
          }
        });

    this.meetingClosed = this.singleMeetingService.meetingClosed;
  }

  ngOnDestroy() {
    this.subscriptionRecommendations.unsubscribe();
  }

  addRecommendation(commentObj: { comment: CommentDTO; id: number }) {
    this.agendaService
      .addComment(commentObj.comment, commentObj.id, CommentType._2)
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
}
