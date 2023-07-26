import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { SettingsRoutingModule } from './settings.routing.module';

import { SettingsWrapperComponent } from './settings-wrapper.component';
import { SettingsListComponent } from './settings-list/settings-list.component';
import { PermissionComponent } from './permissions/permission/permission.component';
import { PermissionsListComponent } from './permissions/permissions-list/permissions-list.component';
import { RolesListComponent } from './roles/roles-list/roles-list.component';
import { RoleComponent } from './roles/role/role.component';
import { LocalizationsListComponent } from './localizations/localizations-list/localizations-list.component';
import { LocalizationComponent } from './localizations/localization/localization.component';
import { CommitteeTypeComponent } from './types/committee-type/committee-type.component';
import { TypesListComponent } from './types/types-list/types-list.component';
import { StatusListComponent } from './statuses/status-list/status-list.component';
import { StatusComponent } from './statuses/status/status.component';
import { CategoriesListComponent } from './categories/categories-list/categories-list.component';
import { CategoryComponent } from './categories/category/category.component';
import { ProjectsListComponent } from './projects/projects-list/projects-list.component';
import { ProjectComponent } from './projects/project/project.component';
import { LetterTemplateListComponent } from './letter-templates/letter-template-list/letter-template-list.component';
import { LetterTemplateComponent } from './letter-templates/letter-template/letter-template.component';
import { TasksListComponent } from './tasks-classifications/tasks-classifications-list/tasks-list.component';
import { TasksComponent } from './tasks-classifications/tasks/tasks.component';
import { SystemSettingsComponent } from './system-settings/system-settings.component';
import { EscalationComponent } from './escalation/escalation.component';
import { CreateEditEscalationComponent } from './escalation/create-edit-escalation/create-edit-escalation.component';
import { UsersListComponent } from './users/user-list/users-list.component';
import { UserComponent } from './users/user/user.component';

@NgModule({
  declarations: [
    SettingsWrapperComponent,
    SettingsListComponent,
    PermissionsListComponent,
    PermissionComponent,
    RolesListComponent,
    RoleComponent,
    LocalizationsListComponent,
    LocalizationComponent,
    CommitteeTypeComponent,
    TypesListComponent,
    TasksListComponent,
    TasksComponent,
    StatusListComponent,
    StatusComponent,
    CategoriesListComponent,
    CategoryComponent,
    ProjectsListComponent,
    ProjectComponent,
    LetterTemplateListComponent,
    LetterTemplateComponent,
    SystemSettingsComponent,
    EscalationComponent,
    CreateEditEscalationComponent,
    UsersListComponent,
    UserComponent
  ],
  imports: [SettingsRoutingModule, SharedModule],
})
export class SettingsModule {}
