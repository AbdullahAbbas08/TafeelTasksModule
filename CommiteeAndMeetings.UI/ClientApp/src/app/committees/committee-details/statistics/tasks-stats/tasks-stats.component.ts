import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { UserTaskCountDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-tasks-stats',
  templateUrl: './tasks-stats.component.html',
  styleUrls: ['./tasks-stats.component.scss']
})
export class TasksStatsComponent implements OnInit {
  tasks: any[] = [];
  committeeId:string;
  tasksStats:UserTaskCountDTO[] =[]
  constructor(private router : ActivatedRoute,private _statistics:StatisticsService,public translateService: TranslateService,private browserService:BrowserStorageService) {
    this.router.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
   }

  ngOnInit(): void {
    this.getStats()
  }
  getStats(){
    this._statistics.getStatsPerUser(this.browserService.encryptCommitteId(this.committeeId)).subscribe((res) => {
      this.tasksStats = res
    })
  }
}
