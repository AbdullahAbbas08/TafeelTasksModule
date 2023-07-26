import { AllCommiteeAttachmentDTO, SavedAttachmentDTO } from './../../../core/_services/swagger/SwaggerClient.service';
import { AuthService } from './../../../auth/auth.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {
  API_BASE_URL,
  DocumentDTODataSourceResult,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { Router } from '@angular/router';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';


@Injectable({
  providedIn: 'root',
})
export class AttachmentsService {
  baseUrl: string;
  commiteeId: number;
  currentUrl: string;
  isTransaction:boolean = false;
  addAttachmentWithComment = new BehaviorSubject<any>(null);
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) baseUrl: string,
    private swaggerServce: SwaggerClient,
    private authService: AuthService,
    private router: Router,
    private BrowserService:BrowserStorageService
  ) {
    this.baseUrl = baseUrl;
  }

  // getDocuments(
  //   take: number,
  //   skip: number,
  //   committeeId: number,
  //   filters: any[] = [],
  //   filteLogic: string = undefined
  // ): Observable<DocumentDTODataSourceResult> {
  //   return this.swaggerServce.apiCommiteeAttachmentsGetAllGet(
  //     take,
  //     skip,
  //     undefined,
  //     `CommiteeId`,
  //     'eq',
  //     `${committeeId}`,
  //     filteLogic,
  //     filters,
  //     false
  //   );
  // }
  getDocuments(
    take: number,
    skip: number,
    committeeId: string,
    dateFrom:Date,
    dateTo:Date,    
    searchText:string
  ): Observable<AllCommiteeAttachmentDTO> {
    return this.swaggerServce.apiCommiteeAttachmentsGetAllAttchmentGet(
      take,
      skip,
      committeeId,
      dateFrom,
      dateTo,
      searchText
    );
  }
  postAttachment(
    files: File[],
    description: string,
    allUsers: boolean,
    selectedUsers,
    committeeId
  ) {
    let selUsers = '';

    if (selectedUsers) {
      selectedUsers.forEach((user) => {
        selUsers += `${user},`;
      });
      selUsers = selUsers.slice(0, -1);
    } else {
      selUsers = undefined;
    }

    const postData = new FormData();
    files.forEach((file) => postData.append(file.name, file));
    postData.append('description', description);
    postData.append('allUsers', allUsers ? 'true' : 'false');
    postData.append('selectedUsers', selUsers);

    return this.http.post(
      `${this.baseUrl}/api/Document/UploadAttachmentToCommitte?CommiteeId=${committeeId}`,
      postData
    );
  }
  getImagesWithURL(attachment: SavedAttachmentDTO) {
    let list = [];
    let count = attachment.pagesCount;

    if (count < 1) return;
    this.currentUrl = this.router.routerState.snapshot.url;

    if(this.currentUrl.includes('transactions')){
      this.isTransaction = true
    } else {
      this.isTransaction = false 
    }
    const encryptedId:string = this.BrowserService.encrypteString(`${attachment.savedAttachmentId}`)
    for (let i = 0; i < count; i++) {
      const srcUrl = `${this.baseUrl}/api/Document/downloadpageoriginal?id=${
        encryptedId
      }&pageNumber=${i + 1}&thumb=false&IsTransaction=${this.isTransaction}`;
      const thumbUrl = `${this.baseUrl}/api/Document/downloadpageoriginal?id=${
        encryptedId
      }&pageNumber=${i + 1}&thumb=true&IsTransaction=${this.isTransaction}`;

      list.push({ srcUrl: srcUrl, thumbUrl: thumbUrl,attachment:attachment});
    }
    return list;
  }

  downloadAttachment(attachment: SavedAttachmentDTO, getOriginal, type?) {
    function saveData(blob, attachmentName) {
      // does the same as FileSaver.js
      let newWindow: any = window.open();
      let a = newWindow.document.createElement('a');
      newWindow.document.body.appendChild(a);
      a.style.display = 'none';

      var url = newWindow.URL.createObjectURL(blob);
      a.href = url;
      a.download = attachmentName;
      a.target = '_blank';
      a.click();
      newWindow.URL.revokeObjectURL(url);
      setTimeout(() => {
        newWindow.close();
      }, 200);
    }
    this.currentUrl = this.router.routerState.snapshot.url;

    if(this.currentUrl.includes('transactions')){
      this.isTransaction = true
    } else {
      this.isTransaction = false 
    }

    if (attachment) {
      let getOriginalParameter;
      if (getOriginal !== null && getOriginal !== undefined)
        getOriginalParameter = `&getOriginal=${getOriginal}`;
      else getOriginalParameter = '';
      let requestUrl = `${this.baseUrl}/api/Document/download?id=${attachment.savedAttachmentId}${getOriginalParameter}&IsTransaction=${this.isTransaction}`;
      if (type === 'pdf')
        requestUrl = `${this.baseUrl}/api/Document/downloadPdf?id=${attachment.savedAttachmentId}&IsTransaction=${this.isTransaction}`;

      let fileName = attachment.attachmentName;

      // const fileNameArray = fileName.split('.');

      const accessToken = this.authService.getToken();

      let xhr = new XMLHttpRequest();
      xhr.open('GET', requestUrl);
      xhr.setRequestHeader('Authorization', 'Bearer ' + accessToken);
      xhr.responseType = 'blob';

      xhr.onload = function () {
        // let mimeType = attachment.attachment.mimeType;
        // fileNameArray.push(mimeType);
        // fileName = fileNameArray.join('.');

        saveData(this.response, fileName); // saveAs is now your function
      };
      xhr.send();
    }
  }
}
