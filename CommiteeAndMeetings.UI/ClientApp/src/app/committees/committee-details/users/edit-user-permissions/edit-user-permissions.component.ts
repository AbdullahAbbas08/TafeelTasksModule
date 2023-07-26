import { Component, OnInit } from '@angular/core';
import { CommiteeRoleDTO, CommiteeRolePermissionDTO, CommiteeUserPermissionDTO, CommiteeUsersRoleDTO, CommitePermissionDTO, LookUpDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { SharedModalService } from 'src/app/core/_services/modal.service';
@Component({
  selector: 'app-edit-user-permissions',
  templateUrl: './edit-user-permissions.component.html',
  styleUrls: ['./edit-user-permissions.component.scss']
})
export class EditUserPermissionsComponent implements OnInit {
  committeId:any;
  roleId:any;
  userId:number;
  isDelagetd:boolean;
  permissions: LookUpDTO[] = [];
  role: CommiteeRoleDTO = new CommiteeRoleDTO();
  userPermissionRole:CommiteeUsersRoleDTO;
  updatedPermissions:CommiteeUserPermissionDTO[] = [];
  constructor(public modalService: SharedModalService,private notificationService: NzNotificationService,private swagger:SwaggerClient,private browserService:BrowserStorageService,private translate: TranslateService,) { }

  ngOnInit(): void {
    this.getRoleByRoleId();
    this.updatedPermissions = []
  }
  getRoleByRoleId(){
   const decryptCommitteId = this.browserService.encryptCommitteId(this.committeId);
   this.swagger.apiCommiteesGetCommitteeRolesGet(decryptCommitteId,this.userId).subscribe((res) => {
      this.userPermissionRole = res[0];
      this.getPermissions()
  })
  }
  getPermissions() {
    this.swagger
      .apiCommitePermissionsGetPermissionLookUpsGet()
      .subscribe((result) => {
        this.permissions = [...result];
        if (result) {
          this.userPermissionRole.role.rolePermissions = this.userPermissionRole.role.rolePermissions && this.userPermissionRole.role.rolePermissions.length ? this.userPermissionRole.role.rolePermissions: []
          result.map(r=>{
            if (!this.userPermissionRole.role.rolePermissions.find(rr=>r.id == rr.permission.commitePermissionId) )
            this.userPermissionRole.role.rolePermissions.push(new CommiteeRolePermissionDTO({permission:new CommitePermissionDTO({commitePermissionNameAr:r.name,commitePermissionId:r.id,isDeleted:r.isDeleted})}));
          })
        }
      });
  }
  pushChanges(event,index,permissioId){
   let permissionFoundBefore = this.updatedPermissions.find((res) => res.permissionId === this.userPermissionRole.role.rolePermissions[index].permission.commitePermissionId);
   if(permissionFoundBefore){
    const indexNumber = this.updatedPermissions.findIndex(x => x.permissionId === permissioId)
    this.updatedPermissions.splice(indexNumber,1);
   } else {
    this.updatedPermissions.push(new CommiteeUserPermissionDTO({
      permissionId:this.userPermissionRole.role.rolePermissions[index].permission.commitePermissionId,
      roleId:this.userPermissionRole.roleId,
      enabled:event,
      commiteeId:+this.browserService.decrypteString(this.committeId),
      isDelegated:this.userPermissionRole.delegated,
      userId:this.userId,
    }))
   }
  }
  printRole(){
   this.swagger.apiCommiteeUserPermissionsUpdateCustomePut(this.updatedPermissions).subscribe((res) => {
      if(res){
        this.modalService.destroyModal()
        this.translate
        .get('RoleUpdated')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
      } else {
        this.translate
        .get('RoleUpdatedError')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
      }
   })
  }
  close(){
    this.modalService.destroyModal()
  }
}
