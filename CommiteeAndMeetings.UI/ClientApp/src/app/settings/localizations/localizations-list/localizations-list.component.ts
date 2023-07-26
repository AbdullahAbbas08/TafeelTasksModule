import {Component} from '@angular/core';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';

@Component({
  selector: 'app-localizations-list',
  template: '<app-setting-business-list [options]="options"></app-setting-business-list>'
})
export class LocalizationsListComponent {
  options: any;

  constructor() {
    this.options = {
      controller: SettingControllers.LOCALIZATION,
      controllerId: 'commiteeLocalizationId',
      columns: [
        {name: 'CommiteeLocalizationAr', field: 'commiteeLocalizationAr', searchable: true, operator: 'contains'},
        {name: 'CommiteeLocalizationEn', field: 'commiteeLocalizationEn', searchable: true, operator: 'contains', dbFields: ['commiteeLocalizationEn', 'key']}
      ]
    };
  }
}
