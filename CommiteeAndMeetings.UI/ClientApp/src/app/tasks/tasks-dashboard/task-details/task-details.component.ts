import { CommitteeService } from 'src/app/committees/committee.service';
import { TranslateService } from '@ngx-translate/core';
import { Component, EventEmitter, Input, OnInit, Output, AfterViewInit } from '@angular/core';

import {
  CommentDTO,
  CommiteeTaskDTO,
  CommiteetaskMultiMissionDTO,
  SavedAttachmentDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { AuthService } from 'src/app/auth/auth.service';
import { CommentsService } from 'src/app/committees/committee-details/comments/comments.service';
import { TasksService } from '../../tasks.service';
import { AttachmentsService } from 'src/app/committees/committee-details/attachments/attachments.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { ActivatedRoute } from '@angular/router';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';

export class MultiTask {
  constructor(
    public label: string,
    public value: number,
    public checked: boolean
  ) { }
}

@Component({
  selector: 'app-task-details',
  templateUrl: './task-details.component.html',
  styleUrls: ['./task-details.component.scss'],
})
export class TaskDetailsComponent implements OnInit, AfterViewInit {
  task: any;
  checkComponent: boolean = false;
  taskId: any;
  commentText = '';
  taskComments = [];
  commentAttachment: SavedAttachmentDTO[] = [];
  count;
  loadingData = false;
  visible;
  currentLang: string;
  showItemFlag = false;
  mainAssinedUser = false;
  createdUserName: string;
  createdUserTitle: string;
  createdUserImage: string;
  committeeActive;
  attachments: SavedAttachmentDTO[] = [];
  dateNow: Date = new Date();
  multiTasks: MultiTask[] = [];
  checkAttachmentIcon: boolean = true;
  taskMissions: CommiteetaskMultiMissionDTO[] = [];
  editGroupTaskPermissions: boolean;
  isloadingComment: boolean = false;
  taskCompleteFlag: boolean = false;
  createdOnDate: Date;
  offset: number;
  isloading: boolean = false;
  constructor(
    private commentService: CommentsService,
    private swagger: SwaggerClient,
    private translateService: TranslateService,
    private authService: AuthService,
    public committeeService: CommitteeService,
    private modalService: SharedModalService,
    private taskService: TasksService,
    private AttachmentService: AttachmentsService,
    private BrowserService: BrowserStorageService,
    private route: ActivatedRoute,
    private layoutService: LayoutService,
    public dateService: NgbDateService
  ) { }
  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id');
    this.getTaskDetails();
  }
  ngAfterViewInit() {

  }
  getTaskDetails() {
    this.layoutService.toggleSpinner(true);
    this.isloading = true
    this.taskService.getTaskDetails(this.taskId).subscribe((res) => {
      if (res) {
        if (res.mainAssinedUserId == +this.authService.getUser().userId || res.createdBy == +this.authService.getUser().userId || res.multiMission.some((z) => { return z.commiteeTaskMultiMissionUserDTOs.some((q) => { return q.userId == +this.authService.getUser().userId }) })) {
          this.task = res;
          this.offset = new Date().getTimezoneOffset();
          this.langChange();
          this.taskComments = res.taskComments;
          this.count = this.taskComments.length;
          this.checkEditGroupPermission()
          if (res.taskAttachments) {
            res.taskAttachments.forEach((attachment) =>
              this.attachments.push(attachment.attachment)
            );
          }
          this.createdOnDate = this.getTimeWithTimeZone(res.createdOn);

          this.setCreatedByUserDetails();
          this.showItem();
          this.showCompleteTask();
          this.committeeActive = this.committeeService.getCommitteeCurrentState();

          if (this.task.multiMission)
            this.multiTasks = this.mapMultiMissionsToMultiTasks(
              res.multiMission
            );
          if (this.task.completeReasonDate.getFullYear() < 1900) {
            this.taskCompleteFlag = false
          } else {
            this.taskCompleteFlag = true
          }
          this.layoutService.toggleSpinner(false);
          this.isloading = false
        } else {
          this.layoutService.toggleSpinner(false);
          this.isloading = false;
          this.task = undefined
        }

      }
    })
  }
  addComment(commentObj: { comment: CommentDTO; id: number, attachmentFiles: File[] }) {
    this.checkComponent = true;
    this.isloadingComment = true;
    this.commentService
      .postTaskComment(commentObj.comment, commentObj.id)
      .subscribe((value) => {
        if (commentObj.attachmentFiles.length) {
          this.count += 1;
          this.taskService.postCommentAttachment(commentObj.attachmentFiles, value[0].commentId).subscribe((res: SavedAttachmentDTO[]) => {
            if (res) {
              res.forEach((attachment) => {
                value[0].comment.savedAttachments.push(attachment)
              })
              this.isloadingComment = false;
              this.taskComments.push({ ...value[0], justInserted: true });
              this.AttachmentService.addAttachmentWithComment.next(value[0].comment.savedAttachments);
              this.checkComponent = false;
            }
          })
        } else {
          this.isloadingComment = false
          this.taskComments.push({ ...value[0], justInserted: true });
          this.count += 1;
        }

      });
  }

  onToggleCompleted() {
    this.modalService.initToggleTaskCompleted(this.task);
    // this.swagger
    //   .apiCommiteeTasksCompleteGet(this.task.commiteeTaskId)
    //   .subscribe((res) => {
    //     if (res) this.task.completed = !this.task.completed;
    //   });
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedByUserDetails();
    });
  }
  setCreatedByUserDetails() {
    if (this.task.createdByUser) {
      this.createdUserName =
        this.currentLang === 'ar'
          ? this.task.createdByUser?.fullNameAr
          : this.task.createdByUser?.fullNameEn;

      this.createdUserImage = this.task.createdByUser?.profileImage;
      if (this.task.createdByRole) {
        this.createdUserTitle =
          this.currentLang === 'ar'
            ? this.task.createdByRole.role?.commiteeRolesNameAr
            : this.task.createdByRole.role?.commiteeRolesNameEn;
      }
    } else if (this.task.justInserted) {
      let user = this.authService.getUser();
      this.createdUserName =
        this.currentLang === 'ar' ? user.fullNameAr : user.fullNameEn;
      this.createdUserImage = user.userImage;
      if (this.task.createdByRole) {
        this.createdUserTitle =
          this.currentLang === 'ar'
            ? this.task.createdByRole.role?.commiteeRolesNameAr
            : this.task.createdByRole.role?.commiteeRolesNameEn;
      }
    }
  }
  checkEditGroupPermission() {
    this.editGroupTaskPermissions = this.authService.isAuthUserHasPermissions(["tasksGroupPage"]);
  }
  getTimeWithTimeZone(date: Date) {
    var targetTime = new Date(date),
      tzDifference = targetTime.getTimezoneOffset(),
      offsetTime = new Date(targetTime.getTime() + tzDifference * 60 * 1000);
    return offsetTime
  }
  showItem() {
    let userId: number = this.authService.getUser().userId;

    if (
      this.task.createdBy == userId ||
      this.task.mainAssinedUserId == userId ||
      this.task.isShared ||
      this.task.taskToView
    )
      this.showItemFlag = true;
    if (
      this.task.assistantUsers.find((user) => {
        return user.userId == userId;
      })
    )
      this.showItemFlag = true;
    if (this.task.taskGroups.map((group) => {
      group.group.groupUsers.find((user) => { return user.userId == userId })
    })
    )
      this.showItemFlag = true
  }
  showCompleteTask() {
    let userId: number = this.authService.getUser().userId;
    if (this.task.mainAssinedUserId == userId) this.mainAssinedUser = true;
  }
  editTask() {
    this.modalService.initModalTask(this.BrowserService.encrypteString(this.task.commiteeId), this.task);
  }
  editHistory() {
    this.modalService.initEditTaskHistory(
      this.task.commiteeId,
      this.task.commiteeTaskId
    );
  }
  mapMultiMissionsToMultiTasks(missions: CommiteetaskMultiMissionDTO[]) {
    return missions.map((mission) => {
      return new MultiTask(
        mission.name,
        mission.commiteeTaskMultiMissionId,
        mission.state
      );
    });
  }
  editGroup() {
    this.modalService.initEditTaskGroup(this.task);
  }
  mapMultiTasksToMultiMissions(tasks: MultiTask[]) {
    return tasks.map((task) => {
      return new CommiteetaskMultiMissionDTO({
        name: task.label,
        commiteeTaskMultiMissionId: task.value,
        state: task.checked,
        commiteeTaskId: this.task.commiteeTaskId,
      });
    });
  }
  updateMultiTasks(data, multimissionId) {
    const missions = this.task.multiMission.map((res) => {
      if (res.commiteeTaskMultiMissionId === multimissionId) {
        return { ...res, state: data }
      } else {
        return res
      }
    })
    this.task.multiMission = missions;
    this.taskService.updateMutiTasksForTask(this.task.multiMission, undefined, undefined).subscribe();
  }
  userPermittedToReopenGeneralTask() {
    return (
      (this.task.createdByUser.userId == this.authService.getUser().userId ||
        this.mainAssinedUser) &&
      this.task?.completed
    );
  }
  userPermittedToEditGroupTask() {
    return (this.task.createdByUser.userId == this.authService.getUser().userId || this.mainAssinedUser) && this.editGroupTaskPermissions && !this.task?.completed
  }
  isMainOrAssistantOrUser(index: number) {
    return (
      this.mainAssinedUser || this.task.createdByUser.userId == this.authService.getUser().userId || this.task.multiMission[index].commiteeTaskMultiMissionUserDTOs.some((q) => { return q.userId == this.authService.getUser().userId })
    );
  }
  isCreatedByOrAssignedUser() {
    return (
      this.task.createdByUser.userId == this.authService.getUser().userId ||
      this.mainAssinedUser
    );
  }
}
