import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-status-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>',
})
export class StatusListComponent {
  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.STATUS,
      controllerId: 'currentStatusId',
      columns: [
        {
          name: 'CurrentStatusNameAr',
          field: 'currentStatusNameAr',
          searchable: true,
          operator: 'contains',
        },
        {
          name: 'CurrentStatusNameEn',
          field: 'currentStatusNameEn',
          searchable: true,
          operator: 'contains',
        },
      ],
    };
  }
}
