import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainTasksComponent } from './main-tasks.component';
import { GroupListComponent } from './tasks-group/group-list/group-list.component';
import { GroupComponent } from './tasks-group/group/group.component';
import { RouterOutletComponent } from '../shared/_components/router-outlet.component';
import { TasksStatisticsComponent } from './tasks-dashboard/tasks-statistics/tasks-statistics.component';
import { TaskDetailsComponent } from './tasks-dashboard/task-details/task-details.component';


const routes: Routes = [
  { path: '', component: MainTasksComponent },
  {path:'taskStatistics',component:TasksStatisticsComponent,  data: {breadcrumb: 'taskStatistics'},},
  {
    path: 'taskgroup',
    component: RouterOutletComponent,
    data: {breadcrumb: 'taskgroup'},
    children:[
      {
        path: '',
        component: GroupListComponent,
        data: { permissionCode: ['taskgroup'] },
      },
      {
        path: 'add',
        component: GroupComponent,
        data: {
          permissionCode: ['CREATENEWGROUP'],
          breadcrumb: 'CreateNewGroup',
        },
      },
      {
        path: 'edit/:GroupId',
        component: GroupComponent,
        data: { permissionCode: ['EDITGROUP'] },
      },
    ]
  },
  {
    path: ':id',
    component: TaskDetailsComponent,
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TasksRoutingModule {}
