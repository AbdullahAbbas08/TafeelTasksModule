import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ScannerService {
  constructor(private swagger: SwaggerClient) {}

  getDefaultScanName() {
    return this.swagger.apiCommitteeMeetingSystemSettingGetByCodeGet(
      'DefaultAttachmentName'
    );
  }
}
