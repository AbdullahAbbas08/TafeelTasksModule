import { Component, OnInit, Inject,} from '@angular/core';
import {  API_BASE_URL, ChangePasswordViewModel, Lookup, SwaggerClient, User, UserDetailsDTO, UserRoleDTO, UserRolePermissionsDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
import { ActivatedRoute, Router } from "@angular/router";
import { NzNotificationService } from 'ng-zorro-antd/notification';
import {
  HttpClient,
  HttpEventType,
  HttpRequest,
  HttpHeaders,
} from "@angular/common/http";
import { AuthService } from 'src/app/auth/auth.service';
import { Subscription ,BehaviorSubject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { debounceTime, switchMap,finalize } from 'rxjs/operators';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { DateFormatterService, DateType } from 'ngx-hijri-gregorian-datepicker';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  providers: [DestroyService]
})
export class UserComponent implements OnInit {
  isLoading:boolean = false;
  routerSubscription: Subscription;
  user: UserDetailsDTO;
  currentLang: string;
  userCreate:boolean = false;
  tempImageData: string;
  tempFormData: FormData;
  baseUrl: string;
  headers: HttpHeaders;
  _jobTitles: Lookup[];
  _genders:Lookup[];
  _roles:Lookup[];
  fromOrganizationLookup:Lookup[] = []
  _nationalities:Lookup[];
  password: ChangePasswordViewModel = new ChangePasswordViewModel();
  passwordMatch: boolean = true;
  withoutPassword: boolean = false;
  isValidConfirmPassword:boolean = false;
  confirmPasswordError: string;
  isValidConfirmSignaturePassword:boolean = false;
  confirmSignaturePassword: string;
  confirmSignaturePasswordError: string;
  defualtOrganizationChanged$ = new BehaviorSubject('');
  lookupTypes = SettingControllers;
  isUserLoading:boolean = true;
  isCollapsed:boolean = false;
  selectedDateType = DateType.Gregorian;
  minHijri: NgbDateStruct;
  minGreg: NgbDateStruct;
  userRoles:UserRolePermissionsDTO;
  permissionToggle: any = {};
  OrganizationId:number;
  selectedToDate:Date;
  selectedFromDate: Date;
  RoleId:number;
  notes:string
  roleOverridesUserPermissions:boolean = false;
  selectedIndex:number = 0;
  userPermissions:UserRolePermissionsDTO;
  userId:any;
  constructor(
    private _auth:AuthService,
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute,
    private swagger:SwaggerClient,
    private browser:BrowserStorageService,
    private translateService: TranslateService,
    @Inject(API_BASE_URL) baseUrl: string,
    private notificationService: NzNotificationService,
    private dateFormatterService: DateFormatterService,
    private layoutService: LayoutService,
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.headers = this._auth.getheaders();
    this.setMinAllowed()
    this.langChange();
     this.getLookup("jobtitles").subscribe((value) => (this._jobTitles = value));
     this.getLookup("genders").subscribe((value) => (this._genders = value));
     this.getLookup("nationalities").subscribe((value) => (this._nationalities = value));
     this.getDefualtOrganization()

    this.routerSubscription = this.route.params.subscribe((res) => {
      if (!res.UserId) {
        this.userCreate = true;
        this.user = new UserDetailsDTO({
          enabled: true,
          isGeneral: false,
          isEmployee: true,
          isIndividual: false,
          isCorrespondent: false,
          isCorrespondentForAllOrganizations: false,
          notificationByMail: false,
          notificationBySMS: false,
          isAdmin: false,
          externalUser: false,
        });
      } else {
      this.swagger.apiUserGetByIdGet(res.UserId).pipe(finalize(()=>{this.isUserLoading = false;})).subscribe((user) => {
        if(user) {
          this.user = user;
          this.userId = res.UserId
          this.user.defaultOrganizationId =
          user.defaultOrganizationId === 0
            ? null
            : user.defaultOrganizationId;
          this.isValidConfirmPassword = true;
          this.isValidConfirmSignaturePassword = true;
        }
      })
      }
    })
  }
  readFile(event: Event, files): void {
    if (
      (<HTMLInputElement>event.target).files &&
      (<HTMLInputElement>event.target).files[0]
    ) {
      const file = (<HTMLInputElement>event.target).files[0];
      const reader = new FileReader();
      // @ts-ignore
      reader.onload = (e) => (this.tempImageData = reader.result);
      reader.readAsDataURL(file);
    }
    this.tempFormData = this.prepareFormData(files);
  }
  changeProfileImage(files) {
    this.upload(this.prepareFormData(files));
  }
  private prepareFormData(files): FormData {
    if (files.length === 0) {
      return;
    }
    const formData = new FormData();
    for (const file of files) {
      formData.append(file.name, file);
    }
    return formData;
  }
  upload(formData: FormData, first: boolean = false, userName: string = "") {
    if (!formData) {
      if (first === true && userName) {
        this.router.navigate([`/settings/users/edit/${userName}`]);
        // this.notifications.successInsert("User");
      }
      return;
    }
    const uploadReq = new HttpRequest(
      "POST",
      `${
        this.baseUrl
      }/api/CommiteeUsers/AddUserImage?userId=${this.user.userId}`,
      formData,
      { reportProgress: true, headers: this.headers }
    );
    this.http.request(uploadReq).subscribe((event) => {
      if (event.type === HttpEventType.Response) {
        this.user.profileImage = (<User>event.body).profileImage;
        this.user.profileImageMimeType = (<User>(
          event.body
        )).profileImageMimeType;
        // true only in insert mode
        if (first === true && userName) {
          this.router.navigate([`/settings/users/edit/${userName}`]);
          // this.notifications.successInsert("User");
        }
      }
    });
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
  PasswordFlag(e) {
    this.passwordMatch = e;
  }
  disablePassword(val){
    this.withoutPassword = val
  }
  checkConfirmPassword(type: number = 1) {
    switch (type) {
      case 1:
        this.isValidConfirmPassword =
          this.password.newPassword &&
          this.password.confirmPassword &&
          this.password.newPassword === this.password.confirmPassword
            ? true
            : false;
        this.confirmPasswordError = !this.isValidConfirmPassword
          ? "PasswordDoesNotMatch"
          : null;
        break;
      case 2:
        this.isValidConfirmSignaturePassword =
          this.user.signaturePassword &&
          this.confirmSignaturePassword &&
          this.user.signaturePassword === this.confirmSignaturePassword
            ? true
            : false;
        this.confirmSignaturePasswordError = !this
          .isValidConfirmSignaturePassword
          ? "SignaturePasswordDoesNotMatch"
          : null;
        break;
    }
  }
  getLookup(lookup, field?, operator?, value?) {
    return this.swagger.apiLookupGet(
      lookup,
      undefined,
      undefined,
      undefined,
      undefined,
      field,
      operator,
      value,
      undefined,
      undefined,
      undefined,
      undefined
    );
  }
  getDefualtOrganization(){
    this.isLoading = true
    let filters: any[] = [
      {
        field: "IsOuterOrganization",
        operator: "eq",
        value: "false",
        logic: "and",
      },
      {
        field: "IsCategory",
        operator: "eq",
        value: "false",
        logic: "and",
      },
      {
        field: "DeletedBy",
        operator: "eq",
        value: "null",
        logic: "and",
      },
      {
        field: "DeletedOn",
        operator: "eq",
        value: "null",
        logic: "and",
      },
    ];
    this.defualtOrganizationChanged$
    .asObservable()
    .pipe(
      debounceTime(500),
      switchMap((text: string) => {
        return  this.swagger
        .apiLookupGet(
          "organizations",
         text ? text : undefined,
          20,
          0,
          undefined,
          undefined,
          undefined,
          undefined,
          "or",
          filters,
          undefined,
          undefined
        )
      })
    )
    .subscribe((data) => {
      this.fromOrganizationLookup = data;
      this.isLoading = false;
      if(this.userId){
        if(!this.fromOrganizationLookup.some((x => x.id === this.user.defaultOrganizationId))){
          this.fromOrganizationLookup.push(new Lookup({id:this.user.defaultOrganizationId,text:this.user.defaultOrganizationNameAr}))
        }
      }
    });
  }
  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.SYSTEMORG:
        this.defualtOrganizationChanged$.next(value);
        break;
      default:
        break;
    }
  }

  submitForm(){
    this.userCreate ? this.insertUser() : this.saveUserData();
  }
  invalidForm() {
    this.translateService
    .get('FormIsInvalid')
    .subscribe((translateValue) =>
      this.notificationService.error(translateValue, '')
    );
  }
  insertUser(){
    if (this.withoutPassword){
      this.layoutService.toggleSpinner(true);
      this.swagger
      .apiUserInsertNewUsersPost([this.user])
      .subscribe((value) => {
        if (value && value[0]) {
          this.user.userId = value[0].userId;
          this.upload(this.tempFormData, true, value[0].userName);
          this.translateService
          .get('userAddeddSuccessfull')
          .subscribe((translateValue) =>
            this.notificationService.success(translateValue, '')
          );
          this.layoutService.toggleSpinner(false);
           this.router.navigate([`/settings/Users`]);
        }
      });
    } else {
      if (this.passwordMatch && this.isValidConfirmSignaturePassword && this.user.userName && this.password.newPassword === this.password.confirmPassword)
      {
        this.user.password = this.password.newPassword;
        this.layoutService.toggleSpinner(true);
        this.swagger
          .apiUserInsertNewUsersPost([this.user])
          .subscribe((value) => {
            if (value && value[0]) {
              this.user.userId = value[0].userId;
              this.upload(this.tempFormData, true, value[0].userName);
              this.translateService
              .get('userAddeddSuccessfull')
              .subscribe((translateValue) =>
                this.notificationService.success(translateValue, '')
              );
              this.layoutService.toggleSpinner(false);
              this.router.navigate([`/settings/Users`]);
            }
          });
      }
    }
  }
   saveUserData(){
    this.user.profileImage = '';
    if (this.passwordMatch) {
      this.layoutService.toggleSpinner(true);
      this.swagger.apiUserUpdateUsersPut([this.user]).subscribe((value) => {
        if (value && value[0]) {
          this.translateService
            .get('userUpdatedSuccessfull')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
            this.layoutService.toggleSpinner(false);
          this.router.navigate([`/settings/Users`]);
        }
      });
    }
   }
   dateFromSelected(selectedDate: NgbDateStruct) {
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.selectedFromDate = new Date(
        Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
      );
    }
  }
  dateSelected(selectedDate: NgbDateStruct){
    if (selectedDate) {
      if (selectedDate.year < 1900)
        selectedDate = this.dateFormatterService.ToGregorian(selectedDate);
      this.selectedToDate = new Date(
        Date.UTC(selectedDate.year, selectedDate.month - 1, selectedDate.day)
      );
    }
  }
  setMinAllowed() {
    this.minGreg = this.dateFormatterService.GetTodayGregorian();
    this.minHijri = this.dateFormatterService.GetTodayHijri();
  }
  getUserRoles(){
    this.swagger.apiUserGetUserRolePermissionsByUserNameSTGet(this.browser.encrypteString(this.user.userName),false).subscribe((res) => {
      if(res) {
        this.userRoles = res;
      }
    })
   
  }
  loadRoles(role:any){
    role["load"] = true;
    if (role.collapse == true) {
      role.collapse = false;
      role["load"] = false;
    } else if (role.collapse == false && role.permissionCategories != null) {
      role["load"] = false;
      role.collapse = true;
    } else {
      this.swagger["apiUserGetuserrolepermissionsbyValuesGet"](
        role.userId,
        role.userRoleId,
        role.organizationId,
        role.roleId,
        role.isEmployeeRole,
        false
      ).subscribe((res) => {
        if (res) {
          role.permissionCategories = res.userRoles[0].permissionCategories;
          role["load"] = false;
          role.collapse = true;
        }
      });
    }
  }
  getAllUserRoles(){
    this.getLookup("roles").subscribe((value) => (this._roles = value));
  }
  assignNewRole(){
    const userRole = new UserRoleDTO({
      userId:this.user.userId,
      organizationId:this.OrganizationId,
      roleId:this.RoleId,
      notes:this.notes,
      enabledSince:this.selectedFromDate,
      enabledUntil:this.selectedToDate,
      roleOverridesUserPermissions:this.roleOverridesUserPermissions
    })
    this.swagger.apiUserRoleInsertUserRoleFromUserPost([userRole]).subscribe((res) => {
      if(res === true){
        this.translateService
        .get('userRoleCreatedSuccefully')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
        this.selectedIndex = 0;
        this.getUserRoles();
      }else {
        this.translateService
        .get('cannotcreateuserrole')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
      }
    })

  }
  check(role: any, permission: any) {
    if (permission.cases.new_case == 3) {
      permission.cases.new_case = 4;
    } else if (permission.cases.new_case == 4) {
      permission.cases.new_case  = undefined;
    } else if (permission.cases.new_case == null) {
      permission.cases.new_case = 3;
    }
    this.swagger["apiUserEditUserRolePermissionByValuesPost"](
      role.userId,
      permission.permissionId,
      role.roleId,
      role.organizationId,
      permission.cases.new_case
    ).subscribe((res) => {
      if (res) {
        this.translateService
        .get('editedsuccessfully')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
      }
    });
  }
}
