import { MultiMeetingResolverService } from './schdule-multiple-meetings/multi-meeting-resolver.service';
import { ScheduleMeetingComponent } from './schedule-meeting/schedule-meeting.component';
import { MeetingsDashboardComponent } from './meetings-dashboard/meetings-dashboard.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthguardService } from '../auth/authguard.service';
import { MultipleMeetingsComponent } from './schdule-multiple-meetings/multiple-meetings/multiple-meetings.component';
import { SingleMeetingComponent } from './schedule-meeting/single-meeting/single-meeting.component';
import { MeetingsComponent } from './meetings.component';
import { SingleMeetingResolverService } from './schedule-meeting/single-meeting/single-meeting-resolver.service';
import { SchduleMultipleMeetingsComponent } from './schdule-multiple-meetings/schdule-multiple-meetings.component';

const routes: Routes = [
  {
    path: '',
    component: MeetingsComponent,
    canActivate: [AuthguardService],
    canActivateChild: [AuthguardService],
    children: [
      {
        path: '',
        component: MeetingsDashboardComponent,
        canActivate: [AuthguardService],
        canActivateChild: [AuthguardService],
      },
      {
        path: 'schedule-meeting',
        component: ScheduleMeetingComponent,
        canActivate: [AuthguardService],
        canActivateChild: [AuthguardService],
        children: [
          {
            path: '',
            component: SingleMeetingComponent,
          },
          {
            path: ':id',
            component: SingleMeetingComponent,
            data: { breadcrumb: 'MeetingDetails' },
            resolve: [SingleMeetingResolverService],
          },
          {
            path: 'committee/:committeeId',
            component: SingleMeetingComponent,
          }
        ],
      },
      {
        path: 'multiple-meeting',
        component: SchduleMultipleMeetingsComponent,
        canActivate: [AuthguardService],
        canActivateChild: [AuthguardService],
        children: [
          {
            path: '',
            component: MultipleMeetingsComponent,
          },
          {
            path: ':refId',
            component: MultipleMeetingsComponent,
            data: { breadcrumb: 'MeetingDetails' },
            resolve: { meetingList: MultiMeetingResolverService },
          },
          {
            path: 'committee/:committeeId',
            component: MultipleMeetingsComponent,
          }
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MeetingssRoutingModule {}
