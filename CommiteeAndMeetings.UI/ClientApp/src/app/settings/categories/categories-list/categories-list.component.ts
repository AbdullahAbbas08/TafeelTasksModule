import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-categories-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>'
})
export class CategoriesListComponent  {

  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.CATEGORY,
      controllerId: 'categoryId',
      columns: [
        {
          name: 'CategoryNameAr',
          field: 'categoryNameAr',
          searchable: true,
          operator: 'contains',
        },
        {
          name: 'CategoryNameEn',
          field: 'categoryNameEn',
          searchable: true,
          operator: 'contains',
        }
      ],
    };
  }
}
