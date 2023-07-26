import { MultipleMeetingService } from './../../schdule-multiple-meetings/multiple-meeting.service';
import { SingleMeetingService } from './../single-meeting.service';
import { UserDetailsDTO } from './../../../core/_services/swagger/SwaggerClient.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-creator-details',
  templateUrl: './creator-details.component.html',
  styleUrls: ['./creator-details.component.scss'],
})
export class CreatorDetailsComponent implements OnInit {
  @Input() multi;
  meetingId: number;
  user: UserDetailsDTO | any;
  createdUserName;
  currentLang;
  createdUserImage;
  createdUserMobile;
  createdUserEmail;
  isCollapsed = true;
  constructor(
    private authService: AuthService,
    private translateService: TranslateService,
    private activatedRoute: ActivatedRoute,
    private singleMeeting: SingleMeetingService,
    private multiMeet: MultipleMeetingService
  ) {}

  ngOnInit(): void {
    this.checkMode();
  }

  setCreatedUserDetails() {
     
      this.createdUserName =
      this.currentLang === 'ar' ? this.user?.fullNameAr : this.user?.fullNameEn;

    this.createdUserImage = this.user?.profileImage
      ? this.user?.profileImage
      : this.user?.userImage;
    this.createdUserEmail = this.user?.email;
    this.createdUserMobile = this.user?.mobile;

  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.setCreatedUserDetails();
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
      this.setCreatedUserDetails();
    });
  }

  checkMode() {
    if (this.multi) {
      this.user = this.multiMeet.userDetails
      ? this.multiMeet.userDetails
      : this.authService.getUser();
    this.langChange();
    return;
    }
    this.user = this.singleMeeting.meeting?.createdByUser
      ? this.singleMeeting.meeting?.createdByUser
      : this.authService.getUser();
    this.langChange();
  }
}
