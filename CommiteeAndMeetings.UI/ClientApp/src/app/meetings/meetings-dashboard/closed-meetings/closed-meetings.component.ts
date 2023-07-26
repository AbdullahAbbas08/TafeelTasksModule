import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DataSourceRequest, MeetingDetailsDTO, MeetingDetailsDTODataSourceResult } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { DashboardService } from '../dashboard.service';

@Component({
  selector: 'app-closed-meetings',
  templateUrl: './closed-meetings.component.html',
  styleUrls: ['./closed-meetings.component.scss']
})
export class ClosedMeetingsComponent implements OnInit {

  take:number = 10;
  skip:number = 0;
  count:number = 0;
  closedMeetings:MeetingDetailsDTO[] = [];
  loadingData: boolean = false;
  constructor(public translateService:TranslateService,private _dashBoardService:DashboardService,private layoutService: LayoutService) { }

  ngOnInit(): void {
    this.getclosedMeetings()
  }
  onScroll(){
    if(this.count > this.closedMeetings.length){
      this.skip += this.take;
      this.getclosedMeetings(true)
    }
  }
  getclosedMeetings(scroll: boolean = false){
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
    this._dashBoardService.getClosedMeetings(body).subscribe((meeting:MeetingDetailsDTODataSourceResult) => {
      if(meeting && meeting.data){
        this.closedMeetings = scroll ? [...this.closedMeetings ,...meeting.data] : meeting.data;
        this.count = meeting.count
      }
      this.layoutService.toggleSpinner(false);
      this.loadingData = false;
    })
  }

}
