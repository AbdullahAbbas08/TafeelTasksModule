import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { AuthguardService } from './auth/authguard.service';
import { TaskguardService } from './TaskGuard.service';

const routes: Routes = [
  { path: '', redirectTo: '/committees', pathMatch: 'full'},
  {
    path: 'committees',
    loadChildren: () =>
      import('./committees/committees.module').then((m) => m.CommitteesModule),

  },
  {
    path: 'meetings',
    loadChildren: () =>
      import('./meetings/meetings.module').then((m) => m.MeetingsModule),
  },
  {
    path: 'settings',
    loadChildren: () =>
      import('./settings/settings.module').then((m) => m.SettingsModule),
  },
  {
    path: 'tasks',
    loadChildren: () =>
      import('./tasks/tasks.module').then((m) => m.TasksModule),
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then((m) => m.AuthModule),
  },
  { path: '**', redirectTo: '/committees', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
