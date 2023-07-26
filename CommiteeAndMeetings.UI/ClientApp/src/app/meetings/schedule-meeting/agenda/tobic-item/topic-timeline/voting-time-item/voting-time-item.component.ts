import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { distinctUntilKeyChanged } from 'rxjs/operators';
import { AuthService } from 'src/app/auth/auth.service';
import { VotingService } from 'src/app/committees/committee-details/votes/voting.service';
import {
  SavedAttachmentDTO,
  SurveyAnswerUserDTO,
  SurveyDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { StoreService } from 'src/app/shared/_services/store.service';

@Component({
  selector: 'app-voting-time-item',
  templateUrl: './voting-time-item.component.html',
  styleUrls: ['./voting-time-item.component.scss'],
})
export class VotingTimeItemComponent implements OnInit {
  @Input('item') item: SurveyDTO;
  @Input() topicId: number;
  attachments: SavedAttachmentDTO[] = [];
  voteAnswer: string[] = [];
  targetIndex = 0;
  loadingData = false;
  userId: number;
  createdUserName: string;
  createdUserImage: string;
  currentLang: string;
  userParticipated = false;

  subscription: Subscription;

  constructor(
    private votingService: VotingService,
    private authService: AuthService,
    public translateService: TranslateService,
    private store: StoreService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.item?.attachments.forEach((attachment) =>
      this.attachments.push(attachment.attachment)
    );
    this.userId = this.authService.getUser().userId;
    this.currentLang = this.translateService.currentLang;
    this.setCreatedByUserDetails();
    this.checkIfUserVoted();

    this.store.refreshAnswerUsersTimeline$
      .pipe(distinctUntilKeyChanged('firstId'))
      .subscribe((res) => {
        if (res && res.topicId === this.topicId) {
          res.answerUsers?.forEach((item) => {
            this.item.surveyAnswers
              .filter((ans) => ans.surveyAnswerId === item.surveyAnswerId)
              .forEach((answer) => answer.surveyAnswerUsers.unshift(item));
          });
        }
      });

    this.subscription = this.store.votingParticipation$.subscribe((id) => {
      this.item.surveyId === id && (this.userParticipated = true);
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  sendVote() {
    if (this.voteAnswer.length < 1) return;
    let surveyAnswer: SurveyAnswerUserDTO[] = this.voteAnswer.map(
      (answerId) => {
        return new SurveyAnswerUserDTO({
          userId: this.userId,
          surveyAnswerId: +answerId,
        });
      }
    );

    this.votingService.sendVoteAnswer(surveyAnswer).subscribe((res) => {
      if (res) {
        res.forEach((item) => {
          this.item.surveyAnswers
            .filter((ans) => ans.surveyAnswerId === item.surveyAnswerId)
            .forEach((answer) => answer.surveyAnswerUsers.unshift(item));
        });

        this.userParticipated = true;
        this.store.votingParticipation$.next(this.item.surveyId);
      }
    });
  }

  setCreatedByUserDetails() {
    if (this.item.createdByUser) {
      this.createdUserName =
        this.currentLang === 'ar'
          ? this.item?.createdByUser.fullNameAr
          : this.item?.createdByUser.fullNameEn;
      this.createdUserImage = this.item?.createdByUser.profileImage;
    }
  }

  checkIfUserVoted() {
    this.item.surveyAnswers.forEach((answer) => {
      answer.surveyAnswerUsers.forEach((user) => {
        if (user.userId == this.userId) {
          this.userParticipated = true;
        }
      });
    });
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedByUserDetails();
    });
  }
}
