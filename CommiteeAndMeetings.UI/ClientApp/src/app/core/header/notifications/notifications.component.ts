import { Component, ElementRef, Input, OnInit, Renderer2, ViewChild, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NotificationsService } from './notifications.service';
import { CommitteeNotificationDTO, SwaggerClient } from '../../_services/swagger/SwaggerClient.service';
import { ThemeService } from 'src/app/shared/_services/theme.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { Router } from '@angular/router';
import { TasksService } from 'src/app/tasks/tasks.service';
@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
  host: {
    '(document:click)': 'onClick($event)',
  },
})
export class NotificationsComponent implements OnInit, OnDestroy {
  @Input() count: number;
  notificationCount: number;
  notifications: CommitteeNotificationDTO[];
  loading = false;
  toggleNotificationlist: boolean = false;
  page: number = 0;
  pageSize: number = 10;
  currentLang: string;
  constructor(
    private notificationService: NotificationsService,
    private translateService: TranslateService,
    public themeService: ThemeService,
    private swagger: SwaggerClient,
    private notification: NzNotificationService,
    private _eref: ElementRef,
    private browserService: BrowserStorageService,
    private router: Router,
    private taskService: TasksService
  ) { }

  ngOnInit(): void {
    this.langChange();
    this.getNotificationCount()
  }
  ngOnDestroy() {

    this.taskService.fromNotifications.next(undefined);
  }
  onClick(event) {
    if (!this._eref.nativeElement.contains(event.target))
      this.toggleNotificationlist = false;
    this.page = 0;
  }
  getNotifications(fromScroll = false) {
    if (this.page === 0) {
      this.loading = true;
    }
    this.notificationService
      .getNotifications(this.pageSize, this.page)
      .subscribe((res) => {
        if (res && res.data) {
          this.notifications = fromScroll
            ? [...this.notifications, ...res.data]
            : res.data;
          this.notificationCount = res.count
        }
        this.loading = false;
      });
  }

  toggleNotificationList() {
    this.toggleNotificationlist = !this.toggleNotificationlist;
    if (this.toggleNotificationlist) this.getNotifications();
    if (!this.toggleNotificationlist) this.page = 0;
  }

  onScrollToEnd() {
    if (this.notificationCount > this.notifications.length) {
      this.page++;
      this.getNotifications(true);
    }
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
  getNotificationCount() {
    this.notificationService.getAllNotify().subscribe((res) => {
      this.notificationService.getNotificationCount().subscribe((res) => {
        this.count = res;
      })
    })
  }
  navigateTo(announcement: CommitteeNotificationDTO) {
    if (announcement.meetingId) {
      if (announcement.textAR.includes('تصويت على الدقائق')) {
        localStorage.setItem('selectedIndex', '3')
        this.router.navigateByUrl(`/meetings/schedule-meeting/${announcement.meetingId}`)
      } else {
        localStorage.removeItem('selectedIndex')
        this.router.navigateByUrl(`/meetings/schedule-meeting/${announcement.meetingId}`)
      }
    } else if (announcement.commiteeId) {
      const id = this.browserService.encrypteString(announcement.commiteeId)
      this.router.navigate(["/committees/", id]);
    }
    else if (announcement.commentId) {
      const id = this.browserService.encrypteString(announcement.commiteeTaskId)
      this.router.navigate(["/tasks/", id]);
    }
    else {
      this.taskService.fromNotifications.next(true)
      this.router.navigateByUrl(`/tasks`)
    }
    this.toggleNotificationlist = !this.toggleNotificationlist;
  }

  makeNotificationRead(notify) {
    // const notification = new CommitteeNotificationDTO({
    //       isRead:true,
    //       commentId:notify.commentId,
    //       commitee:notify.commitee,
    //       commiteeId:notify.commiteeId,
    //       commiteeSavedAttachmentId:notify.commiteeSavedAttachmentId,
    //       commiteeTaskId:notify.commiteeSavedAttachmentId,
    //       committeeNotificationId:notify.committeeNotificationId,
    //       createdBy:notify.createdBy,
    //       createdByRoleId:notify.createdByRoleId,
    //       createdByUser:notify.createdByUser,
    //       createdOn:notify.createdOn,
    //       meetingId:notify.meetingId,
    //       meetingTopicId:notify.meetingTopicId,
    //       meeting:notify.meeting,
    //       minuteOfMeetingId:notify.minuteOfMeetingId,
    //       surveyId:notify.surveyId,
    //       textAR:notify.textAR,
    //       textEn:notify.textEn,
    //       userId:notify.userId
    // })
    this.notificationService.changeNotificationStatus(this.browserService.encrypteString(notify.committeeNotificationId)).subscribe((res) => {
      if (res) {
        this.notifications.map((notify, index) => {
          if (notify.committeeNotificationId == res.committeeNotificationId) {
            this.notifications[index] = res
          }
        })
        this.getNotificationCount()
      }
    })
  }
}
