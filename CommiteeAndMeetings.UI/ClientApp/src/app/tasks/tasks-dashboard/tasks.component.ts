import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { StoreService } from 'src/app/shared/_services/store.service';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { CommitteeService } from 'src/app/committees/committee.service';
import {
  CountResultDTO,
  ValidityPeriodDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { ExtendedCountDTO, ExtendedTaskDTO, TaskFilters, TasksService } from '../tasks.service';
import { TasksFilterEnum } from 'src/app/shared/_enums/AppEnums';
import { AuthService } from 'src/app/auth/auth.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss'],
})
export class TasksComponent implements OnInit, OnDestroy {
  committeeId: any;
  tasks: ExtendedTaskDTO[] = [];
  loadingData: boolean = false;
  loadingTaskCount: boolean = false;
  count: number = 0;
  take: number = 10;
  skip: number = 0;
  filterTake: number = 10;
  filterskip: number = 0;
  filterCount: number = 0;
  currentfilterCount: number = 0;
  filters: any[] = [];
  tasksFilter: any[] = [];
  commentText = '';
  subscription: Subscription;
  stateSubscription: Subscription;
  tasksSubscription: Subscription;
  toggleCompeleteSubs: Subscription;
  fromNotification: Subscription
  validityPeriod: ValidityPeriodDTO;
  currentCount = 0;
  CommitteName: string;
  statisticsCount: ExtendedCountDTO[] = [];
  togglebetweenTasks: boolean;
  selectedFilterObject: TaskFilters;
  tab: any = 'tab3';
  TasksFilterEnum = TasksFilterEnum;
  userTaskPermissions: boolean;
  userId: any;
  showTasksWithCommittePermission: boolean = true
  permissionsValues: string[] = ['allTasks', 'lateTasks', undefined, 'closedTasks', 'assisstantTasks', 'taskToView']
  requiredTaskEnum: number = 7;
  constructor(
    private taskservice: TasksService,
    private storeService: StoreService,
    private searchService: SearchService,
    private layoutService: LayoutService,
    private committeeService: CommitteeService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private BrowserService: BrowserStorageService
  ) {

  }

  ngOnInit(): void {
    this.userId = this.authService.getUser().userId;
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );

