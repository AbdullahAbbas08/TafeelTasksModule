import { CommitteeNotificationDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  constructor(
    private swagger: SwaggerClient,
    private authService: AuthService
  ) {}

  getNotifications(pageSize: number, page: number) {
    let userId = this.authService.getUser().userId;
    return this.swagger.apiCommitteeNotificationsGetNotificationListGet(
      pageSize,
      page
    );
  }
  getAllNotify(): Observable<void>{
    return this.swagger.apiCommitteeNotificationsInsertNotificationPut()
  }
  getNotificationCount() {
    let userId = this.authService.getUser()?.userId;
    return this.swagger.apiCommitteeNotificationsGetNotificationCountGet(
    );
  }
  changeNotificationStatus(notify:string): Observable<CommitteeNotificationDTO>{
    return this.swagger.apiCommitteeNotificationsGetNotificationReadPut(notify)
  }
}
