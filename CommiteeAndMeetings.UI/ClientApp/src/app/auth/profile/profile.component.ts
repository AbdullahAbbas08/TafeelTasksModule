import { Component, OnInit ,Inject } from '@angular/core';
import { User, UserProfileDetailsDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '../auth.service';
import { HttpRequest,HttpEventType ,HttpClient,HttpHeaders} from '@angular/common/http';
import { API_BASE_URL } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  public isCollapsed = true;
  passForm: FormGroup;
  user: UserProfileDetailsDTO;
  tempFormData: FormData;
  tempImageData: string;
  baseUrl: string;
  headers: HttpHeaders;
  constructor(
    @Inject(API_BASE_URL) baseUrl: string,
    private _authService:AuthService,
    public translateService: TranslateService,
    private http:HttpClient,
    private notificationService: NzNotificationService,
    private formBuilder: FormBuilder,
    private message: NzMessageService,
    ) {
      this.baseUrl = baseUrl;
     }

  ngOnInit(): void {
    this.userProfileDetials();
    this.initPasswordForm();
    this.headers = this._authService.getheaders(); 
  }
  private prepareFormData(files):FormData {
     if(files.length === 0){ return;}
     const formData = new FormData();
     for (const file of files) {formData.append(file.name,file);}
     return formData
  }
  changeProfileImage(files) {
    this.upload(this.prepareFormData(files));
  }
  upload(formData: FormData){
    const uploadReq = new HttpRequest('POST',`${this.baseUrl}/api/CommiteeUsers/AddUserImage?userId=${this.user.userId}`,formData,{ reportProgress: true, headers: this.headers })
    this.http.request(uploadReq).subscribe((event) => {
      if(event.type === HttpEventType.Response){
        this.translateService.get('ProfileUpdated').subscribe(translateValue=> this.notificationService.success(translateValue, ''))
        this.user.profileImage = (<User>event.body).profileImage;
      }
    })
  }
  initPasswordForm(){
    this.passForm = this.formBuilder.group({
      OldPassword : ['' , [Validators.required]],
      NewPassword:['' , [Validators.required,Validators.minLength(4)]],
      ConfirmPassword: ['',[Validators.required]]
    },
    {validator: this.checkIfMatchingPasswords('NewPassword', 'ConfirmPassword')});
  }

  checkIfMatchingPasswords(passwordKey: string, passwordConfirmationKey: string) {
    return (group: FormGroup) => {
      let passwordInput = group.controls[passwordKey],
          passwordConfirmationInput = group.controls[passwordConfirmationKey];
      if (passwordInput.value !== passwordConfirmationInput.value) {
        return passwordConfirmationInput.setErrors({notSame: true})
      }
    }
  }
  changePassword(){
    this._authService.changeUserPassword(this.passForm.value).subscribe((res) => {
      this.translateService.get('PasswordchangedSuccessfully').subscribe(translateValue=> this.notificationService.success(translateValue, ''))
    })
  }
  userProfileDetials(){
    this._authService.getUserProfileDetails().subscribe((user) => {
      this.user = user;
    })
  }
}
