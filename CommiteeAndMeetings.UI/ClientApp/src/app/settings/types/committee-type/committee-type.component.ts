import { Component, OnInit ,Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {  HttpClient, HttpRequest,HttpEventType,HttpHeaders} from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { takeUntil } from 'rxjs/operators';
import { CommiteeTypeDTO, SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
import { API_BASE_URL } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { AuthService } from 'src/app/auth/auth.service';
@Component({
  selector: 'app-committee-type',
  templateUrl: './committee-type.component.html',
  providers: [DestroyService]
})
export class CommitteeTypeComponent implements OnInit {
  committeeType: CommiteeTypeDTO = new CommiteeTypeDTO();
  typeCreate = false;
  typeId: any;
  baseUrl: string;
  tempFormData: FormData;
  tempImageData: string;
  headers: HttpHeaders;
  constructor(
    @Inject(API_BASE_URL) baseUrl: string,
    private route: ActivatedRoute,
    private router: Router,
    private swagger: SwaggerClient,
    private translate: TranslateService,
    private notificationService: NzNotificationService,
    private destroyServ: DestroyService,
    private http: HttpClient,
    private _auth:AuthService,
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.route.params
    .pipe(takeUntil(this.destroyServ.subDestroyed))
    .subscribe((r) => {
      if (!r.committeeTypeId) {
        this.typeCreate = true;
        this.committeeType = new CommiteeTypeDTO();
      } else {
        this.typeId = r.committeeTypeId;
        this.getComitteeTypeById();
      }
    });
    this.headers = this._auth.getheaders();
  }

  getComitteeTypeById() {
    this.swagger
      .apiCommiteeTypesGetByIdGet(this.typeId)
      .subscribe((type) => {
        (this.committeeType = type);
      });
  }
  private prepareFormData(files): FormData {
    if (files.length === 0) { return; }
    const formData = new FormData();
    for (const file of files) { formData.append(file.name, file); }
    return formData;
  }
  readFile(event:Event,files):void{
     if(((<HTMLInputElement>event.target).files && (<HTMLInputElement>event.target).files[0])){
      const file = (<HTMLInputElement>event.target).files[0];
      const reader = new FileReader();
      // @ts-ignore
      reader.onload = e => this.tempImageData = reader.result;
      reader.readAsDataURL(file);
     }
     this.tempFormData = this.prepareFormData(files);
  }
  upload(formData: FormData){
  const uploadReq = new HttpRequest('POST',`${this.baseUrl}/api/CommiteeTypes/AddCommiteeTypeImage?id=${this.typeId}`,formData,{ reportProgress: true, headers: this.headers })
  this.http.request(uploadReq).subscribe((event) => {
    if(event.type ===HttpEventType.Response){
      // console.log(event)
    }
  }) 
}
  editCommitteeType() {
    if (
      !this.committeeType.commiteeTypeNameAr ||
      !this.committeeType.commiteeTypeNameEn
    ) {
      return;
    }
    this.swagger
      .apiCommiteeTypesUpdatePut([this.committeeType])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('CommitteeTypeUpdated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.upload(this.tempFormData)
          this.router.navigate(['/settings/types']);
        } else {
          this.translate
            .get('CommitteeTypeUpdatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }

  insertCommitteeType() {
    if (
      !this.committeeType.commiteeTypeNameEn ||
      !this.committeeType.commiteeTypeNameEn 
    ) {
      return;
    }
    this.swagger
      .apiCommiteeTypesInsertPost([this.committeeType])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('CommitteeTypeCreated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
            this.typeId = value[0].commiteeTypeId;
            this.upload(this.tempFormData)
          this.router.navigate(['/settings/types']);
        } else {
          this.translate
            .get('CommitteeTypeCreatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }
}
