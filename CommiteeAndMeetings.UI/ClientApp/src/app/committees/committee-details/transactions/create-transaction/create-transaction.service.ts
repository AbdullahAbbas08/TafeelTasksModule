import { HttpClient } from '@angular/common/http';
import {
  AccessToken,
  API_BASE_URL,
  AttachmentSummaryDTO,
  TransactionAttachment,
  TransactionAttachmentDTO,
  TransactionDetailsDTO,
} from './../../../../core/_services/swagger/SwaggerClient.service';
import { AuthService } from 'src/app/auth/auth.service';
import { SwaggerClient } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Injectable, Inject } from '@angular/core';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Injectable({
  providedIn: 'root',
})
export class CreateTransactionService {
  baseUrl: string;
  commiteeId: number;

  constructor(
    @Inject(API_BASE_URL) baseUrl: string,
    private swagger: SwaggerClient,
    private authService: AuthService,
    private http: HttpClient,
    private browserService:BrowserStorageService
  ) {
    this.baseUrl = baseUrl;
  }

  saveTransaction(subject: string, explanation: string, isSecret: boolean, transactionTypeId: number) {
    let token = this.authService.getToken();

    return this.swagger.apiTransactionsSaveTransactionPost(
      new TransactionDetailsDTO({
        classificationId: 1,
        confidentialityLevelId: isSecret ? 2 : 1,
        importanceLevelId: 1,
        subject: subject,
        notes: explanation,
        transactionBasisTypeId: 8,
        transactionTypeId,
      })
    );
  }

  uploadTransactionAttachments(files: File[], transactionId?: number) {
    const formData = new FormData();
    files.forEach((file) => formData.append(file.name, file));

    return this.http.post(
      `${this.baseUrl}/api/document/upload?TransactionId=${transactionId}`,
      formData
    );
  }

  insertTransactionAttachments(
    attachments: AttachmentSummaryDTO[],
    transactionId: number
  ) {
    let transactionattachments: TransactionAttachmentDTO[] = attachments.map(
      (element) => {
        return new TransactionAttachmentDTO({
          attachmentIdEncypted: this.browserService.encrypteString(element.attachmentId),
          transactionIdEncypted: this.browserService.encrypteString(transactionId),
        });
      }
    );
    return this.swagger.apiTransactionsInsertAttachmentsPost(
      transactionattachments
    );
  }
}
