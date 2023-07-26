
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AttachmentsService } from 'src/app/committees/committee-details/attachments/attachments.service';
import { SavedAttachmentDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';


@Component({
  selector: 'app-download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.scss'],
})
export class DownloadComponent implements OnInit {
  @Output() close = new EventEmitter<void>();
  @Input() file: SavedAttachmentDTO;
  @Input() fileIcon: string;

  constructor(private attachmentService: AttachmentsService) {}

  ngOnInit(): void {}

  downloadFile() {
    this.attachmentService.downloadAttachment(this.file, true);
  }

  onClose() {
    this.close.emit();
  }
}
