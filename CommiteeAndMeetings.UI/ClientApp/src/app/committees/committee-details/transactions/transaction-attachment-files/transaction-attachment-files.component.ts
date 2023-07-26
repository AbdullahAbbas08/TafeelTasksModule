import { Component,Input, OnInit } from '@angular/core';
import { AttachmentsService } from '../../attachments/attachments.service';
import { AttachmentViewDTO, SavedAttachmentDTO } from './../../../../core/_services/swagger/SwaggerClient.service';
@Component({
  selector: 'app-transaction-attachment-files',
  templateUrl: './transaction-attachment-files.component.html',
  styleUrls: ['./transaction-attachment-files.component.scss']
})
export class TransactionAttachmentFilesComponent implements OnInit {

  @Input() attachments: AttachmentViewDTO[];
  imagesListList = [];
  targetIndex = 0;
  showGallery = [];
  showDownload = [];
  iconLogos = [];
  constructor(private attachmentService: AttachmentsService) {}

  ngOnInit(): void {
    this.selectFilesIcons();
    this.getImagesListsList();
    this.attachments.forEach(() => {
      this.showDownload.push(false);
      this.showGallery.push(false);
    });
  }

  selectFilesIcons() {
    for (let i = 0; i < this.attachments.length; i++) {
      let pagesCount = this.attachments[i].pageCount;
      let mime = this.attachments[i].mimeType;

      if (pagesCount > 0) {
        this.iconLogos.push(null);
      } else {
        if (mime.includes('word')) {
          this.iconLogos.push('../../../../../assets/images/word-lg.png');
        } else if (mime.includes('spreadsheet')) {
          this.iconLogos.push('../../../../../assets/images/excel-lg.png');
        } else {
          this.iconLogos.push('../../../../../assets/images/file-icon.png');
        }
      }
    }
  }

  getImagesListsList() {
    for (let i = 0; i < this.attachments.length; i++) {
      let pagesCount = this.attachments[i].pageCount;
      if (!pagesCount || pagesCount < 0) this.imagesListList.push(null);
      else {
        let imageList = this.attachmentService.getImagesWithURL(
          this.attachments[i]
        );
        this.imagesListList.push(imageList);
      }
    }
  }

  openImageGallery(index1, index2) {
    this.targetIndex = index2;
    this.showGallery[index1] = true;
  }

  openDownloadFile(index) {
    this.showDownload[index] = true;
  }
}
