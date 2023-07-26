import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { Lookup, Sort, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { FormControl } from '@angular/forms';
import { switchMap, debounceTime } from 'rxjs/operators';
import { of } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { CommitteeActions, SettingControllers } from '../../_enums/AppEnums';
import { NzModalService } from 'ng-zorro-antd/modal';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { NavigationEnd, Router } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Component({
  selector: 'app-setting-business-list',
  templateUrl: './setting-business-list.component.html',
  styleUrls: ['./setting-business-list.component.scss']
})
export class SettingsBusinessListComponent implements OnInit, OnChanges {
  @Input() options: any;

  controllerData: any[] = [];
  addPermissions: any[] = [];
  editPermissions: any[] = [];
  deletePermissions: any[] = [];
  actionTypes = CommitteeActions;
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
  currentUrl: string;
  hideControls:boolean = true;
  _roles:Lookup[];
  RoleId:number;
  constructor(
    private swagger: SwaggerClient,
    private modalService: NzModalService,
    private translateService: TranslateService,
    private _modalService: SharedModalService,
    private notificationService: NzNotificationService,
    private router: Router,
  ) { }

  ngOnChanges() {
    this.getControllerData(undefined);
  }

  ngOnInit() {
    this.searchInit();
    this.checkPermissions();
    this.checkCurrentComponent();
    if(this.options.controller === 'AllUsers'){
     this.getAllUserRoles()
    }
  }
  getAllUserRoles(){
    this.getLookup("roles").subscribe((value) => (this._roles = value));
  }
  checkPermissions() {
    switch(this.options.controller) {
      case SettingControllers.PERMISSION:
        this.addPermissions = ['CREATENEWPERMISSION'];
        this.editPermissions = ['EDITPERMISSION'];
        this.deletePermissions = ['DELETEPERMISSION'];
      break;
      case SettingControllers.LOCALIZATION:
        this.addPermissions = ['CREATENEWCOMMITTEELOCALIZATION'];
        this.editPermissions = ['EDITCOMMITTEELOCALIZATION'];
        this.deletePermissions = ['DELETECOMMITTEELOCALIZATION'];
      break;
      case SettingControllers.ROLE:
        this.addPermissions = ['CREATENEWCOMMITTEEROLE'];
        this.editPermissions = ['EDITCOMMITTEEROLE'];
        this.deletePermissions = ['DELETECOMMITTEEROLE'];
      break;
      case SettingControllers.STATUS:
        this.addPermissions = ['CREATENEWCOMMITTEESTATUS'];
        this.editPermissions = ['EDITCOMMITTEESTATUS'];
        this.deletePermissions = ['DELETECOMMITTEESTATUS'];
      break;
      case SettingControllers.TYPE:
        this.addPermissions = ['CREATENEWCOMMITTEETYPE'];
        this.editPermissions = ['EDITCOMMITTEETYPE'];
        this.deletePermissions = ['DELETECOMMITTEETYPE'];
      break;
      case SettingControllers.CATEGORY:
        this.addPermissions = ['CREATENEWCOMMITTEECATEGORY'];
        this.editPermissions = ['EDITCOMMITTEECATEGORY'];
        this.deletePermissions = ['DELETECOMMITTEECATEGORY'];
      break;
      case SettingControllers.PROJECTS:
        this.addPermissions = ['CREATENEWMEETINGPROJECT'];
        this.editPermissions = ['EDITMEETINGPROJECT'];
        this.deletePermissions = ['DELETEMEETINGPROJECT'];
      break;
      case SettingControllers.TaskClassification:
        this.addPermissions = ['CREATETASKSCLASSIFICATIONS'];
        this.editPermissions = ['EDITTASKSCLASSIFICATIONS'];
        this.deletePermissions = ['DELETETASKSCLASSIFICATIONS']
        break;
      default:
        this.addPermissions = [''];
        this.editPermissions = [''];
        this.deletePermissions = [''];
      break;
    }
  }

  getControllerData(searchTxt?,OrgId?: any, RoleId?: any) {
    this.loading = true;
    this.filter_Filters = [];
    if (this.searchTxt) {
      this.options.columns.map(x => { 
        if (x.searchable) {
          if (!x.typeNumber || parseInt(this.searchTxt, 10) > 0) {
            if (!x.dbFields) {
              this.filter_Filters.push({
                field: x.field,
                operator: x.operator,
                value: (x.typeNumber ? parseInt(this.searchTxt, 10) : this.searchTxt)
              });
            } else {
              x.dbFields.map(x1 => {
                this.filter_Filters.push({
                  field: x1,
                  operator: x.operator,
                  value: (x.typeNumber ? parseInt(this.searchTxt, 10) : this.searchTxt)
                });
              });
            }
          }
        }
      });
    }

    if (this.options.filters) {
      this.filter_Filters = this.filter_Filters ? [...this.filter_Filters, ...this.options.filters] : this.options.filters;
    }

    this.sort = [{ field: this.options.controllerId, dir: 'asc' } as Sort];
    if(this.options.controller === "CommiteeTaskEscalation"){
      this.swagger.apiCommiteeTaskEscalationGetAllWithCategoryGet(
        this.take,
        this.skip,
        this.sort,
        this.filter_Field,
        this.filter_Operator,
        this.filter_Value,
        this.filter_Logic,
        this.filter_Filters,
        this.countless,
      ).subscribe(
        response => {
          this.loading = false;
          this.controllerData = response.data;
          this.count = response.count;
        }, error => {
          this.loading = false;
        }
      );
    } else if(this.options.controller === "AllUsers"){
      this.swagger.apiUserGetUsersByOrganizationGet(
        OrgId,
        RoleId,
        this.take,
        this.skip,
        this.sort,
        this.filter_Field,
        this.filter_Operator,
        this.filter_Value,
        this.filter_Logic,
        this.filter_Filters,
        this.countless,
      ).subscribe(
        response => {
          this.loading = false;
          this.controllerData = response.data;
          this.count = response.count;
        }, error => {
          this.loading = false;
        }
      );
    }else {
      this.swagger[`api${this.options.controller}GetAllGet`](
        this.take,
        this.skip,
        this.sort,
        this.filter_Field,
        this.filter_Operator,
        this.filter_Value,
        this.filter_Logic,
        this.filter_Filters,
        this.countless,
      ).subscribe(
        response => {
          this.loading = false;
          this.controllerData = response.data;
          this.count = response.count;
        }, error => {
          this.loading = false;
        }
      );
    }
  }

  onDeleteItem(items) {
   const itemsToDelete = items?.length ? [...items] : [items];
   if(this.options.controller === "AllUsers"){
     this.swagger.apiUserDeleteDelete(items).subscribe((response) => {
      if(response) {
        this.getControllerData();
        this.translateService
        .get('ItemDeleted')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
       } else if (response === null){
        this.getControllerData();
        this.translateService
        .get('CanotDelete')
        .subscribe((translateValue) =>
          this.notificationService.warning(translateValue, '')
        );
       }
     })
   } else {
    this.swagger[`api${this.options.controller}DeleteDelete`](itemsToDelete).subscribe(
      response => {
       if(response) {
        this.getControllerData();
        this.translateService
        .get('ItemDeleted')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
       } else if (response === null){
        this.getControllerData();
        this.translateService
        .get('CanotDelete')
        .subscribe((translateValue) =>
          this.notificationService.warning(translateValue, '')
        );
       }
    })
   }


  }

  searchInit() {
    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      switchMap((term: string) => of(term))
    ).subscribe(term => {
      this.searchTxt = term;
      this.getControllerData(term);
    });
  }

  currentPageIndexChange($event: number) {
    if ($event) {
      this.skip = ($event - 1) * this.pageSize;
      this.getControllerData();
    }
  }

  currentPageSizeChange($event: number) {
    if ($event) {
      this.pageSize = $event;
      this.skip = 0;
      this.take = this.pageSize;
      this.pageIndex = 1;
      this.getControllerData();
    }
  }

  onDeleteItemConfirmation(items) {
    // this._modalService.openDrawerModal(
    //   CommitteeActions.deleteModel,
    // );
    this.modalService.confirm({
      nzTitle: this.translateService.instant('DeleteConfirmation'),
      nzContent: this.translateService.instant('Confirm'),
      nzOkText: this.translateService.instant('Yes'),
      nzCancelText: this.translateService.instant('No'),
      nzOkType: 'primary',
      nzOnOk: () => this.onDeleteItem(items)
    });
  }
  checkCurrentComponent(){
    this.currentUrl = this.router.routerState.snapshot.url;
    if (this.currentUrl.includes('statuses')){
      this.hideControls = false 
    } else {
      this.hideControls = true;
    }
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = this.router.routerState.snapshot.url;
        if (this.currentUrl.includes('statuses')){
          this.hideControls = false 
        } else {
          this.hideControls = true;
        }
      }
    })
  }
  getLookup(lookup, field?, operator?, value?) {
    return this.swagger.apiLookupGet(
      lookup,
      undefined,
      undefined,
      undefined,
      undefined,
      field,
      operator,
      value,
      undefined,
      undefined,
      undefined,
      undefined
    );
  }
}
