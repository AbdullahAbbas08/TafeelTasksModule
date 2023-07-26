import { LayoutService } from './../../shared/_services/layout.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { imgFallback } from 'src/app/shared/_constants';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { StoreService } from 'src/app/shared/_services/store.service';
import { Subscription } from 'rxjs';
import {
  CommitteeService,
  ExtendedCommiteeDTODataSourceResult,
  ExtendedCommitteeDTO,
} from '../committee.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzModalService } from 'ng-zorro-antd/modal';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Router } from '@angular/router';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-committees-list',
  templateUrl: './committees-list.component.html',
  styleUrls: ['./committees-list.component.scss'],
})
export class CommitteesListComponent implements OnInit, OnDestroy {
  currentStats: number = undefined;
  committees: ExtendedCommitteeDTO[] = [];
  loadingData: boolean = false;
  skip: number = 0;
  take: number = 12;
  count = 0;
  currentCount = 0;
  filter_Logic: string = 'or';
  filters: any[] = [];
  fallback = imgFallback;
  subscription: Subscription;
  constructor(
    private committeeService: CommitteeService,
    private storeService: StoreService,
    private modalService: SharedModalService,
    private layoutService: LayoutService,
    private searchService: SearchService,
    public translateService: TranslateService,
    private notificationService: NzNotificationService,
    private modelService: NzModalService,
    private swagger:SwaggerClient,
    private router: Router,
    private browserService: BrowserStorageService,
  ) {}

  ngOnInit(): void {
    this.getCommittes();
    this.storeService.refreshCommittees$.subscribe((val) => {
      if (val) {
        this.getCommittes();
      }
    });
    this.getEditedCommittee();
    this.getFilteredCommities();
    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
      if (word) {
        this.getCommittes(false, word);
      } else if (!word) {
        this.currentStats = undefined;
        this.getCommittes();
      }
    });
  }
  ngOnDestroy() {
    this.layoutService.toggleSpinner(false);
    this.subscription.unsubscribe();
  }
  getFilteredCommities() {
    this.committeeService.committeeFilters.subscribe((filterId) => {
      if (filterId) {
        this.currentStats = filterId;
        this.getCommittes();
      }
    });
  }
  getEditedCommittee() {
    this.committeeService.editedCommittee.subscribe((val) => {
      if (val) {
        this.committees.map((committe, index) => {
          if (committe.commiteeId == val.commiteeId) {
            this.committees[index] = val;
          }
        });
      }
    });
  }

  getCommittes(scroll: boolean = false, searchWord: string = undefined) {
    if (!scroll) {
      this.loadingData = true;
      this.layoutService.toggleSpinner(true);
      this.skip = 0;
    }
    if (this.currentStats) {
      this.filters = [
        { field: 'currentStatusId', operator: 'eq', value: this.currentStats },
      ];
    } else if (searchWord) {
      this.filters = [
        { field: 'name', operator: 'contains', value: searchWord },
        { field: 'description', operator: 'contains', value: searchWord },
      ];
    } else {
      this.filters = [];
    }
    this.committeeService
      .getCommittees(this.take, this.skip, this.filters, this.filter_Logic)
      .subscribe((res: ExtendedCommiteeDTODataSourceResult) => {
        if (res && res.data) {
          this.committees = scroll
            ? [...this.committees, ...res.data]
            : res.data;
          this.count = res.count;
          // this.committeeService.setCommittees(this.committees);
          this.storeService.allCommittes$.next(res.data);
        }
        if (scroll) {
          this.currentCount += res.data.length;
        } else {
          this.currentCount = res.data.length;
        }
        this.layoutService.toggleSpinner(false);
        this.loadingData = false;
      });
  }
  onScroll() {
    if (this.currentCount < this.count) {
      this.skip += this.take;
      this.getCommittes(true);
    }
  }
  editCommittee(committeeId: number) {
    this.modalService.openDrawerModal(
      CommitteeActions.EditCommittee,
      committeeId
    );
  }
  onChangeCommitteStatus(id) {
    this.modelService.confirm({
      nzTitle: this.translateService.instant('ChangeStatusConfirmation'),
      nzOkText: this.translateService.instant('Yes'),
      nzCancelText: this.translateService.instant('No'),
      nzOkType: 'primary',
      nzOnOk: () => this.disactiveCommitte(id)
    });
  }
  disactiveCommitte(id){
   this.committeeService.disactiveCommitte(this.browserService.encrypteString(`${id}_${this.browserService.getUserRoleId()}`)).subscribe((res) => {
     if(res){
      let changeStats = this.committees.find((committee) => committee.commiteeId === id);
       if(changeStats.currentStatus.currentStatusId === 1){
         changeStats.currentStatus.currentStatusId = 2;
         changeStats.currentStatus.currentStatusNameAr = 'غير نشطة';
         changeStats.currentStatus.currentStatusNameEn = 'Not Active ';
       } else {
         changeStats.currentStatus.currentStatusId = 1;
         changeStats.currentStatus.currentStatusNameAr = 'نشطة';
         changeStats.currentStatus.currentStatusNameEn = 'Active';
       }
       this.translateService
       .get('CommitteStatusChanged')
       .subscribe((translateValue) =>
         this.notificationService.success(translateValue, '')
       );
     }
   })
  }
  navigateTo(commiteId){
  const id = this.browserService.encrypteString(commiteId)
    this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet("DefaultCommitteeTaskOpenPage").subscribe((value) => {
      if(value.systemSettingValue == '1'){
        this.router.navigate(["/committees/", id,"tasks"]);
      }else {
        this.router.navigate(["/committees/", id]);
      }
    })
  }
}
