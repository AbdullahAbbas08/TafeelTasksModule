import { ActivatedRoute } from '@angular/router';
import { SingleMeetingService } from './../single-meeting.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import {
  AttendeeState,
  MeetingAttendeeDTO,
  MeetingUserDTO,
  UserType,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { LayoutService } from 'src/app/shared/_services/layout.service';
@Component({
  selector: 'app-add-attendees',
  templateUrl: './add-attendees.component.html',
  styleUrls: ['./add-attendees.component.scss'],
})
export class AddAttendeesComponent implements OnInit, OnDestroy {
  attendees: MeetingAttendeeDTO[] = [];
  userType = UserType;
  meetingDate: Date;
  meetingId: number;

  meetingClosed = false;
  
  isCoordinator = false;
  isCreator = false;
  listOfAttendees:MeetingUserDTO[] =[]
  constructor(
    public singleMeeting: SingleMeetingService,
    private activatedRoute: ActivatedRoute,
    private storeService: StoreService,
    public translate: TranslateService,
    private notificationService: NzNotificationService,
    private layoutService: LayoutService,
  ) {}

  ngOnInit() {
    this.checkMode();
    this.setMeetingdata()
    this.isCreator = this.singleMeeting.meeting?.isCreator;
    this.isCoordinator = this.singleMeeting.meeting?.isCoordinator;
    this.meetingClosed = this.singleMeeting.meetingClosed;
    this.storeService.onDeleteUser$.asObservable().subscribe((val) => {
      if(val){
        this.listOfAttendees.map(((ele, index) => {
          if(ele.userId == val){
            this.listOfAttendees.splice(index,1)
          }
        }))
      }
    })
  }

  ngOnDestroy(): void {
    this.storeService.refreshAttendees$.next(undefined)
  }

  onAddAttendee(userId) {
    if(userId){
      userId.map((res) => {
        this.listOfAttendees.push(
          new MeetingUserDTO({
            userId:res,
            userType: UserType._2,
            meetingId: this.meetingId,
          })
        )
      })
    }
    this.singleMeeting.addAttendee(this.listOfAttendees).subscribe((res) => {
      if(res){
        res.forEach((val) => {
          this.attendees.push(
             new MeetingAttendeeDTO({
             attendee: val.user,
             state: AttendeeState._1,
             available: val.available,
             attendeeId: val.userId,
           })
         );
        })
        this.singleMeeting.meeting.meetingAttendees = [...this.attendees];
        this.layoutService.toggleSpinner(false);
        this.storeService.refreshAttendees$.next(res);
      }
    })
  }

  setMeetingdata() {
    this.attendees = this.singleMeeting.meeting?.meetingAttendees;
    this.meetingDate = this.singleMeeting.meeting?.date;
  }

  checkMode() {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.meetingId = +params.get('id');

      if (this.meetingId) {
        this.setMeetingdata();
      }
    });
  }
}
