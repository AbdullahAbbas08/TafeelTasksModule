import { Component, OnInit } from '@angular/core';
import { DataSourceRequest, MeetingDetailsDTO, MeetingDetailsDTODataSourceResult } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DashboardService } from '../dashboard.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { AuthService } from 'src/app/auth/auth.service';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-finished-meetings',
  templateUrl: './finished-meetings.component.html',
  styleUrls: ['./finished-meetings.component.scss']
})
export class FinishedMeetingsComponent implements OnInit {
  take:number = 10;
  skip:number = 0;
  count:number = 0;
  loadingData: boolean = false;
  finishedMeetings:MeetingDetailsDTO[] = []
  
  constructor(public translateService:TranslateService,private _dashBoardService:DashboardService,private layoutService: LayoutService) { }

  ngOnInit(): void {
    this.getFinishedMeetings()
  }
  onScroll(){
    if(this.count > this.finishedMeetings.length){
      this.skip += this.take;
      this.getFinishedMeetings(true)
    }
  }
  getFinishedMeetings(scroll: boolean = false){
    if (!scroll) {
      this.loadingData = true;
      this.layoutService.toggleSpinner(true);
      this.skip = 0;
    }
    let body = new DataSourceRequest({
      take:this.take,
      skip:this.skip,
      sort:[],
      countless:false
    })
    this._dashBoardService.getFinishedMeetings(body).subscribe((meeting:MeetingDetailsDTODataSourceResult) => {
      if(meeting && meeting.data){
        this.finishedMeetings = scroll ? [...this.finishedMeetings ,...meeting.data] : meeting.data;
        this.count = meeting.count;
      }
      this.layoutService.toggleSpinner(false);
      this.loadingData = false;
    })
  }
}
