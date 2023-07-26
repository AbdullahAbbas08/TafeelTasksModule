import { Component,OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { TasksService } from '../../tasks.service';
@Component({
  selector: 'app-toggle-task',
  templateUrl: './toggle-task.component.html',
  styleUrls: ['./toggle-task.component.scss'],
})
export class ToggleTaskComponent implements OnInit {
  task:any;
  reasontext:string
  constructor(private _taskService:TasksService,public translateService: TranslateService,private modalService: SharedModalService,  private swagger: SwaggerClient,private browserService:BrowserStorageService) {}

  ngOnInit(): void {
  }
  close() {
    this.modalService.destroyModal();
  }
  submitToggleTask(){
    const encryptId:string = this.browserService.encrypteString(`${this.task.commiteeTaskId}_${this.browserService.getUserRoleId()}`)
    this.swagger
    .apiCommiteeTasksCompleteGet(encryptId,this.reasontext)
    .subscribe((res) => {
      if (res) {
        this._taskService.toggleCompeleteTask.next(res);
        this.close()
      }
    });
  }

}