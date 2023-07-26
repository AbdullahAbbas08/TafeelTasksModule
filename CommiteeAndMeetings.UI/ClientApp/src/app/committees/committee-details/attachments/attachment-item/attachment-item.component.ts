import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/auth/auth.service';
import { CommitteeService } from 'src/app/committees/committee.service';
import { CommentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';

import { CommentsService } from '../../comments/comments.service';

@Component({
  selector: 'app-attachment-item',
  templateUrl: './attachment-item.component.html',
  styleUrls: ['./attachment-item.component.scss'],
})
export class AttachmentItemComponent implements OnInit {
  @Input('item') item: any;
  @Input() periodState: number;
  commentText = '';
  loadingData = false;
  attachmentComments = [];
  count;
  createdUserName: string;
  createdUserTitle: string;
  createdUserImage: string;
  currentLang: string;
  committeeActive;
  userId:number;
  showCommentComponent:boolean = false
  constructor(
    private commentService: CommentsService,
    private translateService: TranslateService,
    private authService: AuthService,
    private committeeService: CommitteeService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.userId = this.authService.getUser().userId;
    this.attachmentComments = this.item.attachmentComments;
    this.count = this.item.attachmentComments.length;
    this.setCreatedByUserDetails();
    this.committeeActive = this.committeeService.getCommitteeCurrentState();
    this.checkPermissions();
  }
  checkPermissions(){
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
      .postAttachmentComment(commentObj.comment, commentObj.id)
      .subscribe((res) => {
        this.attachmentComments.push({ ...res[0], justInserted: true });
        this.count += 1;
      });
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedByUserDetails();
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
            ? this.item.createdByRole.role?.commiteeRolesNameAr
            : this.item.createdByRole.role?.commiteeRolesNameEn;
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
            ? this.item.createdByRole.role?.commiteeRolesNameAr
            : this.item.createdByRole.role?.commiteeRolesNameEn;
      }
    }
  }
}
