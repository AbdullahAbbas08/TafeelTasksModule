import { SingleMeetingService } from './../single-meeting.service';
import {
  MeetingState,
  MeetingTopicDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { MeetingDataComponent } from '../../meeting-data/meeting-data.component';
import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ThemeService } from 'src/app/shared/_services/theme.service';

@Component({
  selector: 'app-single-meeting',
  templateUrl: './single-meeting.component.html',
  styleUrls: ['./single-meeting.component.scss'],
})
export class SingleMeetingComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  @ViewChild(MeetingDataComponent) meetingDataComponent: MeetingDataComponent;
  formValid: boolean = false;
  initial = 0;
  formChanged: boolean = true;
  meetingApproved: boolean = false;
  showAgenda = false;
  showMoM = false;
  showAttendees = false;
  showMeetingInformation:boolean = true;
  selectedIndex = 0;
  meetingState = MeetingState;
  meetingId: number;
  meetingStarted:boolean = false
  constructor(public singleMeeting: SingleMeetingService,private activatedRoute: ActivatedRoute,public themeService: ThemeService) {}

  ngOnInit(): void {
    this.checkMode()
  }

  ngOnDestroy(): void {
    this.singleMeeting.meeting = undefined;
    localStorage.removeItem('selectedIndex')
  }

  ngAfterViewInit() {
    this.meetingDataComponent.meetingForm.statusChanges.subscribe((value) => {
      this.formValid = value === 'VALID' ? true : false;
    });

    this.meetingDataComponent.meetingForm.valueChanges.subscribe(() => {
      if (this.initial < 2) {
        this.formChanged = false;
        this.initial++;
      } else {
        this.formChanged = true;
      }
    });
  }

  saveForm() {
    this.meetingDataComponent.onSubmit();
  }

  openAgenda(topic: MeetingTopicDTO) {
    if (this.meetingApproved) {
      // this.selectedIndex = 2
      this.selectedIndex = 1;
      this.meetingStarted = true
    }
  }
  checkMode() {
    this.activatedRoute.paramMap.subscribe((params) => {
      this.meetingId = +params.get('id');
      if(localStorage.getItem('selectedIndex') === '3'){
        this.selectedIndex = 3;
        this.showMoM = true
    }
    });
  }
  checkMeeting($event){
    this.meetingStarted = $event
  }
  addIndex(){
    this.selectedIndex = this.selectedIndex - 1
  }
  removeIndex(){
    this.selectedIndex = this.selectedIndex + 1
  }
}
