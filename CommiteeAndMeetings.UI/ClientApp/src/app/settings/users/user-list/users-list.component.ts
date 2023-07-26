import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-users-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>',
})
export class UsersListComponent {
  options: any;

  constructor() {
    this.options = {
        controller: SettingControllers.ALLUSERS,
        controllerId: 'userId',
        columns: [
          {
           name:'userName',
           field:'userName',
           searchable:true,
           operator:'contains'
          },
          {
            name: 'fullName',
            field: 'fullNameAr',
            searchable: true,
            operator: 'contains',
          },
          { 
            name: 'email', 
            field: 'email', 
            searchable: true, 
            operator: 'contains' 
        },
        { 
            name: 'Mobile', 
            field: 'mobile', 
            searchable: true, 
            operator: 'contains' 
        },
        { 
            name: 'defaultOrganizationAr', 
            field: 'defaultOrganizationAr', 
            searchable: true, 
            operator: 'contains' 
        },
        { 
            name: 'Enabled', 
            field: 'enabled' 
        },
        ],
      };
  }
}
