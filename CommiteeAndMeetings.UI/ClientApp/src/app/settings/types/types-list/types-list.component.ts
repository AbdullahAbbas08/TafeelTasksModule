import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-types-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>',
})
export class TypesListComponent {
  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.TYPE,
      controllerId: 'commiteeTypeId',
      columns: [
        {
          name: 'CommiteeTypeNameAr',
          field: 'commiteeTypeNameAr',
          searchable: true,
          operator: 'contains',
        },
        {
          name: 'CommiteeTypeNameEn',
          field: 'commiteeTypeNameEn',
          searchable: true,
          operator: 'contains',
        },
      ],
    };
  }
}