    this.storeService.refreshTasks$.subscribe((val) => {
      if (val) {
        this.tasks.unshift(val);
      }
    });
    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
      if (!word) {

        if (this.committeeId) {
          if (this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId)) {
            this.getfilteredTasks(false, { typeId: this.TasksFilterEnum.Underprocedure }, this.committeeId, this.validityPeriod.validityPeriodFrom, this.validityPeriod.validityPeriodTo);
            this.getTasksStatisticsNum();
            this.tab = 'tab3';
          } else {
            this.getfilteredTasks(false, { typeId: this.TasksFilterEnum.all }, this.committeeId, this.validityPeriod.validityPeriodFrom, this.validityPeriod.validityPeriodTo);
            this.getTasksStatisticsNum();
            this.tab = 'tab3';
          }
        } else {
          this.getfilteredTasks(false, { typeId: this.TasksFilterEnum.Underprocedure }, this.committeeId);
          this.getTasksStatisticsNum();
          this.tab = 'tab3';
        }
      }
    });
    this.toggleCompeleteSubs = this.taskservice.toggleCompeleteTask.subscribe((res) => {
      if (res) {
        this.getfilteredTasks(false, { typeId: this.TasksFilterEnum.Underprocedure }, this.committeeId);
        this.getTasksStatisticsNum();
        this.tab = 'tab3';
      }
    })
    this.taskservice.toggleTasksFilter.subscribe((value) => {
      this.togglebetweenTasks = value;
    });
    this.tasksSubscription = this.taskservice.tasksFilters.subscribe(
      (res) => {
        if (res) {
          this.selectedFilterObject = res;
          // this.tab = 'tab0' ;

          if (this.committeeId) {
            this.getfilteredTasks(false, res, this.committeeId, this.validityPeriod.validityPeriodFrom, this.validityPeriod.validityPeriodTo);
          } else {
            this.getfilteredTasks(false, res);
          }
        }
      }
    );
    this.fromNotification = this.taskservice.fromNotifications.subscribe((res) => {
      if (res) {
        this.filterWzClick(2)
      }
    })
    this.subscribeToPeriodChangeForCommittee();
    if (this.committeeId) {
      this.CommitteName = this.committeeService.CommitteName;
    }
  }

  ngOnDestroy() {
    if (this.subscription) this.subscription.unsubscribe();
    if (this.stateSubscription) this.stateSubscription.unsubscribe();
    if (this.tasksSubscription) this.tasksSubscription.unsubscribe();
    if (this.toggleCompeleteSubs) this.toggleCompeleteSubs.unsubscribe();
    this.taskservice.toggleCompeleteTask.next(undefined);
    this.taskservice.fromNotifications.next(undefined)

  }

  onScroll() {
    // if (this.currentCount < this.count) {
    //   this.skip += this.take;
    //   this.getTasks(true);
    // }
  }

  onFilterScroll() {
    if (this.currentfilterCount < this.filterCount) {
      this.filterskip += this.filterTake;
      if (this.committeeId) {
        this.getfilteredTasks(true, this.selectedFilterObject, this.committeeId);
      } else {
        this.getfilteredTasks(true, this.selectedFilterObject);
      }
    }
  }
  subscribeToPeriodChangeForCommittee() {
    this.stateSubscription =
      this.committeeService.committeePeriodChange$.subscribe((period) => {
        if (period && this.committeeId) {
          if (this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId)) {
            this.selectedFilterObject = { typeId: 7 }
          } else {
            this.selectedFilterObject = { typeId: 1 }
          }
          this.validityPeriod = period;
          this.getfilteredTasks(false, this.selectedFilterObject, this.committeeId, this.validityPeriod.validityPeriodFrom, this.validityPeriod.validityPeriodTo)
        } else {
          this.selectedFilterObject = { typeId: 7 }
          this.getfilteredTasks(false, this.selectedFilterObject, this.committeeId)
        }
        this.getTasksStatisticsNum();
      });
  }

  // getTasks(scroll: boolean = false, searchWord: string = undefined) {
  //   if (!scroll) {
  //     this.loadingData = true;
  //     this.layoutService.toggleSpinner(true);
  //     this.skip = 0;
  //   }

  //   if(this.committeeId){
  //     let dateFromString = this.validityPeriod.validityPeriodFrom.getTime();
  //     let dateToString = this.validityPeriod.validityPeriodTo.getTime();

  //     let dateFrom = new Date(dateFromString).toISOString();
  //     let dateTo = new Date(dateToString).toISOString();
  //     if (searchWord) {
  //       this.filters = [
  //         { field: 'title', operator: 'contains', value: searchWord },
  //         {
  //           field: 'createdOn',
  //           operator: 'gte',
  //           value: dateFrom,
  //         },
  //         {
  //           field: 'createdOn',
  //           operator: 'lte',
  //           value: dateTo,
  //         },
  //         { field: 'commiteeId', operator: 'eq', value:this.committeeId },
  //       ];
  //     } else {
  //       this.filters = [
  //         {
  //           field: 'createdOn',
  //           operator: 'gte',
  //           value: dateFrom,
  //         },
  //         {
  //           field: 'createdOn',
  //           operator: 'lte',
  //           value: dateTo,
  //         },
  //         { field: 'commiteeId', operator: 'eq', value:this.committeeId},
  //       ];
  //     }
  //   }else {
  //     if(searchWord){
  //       this.filters = [{ field: 'Title', operator: 'contains', value: searchWord}]
  //     } else {
  //       this.filters = []
  //     }
  //   }

  //   this.taskservice
  //     .getCommitteeTasks(
  //       this.take,
  //       this.skip,
  //       this.committeeId,
  //       this.filters,
  //       'and',
  //       this.validityPeriod.validityPeriodFrom,
  //       this.validityPeriod.validityPeriodTo,
  //       undefined,
  //       undefined
  //     )
  //     .subscribe((res) => {
  //       if (res && res.data) {
  //         this.tasks = scroll ? [...this.tasks, ...res.data] : res.data;
  //         this.count = res.count;

  //         if (scroll) {
  //           this.currentCount += res.data.length;
  //         } else {
  //           this.currentCount = res.data.length;
  //         }
  //       }
  //       this.layoutService.toggleSpinner(false);
  //       this.loadingData = false;
  //     });
  // }



  getfilteredTasks(scroll: boolean = false, result: TaskFilters, commiteeId?: number, validityPeriodFrom?, validityPeriodTo?) {
    if (!scroll) {
      this.loadingData = true;
      this.layoutService.toggleSpinner(true);
      this.filterskip = 0;
    }
    this.togglebetweenTasks = false;
    if (commiteeId) {
      if (result.classificationId && result.searchtxt) {
        this.tasksFilter = [
          {
            field: 'ComiteeTaskCategoryId',
            operator: 'eq',
            value: result.classificationId,
          },
          { field: 'title', operator: 'contains', value: result.searchtxt },
          { field: 'commiteeId', operator: 'eq', value: commiteeId },
        ];
      } else if (result.searchtxt && !result.classificationId) {
        {
          this.tasksFilter = [
            { field: 'title', operator: 'contains', value: result.searchtxt },
            { field: 'commiteeId', operator: 'eq', value: commiteeId },
          ];
        }
      } else if (result.classificationId && !result.searchtxt) {
        this.tasksFilter = [
          {
            field: 'ComiteeTaskCategoryId',
            operator: 'eq',
            value: result.classificationId,
          },
          { field: 'commiteeId', operator: 'eq', value: commiteeId },
        ];
      }
      else {
        this.tasksFilter = [
          { field: 'commiteeId', operator: 'eq', value: commiteeId },
        ];
      }
    } else {
      if (result.classificationId && result.searchtxt) {
        this.tasksFilter = [
          {
            field: 'ComiteeTaskCategoryId',
            operator: 'eq',
            value: result.classificationId,
          },
          { field: 'title', operator: 'contains', value: result.searchtxt },
        ];
      }
      else if (result.searchtxt && !result.classificationId) {
        this.tasksFilter = [
          { field: 'title', operator: 'contains', value: result.searchtxt },
        ];
      }
      else if (result.classificationId && !result.searchtxt) {
        this.tasksFilter = [
          {
            field: 'ComiteeTaskCategoryId',
            operator: 'eq',
            value: result.classificationId,
          },
        ];
      }
      else {
        this.tasksFilter = [];
      }
    }

    switch (+result.typeId) {
      case 1:
        this.tab = 'tab1';
        break;
      case 2:
        this.tab = 'tab2'
        break;
      case 6:
        this.tab = 'tab4'
        break;
      case 7:
        this.tab = 'tab3'
        break;
      default:
        this.tab = 'tab0'
        break;
    }

    if (result.body) {
      this.taskservice
        .getFilteredTasks(
          this.filterTake,
          this.filterskip,
          this.committeeId,
          this.tasksFilter,
          'and',
          result.typeId,
          undefined,
          undefined,
          result.body.fromDate,
          result.body.toDate,
          result.body.mainUserId,
          result.body.mainAssinedUserId,
          validityPeriodFrom,
          validityPeriodTo,
          result.body.committeId,
          result.body.meetingId
        )
        .subscribe((res) => {
          if (res && res.data) {
            this.tasks = scroll ? [...this.tasks, ...res.data] : res.data;
            this.filterCount = res.count;
            if (scroll) {
              this.currentfilterCount += res.data.length;
            } else {
              this.currentfilterCount = res.data.length;
            }
          }
          this.layoutService.toggleSpinner(false);
          this.loadingData = false;
        });
    } else {
      this.taskservice
        .getFilteredTasks(
          this.filterTake, this.filterskip, this.committeeId, this.tasksFilter, 'and', result.typeId, undefined, undefined, undefined,
          undefined, undefined, undefined, validityPeriodFrom,
          validityPeriodTo
        )
        .subscribe((res) => {
          if (res && res.data) {
            this.tasks = scroll ? [...this.tasks, ...res.data] : res.data;
            this.filterCount = res.count;
            if (scroll) {
              this.currentfilterCount += res.data.length;
            } else {
              this.currentfilterCount = res.data.length;
            }
          }
          this.layoutService.toggleSpinner(false);
          this.loadingData = false;
        });
    }
  }

  getTasksStatisticsNum() {
    this.loadingTaskCount = true;
    if (this.committeeId) {
      this.taskservice
        .getStatistisTasksNumber(undefined, undefined, this.BrowserService.encryptCommitteId(this.committeeId), this.validityPeriod.validityPeriodFrom, this.validityPeriod.validityPeriodTo)
        .subscribe((result) => {
          if (result) {
            this.taskservice.shareTaskStats.next(result)
            this.loadingTaskCount = false;
            this.statisticsCount = result;
            this.permissionsValues.map((x, index) => {
              this.statisticsCount.map((z, y) => {
                if (index === y) {
                  z.permissionValue = x
                }
              })
            })
          }
        });
    } else {
      this.taskservice
        .getStatistisTasksNumber(undefined, undefined)
        .subscribe((result) => {
          if (result) {
            this.taskservice.shareTaskStats.next(result)
            this.loadingTaskCount = false;
            this.statisticsCount = result;
            this.permissionsValues.map((x, index) => {
              this.statisticsCount.map((z, y) => {
                if (index === y) {
                  z.permissionValue = x
                }
              })
            })
          }
        });
    }
  }
  filterWzClick(i: number) {
    let dateFrom = this.validityPeriod ? this.validityPeriod.validityPeriodFrom : undefined;
    let dateTo = this.validityPeriod ? this.validityPeriod.validityPeriodTo : undefined
    switch (i) {
      case 1:
        this.getfilteredTasks(false, { typeId: this.TasksFilterEnum.all }, this.committeeId, dateFrom, dateTo);
        this.selectedFilterObject = { typeId: this.TasksFilterEnum.all }
        this.requiredTaskEnum = 1;
        this.tab = 'tab1';
        break;
      case 2:
        this.getfilteredTasks(false, { typeId: this.TasksFilterEnum.late }, this.committeeId, dateFrom, dateTo)
        this.selectedFilterObject = { typeId: this.TasksFilterEnum.late }
        this.requiredTaskEnum = 2;
        this.tab = 'tab2';
        break;
      case 3:
        this.getfilteredTasks(false, { typeId: this.TasksFilterEnum.Underprocedure }, this.committeeId, dateFrom, dateTo);
        this.selectedFilterObject = { typeId: this.TasksFilterEnum.Underprocedure }
        this.requiredTaskEnum = 7;
        this.tab = 'tab3';
        break;
      case 4:
        this.getfilteredTasks(false, { typeId: 9 }, this.committeeId, dateFrom, dateTo);
        this.selectedFilterObject = { typeId: 9 }
        this.requiredTaskEnum = 9;
        this.tab = 'tab4';
        break;
      case 5:
        this.getfilteredTasks(false, { typeId: 8 }, this.committeeId, dateFrom, dateTo);
        this.selectedFilterObject = { typeId: 8 }
        this.requiredTaskEnum = 8;
        this.tab = 'tab5'
        break;
      case 6:
        this.getfilteredTasks(false, { typeId: 10 }, this.committeeId, dateFrom, dateTo);
        this.selectedFilterObject = { typeId: 10 }
        this.requiredTaskEnum = 10;
        this.tab = 'tab6'
        break;
    }
    this.togglebetweenTasks = false;
    this.taskservice.filterWithClick.next(i)
  }
  checkTasksPermissions(permission: string) {
    if (this.committeeId) {
      if (this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId)) {
        if (this.committeeService.checkPermission(permission) && permission !== 'taskToView') {
          return true
        } else {
          return false
        }
      } else {
        this.showTasksWithCommittePermission = false
      }

    } else {
      if (this.authService.isAuthUserHasPermissions([permission])) {
        return true
      } else {
        return false
      }
    }

  }
}
