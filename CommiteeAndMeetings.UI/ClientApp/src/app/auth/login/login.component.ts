import { TranslateService } from '@ngx-translate/core';
import { LayoutService } from './../../shared/_services/layout.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm ,Validators} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserLoginModel } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { LocalizationService } from 'src/app/shared/_services/localization.service';
import { AuthService } from '../auth.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitAfterVerifyForm: FormGroup;
  errorMessages: string = '';
  returnUrl: string = '';
  currentLang: string;
  isLoading = false;
  subscription: Subscription;
  showVerivication:boolean = false;
  userLoggedName: string = '';
  constructor(
    private authService: AuthService,
    public localizationService: LocalizationService,
    private browserService: BrowserStorageService,
    private layoutService: LayoutService,
    private translateService: TranslateService,
    private modal: SharedModalService,
    private notificationService: NzNotificationService,
    private formBuilder: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.authService.clearStorage();
    this.initForms();
    this.subscription = this.authService
      .listenToAuthStatus()
      .subscribe((status) => (!status ? (this.isLoading = false) : null));
    this.langChange();
    const ssoKey = sessionStorage.getItem('ssoKey');
    if (!ssoKey) {
      this.onSubmit();
    }
   this.removeSSoCode()
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.layoutService.toggleSpinner(false);
  }

  initForms() {
    this.browserService.setLocal('hijriDaysValue', 0);
    this.loginForm = new FormGroup({
      username: new FormControl(),
      password: new FormControl(),
    });
    this.submitAfterVerifyForm = this.formBuilder.group({
      verCode: ['', [Validators.required]]
    });
  }

  onSubmit() {
    // if (!this.loginForm.valid) {
    //   return 
    // }
    this.isLoading = true;
    this.layoutService.toggleSpinner(true);
    if(this.loginForm.get('username').value && this.loginForm.get('password').value){
      var cridentials: UserLoginModel = {
    
        username: this.browserService.encrypteString(this.loginForm.get('username').value),
        password: this.browserService.encrypteString(this.loginForm.get('password').value),
        applicationType: '1',
        continue: true,
        disableSSO: true,
      } as UserLoginModel;
    } else {
      var cridentials: UserLoginModel = {
        username: '',
        password: '',
      } as UserLoginModel;
    }
    let culture = this.translateService.currentLang;
    this.authService.loginUser(culture, cridentials).subscribe((isLoggedIn) => {
        if(!isLoggedIn && cridentials.username === ''){
            this.showVerivication = false 
        }else if(!isLoggedIn && this.authService.isFactorAuth && cridentials.username !== ''){
            this.showVerivication = true;
            this.userLoggedName = this.loginForm.controls.username.value;
        }else if(!isLoggedIn && cridentials.username !== '' && !this.authService.isFactorAuth){
          this.showVerivication = false;
          this.modal.createMessage('error', 'LoginError');
      }else {
          this.authService.getUserAuthData();
        }
    });
  }
  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }
  removeSSoCode(){
    window.onunload = function () {
      sessionStorage.removeItem('ssoKey');
    }
  }
  submitAfterVerification(){
    this.authService.submitAfterVerification(
      this.submitAfterVerifyForm.controls.verCode ? this.submitAfterVerifyForm.controls.verCode.value : '',
      this.userLoggedName,
      '1',
      localStorage['culture']
    ).subscribe((res) => {
      if(!res){
        this.translateService
        .get('VerificatonCodeIsWrong')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
      } else {
        this.authService.getUserAuthData();
        this.userLoggedName = '';
      }
    })
  };
  resendVerificationCode(){
    this.authService.resendUserVerificationCode(this.userLoggedName).subscribe((res: any) => {
      if (res) {
        this.translateService
        .get('verificationcoderesend')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
      } else {
        this.translateService
        .get('failedtoresendcode')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
      }
    });
  };
  returnToLogin(){
    this.showVerivication = false;
    this.submitAfterVerifyForm.reset();
    this.userLoggedName = ''
  }
}
