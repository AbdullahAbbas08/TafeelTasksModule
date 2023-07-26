import { CreateEditEscalationComponent } from './escalation/create-edit-escalation/create-edit-escalation.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthguardService } from '../auth/authguard.service';
import { RouterOutletComponent } from '../shared/_components/router-outlet.component';
import { CategoriesListComponent } from './categories/categories-list/categories-list.component';
import { CategoryComponent } from './categories/category/category.component';
import { EscalationComponent } from './escalation/escalation.component';
import { LetterTemplateListComponent } from './letter-templates/letter-template-list/letter-template-list.component';
import { LetterTemplateComponent } from './letter-templates/letter-template/letter-template.component';
import { LocalizationComponent } from './localizations/localization/localization.component';
import { LocalizationsListComponent } from './localizations/localizations-list/localizations-list.component';
import { PermissionComponent } from './permissions/permission/permission.component';
import { PermissionsListComponent } from './permissions/permissions-list/permissions-list.component';
import { ProjectComponent } from './projects/project/project.component';
import { ProjectsListComponent } from './projects/projects-list/projects-list.component';
import { RoleComponent } from './roles/role/role.component';
import { RolesListComponent } from './roles/roles-list/roles-list.component';
import { SettingsListComponent } from './settings-list/settings-list.component';
import { SettingsWrapperComponent } from './settings-wrapper.component';
import { StatusListComponent } from './statuses/status-list/status-list.component';
import { StatusComponent } from './statuses/status/status.component';
import { SystemSettingsComponent } from './system-settings/system-settings.component';
import { TasksListComponent } from './tasks-classifications/tasks-classifications-list/tasks-list.component';
import { TasksComponent } from './tasks-classifications/tasks/tasks.component';
import { CommitteeTypeComponent } from './types/committee-type/committee-type.component';
import { TypesListComponent } from './types/types-list/types-list.component';
import { UsersListComponent } from './users/user-list/users-list.component';
import { UserComponent } from './users/user/user.component';


