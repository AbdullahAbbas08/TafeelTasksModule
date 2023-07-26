import { Component, OnInit } from '@angular/core';
import { TransactionService } from '../../transactions/transaction.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { TransactionBoxDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { CommitteeService } from 'src/app/committees/committee.service';
import { AuthService } from 'src/app/auth/auth.service';
@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
})
export class TransactionComponent implements OnInit {
  transactionType: string = 'Inbox';
  transactionContainer:TransactionBoxDTO[] = [];
  page:number = 0;
  pageSize:number = 10;
  committeeId: string;
  filterId:number = 1;
  filterCase:number = 1;
  loadingData: boolean = false;
  currentCount = 0;
  permittedInboxTransactions = false;
  permittedOutboxTransactions = false
  count:number = 0;
  userId:any;
  constructor(private authService:AuthService,private committeeService:CommitteeService,private _transactions:TransactionService,private router : ActivatedRoute,private layoutService: LayoutService,public translateService: TranslateService, private browserService: BrowserStorageService) {
   }

  ngOnInit(): void {
    this.userId = this.authService.getUser().userId;
    this.router.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
    this.getBoxType2(this.transactionType);
    this.checkPermissions();
  }
  onTransactionTypeChange(event) {
    if (event) {
      this.transactionType = 'Outbox';
      this.getBoxType2(this.transactionType)
    } else {
      this.transactionType = 'Inbox';
      this.getBoxType2(this.transactionType)
    }
  }
  onScroll(){
    this.page++;
    this.getBoxType2(this.transactionType,true)
}
getBoxType2(transactionType,scroll:boolean = false){
  if(!scroll){
    this.loadingData = true;
    this.layoutService.toggleSpinner(true);
    this.page = 1
  }
  this._transactions.getBoxType(transactionType,undefined,this.page,this.pageSize,this.browserService.encryptCommitteId(this.committeeId),false,false,this.filterId,this.filterCase).subscribe((result) => {
    if(result && result.data){
      this.transactionContainer = scroll ? [...this.transactionContainer,...result.data] : result.data;
      this.count = result.count
    }
    this.layoutService.toggleSpinner(false);
    this.loadingData = false;
  })
}
checkPermissions(){
  if(this.committeeService.CommitteHeadUnitId === +this.userId || this.committeeService.committeMembers.some((el) => el.userId === +this.userId)){
    this.permittedInboxTransactions = this.committeeService.checkPermission('INBOXTRANSACTION');
    this.permittedOutboxTransactions = this.committeeService.checkPermission('OUTBOXTRANSACTION');
  } else{
      this.permittedInboxTransactions = true;
      this.permittedOutboxTransactions = true
    }
  }
}
