import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-roles-list',
  template:'<app-setting-business-list [options]="options"></app-setting-business-list>',
})
export class RolesListComponent {
  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.ROLE,
      controllerId: 'commiteeRoleId',
      columns: [
        { name: 'RoleNameAr', field: 'commiteeRolesNameAr', searchable: true, operator: 'contains' },
        { name: 'RoleNameEn', field: 'commiteeRolesNameEn', searchable: true, operator: 'contains' },
        { name: 'isMangerRole', field: 'isMangerRole' }
      ]
    };
  }

}
