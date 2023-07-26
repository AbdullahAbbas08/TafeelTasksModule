import { Subscription } from 'rxjs';
import { StoreService } from './../../shared/_services/store.service';
import { SearchService } from './../../shared/_services/search.service';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, OnDestroy,ElementRef,ViewChild } from '@angular/core';
import {
  CommitteeService,
  ExtendedCommiteeDTODataSourceResult,
} from '../committee.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { CommiteeDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { PanZoomConfig, PanZoomAPI, PanZoomModel } from 'ng2-panzoom';
import Panzoom from '@panzoom/panzoom'
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-committees-tree',
  templateUrl: './committees-tree.component.html',
  styleUrls: ['./committees-tree.component.scss'],
})
export class CommitteesTreeComponent implements OnInit, OnDestroy {
  currentLang: string;
  committees = [];
  loadingData = false;
  @ViewChild('zoom') zoom: ElementRef;
  editSubscription: Subscription;
  searchSubscription: Subscription;
  addSubscription: Subscription;
  statusSubscription: Subscription;
  noData:boolean = false;
  canvasWidth = 1500;
  canvasHeight;
  cellWidth = 260;
  cellSpacing = 21;
  initialX = 587;
  panZoomAPI: PanZoomAPI;
  panzoomConfig: PanZoomConfig = new PanZoomConfig({
    zoomLevels: 10,
    scalePerZoomLevel: 3, // make it bigger to make scaling faster with mouse wheel
    zoomStepDuration: 0.2,
    zoomToFitZoomLevelFactor: 1,
    zoomButtonIncrement: 1,
    neutralZoomLevel: 1,
    initialZoomLevel: 1,
    initialPanX: this.initialX,
    dragMouseButton: 'left',
    freeMouseWheel: false,
    freeMouseWheelFactor: 0.01,
    invertMouseWheel: true,
  });
  apiSubscription: Subscription;
  constructor(
    public translateService: TranslateService,
    private committeeService: CommitteeService,
    private layoutService: LayoutService,
    private notificationService: NzNotificationService,
    private modalService: SharedModalService,
    private searchService: SearchService,
    private storeService: StoreService,
    private modelService: NzModalService,
    private browserService: BrowserStorageService,
    private swagger:SwaggerClient,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.apiSubscription = this.panzoomConfig.api.subscribe((api: PanZoomAPI) => this.panZoomAPI = api);
    this.fetchCommittees();
    this.langChange();
    this.listenToEditedCommittees();
    this.listenToSearch();
    this.ListenToAddedCommitee();
    this.listenToFilterStatus();
  }

  ngOnDestroy() {
    this.editSubscription.unsubscribe();
    this.searchSubscription.unsubscribe();
    this.addSubscription.unsubscribe();
    this.statusSubscription.unsubscribe();
  }
  resetView(fromView): void {
      this.panZoomAPI.resetView();
  }
  public zoomIn(): void {
    this.panZoomAPI.zoomIn();
  }

  public zoomOut(): void {
    this.panZoomAPI.zoomOut();
  }
  public panRightPercent(): void {
    this.panZoomAPI.panDelta({ x: -1174, y: 0 });
  }

  public panLeftPercent(): void {
    this.panZoomAPI.panDelta({ x: 1174, y: 0 });
  }

  claculateHomePoint(): void {
    this.canvasWidth = this.zoom.nativeElement.clientWidth;
    this.canvasHeight = this.zoom.nativeElement.clientHeight;
    let x = (this.canvasWidth / 2) - (this.cellWidth / 2) - (this.cellSpacing * 2);
    let y = this.cellSpacing * 2;
    this.panzoomConfig.initialPanX = -x + this.initialX;
    this.panzoomConfig.initialPanY = y;
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }

  filterCommittees(word = undefined, statusId = undefined) {
    let committeeList = this.committeeService
      .getCommitteesTree()
      .filter((c) => (word ? c.title.includes(word) : c))
      .filter((c) => (statusId ? c.currentStatusId == statusId : c));

    this.committees = committeeList.map((committee) => {
      return {
        uid: `${committee.commiteeId}`,
        pid: committeeList.find(
          (com) => com.commiteeId === committee.parentCommiteeId
        )
          ? `${committee.parentCommiteeId}`
          : null,
        ...committee,
      };
    });
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
  disactiveCommitte(id) {
    this.committeeService.disactiveCommitte(this.browserService.encrypteString(`${id}_${this.browserService.getUserRoleId()}`)).subscribe((res) => {
      if (res) {
        let updatedCommitte = this.committees.find(
          (committee) => committee.commiteeId === id
        );

        if (updatedCommitte.currentStatus.currentStatusId === 1) {
          updatedCommitte.currentStatus.currentStatusId = 2;
          updatedCommitte.currentStatus.currentStatusNameAr = 'غير نشطة';
          updatedCommitte.currentStatus.currentStatusNameEn = 'Not Active ';
        } else {
          updatedCommitte.currentStatus.currentStatusId = 1;
          updatedCommitte.currentStatus.currentStatusNameAr = 'نشطة';
          updatedCommitte.currentStatus.currentStatusNameEn = 'Active';
        }

        this.translateService
          .get('CommitteStatusChanged')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
      }
    });
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
  editCommittee(committeeId: number) {
    this.modalService.openDrawerModal(
      CommitteeActions.EditCommittee,
      committeeId
    );
  }

  listenToEditedCommittees() {
    this.editSubscription = this.committeeService.editedCommittee.subscribe(
      (committee) => {
        let index = this.committees.findIndex(
          (com) => com.commiteeId === committee.commiteeId
        );
        let newCommittee = {
          uid: `${committee.commiteeId}`,
          pid: this.committees.find(
            (com) => com.commiteeId === committee.parentCommiteeId
          )
            ? `${committee.parentCommiteeId}`
            : null,
          validityPeriod: this.committees[index].validityPeriod,
          ...committee,
        };

        this.committees.splice(index, 1, newCommittee);
        this.committees = [...this.committees];
      }
    );
  }

  fetchCommittees() {
    this.loadingData = true;
    this.layoutService.toggleSpinner(true);

    this.committeeService
      .fetchCommitteesTreeList()
      .subscribe((res: CommiteeDTO[]) => {
        if (res) {
          this.noData = res.length > 0 ? true : false;
          this.committees = res.map((committee) => {
            return {
              uid: `${committee.commiteeId}`,
              pid: res.find(
                (com) => com.commiteeId === committee.parentCommiteeId
              )
                ? `${committee.parentCommiteeId}`
                : null,
              ...committee,
            };
          });
        }

        this.layoutService.toggleSpinner(false);
        this.loadingData = false;
      });
  }

  listenToSearch() {
    this.searchSubscription = this.searchService.searchcriteria.subscribe(
      (word) => {
        if (word) {
          this.filterCommittees(word);
        } else {
          this.fetchCommittees();
        }
      }
    );
  }

  ListenToAddedCommitee() {
    this.addSubscription = this.storeService.refreshCommittees$.subscribe(
      (c) => {
        if (c) {
          this.fetchCommittees();
        }
      }
    );
  }

  listenToFilterStatus() {
    this.statusSubscription = this.committeeService.committeeFilters.subscribe(
      (statusId) => {
        this.filterCommittees(undefined, statusId);
      }
    );
  }
}
