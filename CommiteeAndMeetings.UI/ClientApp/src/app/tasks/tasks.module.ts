import { TasksRoutingModule } from './tasks-routing.module';
import { MainTasksComponent } from './main-tasks.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { EditHistoryComponent } from './tasks-dashboard/edit-task-history/edit-history.component';
import { TaskItemComponent } from './tasks-dashboard/task-item/task-item.component';
import { TasksStatisticsComponent } from './tasks-dashboard/tasks-statistics/tasks-statistics.component';
import { TasksComponent } from './tasks-dashboard/tasks.component';
import { ToggleTaskComponent } from './tasks-dashboard/toggleTaskCompleted/toggle-task.component';
import { GroupListComponent } from './tasks-group/group-list/group-list.component';
import { GroupComponent } from './tasks-group/group/group.component';
import { EditGroup } from './tasks-dashboard/edit-group/edit-group.component';
import { TaskDetailsComponent } from './tasks-dashboard/task-details/task-details.component';

@NgModule({
  declarations: [MainTasksComponent,TasksComponent,EditHistoryComponent,TaskItemComponent,TasksStatisticsComponent,ToggleTaskComponent,GroupListComponent,GroupComponent,EditGroup,TaskDetailsComponent],
  imports: [CommonModule, TasksRoutingModule,SharedModule],
  exports:[TasksComponent,EditHistoryComponent,TaskItemComponent,TasksStatisticsComponent,GroupListComponent,GroupComponent,EditGroup,TaskDetailsComponent]
})
export class TasksModule {}
