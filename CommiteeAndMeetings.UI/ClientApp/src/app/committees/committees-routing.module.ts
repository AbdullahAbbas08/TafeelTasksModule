import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttachmentsComponent } from './committee-details/attachments/attachments.component';
import { StatisticsComponent } from './committee-details/statistics/statistics.component';

import { TimeLineComponent } from './committee-details/time-line/time-line.component';
import { TransactionsComponent } from './committee-details/transactions/transactions.component';
import { UsersComponent } from './committee-details/users/users.component';
import { VotesComponent } from './committee-details/votes/votes.component';
import { CommitteeDetailsComponent } from './committee-details/committee-details.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthguardService } from '../auth/authguard.service';
import { ValididyResolverService } from './committee-details/archiving/archiving.service';
import { InboxComponent } from './committee-details/transactions/inbox/inbox.component';
import { OutboxComponent } from './committee-details/transactions/outbox/outbox.component';
import { TasksComponent } from '../tasks/tasks-dashboard/tasks.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthguardService],
    canActivateChild: [AuthguardService],
    data: { breadcrumb: 'Committees' }
  },
  {
    path: ':id',
    component: CommitteeDetailsComponent,
    canActivate: [AuthguardService],
    canActivateChild: [AuthguardService],
    data: { breadcrumb: 'CommitteeDetails'},
    children: [
      {
        path: '',
        component: TimeLineComponent,
        resolve: [ValididyResolverService],
        data: { breadcrumb: 'timeline' }
      },
      {
        path: 'statistics',
        component: StatisticsComponent,
        data: { breadcrumb: 'statistics' }
      },
      {
        path: 'attachments',
        component: AttachmentsComponent,
        resolve: [ValididyResolverService],
        data: { breadcrumb: 'attachments' }
      },
      {
        path: 'users',
        component: UsersComponent,
        data: { breadcrumb: 'committeUsers' }
      },
      {
        path: 'tasks',
        component: TasksComponent,
        resolve: [ValididyResolverService],
        data: { breadcrumb: 'tasks' }
      },
      {
        path: 'votes',
        component: VotesComponent,
        resolve: [ValididyResolverService],
        data: { breadcrumb: 'votes' }
      },
      {
        path: 'transactions',
        component: TransactionsComponent,
        data: { breadcrumb: 'transactions' },
        children: [
          {
            path: 'inbox',
            component: InboxComponent,
            data: { breadcrumb: 'inbox' },
          },
          {
            path: 'outbox',
            component: OutboxComponent,
            data: { breadcrumb: 'outbox' },
          },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommitteesRoutingModule {}
