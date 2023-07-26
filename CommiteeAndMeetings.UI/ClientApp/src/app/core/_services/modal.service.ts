import { CommiteeTaskDTO, CommiteetaskMultiMissionDTO, MeetingAttendeeDTO, MeetingCoordinatorDTO, SwaggerClient, TransactionBoxDTO } from './swagger/SwaggerClient.service';
import { NzMessageService } from 'ng-zorro-antd/message';

import { CreateTransactionComponent } from './../../committees/committee-details/transactions/create-transaction/create-transaction.component';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NzDrawerRef, NzDrawerService } from 'ng-zorro-antd/drawer';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { CreateAttachmentComponent } from 'src/app/committees/committee-details/attachments/create-attachment/create-attachment.component';
import { CreateUsersComponent } from 'src/app/committees/committee-details/users/create-users/create-users.component';
import { DelgateUserComponent } from 'src/app/committees/committee-details/users/delgate-user/delgate-user.component';

import { CreateCommitteeComponent } from 'src/app/committees/create-committee/create-committee.component';
import {
  CommitteeActions,
  MeetingActions,
} from 'src/app/shared/_enums/AppEnums';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { ChangelangComponent } from '../header/changelang/changelang.component';
import { DeleteModelComponent } from 'src/app/shared/_components/setting-business-list/delete-model/delete-model.component';
import { CreateMeetingComponent } from 'src/app/shared/_components/create-meeting/create-meeting.component';
import { CreateVotingComponent } from 'src/app/shared/_components/create-voting/create-voting.component';
import { VoteUsersListComponent } from 'src/app/shared/_components/vote-users/vote-users-list/vote-users-list.component';
import { TopicVotingComponent } from 'src/app/meetings/meetings-dashboard/meetings-activities/topic-voting/topic-voting.component';
import { CreateTaskComponent } from 'src/app/tasks/tasks-dashboard/create-task/create-task.component';
import { EditHistoryComponent } from 'src/app/tasks/tasks-dashboard/edit-task-history/edit-history.component';
import { ExtendCommitteeComponent } from 'src/app/shared/_components/global-controls/extend-committee/extend-committee.component';
import { ConfirmArchivingComponent } from 'src/app/shared/_components/global-controls/confirm-archiving/confirm-archiving.component';
import { ToggleTaskComponent } from 'src/app/tasks/tasks-dashboard/toggleTaskCompleted/toggle-task.component';
import { EditGroup } from 'src/app/tasks/tasks-dashboard/edit-group/edit-group.component';
import { EditUserPermissionsComponent } from 'src/app/committees/committee-details/users/edit-user-permissions/edit-user-permissions.component';
import { FastDelegateModalComponent } from 'src/app/shared/_components/fast-delegate-modal/fast-delegate-modal.component';
import { CloseMeetingModalComponent } from 'src/app/shared/_components/close-meeting-modal/close-meeting-modal.component';
import { RecomendUserComponent } from 'src/app/meetings/meetings-dashboard/confirm-allmeetings/recomend-user/recomend-user.component';

@Injectable({
  providedIn: 'root',
})
export class SharedModalService {
  value: any;
  drawerRef: NzDrawerRef;
  modalRef: NzModalRef;
  modalTitle: string = '';

  constructor(
    private modalService: NzModalService,
    private drawerService: NzDrawerService,
    private translateService: TranslateService,
    private notification: NzNotificationService,
    private message: NzMessageService,
    private swagger:SwaggerClient
  ) {}

  destroyModal(): void {
    this.modalRef.destroy();
  }

