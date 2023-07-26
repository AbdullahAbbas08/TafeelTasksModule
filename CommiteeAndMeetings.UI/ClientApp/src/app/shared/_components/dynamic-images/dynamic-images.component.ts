
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AttachmentsService } from 'src/app/committees/committee-details/attachments/attachments.service';
import { SavedAttachmentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { LayoutService } from '../../_services/layout.service';


@Component({
  selector: 'app-dynamic-images',
  templateUrl: './dynamic-images.component.html',
  styleUrls: ['./dynamic-images.component.scss'],
})
export class DynamicImagesComponent implements OnInit {
  @Input() images: object[];
  @Output() close = new EventEmitter<void>();
  @Input() targetIndex;
  @Input() file: SavedAttachmentDTO;
  currentImage;
  currentIndex;
  isLoading = false;
  language;
  constructor(
    private layoutService: LayoutService,
    private translate: TranslateService,
    private attachmentsService: AttachmentsService
  ) {}

  ngOnInit(): void {
    this.language = this.translate.currentLang;
    this.currentIndex = this.targetIndex;
    this.setImageSrc();
    this.imageIsloading();
  }

  onClose() {
    this.close.emit();
    this.layoutService.toggleSpinner(false);
  }

  onRight() {
    if (this.language === 'ar') {
      this.currentIndex--;
    } else {
      this.currentIndex++;
    }

    this.setImageSrc();
    this.imageIsloading();
  }

  onLeft() {
    if (this.language === 'ar') {
      this.currentIndex++;
    } else {
      this.currentIndex--;
    }
    this.setImageSrc();
    this.imageIsloading();
  }

  setImageSrc(i = undefined) {
    if (i) {
      this.currentImage = this.images[i]['srcUrl'];
    } else {
      this.currentImage = this.images[this.currentIndex]['srcUrl'];
    }
  }

  displayImage(i: number) {
    this.currentIndex = i;
    this.setImageSrc(this.currentIndex);
    this.imageIsloading();
  }

  imageIsloading() {
    this.isLoading = true;
    this.layoutService.toggleSpinner(true);
    setTimeout(() => {
      this.isLoading = false;
      this.layoutService.toggleSpinner(false);
    }, 1000);
  }

  showRightButton() {
    return (
      (this.currentIndex + 1 < this.images.length &&
        this.images.length > 1 &&
        this.language === 'en') ||
      (this.currentIndex > 0 && this.language === 'ar')
    );
  }

  showLeftButton() {
    return (
      (this.currentIndex + 1 < this.images.length &&
        this.images.length > 1 &&
        this.language === 'ar') ||
      (this.currentIndex > 0 && this.language === 'en')
    );
  }

  downloadFile() {
    if (this.file.attachmentName.includes('pdf')) {
      this.attachmentsService.downloadAttachment(this.file, false, 'pdf');
    } else {
      this.attachmentsService.downloadAttachment(this.file, false);
    }
  }
}
