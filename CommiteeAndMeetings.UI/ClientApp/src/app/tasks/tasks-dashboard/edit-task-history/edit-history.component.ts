import { Component,OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { UpdateTaskLogDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
import { TasksService } from '../../tasks.service';



@Component({
  selector: 'app-edit-history',
  templateUrl: './edit-history.component.html',
  styleUrls: ['./edit-history.component.scss'],
})
export class EditHistoryComponent implements OnInit {
  committeId: number;
  committeTaskId:number;
  isLoading:boolean = false;
  listofEditHistory:UpdateTaskLogDTO[]=[];
  constructor(private _taskService:TasksService,public translateService: TranslateService,private browserService:BrowserStorageService,public dateService:NgbDateService) {}

  ngOnInit(): void {
    this.getEditTaskHistory()
  }
  getEditTaskHistory(){
    const encryptedId:string = this.browserService.encrypteString(`${this.committeTaskId}_${this.browserService.getUserRoleId()}`)
    this.isLoading = true
    this._taskService.editTaskHistory(encryptedId).subscribe((result) => {
       if(result){
         this.isLoading = false;
         this.listofEditHistory = result
       }
    })
  }
}
