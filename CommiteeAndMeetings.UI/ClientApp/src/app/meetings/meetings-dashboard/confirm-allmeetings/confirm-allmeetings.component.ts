import { Component, OnInit , OnDestroy,  Output, EventEmitter, } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import {
  DisplayMeetingCallType,
  MeetingAttendeeDTO,
  MeetingCoordinatorDTO,
  MeetingDetailsDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DashboardService } from '../dashboard.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SharedModalService } from 'src/app/core/_services/modal.service';
@Component({
  selector: 'app-confirm-allmeetings',
  templateUrl: './confirm-allmeetings.component.html',
  styleUrls: ['./confirm-allmeetings.component.scss'],
})
export class ConfirmAllmeetingsComponent implements OnInit , OnDestroy{
  fromDate: Date = new Date();
  toDate: Date = new Date();
  meetingsState: MeetingDetailsDTO[] = [];
  userId: number;
  meetingsAttendeId: any;
  loadingData = false;
  subscription:Subscription;
  stateSubscription:Subscription;
  indexNumber:number;
  constructor(
    private _dashboardService: DashboardService,
    public translateService: TranslateService,
    private auth: AuthService,
    private notificationService: NzNotificationService,
    private modelService:SharedModalService
  ) {}

  ngOnInit(): void {
    this.MeetingsState();
  this.subscription =  this.auth.user$.subscribe((value) => {
      this.userId = value.userId;
    });
  this.stateSubscription =  this._dashboardService.changeAttendeCordinatorState.subscribe((res) => {
      if(res){
        this.changeState(res.userId,res.meetingId,res.userType,res.userState)
      }
    })
  this._dashboardService.cancelMeetingState.subscribe((res) => {
    if(res){
      const indexNumber = this.meetingsState.findIndex(x => x.id === res);
      this.meetingsState.splice(indexNumber,1)
    }
  })
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.stateSubscription.unsubscribe();
    this._dashboardService.changeAttendeCordinatorState.next(null);
    this._dashboardService.cancelMeetingState.next(null)
}
  MeetingsState() {
    this.fromDate.setDate(this.fromDate.getDate() - 1);
    this._dashboardService
      .getAllMeetings(this.fromDate,undefined,DisplayMeetingCallType._1)
      .subscribe((meetings: MeetingDetailsDTO[]) => {
        this.meetingsState = meetings;
        this.fromDate = new Date()
      });
  }
  changeState(userId, meetingId, userType, userState) {
    this._dashboardService
      .changeUserState(userId, meetingId, userType, userState)
      .subscribe((res) => {
        if (res) {
          if(userState === 4){
            this._dashboardService.changeMeetingState.next(meetingId)
            this.meetingsState.splice(this.indexNumber,1)
            this.translateService
              .get('userStateChanged')
              .subscribe((translateValue) =>
                this.notificationService.success(translateValue, '')
              );
          } else {
            this.MeetingsState();
            this.fromDate = new Date()
            this.translateService
              .get('userStateChanged')
              .subscribe((translateValue) =>
                this.notificationService.success(translateValue, '')
              );
          }
        }
      });
  }
  openRecommendationModel(id:number,attendes:MeetingAttendeeDTO[],corrdinator:MeetingCoordinatorDTO[],userType, userState,userId,userDelegate,index){
        this.indexNumber = index
       if(!userDelegate){
        this.modelService.initModalRecommendUsers(id,attendes,corrdinator,userType, userState,userId,index)
       } else {
        this.changeState(userId,id,userType,userState)
       }
  }
}