  openDrawerModal<T>(
    type: number,
    committeeId?: any,
    userId?: number,
    commiteeMemberId?: number,
    committeeEndDate?: Date,
    topicId?: number,
    meetingId?: number,
    enableDecisions?: boolean,
    selectedDayViewDate?: Date,
    isCoordinator?: boolean,
    isCreator?: boolean,
    isMeetingClosed?: boolean,
    byPassStartForActivities?: boolean,
    departmentLink?:number,
    roleId?:number,
    isDelagted?:boolean,
    metaData?:T,
    fromMom?:boolean
  ) {
    switch (type) {
      case CommitteeActions.CreateNewCommittee:
        this.initDrawer();
        break;
      case MeetingActions.CreateNewMeeting:
        this.initModelCreateMeeting(committeeId);
        break;
      case MeetingActions.TopicVoting:
        this.translateService
          .get('Voting')
          .subscribe((text) => (this.modalTitle = text));
        this.initTopicVotingModel(
          committeeId,
          isCoordinator,
          isCreator,
          isMeetingClosed,
          byPassStartForActivities
        );
        break;
      case CommitteeActions.EditCommittee:
        if (committeeId) {
          this.initDrawer(committeeId);
        }
        break;
      case CommitteeActions.CreateAttachment:
        this.translateService
          .get('CreateAttachment')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalAttachment(committeeId);
        break;
      case CommitteeActions.CreateTask:
        this.translateService
          .get('CreateTask')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalTask(committeeId,undefined,meetingId);
        break;
      case CommitteeActions.CreateVote:
        this.translateService
          .get('CreateVoting')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalVoting(committeeId);
        break;
      case MeetingActions.CreateTopicVoting:
        this.translateService
          .get('CreateVoting')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalVoting(undefined, topicId);
        break;
      case MeetingActions.CreateMeetingVoting:
        this.translateService
          .get('CreateVoting')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalVoting(undefined, undefined, meetingId,fromMom);
        break;
    
      case CommitteeActions.CreateTransaction:
        this.translateService
          .get('CreateTransaction')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalTransaction(committeeId, enableDecisions,departmentLink);
        break;
        case CommitteeActions.FastDelegation:
          this.translateService
            .get('FastDelegation')
            .subscribe((text) => (this.modalTitle = text));
          this.initFastDelegateModal(metaData as unknown as TransactionBoxDTO);
          break;
      case CommitteeActions.CreateNewUser:
        this.translateService
          .get('CreateNewUser')
          .subscribe((text) => (this.modalTitle = text));
        this.initUserModel(committeeId);
        break;
        case CommitteeActions.EditCommitteUserPermissions:
          this.translateService
            .get('EditPermissions')
            .subscribe((text) => (this.modalTitle = text));
          this.initEditCommitteUserPermissions(committeeId,roleId,userId,isDelagted);
        break;
      case CommitteeActions.DelegateUser:
        this.translateService
          .get('DelegateUser')
          .subscribe((text) => (this.modalTitle = text));
        this.initDelegateUser(committeeId, userId, commiteeMemberId);
        break;
      case CommitteeActions.ExtendCommittee:
        this.translateService
          .get('ExtendCommitte')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalExtend(committeeId, committeeEndDate);
        break;
      case CommitteeActions.changeLang:
        this.translateService
          .get('changeLang')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalChangeLang();
        break;
      case CommitteeActions.deleteModel:
        this.translateService
          .get('confirmDelete')
          .subscribe((text) => (this.modalTitle = text));
        this.initModalDeleteModel();
        break;
      default:
        break;
    }
  }

  private initDrawer(committeeId?: any) {
    this.swagger.apiCommitteeMeetingSystemSettingCheckCredentialPost().subscribe((res) => {
      if(res) {
        this.drawerRef = this.drawerService.create<
        CreateCommitteeComponent,
        { value: any },
        any
      >({
        nzTitle: this.translateService.instant('CreateNewCommittee'),
        nzContent: CreateCommitteeComponent,
        nzKeyboard: false,
        nzMaskClosable: false,
        nzContentParams: {
          value: this.value,
          committeeId: committeeId,
        },
        nzFooter: null,
        nzWrapClassName: 'create-committee-drawer-wrapper',
      });
  
      } else {
        this.translateService
        .get('The number of allowed users has been exceeded. Please see your system administrator')
        .subscribe((translateValue) =>
          this.notification.success(translateValue, '')
        );
      }
    })

  }

  closeDrawer() {
    if (this.drawerRef) {
      this.drawerRef.close();
    } else if (this.modalRef) {
      this.modalRef.close();
    }
  }

