import { MultipleMeetingService } from './../multiple-meeting.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MeetingDataComponent } from '../../meeting-data/meeting-data.component';

@Component({
  selector: 'app-multiple-meetings',
  templateUrl: './multiple-meetings.component.html',
  styleUrls: ['./multiple-meetings.component.scss'],
})
export class MultipleMeetingsComponent implements OnInit, OnDestroy {
  @ViewChild(MeetingDataComponent) meetingDataComponent: MeetingDataComponent;
  formValid: boolean = false;
  meetingApproved: boolean = false;
  refId: number;

  constructor(private activatedRoute: ActivatedRoute, private multiMeeting: MultipleMeetingService) {}
  ngOnDestroy(): void {
    this.multiMeeting.userDetails = undefined;
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(
      (params) => (this.refId = +params.get('refId'))
    );
  }

  ngAfterViewInit() {

    !this.refId && this.meetingDataComponent.meetingForm.statusChanges.subscribe((value) => {
      this.formValid = value === 'VALID' ? true : false;
    });
  }

  saveForm() {
    this.meetingDataComponent.onSubmit();
  }
}
