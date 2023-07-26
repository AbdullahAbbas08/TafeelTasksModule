import {
  CommiteeAttachmentDTO,
  CommiteeTaskDTO,
  SurveyDTO,
} from './../../../core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

export class TaskTimelineItemDTO extends CommiteeTaskDTO {
  type?: string;
}
export class AttachmentTimelineItemDTO extends CommiteeAttachmentDTO {
  type?: string;
}
export class VotingTimelineItemDTO extends SurveyDTO {
  type?: string;
}

@Injectable({
  providedIn: 'root',
})
export class TimeLineService {
  tasks: TaskTimelineItemDTO[] = [];
  attachments: AttachmentTimelineItemDTO[] = [];
  votings: VotingTimelineItemDTO[] = [];

  resultTimelineItems: (CommiteeTaskDTO | CommiteeAttachmentDTO | SurveyDTO)[];

  constructor(private swagger: SwaggerClient) {}

  getCommiteWallItems(
    take: number,
    skip: number,
    dateFrom: Date,
    dateTo: Date,
    committeeId: string,
    searchText: string
  ) {
    return this.swagger
      .apiCommiteesGetCommitteWallGet(
        take,
        skip,
        dateFrom,
        dateTo,
        committeeId,
        searchText,
        false
      )
      .pipe(
        map((res) => {
          if (res) {
            if (res.tasks.data.length) {
              this.tasks = res.tasks.data.map((item) => {
                return {
                  ...item,
                  type: 'Task',
                } as TaskTimelineItemDTO;
              });
            } else this.tasks = [];
            if (res.attachments.data.length) {
              this.attachments = res.attachments.data.map((item) => {
                return {
                  ...item,
                  type: 'Attachment',
                } as AttachmentTimelineItemDTO;
              });
            } else this.attachments = [];
            if (res.surveys.data.length) {
              this.votings = res.surveys.data.map((item) => {
                return {
                  ...item,
                  type: 'Voting',
                } as VotingTimelineItemDTO;
              });
            } else this.votings = [];
            this.resultTimelineItems = [
              ...this.attachments,
              ...this.votings,
              ...this.tasks,
            ];
            return {
              result: this.resultTimelineItems.sort(
                (
                  a: CommiteeAttachmentDTO | SurveyDTO | CommiteeTaskDTO,
                  b: CommiteeAttachmentDTO | SurveyDTO | CommiteeTaskDTO
                ) => {
                  return b.createdOn.getTime() - a.createdOn.getTime();
                }
              ),
              count:
                res.tasks.count + res.surveys.count + res.attachments.count,
            };
          }
        })
      );
  }
}
