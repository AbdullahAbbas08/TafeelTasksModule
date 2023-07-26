import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SavedAttachmentDTO, TransactionBoxDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommitteeActions } from 'src/app/shared/_enums/AppEnums';
import { SharedModalService } from 'src/app/core/_services/modal.service';

@Component({
  selector: 'app-transaction-attachment-item',
  templateUrl: './transaction-attachment-item.component.html',
  styleUrls: ['./transaction-attachment-item.component.scss']
})
export class TransactionAttachmentItemComponent implements OnInit {
  @Input('items') items: TransactionBoxDTO[] ;
  savedAttachemntsList:any[]=[]
  routerPath:string;
  constructor(public translateService: TranslateService,private router:Router,private modalService:SharedModalService,private route:ActivatedRoute ) { 
    let routeArr: string[] = this.router.url.split('/');
    this.routerPath = routeArr[routeArr.length - 1];
  }

  ngOnInit(): void {
    this.initAttachments();
  }
  initAttachments(){
    this.items.forEach((item)=> {
      let savedAttachmentsItem : SavedAttachmentDTO[] = [];
      item.transactionActionRecipientAttachment.forEach(attachment => {
        savedAttachmentsItem.push(new SavedAttachmentDTO({
          attachmentName: attachment.attachmentName,
          savedAttachmentId: attachment.attachmentId,
          pagesCount: attachment.pageCount
        }))
      })
      this.savedAttachemntsList.push(savedAttachmentsItem);
    })
  }

  openFastDelegation<TransactionBoxDTO>(transaction:TransactionBoxDTO){
    this.modalService.openDrawerModal(CommitteeActions.FastDelegation,undefined,undefined,undefined,undefined,undefined,undefined,
      undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,undefined,transaction);
  }
}
