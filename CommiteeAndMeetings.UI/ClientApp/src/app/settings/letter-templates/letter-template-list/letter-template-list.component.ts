import { Component } from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
@Component({
  selector: 'app-letter-template-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>'
})
export class LetterTemplateListComponent {
  
  options: any;
  constructor() {
    this.options = {
      controller: SettingControllers.LETTERTEMPLATES,
      controllerId: 'id',
      columns: [
        { name: 'titleAR', field: 'titleAR', searchable: true, operator: 'contains' },
        { name: 'titleEn', field: 'titleEn', searchable: true, operator: 'contains' },
        { name: 'HeaderAndFooterType', field: 'headerAndFooterTypeString' },
      ]
    };
  }

}
