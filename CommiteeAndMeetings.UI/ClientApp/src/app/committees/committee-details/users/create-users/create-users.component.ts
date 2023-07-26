import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { debounceTime, switchMap } from 'rxjs/operators';
import {
  CommiteeMemberDTO,
  CommiteeUsersRoleDTO,
  LookUpDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { UserService } from '../user.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BehaviorSubject } from 'rxjs';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
@Component({
  selector: 'app-create-users',
  templateUrl: './create-users.component.html',
  styleUrls: ['./create-users.component.scss'],
})
export class CreateUsersComponent implements OnInit {
  UserForm: FormGroup;
  take: number = 10;
  skip: number = 0;
  InternalUsers: LookUpDTO[] = [];
  ExternalUsers: LookUpDTO[] = [];
  UserRoles: LookUpDTO[] = [];
  isLoading = false;
  committeId: string;
  selectedUser;
  selectedUserState:boolean = true;
  externalUserChanged$ = new BehaviorSubject('');
  internalUserChanged$ = new BehaviorSubject('');
  lookupTypes = SettingControllers;
  isLoadingUser:boolean = false
  constructor(
    private modalService: SharedModalService,
    private formBuilder: FormBuilder,
    private _UserServices: UserService,
    private storeService: StoreService,
    private translate: TranslateService,
    private notificationService: NzNotificationService,
    private browserService: BrowserStorageService,
  ) {}

  ngOnInit(): void {
    this.getUserRole();
    this.initUserForm();
  }
  getExternalUsers() {
    this.externalUserChanged$.asObservable().pipe(
      debounceTime(500),
      switchMap((text:string) => 
      this._UserServices.getExternalUser(
        this.take,
        this.skip,
        text ? text : undefined,
        )
      )
    ).subscribe((res:LookUpDTO[]) => {
      this.ExternalUsers = res;
      this.isLoading = false
    })
  }
  getInternalUser() {
    this.internalUserChanged$.asObservable().pipe(
      debounceTime(500),
      switchMap((text:string) => 
      this._UserServices.getInternalUser(
        this.take,
        this.skip,
        text ? text : undefined,
      )
      )
    ).subscribe((res:LookUpDTO[]) => {
      this.InternalUsers = res;
      this.isLoading = false;
    })
  }
  onSearch(type: string, value: string): void{
    this.isLoading = true;
    switch (type) {
      case SettingControllers.ExternalUsers:
        this.externalUserChanged$.next(value);
        break;
      case SettingControllers.InternalUsers:
        this.internalUserChanged$.next(value);
        break;
      default:
        break;
    }
  }
  getUserRole() {
    this._UserServices
      .getUserRole(this.take, this.skip)
      .subscribe((res: LookUpDTO[]) => {
        if (res) {
          this.UserRoles = res;
          this.isLoading = false;
        }
      });
  }
  selectChange() {
    if (this.selectedUser == 'Internal' || this.selectedUser == 'داخلي') {
      this.getInternalUser();
    } else if (
      this.selectedUser == 'External' ||
      this.selectedUser == 'خارجي'
    ) {
      this.getExternalUsers();
    }
  }
  initUserForm() {
    this.UserForm = this.formBuilder.group({
      userId: ['', [Validators.required]],
      roleId: ['', [Validators.required]],
    });
  }
  close() {
    this.modalService.destroyModal();
  }
  createUser() {
    this.isLoadingUser = true
    if(this.selectedUserState === false){
      var data = new CommiteeMemberDTO({
        commiteeMemberId: 0,
        userId: this.UserForm.controls['userId'].value,
        user: null,
        commiteeIdEncrypt: this.browserService.encryptCommitteId(this.committeId),
        commiteeRoles: [
          new CommiteeUsersRoleDTO({
            roleId: this.UserForm.controls['roleId'].value,
            commiteeIdEncrypt:this.browserService.encryptCommitteId(this.committeId),
            userId:this.UserForm.controls['userId'].value,
            enabled:true
          }),
        ],
        active: true,
        isReserveMember:true,
        memberState:3
      });
    } else {
      var data = new CommiteeMemberDTO({
        commiteeMemberId: 0,
        userId: this.UserForm.controls['userId'].value,
        user: null,
        commiteeIdEncrypt: this.browserService.encryptCommitteId(this.committeId),
        isReserveMember:false,
        commiteeRoles: [
          new CommiteeUsersRoleDTO({
            roleId: this.UserForm.controls['roleId'].value,
            commiteeIdEncrypt:this.browserService.encryptCommitteId(this.committeId),
            userId:this.UserForm.controls['userId'].value,
            enabled:true
          }),
        ],
        active: true,
        memberState:1
      });
    }

    this._UserServices.createUsers([data]).subscribe((res) => {
      if (res && res.length) {
        // res[0].memberState = 1
        this.storeService.refreshUsers$.next(res[0]);
        this.isLoadingUser = false
        this.close();
      } else {
        this.translate
        .get('Thismemberalreadyexists')
        .subscribe((translateValue) =>
          this.notificationService.warning(translateValue, '')
        );
      }
    });
  }
}
