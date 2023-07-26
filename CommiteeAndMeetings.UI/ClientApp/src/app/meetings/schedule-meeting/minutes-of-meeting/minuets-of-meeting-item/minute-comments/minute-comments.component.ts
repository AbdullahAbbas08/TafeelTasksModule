import { MOMService } from './../../mom.service';
import { CommentDTO, CommentType, MinuteOfMeetingDTO, MOMCommentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-minute-comments',
  templateUrl: './minute-comments.component.html',
  styleUrls: ['./minute-comments.component.scss']
})
export class MinuteCommentsComponent implements OnInit {
  @Input() minute: MinuteOfMeetingDTO;
  @Input() meetingClosed;
  count: number;
  minuteComments: MOMCommentDTO[] = [];
  currentLang: string;

  constructor(
    private momService: MOMService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.langChange();
    this.minuteComments = this.minute.momComment;
    this.count = this.minuteComments.length;
  }

  addComment(commentObj: { comment: CommentDTO; id: number }) {

    this.momService
      .addComment(commentObj.comment, commentObj.id, CommentType._1)
      .subscribe((res) => {
        this.minuteComments.push(res[0]);
        this.count += 1;
      });
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }

}
