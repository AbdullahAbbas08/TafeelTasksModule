import { CurrentStatusDTO, SwaggerClient } from './../../../core/_services/swagger/SwaggerClient.service';
import { Component, OnInit } from '@angular/core';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
  providers: [DestroyService]
})
export class StatusComponent implements OnInit {
  committeeStatus: CurrentStatusDTO = new CurrentStatusDTO();
  statusCreate = false;
  statusId: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private swagger: SwaggerClient,
    private translate: TranslateService,
    private notificationService: NzNotificationService,
    private destroyServ: DestroyService
  ) {}

  ngOnInit() {
    this.route.params
    .pipe(takeUntil(this.destroyServ.subDestroyed))
    .subscribe((r) => {
      if (!r.committeeStatusId) {
        this.statusCreate = true;
        this.committeeStatus = new CurrentStatusDTO();
      } else {
        this.statusId = r.committeeStatusId;
        this.getComitteeStatusById();
      }
    });
  }

  getComitteeStatusById() {
    this.swagger
      .apiCurrentStatusGetByIdGet(this.statusId)
      .subscribe((status) => (this.committeeStatus = status));
  }

  editCommitteeStatus() {
    if (
      !this.committeeStatus.currentStatusNameAr ||
      !this.committeeStatus.currentStatusNameEn
    ) {
      return;
    }
    this.swagger
      .apiCurrentStatusUpdatePut([this.committeeStatus])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('CommitteeStatusUpdated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/statuses']);
        } else {
          this.translate
            .get('CommitteeStatusUpdatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }

  insertCommitteeStatus() {
    if (
      !this.committeeStatus.currentStatusNameEn ||
      !this.committeeStatus.currentStatusNameAr 
    ) {
      return;
    }
    this.swagger
      .apiCurrentStatusInsertPost([this.committeeStatus])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('CommitteeStatusCreated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/statuses']);
        } else {
          this.translate
            .get('CommitteeStatusCreatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }

}
