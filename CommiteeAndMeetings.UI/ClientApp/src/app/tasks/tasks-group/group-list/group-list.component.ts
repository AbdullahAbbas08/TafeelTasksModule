import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Sort, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { switchMap, debounceTime } from 'rxjs/operators';
import { of } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { AuthService } from 'src/app/auth/auth.service';
import { NzModalService } from 'ng-zorro-antd/modal';
@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.scss']
})
export class GroupListComponent implements OnInit {
  searchControl: FormControl = new FormControl();
  controllerData: any[] = [];
  count: number = 0;
  pageSize: number = 10;
  pageIndex: number = 1;
  loading: boolean = false;
  filter_Filters = undefined;
  sort = undefined;
  take:number = 10;
  skip: number = 0;
  filter_Field: string = undefined;
  filter_Operator: string = undefined;
  filter_Value: string = undefined;
  filter_Logic: string = 'or';
  countless = undefined;
  searchTxt: string = '';
  constructor(private modalService: NzModalService,private authService:AuthService,private notificationService: NzNotificationService,private swagger:SwaggerClient,public translateService: TranslateService) {}
  ngOnInit(): void {
    this.getGroupList(undefined);
    this.searchInit();
  }
  getGroupList(searchTxt?){
    this.loading = true;
    this.filter_Filters = [];
    if(searchTxt){
      this.filter_Filters = [
        { field: 'groupNameAr', operator: 'contains', value: searchTxt },
        { field: 'groupNameEn', operator: 'contains', value: searchTxt },
      ];
    } else {
      this.filter_Filters = []
    }
    this.sort = [{ field: 'groupId', dir: 'asc' } as Sort];
    this.swagger.apiGroupGetAllGet(this.take,
      this.skip,
      this.sort,
      this.filter_Field,
      this.filter_Operator,
      this.filter_Value,
      this.filter_Logic,
      this.filter_Filters,
      this.countless).subscribe((res) => {
        if(res){
          this.loading = false;
          this.controllerData = res.data;
          this.count = res.count;
        }
      })
  }
  currentPageIndexChange($event: number) {
    if ($event) {
      this.skip = ($event - 1) * this.pageSize;
      this.getGroupList();
    }
  }

  currentPageSizeChange($event: number) {
    if ($event) {
      this.pageSize = $event;
      this.skip = 0;
      this.take = this.pageSize;
      this.pageIndex = 1;
      this.getGroupList();
    }
  }
  searchInit() {
    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      switchMap((term: string) => of(term))
    ).subscribe(term => {
      this.searchTxt = term;
      this.getGroupList(term);
    });
  }

  onDeleteItemConfirmation(id:number){
    this.modalService.confirm({
      nzTitle: this.translateService.instant('DeleteConfirmation'),
      nzContent: this.translateService.instant('Confirm'),
      nzOkText: this.translateService.instant('Yes'),
      nzCancelText: this.translateService.instant('No'),
      nzOkType: 'primary',
      nzOnOk: () => this.onDeleteItem(id)
    });
  }
  onDeleteItem(id){
    this.swagger.apiGroupDeleteGroupFromCreatedUserDelete(id).subscribe((res) => {
      if(res){
        this.getGroupList();
        this.translateService
        .get('deleteconfirmed')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
      }else {
        this.translateService
        .get('cannotdeleteGroup')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
      }
     })
  }
}
