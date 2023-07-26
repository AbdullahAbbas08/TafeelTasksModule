import { Component, OnInit } from '@angular/core';
import { ChartDTO } from 'src/app/core/_models/chart.model';
import { ActivatedRoute, Router } from '@angular/router';
import { StatisticsService } from '../statistics.service';
import { LineChartDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
})
export class ActivitiesComponent implements OnInit {
  activitiesChart: ChartDTO;
  activities:any[] = []
  userActivities:LineChartDTO[] =[]
  committeeId: string;
  series:any[] = []
  constructor(private route : ActivatedRoute,private _statistics:StatisticsService,private browserService:BrowserStorageService) { 
  }

  ngOnInit(): void {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
  this.getUserActivity()
  }
  onSelect(data): void {
    // console.log('Item clicked', JSON.parse(JSON.stringify(data)));
  }
  getUserActivity(){
    this._statistics.getActivitiesPerMonth(this.browserService.encryptCommitteId(this.committeeId)).subscribe((result) => {
      this.activities = result
      result.forEach((res) => {
        this.series.push({
          name:res?.name,
          value:res?.value
        })
      })
      this.activitiesChart = {
        results: [
         {
           "name":"",
           "series": this.series
      },
        ],
        legend: false,
        animations: true,
        xAxis: true,
        yAxis: true,
        showYAxisLabel: false,
        showXAxisLabel: false,
        timeline: false,
        autoScale: false,
        scheme: {
          domain: ['#0075c9', '#0075c9', '#0075c9', '#0075c9', '#0075c9', '#0075c9']
        }
      };

    })
  }
}
