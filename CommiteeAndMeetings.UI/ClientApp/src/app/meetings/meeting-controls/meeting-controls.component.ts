import { Component, OnInit } from '@angular/core';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { MeetingActions } from 'src/app/shared/_enums/AppEnums';
import { Router, NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';
import { ThemeService } from 'src/app/shared/_services/theme.service';
@Component({
  selector: 'app-meeting-controls',
  templateUrl: './meeting-controls.component.html',
  styleUrls: ['./meeting-controls.component.scss'],
})
export class MeetingControlsComponent implements OnInit {
  currentUrl: string;
  hideControlsBtn: boolean = false;
  constructor(
    private modalService: SharedModalService,
    private router: Router,
    private _location: Location,
    public themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this.checkRoute();
  }

  addNewMeeting() {
    this.modalService.openDrawerModal(MeetingActions.CreateNewMeeting);
  }

  checkRoute() {
    this.currentUrl = this.router.routerState.snapshot.url;
    if (
      this.currentUrl.includes('schedule-meeting') ||
      this.currentUrl.includes('multiple-meeting')
    ) {
      this.hideControlsBtn = true;
    } else {
      this.hideControlsBtn = false;
    }
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = this.router.routerState.snapshot.url;
        if (
          this.currentUrl.includes('schedule-meeting') ||
          this.currentUrl.includes('multiple-meeting')
        ) {
          this.hideControlsBtn = true;
        } else {
          this.hideControlsBtn = false;
        }
      }
    });
  }
  backClicked() {
    this.router.navigateByUrl('/meetings');
  }
}
