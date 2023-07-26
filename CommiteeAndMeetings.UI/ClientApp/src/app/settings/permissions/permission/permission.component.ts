import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  CommitePermissionDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-permission',
  templateUrl: './permission.component.html',
  providers: [DestroyService],
})
export class PermissionComponent implements OnInit {
  permission: CommitePermissionDTO = new CommitePermissionDTO();
  permissionCreate = false;
  permissionId: any;

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
      if (!r.permissionId) {
        this.permissionCreate = true;
        this.permission = new CommitePermissionDTO();
      } else {
        this.permissionId = r.permissionId;
        this.getPermissionById();
      }
    });
  }

  getPermissionById() {
    this.swagger
      .apiCommiteePermissionsGetByIdGet(this.permissionId)
      .subscribe((permission) => (this.permission = permission));
  }

  editPermission() {
    if (
      !this.permission.commitePermissionNameEn ||
      !this.permission.commitePermissionNameAr ||
      !this.permission.permissionCode
    ) {
      return;
    }
    this.swagger
      .apiCommiteePermissionsUpdatePut([this.permission])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('PermissionUpdated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/permissions']);
        } else {
          this.translate
            .get('PermissionUpdatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }

  insertPermission() {
    if (
      !this.permission.commitePermissionNameEn ||
      !this.permission.commitePermissionNameAr ||
      !this.permission.permissionCode
    ) {
      return;
    }
    this.permission.enabled = true;
    this.swagger
      .apiCommiteePermissionsInsertPost([this.permission])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('PermissionCreated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/permissions']);
        } else {
          this.translate
            .get('PermissionCreatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }
}
