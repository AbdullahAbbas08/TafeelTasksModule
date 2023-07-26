import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CommitteesRoutingModule } from './committees-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CommitteesListComponent } from './committees-list/committees-list.component';
import { CommitteesTreeComponent } from './committees-tree/committees-tree.component';
import { CommitteeDetailsComponent } from './committee-details/committee-details.component';
import { SideMenuComponent } from './committee-details/side-menu/side-menu.component';
import { TimeLineComponent } from './committee-details/time-line/time-line.component';
import { StatisticsComponent } from './committee-details/statistics/statistics.component';
import { AttachmentsComponent } from './committee-details/attachments/attachments.component';
import { UsersComponent } from './committee-details/users/users.component';
import { VotesComponent } from './committee-details/votes/votes.component';
import { TransactionsComponent } from './committee-details/transactions/transactions.component';
import { CreateCommitteeComponent } from './create-committee/create-committee.component';
import { OnlineUsersComponent } from './committee-details/online-users/online-users.component';
import { SpecificControlsComponent } from './committee-details/specific-controls/specific-controls.component';
import { TimelineItemComponent } from './committee-details/time-line/timeline-item/timeline-item.component';
import { CreateAttachmentComponent } from './committee-details/attachments/create-attachment/create-attachment.component';
import { AttachmentItemComponent } from './committee-details/attachments/attachment-item/attachment-item.component';
import { VotingItemComponent } from './committee-details/votes/voting-item/voting-item.component';
import { FilterPipe } from '../shared/_pipes/filter.pipe';
import { CreateUsersComponent } from './committee-details/users/create-users/create-users.component';
import { UserSpecificControlsComponent } from './committee-details/users/user-specific-controls/user-specific-controls.component';
import { ExternalUsersComponent } from './committee-details/users/external-users/external-users.component';
import { PagesComponent } from '../pages/pages.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { TransactionComponent } from './committee-details/statistics/transaction/transaction.component';
import { DescisionsComponent } from './committee-details/statistics/descisions/descisions.component';
import { MembersComponent } from './committee-details/statistics/members/members.component';
import { ActivitiesComponent } from './committee-details/statistics/activities/activities.component';
import { MeetingsReminderComponent } from './committee-details/statistics/meetings-reminder/meetings-reminder.component';
import { TasksStatsComponent } from './committee-details/statistics/tasks-stats/tasks-stats.component';
import { StatsFilesComponent } from './committee-details/statistics/stats-files/stats-files.component';
import { DelgateUserComponent } from './committee-details/users/delgate-user/delgate-user.component';
import { InboxComponent } from './committee-details/transactions/inbox/inbox.component';
import { OutboxComponent } from './committee-details/transactions/outbox/outbox.component';
import { CreateTransactionComponent } from './committee-details/transactions/create-transaction/create-transaction.component';
import { TransactionAttachmentFilesComponent } from './committee-details/transactions/transaction-attachment-files/transaction-attachment-files.component';
import { TransactionAttachmentItemComponent } from './committee-details/transactions/transaction-attachment-item/transaction-attachment-item.component';
import { TransactionControlsComponent } from './committee-details/transaction-controls/transaction-controls.component';
import { ConverterPipe } from './committees-tree/converter.pipe';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { TasksModule } from '../tasks/tasks.module';
import { CommitteTasksStatsComponent } from './committee-details/statistics/committe-tasks-stats/committe-tasks-stats.component';
import { EditUserPermissionsComponent } from './committee-details/users/edit-user-permissions/edit-user-permissions.component';
@NgModule({
  declarations: [
    DashboardComponent,
    CommitteesListComponent,
    CommitteesTreeComponent,
    CommitteeDetailsComponent,
    SideMenuComponent,
    TimeLineComponent,
    StatisticsComponent,
    AttachmentsComponent,
    UsersComponent,
    VotesComponent,
    TransactionsComponent,
    CreateCommitteeComponent,
    OnlineUsersComponent,
    SpecificControlsComponent,
    TimelineItemComponent,
    CreateAttachmentComponent,
    FilterPipe,
    CreateUsersComponent,
    UserSpecificControlsComponent,
    ExternalUsersComponent,
    PagesComponent,
    AttachmentItemComponent,
    VotingItemComponent,
    TransactionComponent,
    DescisionsComponent,
    MembersComponent,
    ActivitiesComponent,
    MeetingsReminderComponent,
    TasksStatsComponent,
    StatsFilesComponent,
    DelgateUserComponent,
    InboxComponent,
    OutboxComponent,
    DelgateUserComponent,
    CreateTransactionComponent,
    TransactionAttachmentFilesComponent,
    TransactionAttachmentItemComponent,
    TransactionControlsComponent,
    ConverterPipe,
    CommitteTasksStatsComponent,
    EditUserPermissionsComponent
  ],
  imports: [
    SharedModule,
    CommitteesRoutingModule,
    TasksModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
  ],
})
export class CommitteesModule {}
