import {
  AttachmentCommentDTO,
  CommentDTO,
  SurveyCommentDTO,
  SwaggerClient,
  TaskCommentDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CommentsService {
  take = 20;
  skip = 0;
  filters = [];

  constructor(private swagger: SwaggerClient) {}

  // Post Task Comment
  postTaskComment(comment: CommentDTO, taskId: number) {
    let taskComment = new TaskCommentDTO({ comment, taskId });

    return this.swagger.apiTaskCommentsInsertPost([taskComment]);
  }

  // Get Task Comments
  getTaskComments(taskId: number) {
    return this.swagger.apiTaskCommentsGetAllGet(
      this.take,
      this.skip,
      undefined,
      'taskId',
      'eq',
      `${taskId}`,
      undefined,
      this.filters,
      false
    );
  }

  // Post Voting Comment
  postVotingComment(comment: CommentDTO, surveyId: number) {
    let votingComment = new SurveyCommentDTO({ comment, surveyId });
    return this.swagger.apiSurveyCommentsInsertPost([votingComment]);
  }

  // Get Voting Comments
  getVotingComments(votingId: number) {
    return this.swagger.apiSurveyCommentsGetAllGet(
      this.take,
      this.skip,
      undefined,
      'surveyId',
      'eq',
      `${votingId}`,
      undefined,
      this.filters,
      false
    );
  }

  // Post Attachment Comment
  postAttachmentComment(comment: CommentDTO, attachmentId: number) {
    let attachmentComment = new AttachmentCommentDTO({
      comment,
      attachmentId,
    });
    return this.swagger.apiAttachmentCommentsInsertPost([attachmentComment]);
  }

  // Get Attachment Comments
  getAttachmentComments(attachmentId: number) {
    return this.swagger.apiAttachmentCommentsGetAllGet(
      this.take,
      this.skip,
      undefined,
      'attachmentId',
      'eq',
      `${attachmentId}`,
      undefined,
      this.filters,
      false
    );
  }
}
