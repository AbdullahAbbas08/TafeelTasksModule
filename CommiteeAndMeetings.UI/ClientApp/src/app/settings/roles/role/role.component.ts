import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { takeUntil } from 'rxjs/operators';
import {
  CommiteeRoleDTO,
  CommiteeRolePermissionDTO,
  CommitePermissionDTO,
  Lookup,
  LookUpDTO,
  RoleDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DestroyService } from 'src/app/shared/_services/destroy.service';

@Component({
  selector: 'app-role',
  styleUrls: ['./role.component.scss'],
  templateUrl: './role.component.html',
  providers: [DestroyService],
})
export class RoleComponent implements OnInit {
  role: CommiteeRoleDTO = new CommiteeRoleDTO();
  roleId: string = '';
  roleCreate = false;
  isArabic: boolean = false;
  updatedRolePermissions: any[] = [];
  empRoleMessage: string = '';
  permissions: LookUpDTO[] = [];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private swagger: SwaggerClient,
    private translate: TranslateService,
    private destroyServ: DestroyService,
    private notificationService: NzNotificationService
  ) {
    this.isArabic = this.translate.currentLang === 'ar';
  }

  ngOnInit() {
    this.route.params
      .pipe(takeUntil(this.destroyServ.subDestroyed))
      .subscribe((r) => {
        if (!r.committeeRoleId) {
          this.roleCreate = true;
          this.role = new CommiteeRoleDTO();
        } else {
          this.roleId = r.committeeRoleId;
          this.getRoleData();
        }
      });
      if (!this.roleId){
        this.getPermissions();
      }
  }

  getRoleData() {
    this.swagger.apiCommiteeRoleGetByIdGet(this.roleId).subscribe((role) => {
      this.role = role;
      this.getPermissions();
      this.empRoleMessage = this.isArabic
        ? 'الدور الوظيفى: (' +
          this.role.commiteeRolesNameAr +
          ')' +
          'دور وظيفى خاص بالموظف '
        : 'Role: (' +
          this.role.commiteeRolesNameEn +
          ')' +
          ' Is An Emplyee Role';
    });
  }

  editRole() {
    if (!this.role.commiteeRolesNameEn || !this.role.commiteeRolesNameAr) {
      return;
    }
    let enabled:CommiteeRolePermissionDTO[] = [...this.role.rolePermissions.filter(rp=>rp.permission.enabled)];
    this.role.rolePermissions = []
    if (enabled && enabled.length){
      enabled.map(e=> this.role.rolePermissions.push(new CommiteeRolePermissionDTO({permissionId: e.permission.commitePermissionId,roleId:+this.roleId})))
    }
    this.swagger.apiCommiteeRoleUpdatePut([this.role]).subscribe((value) => {
      if (value) {
        this.translate
          .get('RoleUpdated')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
        this.router.navigate(['/settings/roles']);
      } else {
        this.translate
          .get('RoleUpdatedError')
          .subscribe((translateValue) =>
            this.notificationService.error(translateValue, '')
          );
      }
    });
  }

  insertRole() {
    if (!this.role.commiteeRolesNameAr || !this.role.commiteeRolesNameEn) {
      return;
    }
    let enabled:CommiteeRolePermissionDTO[] = [...this.role.rolePermissions.filter(rp=>rp.permission.enabled)];
    this.role.rolePermissions = []
    if (enabled && enabled.length){
      enabled.map(e=> this.role.rolePermissions.push(new CommiteeRolePermissionDTO({permissionId: e.permission.commitePermissionId})))
    }
    this.swagger.apiCommiteeRoleInsertPost([this.role]).subscribe((value) => {
      if (value) {
        this.translate
          .get('RoleCreated')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
        this.router.navigate(['/settings/roles']);
      } else  {
        this.translate
          .get('RoleCreatedError')
          .subscribe((translateValue) =>
            this.notificationService.error(translateValue, '')
          );
          this.getPermissions()
      }
    });
  }

  onChangePermission(permission, index) {
    const permissionFoundBefore =
      this.role.rolePermissions[index].permission.enabled;
    if (permissionFoundBefore) {
      this.role.rolePermissions[index].permission.enabled = false;
      return;
    }
    this.role.rolePermissions[index].permission.enabled = true;
  }
  getPermissions() {
    this.swagger
      .apiCommitePermissionsGetPermissionLookUpsGet()
      .subscribe((result) => {
        this.permissions = [...result];
        if (result) {
          this.role.rolePermissions = this.role.rolePermissions && this.role.rolePermissions.length ? this.role.rolePermissions: []
          result.map(r=>{
            if (!this.role.rolePermissions.find(rr=>r.id == rr.permission.commitePermissionId) )
            this.role.rolePermissions.push(new CommiteeRolePermissionDTO({permission:new CommitePermissionDTO({commitePermissionNameAr:r.name,commitePermissionId:r.id,isDeleted:r.isDeleted})}));
          })
          if (!this.roleId){
            this.role.rolePermissions.map(rp=>{
              rp.permission.enabled = false;
            })
          }
        }
      });
  }
}
