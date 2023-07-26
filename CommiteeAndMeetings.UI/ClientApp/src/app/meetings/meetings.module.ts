import { MultiMeetingResolverService } from './schdule-multiple-meetings/multi-meeting-resolver.service';
import { AddAttendeesComponent } from './schedule-meeting/add-attendees/add-attendees.component';
import { MeetingssRoutingModule } from './meetings-routing.module';
import { NgModule } from '@angular/core';
import { MeetingsDashboardComponent } from './meetings-dashboard/meetings-dashboard.component';
import { ScheduleMeetingComponent } from './schedule-meeting/schedule-meeting.component';
import { SharedModule } from '../shared/shared.module';
import { AgendaComponent } from './schedule-meeting/agenda/agenda.component';
import { MinutesOfMeetingComponent } from './schedule-meeting/minutes-of-meeting/minutes-of-meeting.component';
import { MeetingsListComponent } from './schedule-meeting/meetings-list/meetings-list.component';
import { GroupMeetingDataComponent } from './schedule-meeting/group-meetings-data/group-meeting-data.component';
import { MeetingsComponent } from './meetings.component';
import { MeetingDataComponent } from './meeting-data/meeting-data.component';
import { MultipleMeetingsComponent } from './schdule-multiple-meetings/multiple-meetings/multiple-meetings.component';
import { SingleMeetingComponent } from './schedule-meeting/single-meeting/single-meeting.component';
import { MeetingControlsComponent } from './meeting-controls/meeting-controls.component';
import { CreatorDetailsComponent } from './schedule-meeting/creator-details/creator-details.component';
import { InvitedUsersComponent } from './schedule-meeting/invited-users/invited-users.component';
import { ConfirmAllmeetingsComponent } from './meetings-dashboard/confirm-allmeetings/confirm-allmeetings.component';
import { ClosedMeetingsComponent } from './meetings-dashboard/closed-meetings/closed-meetings.component';
import { MeetingsCalenderComponent } from './meetings-dashboard/meetings-calender/meetings-calender.component';
import { FinishedMeetingsComponent } from './meetings-dashboard/finished-meetings/finished-meetings.component';
import { MeetingsActivitiesComponent } from './meetings-dashboard/meetings-activities/meetings-activities.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { UserScheduleComponent } from './schedule-meeting/add-attendees/user-schedule/user-schedule.component';
import { UsersListComponent } from './schedule-meeting/add-attendees/users-list/users-list.component';
import { SearchUsersComponent } from './schedule-meeting/search-users/search-users.component';
import { UserItemComponent } from './schedule-meeting/add-attendees/users-list/user-item/user-item.component';
import { AddTobicComponent } from './schedule-meeting/agenda/add-tobic/add-tobic.component';
import { TobicItemComponent } from './schedule-meeting/agenda/tobic-item/tobic-item.component';
import { VotingListComponent } from './schedule-meeting/agenda/tobic-item/voting-list/voting-list.component';
import { RecommendationListComponent } from './schedule-meeting/agenda/tobic-item/recommendation-list/recommendation-list.component';
import { TopicTimelineComponent } from './schedule-meeting/agenda/tobic-item/topic-timeline/topic-timeline.component';
import { TopicTimingControlComponent } from './schedule-meeting/agenda/tobic-item/topic-timing-control/topic-timing-control.component';
import { TopicCommentsComponent } from './schedule-meeting/agenda/tobic-item/topic-comments/topic-comments.component';
import { TopicVotingItemComponent } from './schedule-meeting/agenda/tobic-item/voting-list/voting-item/voting-item.component';
import { AddMOMItemComponent } from './schedule-meeting/minutes-of-meeting/add-mom-item/add-mom-item.component';
import { MinuetsOfMeetingItemComponent } from './schedule-meeting/minutes-of-meeting/minuets-of-meeting-item/minuets-of-meeting-item.component';
import { MinuteCommentsComponent } from './schedule-meeting/minutes-of-meeting/minuets-of-meeting-item/minute-comments/minute-comments.component';
import { TopicVotingComponent } from './meetings-dashboard/meetings-activities/topic-voting/topic-voting.component';
import { RecommendationItemComponent } from './schedule-meeting/minutes-of-meeting/recommendation-item/recommendation-item.component';
import { AddRecommendationComponent } from './schedule-meeting/minutes-of-meeting/add-recommendation/add-recommendation.component';
import { PrintContainerComponent } from './meetings-dashboard/printreferral/print-container/print-container.component';
import { SchduleMultipleMeetingsComponent } from './schdule-multiple-meetings/schdule-multiple-meetings.component';
import { MeetingListComponent } from './schdule-multiple-meetings/meeting-list/meeting-list.component';
import { VotingTimeItemComponent } from './schedule-meeting/agenda/tobic-item/topic-timeline/voting-time-item/voting-time-item.component';
import { RecomendUserComponent } from './meetings-dashboard/confirm-allmeetings/recomend-user/recomend-user.component';
import { PrintRecommendationsComponent } from './meetings-dashboard/print-recommendations/print-recommendations.component';


@NgModule({
  declarations: [
    MeetingListComponent,
    MeetingsDashboardComponent,
    ScheduleMeetingComponent,
    AgendaComponent,
    MinutesOfMeetingComponent,
    MeetingsListComponent,
    GroupMeetingDataComponent,
    AddAttendeesComponent,
    MeetingDataComponent,
    MultipleMeetingsComponent,
    SingleMeetingComponent,
    MeetingsComponent,
    MeetingControlsComponent,
    CreatorDetailsComponent,
    InvitedUsersComponent,
    ConfirmAllmeetingsComponent,
    ClosedMeetingsComponent,
    MeetingsCalenderComponent,
    FinishedMeetingsComponent,
    MeetingsActivitiesComponent,
    UserScheduleComponent,
    UsersListComponent,
    SearchUsersComponent,
    UserItemComponent,
    AddTobicComponent,
    TobicItemComponent,
    VotingListComponent,
    RecommendationListComponent,
    TopicTimelineComponent,
    TopicTimingControlComponent,
    TopicCommentsComponent,
    TopicVotingItemComponent,
    AddMOMItemComponent,
    MinuetsOfMeetingItemComponent,
    MinuteCommentsComponent,
    TopicVotingComponent,
    RecommendationItemComponent,
    AddRecommendationComponent,
    PrintContainerComponent,
    SchduleMultipleMeetingsComponent,
    VotingTimeItemComponent,
    RecomendUserComponent,
    PrintRecommendationsComponent,

  ],
  imports: [
    SharedModule,
    MeetingssRoutingModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
  ],
  providers: [MultiMeetingResolverService]
})
export class MeetingsModule {}
