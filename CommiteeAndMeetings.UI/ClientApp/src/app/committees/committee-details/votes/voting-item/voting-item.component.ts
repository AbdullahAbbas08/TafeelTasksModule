import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/auth/auth.service';
import { VotingService } from './../voting.service';
import {
  SavedAttachmentDTO,
  SurveyAnswerUserDTO,
} from './../../../../core/_services/swagger/SwaggerClient.service';
import { Component, Input, OnInit } from '@angular/core';
import { CommentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { CommentsService } from '../../comments/comments.service';
import { CommitteeService } from 'src/app/committees/committee.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateFormatterService } from 'ngx-hijri-gregorian-datepicker';

@Component({
  selector: 'app-voting-item',
  templateUrl: './voting-item.component.html',
  styleUrls: ['./voting-item.component.scss'],
})
export class VotingItemComponent implements OnInit {
  @Input('item') item: any;
  @Input() periodState: number;
  attachments: SavedAttachmentDTO[] = [];
  voteAnswer: string[] = [];
  targetIndex = 0;
  commentText = '';
  loadingData = false;
  votingComments = [];
  commentCount: number;
  userId: number;
  createdUserName: string;
  createdUserTitle: string;
  createdUserImage: string;
  currentLang: string;
  userParticipated = false;
  committeeActive;
  voteEndDate:Date;
  checkEndDate:boolean = false;
  checkUserPermissions:boolean = false;
  showCommentComponent:boolean = false
  constructor(
    private commentService: CommentsService,
    private votingService: VotingService,
    private authService: AuthService,
    private translateService: TranslateService,
    private committeeService: CommitteeService,
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.item.attachments.forEach((attachment) =>
      this.attachments.push(attachment.attachment)
    );
    this.voteEndDate = new Date(this.item.surveyEndDate)
    if(new Date > this.voteEndDate){
      this.checkEndDate = true 
    }
    this.userId = this.authService.getUser().userId;
    this.votingComments = this.item.comments ? this.item.comments : [];
    this.commentCount = this.votingComments.length;
    this.currentLang = this.translateService.currentLang;
    this.setCreatedByUserDetails();
    this.checkIfUserVoted();
    this.committeeActive = this.committeeService.getCommitteeCurrentState();
    this.checkPermissions();
    this.checkPermission()
  }
  checkPermission(){
    if(this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId)){
         if(this.periodState !== 2 && this.committeeActive){
          this.showCommentComponent = true
         } else {
          this.showCommentComponent = false
         }
    } else{
        this.showCommentComponent = false
      }
    }
  addComment(commentObj: { comment: CommentDTO; id: number }) {
    this.commentService
      .postVotingComment(commentObj.comment, commentObj.id)
      .subscribe((res) => {
        this.votingComments.push({ ...res[0], justInserted: true });
        this.commentCount += 1;
      });
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
      }
    });
  }

  setCreatedByUserDetails() {
    if (this.item.createdByUser) {
      this.createdUserName =
        this.currentLang === 'ar'
          ? this.item.createdByUser.fullNameAr
          : this.item.createdByUser.fullNameEn;

      this.createdUserImage = this.item.createdByUser.profileImage;
      if (this.item.createdByRole) {
        this.createdUserTitle =
          this.currentLang === 'ar'
            ? this.item.createdByRole.role.commiteeRolesNameAr
            : this.item.createdByRole.role.commiteeRolesNameEn;
      }
      return;
    } else if (this.item.justInserted) {
      let user = this.authService.getUser();
      this.createdUserName =
        this.currentLang === 'ar' ? user.fullNameAr : user.fullNameEn;
      this.createdUserImage = user.userImage;
      if (this.item.createdByRole) {
        this.createdUserTitle =
          this.currentLang === 'ar'
            ? this.item.createdByRole.role.commiteeRolesNameAr
            : this.item.createdByRole.role.commiteeRolesNameEn;
      }
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
  checkPermissions(){
    if(this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId)){
      this.checkUserPermissions = false
    } else {
      this.checkUserPermissions = true
    }
  }
}
