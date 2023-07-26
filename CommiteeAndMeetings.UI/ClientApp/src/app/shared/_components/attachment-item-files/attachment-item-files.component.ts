
import { Component, Input, OnInit,  OnDestroy } from '@angular/core';
import { AttachmentsService } from 'src/app/committees/committee-details/attachments/attachments.service';
import { SavedAttachmentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { CommitteeService } from 'src/app/committees/committee.service';
import { ActivatedRoute } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-attachment-item-files',
  templateUrl: './attachment-item-files.component.html',
  styleUrls: ['./attachment-item-files.component.scss'],
})
export class AttachmentItemFilesComponent implements OnInit,OnDestroy {
  @Input() attachments: SavedAttachmentDTO[];
  imagesListList = [];
  targetIndex = 0;
  showGallery = [];
  showDownload = [];
  iconLogos = [];
  checkValue:boolean = false;
  subscription: Subscription;
  committeeId: any;
  userId: any;
  constructor(  private translateService: TranslateService,private notificationService: NzNotificationService, private route: ActivatedRoute, private authService:AuthService,private attachmentService: AttachmentsService, public committeeService: CommitteeService,) {}

  ngOnInit(): void {
    this.userId = this.authService.getUser().userId;
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
    this.selectFilesIcons();
    this.getImagesListsList();
    this.attachments.forEach(() => {
      this.showDownload.push(false);
      this.showGallery.push(false);
    });
    this.subscription =  this.attachmentService.addAttachmentWithComment.subscribe((value)=> {
      if(value){
       this.selectFilesIcons()
        this.getImagesListsList();
      }
    })
  }
  
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
  selectFilesIcons() {
    for (let i = 0; i < this.attachments.length; i++) {
      let pagesCount = this.attachments[i]?.pagesCount;
      let mime = this.attachments[i]?.mimeType;

      if (pagesCount > 0) {
        this.iconLogos.push(null);
      } else {
        if (mime?.includes('word')) {
          this.iconLogos.push('../../../../../assets/images/word-lg.png');
        } else if (mime?.includes('spreadsheet')) {
          this.iconLogos.push('../../../../../assets/images/excel-lg.png');
        } else {
          this.iconLogos.push('../../../../../assets/images/file-icon.png');
        }
      }
    }
  }

  getImagesListsList() {
  
    for (let i = 0; i < this.attachments.length; i++) {
      let pagesCount = this.attachments[i]?.pagesCount;
      if (!pagesCount || pagesCount < 0) this.imagesListList.push(null);
      else {
        let imageList = this.attachmentService.getImagesWithURL(
          this.attachments[i]
        );
        this.imagesListList.push(imageList);
      }
    }
  }

  openImageGallery(index1, index2?) {
     if (this.committeeId) {
      if(this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId) ||  !this.committeeService.committeStaus){
        this.targetIndex = index2;
        this.showGallery[index1] = true;
      }else  if (
         this.authService.isAuthUserHasPermissions(['ForAllCommitte']) &&
         this.authService.isAuthUserHasPermissions(['SecretCommitteeAttachments']) &&
         this.committeeService.committeStaus
       ) {
        this.targetIndex = index2;
        this.showGallery[index1] = true;
       } else {
        this.translateService
        .get('donotHavePermissionToShowThisAttachment')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
       }
     } else {
       this.targetIndex = index2;
       this.showGallery[index1] = true;
     }

  }

  openDownloadFile(index) {
    this.showDownload[index] = true;
  }
}