  initModalVoting(
    committeId?: any,
    meetingTopicId?: number,
    meetingId?: number,
    fromMom?:boolean
  ) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: CreateVotingComponent,
      nzKeyboard: false,
      nzMaskClosable: false,
      nzClassName: 'my-modal',
      nzFooter: null,
      nzClosable: true,
      nzWidth: 450,
      nzComponentParams: {
        committeeId: committeId,
        meetingTopicId: meetingTopicId,
        meetingId: meetingId,
        fromMom:fromMom
      },
    });
  }

  initModalTransaction(committeId?: any, enableDecisions?: boolean,departmentLink?:number) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: CreateTransactionComponent,
      nzClassName: 'my-modal',
      nzKeyboard: false,
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 500,
      nzComponentParams: {
        committeeId: committeId,
        enableDecisions: enableDecisions,
        departmentLink:departmentLink
      },
    });
  }

  initFastDelegateModal(metaData:TransactionBoxDTO) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: FastDelegateModalComponent,
      nzClassName: 'my-modal',
      nzKeyboard: false,
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 500,
      nzComponentParams: {
        metaData: metaData,
      },
    });
  }

  initModalAttachment(committeId?: any) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: CreateAttachmentComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 420,
      nzComponentParams: {
        committeeId: committeId,
      },
    });
  }

  initModalTask(committeId?: any, task: CommiteeTaskDTO = undefined,meetingId?:number) {
    this.swagger.apiCommitteeMeetingSystemSettingCheckCredentialPost().subscribe((res) => {
      if(res){
        this.modalRef = this.modalService.create({
          nzTitle:
            task === undefined
              ? this.translateService.instant('CreateTask')
              : this.translateService.instant('editTask'),
          nzContent: CreateTaskComponent,
          nzKeyboard: false,
          nzClassName: 'my-modal',
          nzMaskClosable: false,
          nzFooter: null,
          nzWidth: 700,
          nzComponentParams: {
            committeeId: committeId,
            task,
            meetingId:meetingId
          },
        });
      } else {
        this.translateService
        .get('The number of allowed users has been exceeded. Please see your system administrator')
        .subscribe((translateValue) =>
          this.notification.success(translateValue, '')
        );
      }
    }
    )
   
  }
  initEditTaskHistory(committeId: any, committeTaskId: number) {
    this.modalRef = this.modalService.create({
      nzTitle: this.translateService.instant('editHistory'),
      nzContent: EditHistoryComponent,
      nzKeyboard: false,
      nzClassName: 'edit-History-model',
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 1300,
      nzComponentParams: {
        committeId: committeId,
        committeTaskId: committeTaskId,
      },
    });
  }
  initEditTaskGroup(task: CommiteeTaskDTO) {
    this.modalRef = this.modalService.create({
      nzTitle: this.translateService.instant('editgroup'),
      nzContent: EditGroup,
      nzKeyboard: false,
      nzClassName: 'edit-group',
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 600,
      nzComponentParams: {
        task:task
      },
    });
  }
  initToggleTaskCompleted(task) {
    this.modalRef = this.modalService.create({
      nzTitle: task.completed ? this.translateService.instant('ReOpen') : this.translateService.instant('MarkComplete'),
      nzContent: ToggleTaskComponent,
      nzKeyboard: false,
      nzClassName: this.translateService.currentLang === 'ar' ? 'toggle-task-model-Ar' : 'toggle-task-model-En',
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 500,
      nzComponentParams: {
        task: task
      },
    });
  }
  initUserModel(committeId?: any) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: CreateUsersComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: false,
      nzFooter: null,
      nzComponentParams: {
        committeId: committeId,
      },
    });
  }
  initEditCommitteUserPermissions(committeId?: any,roleId?:number,userId?:number,isDelagetd?:boolean) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: EditUserPermissionsComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 900,
      nzComponentParams: {
        committeId: committeId,
        roleId:roleId,
        userId:userId,
        isDelagetd:isDelagetd
      },
    });
  }
  initTopicVotingModel(
    surveyId: any,
    isCoordinator,
    isCreator,
    isMeetingClosed,
    byPassStartForActivities
  ) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: TopicVotingComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: false,
      nzFooter: null,
      nzComponentParams: {
        surveyId: surveyId,
        isCoordinator: isCoordinator,
        isCreator: isCreator,
        isMeetingClosed: isMeetingClosed,
        byPassStartForActivities: byPassStartForActivities,
      },
    });
  }
  initAnswerUsermodal(answerUsers, title) {
    this.modalRef = this.modalService.create({
      nzTitle: title,
      nzContent: VoteUsersListComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: false,
      nzFooter: null,
      nzWidth: 420,
      nzComponentParams: {
        answerUsers: answerUsers,
      },
    });
  }
  initDelegateUser(
    committeeId?: any,
    userId?: number,
    commiteeMemberId?: number
  ) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: DelgateUserComponent,
      nzKeyboard: false,
      nzMaskClosable: false,
      nzFooter: null,
      nzComponentParams: {
        committeId: committeeId,
        userId: userId,
        committeMemberId: commiteeMemberId,
      },
    });
  }

  initModalExtend(committeId?:any, committeeEndDate?: Date) {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: ExtendCommitteeComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzFooter: null,
      nzWidth: 420,
      nzComponentParams: {
        committeeId: committeId,
        committeeEndDate: committeeEndDate,
      },
    });
  }
  initModalChangeLang() {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: ChangelangComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzFooter: null,
      nzWidth: 450,
    });
  }
  initModalRecommendUsers(id:number,attendees:MeetingAttendeeDTO[],coordiantors:MeetingCoordinatorDTO[],userType, userState,userId,index) {
    this.modalRef = this.modalService.create({
      nzTitle: this.translateService.instant('refusedReason'),
      nzContent: RecomendUserComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzFooter: null,
      nzWidth: 600,
      nzComponentParams: {
        id:id,
        attendees:attendees,
        coordiantors:coordiantors,
        userType:userType,
        userState:userState,
        userId:userId,
      },
    });
  }
  initModalConfirmArchiving(
    committeeId: any,
    validityPeriodId: number,
    validityPeriodFrom: Date,
    createdOn:Date
  ) {
    this.modalRef = this.modalService.create({
      nzTitle:
        this.translateService.currentLang == 'ar'
          ? 'أرشفة الفترة'
          : 'Archive Period',
      nzContent: ConfirmArchivingComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzFooter: null,
      nzWidth: 450,
      nzComponentParams: {
        committeeId,
        validityPeriodId,
        validityPeriodFrom,
        createdOn
      },
    });
  }

  initModelCreateMeeting(committeeId?: number) {
    this.swagger.apiCommitteeMeetingSystemSettingCheckCredentialPost().subscribe((res) => {
      if(res){
        this.modalRef = this.modalService.create({
          nzContent: CreateMeetingComponent,
          nzKeyboard: false,
          nzClassName: 'my-modal',
          nzMaskClosable: true,
          nzFooter: null,
          nzWidth: 320,
          nzComponentParams: {
            committeeId,
          },
        });
      } else {
        this.translateService
        .get('The number of allowed users has been exceeded. Please see your system administrator')
        .subscribe((translateValue) =>
          this.notification.success(translateValue, '')
        );
      }
    })

  }
  initModalDeleteModel() {
    this.modalRef = this.modalService.create({
      nzTitle: this.modalTitle,
      nzContent: DeleteModelComponent,
      nzKeyboard: false,
      nzClassName: 'my-modal',
      nzMaskClosable: true,
      nzFooter: null,
      nzWidth: 450,
    });
  }
  createNotification(
    type: string,
    title: string,
    content: string = null
  ): void {
    let translatedTitle;
    let translatedContent = '';
    this.translateService
      .get(title)
      .subscribe((text) => (translatedTitle = text));

    if (content)
      this.translateService
        .get(content)
        .subscribe((text) => (translatedContent = text));

    this.notification.create(type, translatedTitle, translatedContent);
  }

  createMessage(type: string, content: string = null): void {
    let translatedContent;
    this.translateService.get(content).subscribe((text) => {
      translatedContent = text;
    });
    this.message.create(type, translatedContent);
  }
}
