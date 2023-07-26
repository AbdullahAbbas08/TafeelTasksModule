import { SettingControllers } from './../../shared/_enums/AppEnums';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-escalation',
  template:
    '<app-setting-business-list [options]="options"></app-setting-business-list>',
})
export class EscalationComponent {
  options: any;
  constructor() {
    this.options = {
      controller: SettingControllers.Escalation,
      controllerId: 'commiteeTaskEscalationIndex',
      columns: [
        {
          name: 'TaskMainUser',
          field: 'mainAssinedUserFullNameAr',
          searchable: true,
          operator: 'contains',
        },
        { name: 'DelayPeriod', field: 'delayPeriod' },
        {
          name: 'Classification',
          field: 'comiteeTaskCategorycategoryNameAr',
          searchable: true,
          operator: 'contains',
        },
        {
          name: 'NewAssignedUser',
          field: 'newMainAssinedUserFullNameAr',
          searchable: true,
          operator: 'contains',
        },
      ],
    };
  }
}
