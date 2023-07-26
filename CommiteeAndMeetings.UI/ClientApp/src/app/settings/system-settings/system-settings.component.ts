import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import {
  CommitteeMeetingSystemSettingDTO,
  CommitteeMeetingSystemSettingDTODataSourceResult,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { FormControl } from '@angular/forms';
import { switchMap, debounceTime } from 'rxjs/operators';
import { of } from 'rxjs';
import { Subject, combineLatest } from 'rxjs';
import { NzNotificationService } from 'ng-zorro-antd/notification';
export class SystemSetting extends CommitteeMeetingSystemSettingDTO {
  showSuccessLabel?: boolean;
  userTouchedAndChanged?: boolean;
  successfullyChanged?: boolean;
  systemSettingName?: string;
}
@Component({
  selector: 'app-system-settings',
  templateUrl: './system-settings.component.html',
  styleUrls: ['./system-settings.component.scss'],
})
export class SystemSettingsComponent implements OnInit {
  systemSettings: SystemSetting[] = [];
  settingsChanged: Array<SystemSetting> = [];
  settingsOldValues: Array<SystemSetting> = [];
  count: number = 0;
  take: number = 10;
  skip: number = 0;
  pageIndex: number = 1;
  pageSize: number = 10;
  loading: boolean = false;
  sort = undefined;
  filter_Field: string = undefined;
  filter_Operator: string = undefined;
  filter_Value: string = undefined;
  filter_Logic: string = 'or';
  filter_Filters = undefined;
  countless = undefined;
  searchControl: FormControl = new FormControl();
  searchTxt: string = '';
  constructor(
    private swagger: SwaggerClient,
    public translate: TranslateService,
    private notificationService: NzNotificationService
  ) {}

  ngOnInit() {
    this.getAllSystemSetttings();
    this.searchInit();
  }

  getAllSystemSetttings(searchTxt?) {
    this.loading = true;
    this.filter_Filters = [];
    if (searchTxt) {
      this.filter_Filters.push({
        field: 'systemSettingNameAr',
        operator: 'contains',
        value: searchTxt,
      });
    } else {
      this.filter_Filters = [];
    }
    this.swagger
      .apiCommitteeMeetingSystemSettingGetAllGet(
        this.take,
        this.skip,
        this.sort,
        this.filter_Field,
        this.filter_Operator,
        this.filter_Operator,
        this.filter_Logic,
        this.filter_Filters,
        this.countless
      )
      .subscribe((result: CommitteeMeetingSystemSettingDTODataSourceResult) => {
        if (result) {
          this.loading = false;
          this.systemSettings = result.data;
          this.count = result.count;
        }
      });
  }
  searchInit() {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(400),
        switchMap((term: string) => of(term))
      )
      .subscribe((term) => {
        this.searchTxt = term;
        this.getAllSystemSetttings(term);
      });
  }
  currentPageIndexChange($event: number) {
    if ($event) {
      this.skip = ($event - 1) * this.pageSize;
      this.getAllSystemSetttings();
    }
  }

  currentPageSizeChange($event: number) {
    if ($event) {
      this.pageSize = $event;
      this.skip = 0;
      this.take = this.pageSize;
      this.pageIndex = 1;
      this.getAllSystemSetttings();
    }
  }
  changeSettingValue(setting: SystemSetting, e: Event) {
    const settingOldValueExist = this.settingsOldValues.find(
      (x) => x.systemSettingId == setting.systemSettingId
    );
    if (!settingOldValueExist) {
      this.settingsOldValues.push(<SystemSetting>{ ...setting });
    }
    const value = (e.target as HTMLInputElement).value;
    setting.systemSettingValue = value;

    if (!setting.userTouchedAndChanged) {
      setting.userTouchedAndChanged = true;
    } else if (value == settingOldValueExist.systemSettingValue) {
      setting.userTouchedAndChanged = false;
    }
    const settingWasChangedBefore = this.settingsChanged.find(
      (x) => x.systemSettingId == setting.systemSettingId
    );
    if (settingWasChangedBefore) {
      settingWasChangedBefore.systemSettingValue = value;
    } else {
      this.settingsChanged.push(setting);
    }
  }
  saveChanges(setting: SystemSetting) {
    this.updateSettings([setting]);
    const index = this.systemSettings.indexOf(
      this.systemSettings.find(
        (x) => x.systemSettingId == setting.systemSettingId
      )
    );
    this.settingsChanged.splice(index, 1);
  }
  saveAllChanges(){
    this.updateSettings(this.settingsChanged);
  }
  updateSettings(changedSettingsArr: Array<SystemSetting>) {
    this.swagger
      .apiCommitteeMeetingSystemSettingUpdatePut(changedSettingsArr)
      .subscribe(async (res) => {
        const [EditedSuccessfully] = await combineLatest(
          this.translate.get('EditedSuccessfully')
        ).toPromise();
        this.notificationService.success(EditedSuccessfully, '');
      });
    this.systemSettings.forEach((setting) => {
      changedSettingsArr.forEach((changedSetting) => {
        if (setting.systemSettingId == changedSetting.systemSettingId) {
          setting.successfullyChanged = true;
          setting.userTouchedAndChanged = false;
          setTimeout(() => (setting.successfullyChanged = false), 2000);
        }
      });
    });
  }
}
