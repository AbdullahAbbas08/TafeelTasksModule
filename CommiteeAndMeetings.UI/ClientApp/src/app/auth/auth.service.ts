import { SharedModalService } from 'src/app/core/_services/modal.service';
import { LayoutService } from './../shared/_services/layout.service';
import { Injectable } from '@angular/core';
// import { SwaggerClient, LoginViewModel, LoginCridentialResponse, RegistrationViewModel, RegisterationDTOObjectSourceResult } from '../core/_services/swagger/SwaggerClient.service';
import { BehaviorSubject, Observable, Subject,throwError } from 'rxjs';
import { BrowserStorageService } from '../shared/_services/browser-storage.service';
import {
  AccessToken,
  AuthTicketDTO,
  ChangePasswordViewModel,
  EncriptedAuthTicketDTO,
  SwaggerClient,
  UserLoginModel,
  UserProfileDetailsDTO,
} from '../core/_services/swagger/SwaggerClient.service';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { NzModalService } from 'ng-zorro-antd/modal';
import { StoreService } from '../shared/_services/store.service';
import { NavigationEnd, Router } from '@angular/router';
import {catchError, finalize, map,takeLast } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public user$ = new BehaviorSubject<AuthTicketDTO>(null);
  private token: string;
  private user;
  currentUrl: string;
  private authStatus: boolean = false;
  authStatusListener = new BehaviorSubject<boolean>(null);
  private authTimer;
  checkTaskModule:boolean = false;
  isFactorAuth:boolean;
  constructor(
    private router: Router,
    private swaggerService: SwaggerClient,
    private browserStorageService: BrowserStorageService,
    private layoutService: LayoutService,
    private modal: SharedModalService,
    private model: NzModalService,
    private store:StoreService
  ) {}

  getToken() {
    return this.token;
  }

  getUser() {
    return this.user;
  }

  listenToAuthStatus() {
    return this.authStatusListener.asObservable();
  }

  currentUserSub() {
    return this.user$.asObservable();
  }

  getAuthStatus() {
    return this.authStatus;
  }
  submitAfterVerification(factorAuthCode: string, userName: string, applicationType: string, culture: string) {
    return this.swaggerService.apiAccountCheckVerfCodePost(factorAuthCode,this.browserStorageService.encrypteString(userName), applicationType, culture)
      .pipe(
        map((response: AccessToken) => {
          if (response.access_token && response.is_factor_auth) {
            this.token = response.access_token
            return true;
          } else if (!response.access_token && response.is_factor_auth) {
            return false;
          }
        }),
        )
  }
  loginUser(culture: string, body: UserLoginModel) {
  return  this.swaggerService.apiAccountLoginPost(culture, body).pipe(map((res: AccessToken) => {
      this.token = res.access_token;
      if (!this.token) {
        this.authStatus = false;
        this.authStatusListener.next(false);
        this.user = undefined;
        this.user$.next(undefined);
        this.layoutService.toggleSpinner(false);
        this.isFactorAuth = res.is_factor_auth;
        return false ;
      } else{
        return true
      }
    },
    (e) => this.logoutUser())
   
    );
  }
  getUserAuthData() {
    this.swaggerService
    .apiAccountGetUserAuthTicketGet(undefined, undefined, false)
    .subscribe(
      (value: EncriptedAuthTicketDTO) => {
        if (value) {
          const decryptedUser =
            this.browserStorageService.decryptUser(value);
          this.user$.next(decryptedUser);
          this.user = decryptedUser;

          this.authStatus = true;
          this.authStatusListener.next(true);

          const authDuration =
            +decryptedUser.accessTokenExpirationMinutes * 60;

          const expirationDate = new Date(
            new Date().getTime() + authDuration * 1000
          );
          this.setAuthTimer(authDuration);
          this.saveAuthData(this.token, value, expirationDate);
           this.checkCurrentModule();
          this.swaggerService.apiCommitteeMeetingSystemSettingGetByCodeGet("ShowHideCommiteeMeetingModule").subscribe((res) => {
            if(res.systemSettingValue == '0'){
              this.router.navigate(['/tasks']);
            } else {
              this.swaggerService.apiCommitteeMeetingSystemSettingGetByCodeGet("DefaultOpenPage").subscribe((value) => {
                if(value.systemSettingValue == '1'){
                  this.router.navigate(['/']);
                } else if (value.systemSettingValue == '2'){
                  this.router.navigate(['/meetings']);
                } else if(value.systemSettingValue == '3'){
                  this.router.navigate(['/tasks']);
                }else {
                  this.router.navigate(['/']);
                }
              })
            }
          })
        }
      },
      (e) => {
        this.authStatus = false;
        this.authStatusListener.next(false);
        this.user = undefined;
        this.user$.next(undefined);
      }
    );
  }
  resendUserVerificationCode(userName) {
    return this.swaggerService.apiAccountResendVerfCodePost(this.browserStorageService.encrypteString(userName)).pipe(map((res: any) => {
      if (res === "False") {
        return false;
      }
      return true;
    }));
  }
  saveAuthData(token, user, expirationDate) {
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('expirationDate', expirationDate.toISOString());
  }

  getAuthData() {
    const token = localStorage.getItem('token');
    const user = JSON.parse(localStorage.getItem('user'));
    const expirationDate = localStorage.getItem('expirationDate');

    if (!token || !expirationDate) return;

    return {
      token: token,
      user: this.browserStorageService.decryptUser(user),
      expirationDate: new Date(expirationDate),
    };
  }

  clearAuthData() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('expirationDate');
    localStorage.removeItem('defaultModule')
  }

  autoAuthUser() {
    const authData = this.getAuthData();

    if (!authData) {
      return;
    }

    const now = new Date();
    const expiresIn = authData.expirationDate.getTime() - now.getTime();

    if (expiresIn > 0) {
      this.token = authData.token;
      const user = this.browserStorageService.decryptUser();
      this.user = user;
      this.authStatus = true;
      this.authStatusListener.next(true);
      this.user$.next(user);
      this.setAuthTimer(expiresIn / 1000);
    }
  }

  clearStorage() {
    this.token = undefined;
    this.user = undefined;
    this.authStatus = false;
    this.authStatusListener.next(false);
    this.user$.next(undefined);
    clearTimeout(this.authTimer);
    this.clearAuthData();
    this.model.closeAll();
    this.router.navigate(['/auth/login']);
  }
  logoutUser(){
    this.swaggerService.apiAccountLogoutGet(this.token).pipe(map(response => response || {}),
    catchError((error: HttpErrorResponse) => throwError(error)),
    finalize(async() =>{
      this.token = undefined;
      this.user = undefined;
      this.authStatus = false;
      this.authStatusListener.next(false);
      this.user$.next(undefined);
      clearTimeout(this.authTimer);
      this.clearAuthData();
      this.model.closeAll();
      this.router.navigate(['/auth/login']);
    }) 
    ).subscribe();
  }
  setAuthTimer(authDuration) {
    this.authTimer = setTimeout(() => {
      this.logoutUser();
    }, authDuration * 1000);
  }

  isAuthUserHasPermissions(requiredPermissions: string[]): boolean {
    const user = this.user$.value;
    if (!user || !user.permissions) {
      return false;
    } else {
      return requiredPermissions.some((requiredPermissions) => {
        if (user.permissions) {
          return (
            user.permissions.indexOf(requiredPermissions.toUpperCase()) >= 0
          );
        } else {
          return false;
        }
      });
    }
  }

  getUserProfileDetails(): Observable<UserProfileDetailsDTO> {
    return this.swaggerService.apiCommiteeUsersGetUserProfileGet();
  }

  changeUserPassword(body: ChangePasswordViewModel): Observable<void> {
    return this.swaggerService.apiCommiteeUsersChangeUserPasswordPost(body);
  }

  getheaders(): HttpHeaders {
    let accessToken = this.token;
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('Cache-Control', 'no-cache');
    headers = headers.append('Pragma', 'no-cache');
    headers = headers.append('Authorization', `Bearer ${accessToken}`);
    return headers;
  }
  checkCurrentModule(){
    this.swaggerService.apiCommitteeMeetingSystemSettingGetByCodeGet("ShowHideCommiteeMeetingModule").subscribe((res) => {
      if(res.systemSettingValue == '0'){
        this.checkTaskModule = true;
        localStorage.setItem('defaultModule', res.systemSettingValue);
        this.router.navigate(['/tasks']);
        this.router.events.subscribe((env) =>{
          if(env instanceof NavigationEnd){
            let currentModule = localStorage.getItem('defaultModule');
            if(currentModule == '0'){
              if((env.url.includes('/committees') || env.url.includes('/meetings') || env.urlAfterRedirects.includes('/committees') || env.urlAfterRedirects.includes('/meetings'))){
                this.router.navigate(['/tasks']);
               }
            }
          }
        })
      } else {
        this.checkTaskModule = false;
        
        this.swaggerService.apiCommitteeMeetingSystemSettingGetByCodeGet("DefaultOpenPage").subscribe((value) => {
          if(value.systemSettingValue == '1'){
            this.router.navigate(['/']);
          } else if (value.systemSettingValue == '2'){
            this.router.navigate(['/meetings']);
          } else if(value.systemSettingValue == '3'){
            this.router.navigate(['/tasks']);
          }else {
            this.router.navigate(['/']);
          }
        })
      }
    })
  }
  checkModule(){
    return this.checkTaskModule
  }
}
