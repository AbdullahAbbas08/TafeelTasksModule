
import { Component, Input, OnInit, Inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { debounceTime, switchMap } from 'rxjs/operators';
import { API_BASE_URL, CommiteeTaskDTO, CountResultDTO, LookUpDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { ExtendedTaskDTO, StatsArray, TaskFilters, TasksService } from '../../tasks.service';
import { TranslateService } from '@ngx-translate/core';
import * as echarts from 'echarts';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
import * as FileSaver from "file-saver";
import { AuthService } from 'src/app/auth/auth.service';
interface ItemData {
  name: string;
  age: number;
  address: string;
}
@Component({
  selector: 'app-task-statistics-item',
  templateUrl: './tasks-statistics.component.html',
  styleUrls: ['./tasks-statistics.component.scss'],
})
export class TasksStatisticsComponent implements OnInit {
  statisticsCount: CountResultDTO[] = [];
  statisticsArray: StatsArray[] = [];
  tasks: ExtendedTaskDTO[] = [];
  isLoading: boolean = false;
  isLoadingTable: boolean = false
  lookupTypes = SettingControllers;
  usersChanged$ = new BehaviorSubject('');
  orgChanged$ = new BehaviorSubject('');
  selectedUser: any;
  selectedOrganization: any;
  users: LookUpDTO[] = [];
  organizations: LookUpDTO[] = [];
  listOfData: ItemData[] = [];
  activeTab: string;
  displayTable: boolean = false;
  loadingData: boolean = false;
  filterTake: number = 5;
  filterskip: number = 0;
  filterCount: number = 0;
  tasksFilter: any[] = [];
  pageIndex: number = 1;
  pageSize: number = 5;
  tasksSummary: CommiteeTaskDTO[] = [];
  filterNum: number;
  selectedOrgName: string;
  selectedUserName: string;
  tableExpand: any = {};
  newSelectedOrg: number;
  currentLang: string;
  isLoadingStats: boolean = false;
  dateNow: Date = new Date();
  createdUserImage: string;
  gaugeValue: number;
  gaugeLabel = this.translateService.currentLang === 'ar' ? "المهمام المتأخرة" : "Late Tasks"
  collapse: any = {};
  tab: any = 'tab7';
  blob: any;
  showStats: boolean = false;
  constructor(private _authService: AuthService, public swagger: SwaggerClient, @Inject(API_BASE_URL) public baseUrl: string, private layoutService: LayoutService, private modalService: SharedModalService, public translateService: TranslateService, public lookupService: LookupService, private BrowserService: BrowserStorageService, private taskservice: TasksService, public dateService: NgbDateService) {
    this.selectedUser = +this.BrowserService.decrypteString(
      JSON.parse(localStorage.getItem("user"))["userId"]
    );
    this.selectedOrganization = +this.BrowserService.decrypteString(
      JSON.parse(localStorage.getItem("user"))["organizationId"]
    );
    this.selectedUserName = this.BrowserService.decrypteString(
      JSON.parse(localStorage.getItem("user"))["fullNameAr"]
    )
    this.taskservice.getOrganizationByOrganizationName().subscribe((res) => {
      if (!this.organizations.some(x => x.id == res.id)) {
        this.organizations.push(new LookUpDTO({ id: res.id, name: res.organizationAr }))
      }
    })
  }

  ngOnInit(): void {
    this.initUsers(this.selectedOrganization)
    this.initOrgs();

    this.getTasksStatisticsNum(undefined, this.BrowserService.encrypteString(this.selectedUser));
    if (!this.users.some(x => x.id == this.selectedUser)) {
      this.users.push(new LookUpDTO({ id: this.selectedUser, name: this.selectedUserName }))
    };
    this.getfilteredTasks(7, this.selectedUser, undefined, true)
    this.langChange()
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
      case SettingControllers.ALLORGANIZATION:
        this.orgChanged$.next(value)
        break;
      default:
        break;
    }
  }
  getStatisticsForUser() {
    if (this.selectedUser) {
      // this.displayTable = false;
      this.getTasksStatisticsNum(undefined, this.BrowserService.encrypteString(this.selectedUser));
      this.getfilteredTasks(7, this.selectedUser, undefined, true);
    } else {
      // this.displayTable = false;
      this.getTasksStatisticsNum(this.BrowserService.encrypteString(this.selectedOrganization))
      this.getfilteredTasks(7, undefined, this.selectedOrganization, true);
    }
    this.activeTab = `activeTab7`;
    this.tab = `tab7`
  }
  currentPageIndexChange($event: number) {
    if ($event) {
      this.filterskip = ($event - 1) * this.pageSize;
      if (this.selectedUser) {
        this.getfilteredTasks(this.filterNum, this.selectedUser, undefined, false)
      } else {
        this.getfilteredTasks(this.filterNum, undefined, this.selectedOrganization, false)
      }
    }
  }
  currentPageSizeChange($event: number) {
    if ($event) {
      this.pageSize = $event;
      this.filterskip = 0;
      this.filterTake = this.pageSize;
      this.pageIndex = 1;
      if (this.selectedUser) {
        this.getfilteredTasks(this.filterNum, this.selectedUser, undefined, false)
      } else {
        this.getfilteredTasks(this.filterNum, undefined, this.selectedOrganization, false)
      }
    }
  }
  changeOrganization(event) {
    this.initUsers(event)
    this.selectedUser = undefined
  }

  initUsers(orgId?: number) {
    this.usersChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.lookupService.getUsersWithOrgLookup(
            25,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,
            orgId
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.users = data;
        this.isLoading = false;
      });
  }
  initOrgs() {
    this.orgChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.lookupService.getOrgsLookup(
            25,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,
            false
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.organizations = data;
        this.isLoading = false;

      });
  }
  getTasksStatisticsNum(orgId?: string, userId?: any) {
    this.showStats = true
    this.taskservice
      .getStatistisTasksNumber(orgId, userId)
      .subscribe((result) => {
        if (result) {
          this.statisticsCount = result;
          this.gaugeValue = Math.round((this.statisticsCount[1].count / this.statisticsCount[0].count) * 100) ? Math.round((this.statisticsCount[1].count / this.statisticsCount[0].count) * 100) : 0;
          this.statisticsCount.map((x, i) => {
            this.statisticsArray.push({
              index: i, value: x.count, name: this.translateService.get(x.name)
                .toPromise()
            })
          })
          setTimeout(() => { this.drawCharts() }, 500)
        }
        this.showStats = false;
        this.isLoadingTable = false
      });
  }

  drawCharts() {
    type EChartsOption = echarts.EChartsOption;
    var chartDom = document.getElementById('main');
    var myChart = echarts.init(chartDom);
    var option: EChartsOption;
    const cellSize = [30, 20];
    option = {
      tooltip: {
        trigger: 'item',
        triggerOn: 'mousemove'
      },
      legend: {
        top: '85%',
        left: 'right',
        show: false,
      },
      series: [
        {
          name: '',
          type: 'pie',
          radius: ['40%', '74%'],
          labelLine: {
            length: 10
          },
          label: {
            show: true,
            position: 'inside',
            color: '#Fff',
            formatter: '{c}',
            offset: [-cellSize[0] / 2 + 10, -cellSize[1] / 2 + 10],
            fontSize: 14
          },
          color: ['#1A73E8', '#D81B60', '#191919', '#43A047', '#6d7475', '#f7cd00'],
          data: [
            { value: this._authService.isAuthUserHasPermissions(['allTasks']) ? this.statisticsCount[0].count === 0 ? NaN : this.statisticsCount[0].count : NaN, name: this.translateService.currentLang === 'ar' ? 'كل المهام' : 'All Tasks' }, //All tasks
            { value: this._authService.isAuthUserHasPermissions(['lateTasks']) ? this.statisticsCount[1].count === 0 ? NaN : this.statisticsCount[1].count : NaN, name: this.translateService.currentLang === 'ar' ? 'المهام المتاخرة' : 'late Tasks' }, // lateTasks
            { value: this._authService.isAuthUserHasPermissions(['assisstantTasks']) ? this.statisticsCount[4].count === 0 ? NaN : this.statisticsCount[4].count : NaN, name: this.translateService.currentLang === 'ar' ? 'تكليف فرعي' : 'Assistant' }, // assistant users
            { value: this._authService.isAuthUserHasPermissions(['closedTasks']) ? this.statisticsCount[3].count === 0 ? NaN : this.statisticsCount[3].count : NaN, name: this.translateService.currentLang === 'ar' ? 'كل المغلق' : 'All Close' }, // All close
            { value: this._authService.isAuthUserHasPermissions(['taskToView']) ? this.statisticsCount[5].count === 0 ? NaN : this.statisticsCount[5].count : NaN, name: this.translateService.currentLang === 'ar' ? 'مهام للإطلاع' : 'Task to View' }, // taskToView
            { value: this.statisticsCount[2].count === 0 ? NaN : this.statisticsCount[2].count, name: this.translateService.currentLang === 'ar' ? 'تحت الإجراء' : 'Underprocedure' } // Underprocedure
          ]
        }
      ]
    };

    option && myChart.setOption(option);
    myChart.on('click', function (params) {

    });
  }
  returnUserId() {
    return +this.BrowserService.decrypteString(
      JSON.parse(localStorage.getItem("user"))["userId"]
    );
  }
  returnUserName() {
    return this.BrowserService.decrypteString(
      JSON.parse(localStorage.getItem("user"))["fullNameAr"]
    );
  }

  getfilteredTasks(filterNum, userId: string, orgId: string, checkChart: boolean) {
    this.filterNum = filterNum;
    this.isLoadingStats = true;
    if (checkChart) {
      this.isLoadingTable = true
    }
    this.taskservice
      .getFilteredTasks(
        this.filterTake,
        this.filterskip,
        undefined,
        this.tasksFilter,
        'and',
        filterNum,
        userId,
        orgId
      )
      .subscribe((res) => {
        if (res && res.data) {
          this.tasks = [...res.data];
          this.filterCount = res.count;
        }
        this.isLoadingStats = false;

      });
  }

  filterWithClick(filterNumer: number) {
    this.filterTake = 5;
    this.filterskip = 0;
    this.filterCount = 0;
    this.pageIndex = 1;
    this.pageSize = 5;
    this.activeTab = `activeTab${filterNumer}`;
    this.tab = `tab${filterNumer}`
    if (this.selectedUser) {
      this.getfilteredTasks(filterNumer, this.selectedUser, undefined, false)
    } else {
      this.getfilteredTasks(filterNumer, undefined, this.selectedOrganization, false)
    }
  }
  editHistory(committeId: number, taskId: number, event: Event) {
    event.stopPropagation();
    this.modalService.initEditTaskHistory(
      committeId,
      taskId
    );
  }
  getAllTasksForPrint() {
    this.layoutService.toggleSpinner(true);
    this.swagger
      .apiCommiteeTasksGetAllForPrintGet(
        this.filterNum, undefined, undefined, undefined, this.selectedUser, undefined, undefined, undefined, undefined, undefined, undefined, undefined
      )
      .subscribe((result) => {
        if (result) {
          this.tasksSummary = result;
          this.layoutService.toggleSpinner(false);
          this.drawPrintTable()
        }
      });
  }
  drawPrintTable() {
    setTimeout(() => {
      let direction = this.translateService.currentLang == 'ar' ? 'rtl' : 'ltr';
      let popupWinindow;
      let innerContents = document.getElementById(
        'main-print-section-id'
      ).innerHTML;
      popupWinindow = window.open(
        '',
        '_blank',
        'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no'
      );
      popupWinindow.document.open();
      popupWinindow.document.write(
        `<html dir="${direction}" lang="${this.translateService.currentLang}"><head><link rel="stylesheet" type="text/css" href="${this.baseUrl}/assets/css/print-referral.css" /></head><body onload="setTimeout(() => {window.print()},100);">` +
        innerContents +
        '</html>'
      );
      popupWinindow.document.close();
    }, 1000)
  }
  getFormattedTime() {
    var today = new Date();
    var x = today.getTime()
    return x;
  }
  exportDocument(exportType: number) {
    this.taskservice.exportDocument(this.filterNum, this.selectedUser, exportType)
  }
}


