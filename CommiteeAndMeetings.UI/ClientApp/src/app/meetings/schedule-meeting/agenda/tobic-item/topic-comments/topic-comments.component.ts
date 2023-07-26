import { distinctUntilKeyChanged } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { AgendaService } from './../../agenda.service';
import {
  CommentDTO,
  CommentType,
  MeetingTopicDTO,
  TopicCommentDTO,
  TopicState,
} from './../../../../../core/_services/swagger/SwaggerClient.service';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { StoreService } from 'src/app/shared/_services/store.service';

@Component({
  selector: 'app-topic-comments',
  templateUrl: './topic-comments.component.html',
  styleUrls: ['./topic-comments.component.scss'],
})
export class TopicCommentsComponent implements OnInit, OnDestroy {
  @Input() topic: MeetingTopicDTO;
  @Input() isMeetingCanceled
  count: number;
  topicComments: TopicCommentDTO[] = [];
  currentLang: string;
  topicState = TopicState;
  subscriptionComments: Subscription;
  constructor(
    private agendaService: AgendaService,
    public translateService: TranslateService,
    private storeService: StoreService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.topicComments = this.topic.topicComments.filter(
      (comment) => comment.commentType === CommentType._1
    );
    this.count = this.topicComments.length;

    this.subscriptionComments =
      this.storeService.refreshTopicCommentsRecommendations$
        .pipe(distinctUntilKeyChanged('id'))
        .subscribe((item: TopicCommentDTO) => {
          if (
            item &&
            item.commentType === CommentType._1 &&
            item.topicId === this.topic.id
          ) {
            this.topicComments.push(item);
            this.count += 1;
          }
        });
  }

  ngOnDestroy() {
    this.subscriptionComments.unsubscribe();
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
}
