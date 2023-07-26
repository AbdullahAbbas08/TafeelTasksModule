import { CommitteeService } from 'src/app/committees/committee.service';
import { TranslateService } from '@ngx-translate/core';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import {
  CommentDTO,
  CommiteeMemberDTO,
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
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
export class MultiTask {
  constructor(
    public label: string,
    public value: number,
    public checked: boolean,
    public endSubTask: Date
  ) { }
}

@Component({
  selector: 'app-task-item',
  templateUrl: './task-item.component.html',
  styleUrls: ['./task-item.component.scss'],
})
export class TaskItemComponent implements OnInit {
  @Input() task: any;
  @Input() periodState: number;
  @Input() CommitteName: string;
  @Input() isForGeneralTasks = false;
  checkComponent: boolean = false
  commentText = '';
  taskComments = [];
  commentAttachment: SavedAttachmentDTO[] = []
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
  dateNow: Date = new Date()
  multiTasks: MultiTask[] = [];
  checkAttachmentIcon: boolean = true;
  taskMissions: CommiteetaskMultiMissionDTO[] = [];
  editGroupTaskPermissions: boolean;
  isloadingComment: boolean = false;
  taskCompleteFlag: boolean = false;
  createdOnDate: Date;
  offset: number;
  isCollapsed = false;
  userId: any;
  permittedShowComments: boolean = false;
  committeeId: any
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
    private router: Router,
    public dateService: NgbDateService,
    private route: ActivatedRoute,
  ) {

  }


  ngOnInit(): void {
    this.userId = this.authService.getUser().userId;
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id')))
    this.offset = new Date().getTimezoneOffset();
    this.langChange();
    this.taskComments = this.task.taskComments;
    this.count = this.taskComments.length;
    this.checkEditGroupPermission()
    if (this.task.taskAttachments) {
      this.task.taskAttachments.forEach((attachment) =>
        this.attachments.push(attachment.attachment)
      );
    }
    this.createdOnDate = this.dateService.getTimeZoneOffset(this.task.createdOn);

    this.setCreatedByUserDetails();
    this.showItem();
    this.showCompleteTask();
    this.checkPermissions()
    this.committeeActive = this.committeeService.getCommitteeCurrentState();

    if (this.task.multiMission)
      this.multiTasks = this.mapMultiMissionsToMultiTasks(
        this.task.multiMission
      );
    if (this.task.completeReasonDate.getFullYear() < 1900) {
      this.taskCompleteFlag = false
    } else {
      this.taskCompleteFlag = true
    }
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
  parseToArabic(str) {
    str = str.toString();
    return str.replace(/[0-9]/g, function (d) {
      return String.fromCharCode(d.charCodeAt(0) + 1584) // Convert To Arabic Numbers
    });
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
  editGroup() {
    this.modalService.initEditTaskGroup(this.task);
  }
  setCreatedByUserDetails() {
    if (this.task.createdByUser) {
      this.createdUserName =
        this.currentLang === 'ar'
          ? this.task.createdByUser?.fullNameAr
          : this.task.createdByUser?.fullNameEn;

      this.createdUserImage = this.task.createdByUser?.profileImage;
      if (this.task.createdByRole) {
        // this.createdUserTitle =
        //   this.currentLang === 'ar'
        //     ? this.task.createdByRole.role?.commiteeRolesNameAr
        //     : this.task.createdByRole.role?.commiteeRolesNameEn;
        // let changeStats = this.committees.find((committee) => committee.commiteeId === id);
        if (this.committeeService.CommitteMembers) {
          let users: CommiteeMemberDTO[] = this.committeeService.CommitteMembers?.filter((user) => user.userId == this.task.createdByUser.userId);
          this.createdUserTitle =
            this.currentLang === 'ar' ?
              users[0].commiteeRoles[0].role.commiteeRolesNameAr :
              users[0].commiteeRoles[0].role.commiteeRolesNameEn
        }

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

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedByUserDetails();
    });
  }

  mapMultiMissionsToMultiTasks(missions: CommiteetaskMultiMissionDTO[]) {
    return missions.map((mission) => {
      return new MultiTask(
        mission.name,
        mission.commiteeTaskMultiMissionId,
        mission.state,
        mission.endDateMultiMission,
      );
    });
  }

  mapMultiTasksToMultiMissions(tasks: MultiTask[]) {
    return tasks.map((task) => {
      return new CommiteetaskMultiMissionDTO({
        name: task.label,
        commiteeTaskMultiMissionId: task.value,
        state: task.checked,
        commiteeTaskId: this.task.commiteeTaskId,
        endDateMultiMission: task.endSubTask
      });
    });
  }

  updateMultiTasks(data, multimissionId) {
    // const missions = this.task.multiMission.map((res) => {
    //   if(res.commiteeTaskMultiMissionId === multimissionId){
    //      return {...res,state:data}
    //   } else {
    //     return res
    //   }
    // })
    // this.task.multiMission = missions;
    this.taskService.updateMutiTasksForTask(this.BrowserService.encrypteString(multimissionId)).subscribe();
  }

  userPermittedToReopenGeneralTask() {
    return (
      (this.task.createdByUser.userId == this.authService.getUser().userId ||
        this.mainAssinedUser) &&
      this.task?.completed &&
      this.isForGeneralTasks
    );
  }

  userPermittedToReopenCommitteeTask() {
    return (
      (this.task.createdByUser.userId == this.authService.getUser().userId ||
        this.mainAssinedUser) &&
      this.task?.completed &&
      !this.isForGeneralTasks &&
      this.committeeService.checkPermission('ReopenTask')
    );
  }

  isCreatedByOrAssignedUser() {
    return (
      this.task.createdByUser.userId == this.authService.getUser().userId ||
      this.mainAssinedUser
    );
  }

  userPermittedToEditGeneralTask() {
    return (
      (this.task.createdByUser.userId == this.authService.getUser().userId ||
        this.mainAssinedUser) &&
      !this.task?.completed &&
      this.isForGeneralTasks
    );
  }

  userPermittedToEditCommitteeTask() {
    return (
      (this.task.createdByUser.userId == this.authService.getUser().userId ||
        this.mainAssinedUser) &&
      !this.task?.completed &&
      !this.isForGeneralTasks &&
      this.committeeService.checkPermission('EditTask')
    );
  }
  userPermittedToEditGroupTask() {
    return (this.task.createdByUser.userId == this.authService.getUser().userId || this.mainAssinedUser) && this.editGroupTaskPermissions && !this.task?.completed
  }
  checkEditGroupPermission() {
    this.editGroupTaskPermissions = this.authService.isAuthUserHasPermissions(["tasksGroupPage"]);
  }
  changeTaskStatus(index: number) {
    return (this.task.createdByUser.userId == this.authService.getUser().userId || this.mainAssinedUser ||
      this.task.multiMission[index].commiteeTaskMultiMissionUserDTOs.some((user) => user.userId == this.authService.getUser().userId))
  }
  isMainOrAssistantOrUser(index: number) {
    return (
      this.mainAssinedUser || this.task.createdByUser.userId == this.authService.getUser().userId || this.task.multiMission[index].commiteeTaskMultiMissionUserDTOs.some((q) => { return q.userId == this.authService.getUser().userId })
    );
  }
  checkIsArchived() {
    return (
      (this.periodState !== 2 && this.committeeActive) || this.isForGeneralTasks
    )
  }
  navigateTo(taskId) {
    const id = this.BrowserService.encrypteString(taskId);
    this.router.navigate(["/tasks/", id])
  }
  checkPermissions() {
    if (this.committeeId) {
      if (this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers?.some((el) => el.userId === +this.userId)) {
        this.permittedShowComments = true;
      } else {
        this.permittedShowComments = false;
      }
    } else {
      this.permittedShowComments = true
    }
  }
}
