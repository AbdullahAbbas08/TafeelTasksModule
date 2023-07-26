import { Component, OnInit,OnDestroy } from '@angular/core';
import { DataSourceRequest, MeetingActivityLookup } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { DashboardService } from '../dashboard.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { MeetingActions } from 'src/app/shared/_enums/AppEnums';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/auth/auth.service';
import { areAllEquivalent } from '@angular/compiler/src/output/output_ast';
import { StoreService } from 'src/app/shared/_services/store.service';
@Component({
  selector: 'app-meetings-activities',
  templateUrl: './meetings-activities.component.html',
  styleUrls: ['./meetings-activities.component.scss']
})
export class MeetingsActivitiesComponent implements OnInit,OnDestroy {
  take:number = 10;
  skip:number = 0;
  count:number = 0;
  loadingData: boolean = false;
  meetingActivites:MeetingActivityLookup[]= [];
  surveyToggle: any = {};
  selectedValue:number = 1
  userId:number;
  listofActivites:any=[];
  constructor(private store:StoreService,private authService: AuthService,private _dashBoardService:DashboardService,private layoutService: LayoutService,private modalService: SharedModalService,private translateService: TranslateService) { }

  ngOnInit(): void {
    this.getAllActivities();
    this.userId = +this.authService.getUser().userId;
    // this.store.refreshAnswerUsersforCurrentUser$.subscribe((val) => {
    //   if(val){      
    //    this.selectedValue = 1;
    //    this.getAllActivities();
    //   }
    // })
  }
  ngOnDestroy() {
    this.store.refreshAnswerUsersforCurrentUser$.next(null)
  }
  openModel(surveyId:number,isCoordinator,isCreator,isMeetingClosed,byPassStartForActivities){
    this.modalService.openDrawerModal(
      MeetingActions.TopicVoting,
      surveyId,
      undefined,undefined,undefined,undefined,undefined,undefined,undefined,isCoordinator,isCreator,isMeetingClosed,byPassStartForActivities
    );
  }
  refresh(){
    this.selectedValue = 1;
    this.getAllActivities();
    this.surveyToggle = {}
  }
  getAllActivities(){
    let body = new DataSourceRequest({
      take:this.take,
      skip:this.skip,
      sort:[],
      countless:false
    })
    this._dashBoardService.getAllActivities(body).subscribe((activities:MeetingActivityLookup[]) => {
      this.meetingActivites = activities.sort((a,b) => (b.meettingDate.getTime() - a.meettingDate.getTime()));
      this.listofActivites = activities.sort((a,b) => (b.meettingDate.getTime() - a.meettingDate.getTime()));
    })
  }
   onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
  }
  filterVoting(event){
    this.meetingActivites = this.listofActivites;
    this.surveyToggle = {}
     if(event == 1){
       this.getAllActivities()
     }else if(event == 2){
      this.meetingActivites=this.meetingActivites.filter((x) => {         
        return (x.isClosed == true ||(x.surveyAnswers.some((x2)=>{
          return x2.some((aa)=>{return aa.some(aq=>aq.userId == this.userId)})==true
        }) ==true || x.surveyAnswers.length == 0
        )&& x.meetingTopicSurveys.every((y) => {         
            return y.surveyAnswers.some((x2)=>{
            return x2.some((aa)=>
              aa.userId == this.userId
              ) ==true;
          }) ==true;
         }))
       })
     } else if (event == 3){
      this.meetingActivites=this.meetingActivites.filter((x) => {         
        return !(x.isClosed == true ||(x.surveyAnswers.some((x2)=>{
          return x2.some((aa)=>{return aa.some(aq=>aq.userId == this.userId)})==true
        }) ==true || x.surveyAnswers.length == 0
        )&& x.meetingTopicSurveys.every((y) => {         
            return y.surveyAnswers.some((x2)=>{
            return x2.some((aa)=>
              aa.userId == this.userId
              ) ==true;
          }) ==true;
         }))
       })
     }
  }
}
