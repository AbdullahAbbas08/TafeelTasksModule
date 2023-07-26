import { Component, OnInit, OnDestroy } from '@angular/core';
import { ExtentedCommiteeMemberDTO, UserService } from '../user.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { CommiteeUsersRoleDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { CommitteeService } from 'src/app/committees/committee.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { AuthService } from 'src/app/auth/auth.service';

class CommiteeUsersRoles extends CommiteeUsersRoleDTO {
  delegateName: string;
}
@Component({
  selector: 'app-external-users',
  templateUrl: './external-users.component.html',
  styleUrls: ['./external-users.component.scss'],
})
export class ExternalUsersComponent implements OnInit, OnDestroy {
  AllUsers: ExtentedCommiteeMemberDTO[] = [];
  externalUsers:ExtentedCommiteeMemberDTO[]=[]
  internalUsers:ExtentedCommiteeMemberDTO[]=[];
  skip: number = 0;
  take: number = 10;
  filters: any[] = [];
  loadingData: boolean = false;
  subscription: Subscription;
  currentCount = 0;
  count = 0;
  isCollapsed = false;
  isCollapsed2 = true;
  delagateToggle: any = {};
  userId: number;
  committeeId: string;
  filter_Logic: string = 'or';
  searchText: string;
  permittedToDisableMember = false;
  permittedToDelegateMember = false;
  permittedToEditCommittePermissions = false;
  permittedToChangeUserStatus = false;
  activeStats: any[] = [
    { name: 'Active', value: true },
    { name: 'Not Active', value: false },
    { name: 'Delegate', value: '' },
  ];

  constructor(
    private UserService: UserService,
    private storeService: StoreService,
    private layoutService: LayoutService,
    private searchService: SearchService,
    public translateService: TranslateService,
    private route: ActivatedRoute,
    private modalService: SharedModalService,
    private translate: TranslateService,
    private notificationService: NzNotificationService,
    private committeeService: CommitteeService,
    private BrowserService:BrowserStorageService,
    private authService:AuthService
  ) {
  }

  ngOnInit(): void {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
    this.userId = this.authService.getUser().userId;
    this.getAllUsers();
    this.storeService.refreshUsers$.subscribe((val) => {
      if (val) {
        this.AllUsers.push(val);
        console.log(this.AllUsers)
        if(val.user.externalUser === true){
          this.externalUsers.length += 1
        } else {
          this.internalUsers.length += 1
        }
      }
    });
    this.UserService.userDelegate.subscribe((val) => {
      if (val) {
        this.AllUsers.map((x, index) => {
          if (x.userId == val.userId) {
            this.AllUsers[index].commiteeRoles.push(val);
          }
        });
      }
    });
    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
      this.getAllUsers(false, word);
    });
    this.checkIfUserExistInCommitte();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
  delegateUser(commiteeId:any, userId: number, commiteeMemberId: number) {
    this.modalService.openDrawerModal(
      CommitteeActions.DelegateUser,
      commiteeId,
      userId,
      commiteeMemberId
    );
  }
  onItemClick(value: boolean,memberState:number ,userId: number) {
    const userIds = [userId];
    this.UserService.ActiveDisactiveUser(value, memberState,`${userIds}`).subscribe(
      (result) => {
        if (result) {
          this.AllUsers.map((user) => {
            if (user.commiteeMemberId == +result) {
              this.UserService.userMemberId.next(user.commiteeMemberId);
              if(user.memberState === 1 || user.memberState === 3 || user.memberState === 4){
                user.memberState = 2
              } else if(user.memberState === 2){
                user.memberState = 3
              }
            }
          });
        }
      }
    );
  }
  onScroll() {
    if (this.currentCount < this.count) {
      this.skip += this.take;
      this.getAllUsers(true);
    }
  }
  getAllUsers(scroll: boolean = false, searchWord: string = undefined) {
    if (!scroll) {
      this.loadingData = true;
      this.layoutService.toggleSpinner(true);
      this.skip = 0;
    }
    if (searchWord) {
      this.searchText = searchWord;
    } else {
      this.searchText = undefined;
    }
    this.UserService.getCommitteUsers(
      this.take,
      this.skip,
      this.committeeId,
      this.filters,
      this.filter_Logic,
      this.searchText
    ).subscribe((result) => {
      if (result && result.data) {
        this.AllUsers = scroll
          ? [...this.AllUsers, ...result.data]
          : result.data;
        this.count = result.count;
        this.externalUsers = result.data.filter((user) => {
          return user.user.externalUser
        })
        this.internalUsers = result.data.filter((user) => {
          return !user.user.externalUser
        })
      }
      if (scroll) {
        this.currentCount += result.data.length;
      } else {
        this.currentCount = result.data.length;
      }
      this.layoutService.toggleSpinner(false);
      this.loadingData = false;
    });
  }
  disableDelegate(roleId: number) {
    this.UserService.disableDelegateUser(roleId).subscribe((result) => {
      if (result) {
        this.getAllUsers();
        this.translate
          .get('DelegationDisabled')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
      }
    });
  }

  checkPermissions() {
    this.permittedToDisableMember = this.committeeService.checkPermission(
      'DISABLEMEMEBER'
    );
    this.permittedToDelegateMember = this.committeeService.checkPermission(
      'DELEGATEMEMEBER'
    );
    this.permittedToEditCommittePermissions =  
    this.committeeService.checkPermission('EditCommitteRolePermissions');
    this.permittedToChangeUserStatus = 
     this.committeeService.checkPermission('changeUserStatus')
  }
  checkIfUserExistInCommitte(){
    if(!this.committeeService.committeMembers.some((el) => el.userId === +this.userId)){
     if(this.authService.isAuthUserHasPermissions(['ForAllCommitte'])){
      this.permittedToDisableMember = true;
      this.permittedToDelegateMember = true;
      this.permittedToEditCommittePermissions = true;
      this.permittedToChangeUserStatus = true 
     }
    } else {
      this.checkPermissions()
    }
 }
  EditCommitteUserPermissions(roleId:number,isDelagted:boolean,userId:number) {
    this.modalService.openDrawerModal(
      CommitteeActions.EditCommitteUserPermissions,
      this.committeeId,
      userId,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,roleId,isDelagted
    );
  }
}
