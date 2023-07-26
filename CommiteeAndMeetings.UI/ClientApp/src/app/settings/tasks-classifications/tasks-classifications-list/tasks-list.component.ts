import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-tasks-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>',
})
export class TasksListComponent {
  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.TaskClassification,
      controllerId: 'comiteeTaskCategoryId',
      columns: [
        {
          name: 'categoryNameAr',
          field: 'categoryNameAr',
          searchable: true,
          operator: 'contains',
        },
        {
          name: 'categoryNameEn',
          field: 'categoryNameEn',
          searchable: true,
          operator: 'contains',
        },
      ],
    };
  }
}
