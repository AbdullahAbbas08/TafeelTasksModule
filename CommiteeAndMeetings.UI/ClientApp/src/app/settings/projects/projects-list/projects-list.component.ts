import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
@Component({
  selector: 'app-projects-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>'
})
export class ProjectsListComponent {

  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.PROJECTS,
      controllerId: 'id',
      columns: [
        { name: 'ProjectNameAr', field: 'projectNameAr', searchable: true, operator: 'contains' },
        { name: 'ProjectNameEn', field: 'projectNameEn', searchable: true, operator: 'contains' },
      ]
    };
  }

}
