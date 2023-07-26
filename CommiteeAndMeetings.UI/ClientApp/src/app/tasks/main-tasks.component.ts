import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CountResultDTO } from '../core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from '../shared/_services/browser-storage.service';
import { TasksService } from './tasks.service';

@Component({
  selector: 'app-main-tasks',
  templateUrl: './main-tasks.component.html',
  styleUrls: ['./main-tasks.component.scss']
})
export class MainTasksComponent implements OnInit {
  committeeId: string;
  loadingTaskCount: boolean = false;
  statisticsCount: CountResultDTO[] = [];
  constructor(private route: ActivatedRoute,private taskservice: TasksService,private BrowserService:BrowserStorageService) {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
   }

  ngOnInit(): void {
  }
  getTasksStatisticsNum() {
    this.loadingTaskCount = true;
    if(this.committeeId){
      this.taskservice
      .getStatistisTasksNumber(undefined,undefined,this.BrowserService.encryptCommitteId(this.committeeId))
      .subscribe((result) => {
        if (result) {
          this.loadingTaskCount = false;
          this.statisticsCount = result;
        }
      });
    } else {
      this.taskservice
      .getStatistisTasksNumber(undefined,undefined)
      .subscribe((result) => {
        if (result) {
          this.loadingTaskCount = false;
          this.statisticsCount = result;
        }
      });
    }
  }
}
