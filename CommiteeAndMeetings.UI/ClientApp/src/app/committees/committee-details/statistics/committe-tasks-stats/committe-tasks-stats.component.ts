import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import * as echarts from 'echarts';
import { CountResultDTO, ValidityPeriodDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { StatsArray, TasksService } from 'src/app/tasks/tasks.service';
import { Subscription } from 'rxjs';
import { SearchService } from 'src/app/shared/_services/search.service';
import { CommitteeService } from 'src/app/committees/committee.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-committe-tasts-stats',
  templateUrl: './committe-tasks-stats.component.html',
  styleUrls: ['./committe-tasks-stats.component.scss']
})
export class CommitteTasksStatsComponent implements OnInit, OnDestroy  {
  committeeId:string;
  statisticsCount: CountResultDTO[] = [];
  statisticsArray:StatsArray[]=[];
  subscription: Subscription;
  validityPeriod: ValidityPeriodDTO;
  stateSubscription: Subscription;
  constructor(private committeeService:CommitteeService, private searchService: SearchService,private router : Router,public translateService: TranslateService,private taskservice: TasksService,private browserService:BrowserStorageService) {
    let routeArr:string[] = this.router.url.split('/');
    this.committeeId = routeArr[routeArr.length - 2]
   }

  ngOnInit(): void {
    this.taskservice.shareTaskStats.subscribe((result) => {
      if(result){
        this.statisticsCount = result;
        setTimeout(()=> {this.drawCharts()},200)
      }
    })
  }
  ngOnDestroy() {
    if (this.subscription) this.subscription.unsubscribe();
    if (this.stateSubscription) this.stateSubscription.unsubscribe();
  }
  drawCharts(){
    type EChartsOption = echarts.EChartsOption;
    var chartDom = document.getElementById('main');
    var myChart = echarts.init(chartDom);
    var option: EChartsOption;
    option = {
        tooltip: {
            trigger: 'item'
          },
        legend: {
          top: '85%',
          left: 'right',
          show:false,
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
            color:['#1A73E8', '#D81B60', '#f7cd00', '#43A047', '#191919'],
            data: [
              {value:this.statisticsCount[0].count,name:''},
              {value:this.statisticsCount[1].count,name:''},
              {value:this.statisticsCount[2].count,name:''},
              {value:this.statisticsCount[3].count,name:''},
              {value:this.statisticsCount[4].count,name:''},
          ]
          }
        ]
      };
      
      option && myChart.setOption(option);
  }
  getTasksStatisticsNum() {
    if(this.committeeId){
      this.taskservice
      .getStatistisTasksNumber(undefined,undefined,this.browserService.encryptCommitteId(this.committeeId),this.validityPeriod.validityPeriodFrom,this.validityPeriod.validityPeriodTo)
      .subscribe((result) => {
        if (result) {
          this.statisticsCount = result;
          this.statisticsCount.map((x,i) => {
              this.statisticsArray.push({index:i,value:x.count,name:this.translateService.get(x.name)
              .toPromise()})
          })
           setTimeout(()=> {this.drawCharts()},100)
        }
      });
    } else {
      this.taskservice
      .getStatistisTasksNumber(undefined,undefined)
      .subscribe((result) => {
        if (result) {
          this.statisticsCount = result;
          this.statisticsCount.map((x,i) => {
            this.statisticsArray.push({index:i,value:x.count,name:this.translateService.get(x.name)
              .toPromise()})
        })
         setTimeout(()=> {this.drawCharts()},100)
        }
      });
    }
  }
  
}