const routes: Routes = [
  {
    path: '',
    component: SettingsWrapperComponent,
    canActivate: [AuthguardService],
    canActivateChild: [AuthguardService],
    children: [
      { path: '', component: SettingsListComponent },
      {
        path: 'permissions',
        component: RouterOutletComponent,
        data: { breadcrumb: 'Permissions' },
        children: [
          {
            path: '',
            component: PermissionsListComponent,
            data: { permissionCode: ['PERMISSIONSPAGE'] },
          },
          {
            path: 'add',
            component: PermissionComponent,
            data: {
              permissionCode: ['CREATENEWPERMISSION'],
              breadcrumb: 'CreateNewPermission',
            },
          },
          {
            path: 'edit/:permissionId',
            component: PermissionComponent,
            data: { permissionCode: ['EDITPERMISSION'] },
          },
        ],
      },
      {
        path: 'roles',
        component: RouterOutletComponent,
        data: { breadcrumb: 'CommitteeRoles' },
        children: [
          {
            path: '',
            component: RolesListComponent,
            data: { permissionCode: ['COMMITTEEROLESPAGE'] },
          },
          {
            path: 'add',
            component: RoleComponent,
            data: {
              permissionCode: ['CREATENEWCOMMITTEEROLE'],
              breadcrumb: 'CreateNewCommitteeRole',
            },
          },
          {
            path: 'edit/:committeeRoleId',
            component: RoleComponent,
            data: { permissionCode: ['EDITCOMMITTEEROLE'] },
          },
        ],
      },
      {
        path: 'types',
        component: RouterOutletComponent,
        data: { breadcrumb: 'CommitteeTypes' },
        children: [
          {
            path: '',
            component: TypesListComponent,
            data: { permissionCode: ['COMMITTEETYPESPAGE'] },
          },
          {
            path: 'add',
            component: CommitteeTypeComponent,
            data: {
              permissionCode: ['CREATENEWCOMMITTEETYPE'],
              breadcrumb: 'CreateNewCommitteeType',
            },
          },
          {
            path: 'edit/:committeeTypeId',
            component: CommitteeTypeComponent,
            data: { permissionCode: ['EDITCOMMITTEETYPE'] },
          },
        ],
      },
      {
        path: 'task-classifications',
        component: RouterOutletComponent,
        data: { breadcrumb: 'ComiteeTaskCategory' },
        children: [
          {
            path: '',
            component: TasksListComponent,
            data: { permissionCode: ['tasksClassifications'] },
          },
          {
            path: 'add',
            component: TasksComponent,
            data: {
              permissionCode: ['CREATECOMITEETASKCATEGORY'],
              breadcrumb: 'CreateComiteeTaskCategory',
            },
          },
          {
            path: 'edit/:comiteeTaskCategoryId',
            component: TasksComponent,
            data: { permissionCode: ['EDITCOMITEETASKCATEGORY'] },
          },
        ],
      },
      {
        path: 'system-settings',
        component: RouterOutletComponent,
        data: {
          breadcrumb: 'SystemSettings',
          permissionCode: ['ViewSystemSettings'],
        },
        children: [
          {
            path: '',
            component: SystemSettingsComponent,
            data: {
              permissionCode: ['ViewSystemSettings'],
            },
          },
        ],
      },
      {
        path: 'statuses',
        component: RouterOutletComponent,
        data: { breadcrumb: 'CommitteeStatuses' },
        children: [
          {
            path: '',
            component: StatusListComponent,
            data: { permissionCode: ['COMMITTEESTATUSPAGE'] },
          },
          {
            path: 'add',
            component: StatusComponent,
            data: {
              permissionCode: ['CREATENEWCOMMITTEESTATUS'],
              breadcrumb: 'CreateNewCommitteeStatus',
            },
          },
          {
            path: 'edit/:committeeStatusId',
            component: StatusComponent,
            data: { permissionCode: ['EDITCOMMITTEESTATUS'] },
          },
        ],
      },
      {
        path: 'categories',
        component: RouterOutletComponent,
        data: { breadcrumb: 'CommitteeCategories' },
        children: [
          {
            path: '',
            component: CategoriesListComponent,
            data: { permissionCode: ['COMMITTEECATEGPRYPAGE'] },
          },
          {
            path: 'add',
            component: CategoryComponent,
            data: {
              permissionCode: ['CREATENEWCOMMITTEECATEGORY'],
              breadcrumb: 'CreateNewCommitteeCategory',
            },
          },
          {
            path: 'edit/:categoryId',
            component: CategoryComponent,
            data: { permissionCode: ['EDITCOMMITTEECATEGORY'] },
          },
        ],
      },
      {
        path: 'localizations',
        component: RouterOutletComponent,
        data: { breadcrumb: 'CommitteeLocalizations' },
        children: [
          {
            path: '',
            component: LocalizationsListComponent,
            data: { permissionCode: ['COMMITTEELOCALIZATIONSPAGE'] },
          },
          {
            path: 'add',
            component: LocalizationComponent,
            data: {
              permissionCode: ['CREATENEWCOMMITTEELOCALIZATION'],
              breadcrumb: 'CreateNewCommitteeLocalization',
            },
          },
          {
            path: 'edit/:committeeLocalizationId',
            component: LocalizationComponent,
            data: { permissionCode: ['EDITCOMMITTEELOCALIZATION'] },
          },
        ],
      },
      {
        path: 'Users',
        component: RouterOutletComponent,
        data: { breadcrumb: 'Users' },
        children: [
          {
            path: '',
            component: UsersListComponent,
            data: { permissionCode: ['Users'] },
          },
          {
            path: 'add',
            component: UserComponent,
            data: {
              permissionCode: ['CREATENEWUSER'],
              breadcrumb: 'CreateUser',
            },
          },
          {
            path: 'edit/:UserId',
            component: UserComponent,
            data: { permissionCode: ['EDITUSER'] },
          },
        ],
      },
      {
        path: 'projects',
        component: RouterOutletComponent,
        data: { breadcrumb: 'MeetingsProjects' },
        children: [
          {
            path: '',
            component: ProjectsListComponent,
            data: { permissionCode: ['MEETINGSPROJECTSPAGE'] },
          },
          {
            path: 'add',
            component: ProjectComponent,
            data: {
              permissionCode: ['CREATENEWMEETINGPROJECT'],
              breadcrumb: 'CreateNewMeetingProject',
            },
          },
          {
            path: 'edit/:projectId',
            component: ProjectComponent,
            data: { permissionCode: ['EDITMEETINGPROJECT'] },
          },
        ],
      },
      {
        path: 'letter-templates',
        component: RouterOutletComponent,
        data: { breadcrumb: 'MeetingsLetterTemplates' },
        children: [
          {
            path: '',
            component: LetterTemplateListComponent,
            data: { permissionCode: ['MEETINGSLETTERTEMPLATESPAGE'] },
          },
          {
            path: 'add',
            component: LetterTemplateComponent,
            data: {
              permissionCode: ['CREATENEWMEETINGLETTERTEMPLATE'],
              breadcrumb: 'CreateNewMeetingLetterTemplate',
            },
          },
          {
            path: 'edit/:letterId',
            component: LetterTemplateComponent,
            data: { permissionCode: ['EDITMEETINGLETTERTEMPLATE'] },
          },
        ],
      },
      {
        path: 'escalation',
        component: RouterOutletComponent,
        data: { breadcrumb: 'Escalation' },
        children: [
          {
            path: '',
            component: EscalationComponent,
          },
          {
            path: 'add',
            component: CreateEditEscalationComponent,
            data: { breadcrumb: 'Add' },
          },
          {
            path: 'edit/:escalationId',
            component: CreateEditEscalationComponent,
            data: { breadcrumb: 'Edit' },
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
export class SettingsRoutingModule {}
