import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SwaggerClient, UserDetailsDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Component({
  selector: 'app-close-meeting-modal',
  templateUrl: './close-meeting-modal.component.html',
  styleUrls: ['./close-meeting-modal.component.scss']
})
export class CloseMeetingModalComponent implements OnInit {
  meetingId:number;
  currentLang:string;
  UserDetailsDTO:UserDetailsDTO[]=[];
  isLoading:boolean=false;
  constructor(private translate:TranslateService,private swagger:SwaggerClient) { 
    this.currentLang = this.translate.currentLang;
  }

  ngOnInit() {
    this.getData()
  }

  getData(){
    this.isLoading=true;
    this.swagger.apiMeetingsCloseMeetingNewGet(this.meetingId).subscribe(res=>{
      this.UserDetailsDTO=res;
      this.isLoading=false;
    });
  }

}
