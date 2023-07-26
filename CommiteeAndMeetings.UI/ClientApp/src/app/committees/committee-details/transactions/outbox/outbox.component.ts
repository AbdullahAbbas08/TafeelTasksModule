import { Component, OnInit , OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { TransactionBoxDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { TransactionService } from '../transaction.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { Subscription } from 'rxjs';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { CommitteeService } from 'src/app/committees/committee.service';
@Component({
  selector: 'app-outbox',
  templateUrl: './outbox.component.html',
  styleUrls: ['./outbox.component.scss']
})
export class OutboxComponent implements OnInit , OnDestroy  {
  outComingContainer:TransactionBoxDTO[] = []
  boxType:string = 'OUTBOX';
  committeeId: any;
  page:number = 1;
  pageSize:number = 10;
  filterId:number = 1;
  filterCase:number = 1;
  loadingData: boolean = false;
  currentCount = 0;
  count:number = 0;
  searchText:string;
  subscription: Subscription;
  constructor(private committeService:CommitteeService,private _transaction:TransactionService,private router : ActivatedRoute,public translateService: TranslateService, private layoutService: LayoutService,private searchService: SearchService,private browserService:BrowserStorageService) {
   }

  ngOnInit(): void {
    this.committeeId = this.committeService.CommitteId
    this.getBoxType2();
    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
      this.getBoxType2(false, word);
    });
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
  onScroll(){
    this.page += 1;
    this.getBoxType2(true)
}
getBoxType2(scroll:boolean = false, searchWord: string = undefined){
  if(!scroll){
    this.loadingData = true;
    this.layoutService.toggleSpinner(true);
    this.page = 1
  }
  if(searchWord){
    this.searchText = searchWord
  } else {
    this.searchText = undefined
  }
  this._transaction.getBoxType(this.boxType,this.searchText,this.page,this.pageSize,this.browserService.encrypteString(`${this.committeeId}_${this.browserService.getUserRoleId()}`),false,false,this.filterId,this.filterCase).subscribe((result) => {
    if(result && result.data){
      this.outComingContainer = scroll ? [...this.outComingContainer,...result.data] : result.data;
      this.count = result.count
    }
    this.layoutService.toggleSpinner(false);
    this.loadingData = false;
  })

}

}
