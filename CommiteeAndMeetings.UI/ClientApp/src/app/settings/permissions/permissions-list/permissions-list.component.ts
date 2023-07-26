import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-permissions-list',
  template:
    '<app-setting-business-list [options]="options"></app-setting-business-list>',
})
export class PermissionsListComponent {
  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.PERMISSION,
      controllerId: 'commitePermissionId',
      columns: [
        {
          name: 'CommitePermissionNameAr',
          field: 'commitePermissionNameAr',
          searchable: true,
          operator: 'contains',
        },
        {
          name: 'CommitePermissionNameEn',
          field: 'commitePermissionNameEn',
          searchable: true,
          operator: 'contains',
        },
        { name: 'Enabled', field: 'enabled' },
      ],
    };
  }
}
