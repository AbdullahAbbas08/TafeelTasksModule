import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import * as echarts from 'echarts';
import { CountResultDTO, LookUpDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { StatsArray, TasksService } from 'src/app/tasks/tasks.service';
import { Subscription } from 'rxjs';
import { SearchService } from '../../_services/search.service';
import { BrowserStorageService } from '../../_services/browser-storage.service';
import { BehaviorSubject } from 'rxjs';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-tasks-chart',
  templateUrl: './tasks-chart.component.html',
  styleUrls: ['./tasks-chart.component.scss']
})
export class TasksChartComponent implements OnInit, OnDestroy {
  committeeId: string;
  statisticsCount: CountResultDTO[] = [];
  statisticsArray: StatsArray[] = [];
  subscription: Subscription;
  isFullScrren: boolean = false;
  status: boolean = false;
  users: LookUpDTO[] = [];
  usersChanged$ = new BehaviorSubject('');
  isLoading: boolean = false;
  lookupTypes = SettingControllers;

  constructor(private _authService: AuthService, public lookupService: LookupService, private searchService: SearchService, private router: Router, public translateService: TranslateService, private taskservice: TasksService, private BrowserService: BrowserStorageService) {
    let routeArr: string[] = this.router.url.split('/');
    this.committeeId = routeArr[routeArr.length - 2];
  }

  ngOnInit(): void {
    this.taskservice.shareTaskStats.subscribe((result) => {
      if (result) {
        this.statisticsCount = result;
        setTimeout(() => { this.drawCharts() }, 200)
      }
    })
  }
  ngOnDestroy() {
    if (this.subscription) this.subscription.unsubscribe();
  }
  drawCharts() {
    type EChartsOption = echarts.EChartsOption;
    var chartDom = document.getElementById('main');
    var myChart = echarts.init(chartDom);
    var option: EChartsOption;
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
          radius: ['40%', '70%'],

          avoidLabelOverlap: true,
          label: {
            show: false,
            position: 'center',

          },
          emphasis: {
            label: {
              show: false,
              fontSize: '20',
              fontWeight: 'bold',

            }
          },
          labelLine: {
            show: false
          },
          // color:['#1A73E8', '#D81B60', '#191919', '#43A047', '#f7cd00'],
          color: ['#1A73E8', '#D81B60', '#191919', '#43A047', '#6d7475', '#f7cd00'],
          data: [
            { value: this._authService.isAuthUserHasPermissions(['allTasks']) ? this.statisticsCount[0].count : 0, name: '' }, //All tasks
            { value: this._authService.isAuthUserHasPermissions(['lateTasks']) ? this.statisticsCount[1].count : 0, name: '' }, // lateTasks
            { value: this._authService.isAuthUserHasPermissions(['assisstantTasks']) ? this.statisticsCount[4].count : 0, name: '' }, // assistant users
            { value: this._authService.isAuthUserHasPermissions(['closedTasks']) ? this.statisticsCount[3].count : 0, name: '' }, // All close
            { value: this._authService.isAuthUserHasPermissions(['taskToView']) ? this.statisticsCount[5].count : 0, name: '' }, // tasks to view
            { value: this.statisticsCount[2].count, name: '' }, // Underprocedure
          ]
        }
      ]
    };

    option && myChart.setOption(option);
    // myChart.on('click', function(params) {
    //    console.log(params.value)
    // });
  }
  getTasksStatisticsNum(userId?: any) {
    if (this.committeeId) {
      this.taskservice
        .getStatistisTasksNumber(undefined, undefined, this.BrowserService.encryptCommitteId(this.committeeId))
        .subscribe((result) => {
          if (result) {
            this.statisticsCount = result;
            this.statisticsCount.map((x, i) => {
              this.statisticsArray.push({
                index: i, value: x.count, name: this.translateService.get(x.name)
                  .toPromise()
              })
            })
            setTimeout(() => { this.drawCharts() }, 100)
          }
        });
    } else {
      this.taskservice
        .getStatistisTasksNumber(undefined, userId)
        .subscribe((result) => {
          if (result) {
            this.statisticsCount = result;
            this.statisticsCount.map((x, i) => {
              this.statisticsArray.push({
                index: i, value: x.count, name: this.translateService.get(x.name)
                  .toPromise()
              })
            })
            setTimeout(() => { this.drawCharts() }, 100)
          }
        });
    }
  }


  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
      default:
        break;
    }
  }
}
