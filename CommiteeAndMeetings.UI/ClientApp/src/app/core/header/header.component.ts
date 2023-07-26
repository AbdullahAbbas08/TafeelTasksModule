import { SignalRService } from './../../shared/_services/signal-r.service';
import { Router, NavigationEnd } from '@angular/router';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { AuthTicketDTO } from './../_services/swagger/SwaggerClient.service';
import { AuthService } from 'src/app/auth/auth.service';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LocalizationService } from 'src/app/shared/_services/localization.service';
import { SharedModalService } from '../_services/modal.service';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { NotificationsService } from './notifications/notifications.service';
import { ThemeService } from 'src/app/shared/_services/theme.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  actionTypes = CommitteeActions;
  user: AuthTicketDTO;
  urlLink: string;
  reportLink:string;
  currentLang: string;
  routingFlag: boolean = false;
  currentUrl: string;
  count: number = 0;
  hideIcon:boolean = false;
  hideMasarIcon:boolean = false;
  hideReportModule:boolean = false;
  constructor(
    private translateService: TranslateService,
    private _modalService: SharedModalService,
    private authService: AuthService,
    private swagger: SwaggerClient,
    private router: Router,
    private signalRService: SignalRService,
    private notificationService: NotificationsService,
    public themeService: ThemeService,
    private bs: BrowserStorageService,
  ) {
    localStorage.setItem('isHijriDateCalendar', '1');
    localStorage.setItem('hijriDaysValue', '0');
    router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        if (
          this.router.url.includes('committees') ||
          this.router.url.includes('settings')
        ) {
          this.routingFlag = false;
        } else {
          this.routingFlag = true;
        }
      }
    });

  }

  ngOnInit(): void {
    this.authService.currentUserSub().subscribe((user) => {
      this.user = user;
    });
    this.getMasarUrl();
    this.getReportUrl();
    this.langChange();
    this.signalRService.initialize();
    this.signalRService.checkIfClosed();
    this.checkCommitteIcon()
    this.getNotificationsTotalCount();
    this.themeService.getCurrentTheme("CommitteeAndMeetingAndTasksModuleThemeFSC");
  }

  getMasarUrl() {
    this.swagger
      .apiCommiteesGeMasarUrlGet()
      .subscribe((url) => (this.urlLink = url));
  }
 getReportUrl(){
  this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet('ShowhideReportModule').subscribe((res) => {
    if(res.systemSettingValue === "1"){
      this.hideReportModule = true 
      this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet('ShowHideIconReportingTool').subscribe((res) =>{
        (this.reportLink = res.systemSettingValue)
      })
    } else {
      this.hideReportModule = false 
    }
  })

 }
 getUserRoleId(){
  return this.bs.decrypteString(
    JSON.parse(localStorage.getItem("user"))["userRoleId"]
  );
 }
  changeLanguage() {
    this._modalService.openDrawerModal(CommitteeActions.changeLang);
  }

  onLogout() {
    sessionStorage.setItem('ssoKey', '1')
    // this.router.navigate(['/auth/login']);
    this.authService.logoutUser()
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
  mainNav() {
    this.router.navigate(['/committees']);
  }
  navigateBtn() {
    this.currentUrl = this.router.routerState.snapshot.url;
    if (
      this.currentUrl.includes('committees') ||
      this.currentUrl.includes('settings')
    ) {
      this.router.navigateByUrl('/meetings');
    } else {
      this.router.navigateByUrl('/committees');
    }
  }
  
  getNotificationsTotalCount() {
    this.notificationService.getNotificationCount().subscribe((res) => {
      if (res) this.count = res;
    });
  }
  checkCommitteIcon(){
    this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet("ShowHideCommiteeMeetingModule").subscribe((res) => {
      if(res.systemSettingValue == '1'){
         this.hideIcon = true
      } else if(res.systemSettingValue == '0'){
        this.hideIcon = false
      }
    })
    this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet("ShowHideiconMasar").subscribe((res) => {
      if(res.systemSettingValue == '1'){
        this.hideMasarIcon = true
      } else if (res.systemSettingValue == '0'){
        this.hideMasarIcon = false
      }
    })
  }
}
