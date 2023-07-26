import { Component, OnInit , OnDestroy  } from '@angular/core';
import { TransactionService } from '../transaction.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionBoxDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { Subscription } from 'rxjs';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { CommitteeService } from 'src/app/committees/committee.service';
@Component({
  selector: 'app-inbox',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.scss']
})
export class InboxComponent implements OnInit , OnDestroy  {
  inComingContainer:TransactionBoxDTO[] = [];
  boxType:string = 'INBOX';
  committeeId: any;
  page:number = 7;
  pageSize:number = 10;
  filterId:number = 1;
  filterCase:number = 1;
  loadingData: boolean = false;
  currentCount = 0;
  count:number = 0;
  searchText:string;
  subscription: Subscription;
  constructor(private committeService:CommitteeService ,private _transaction:TransactionService,private router : ActivatedRoute,public translateService: TranslateService,private layoutService: LayoutService,private searchService: SearchService,private BrowserService:BrowserStorageService) {
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
    this._transaction.getBoxType(this.boxType,this.searchText,this.page,this.pageSize,this.BrowserService.encrypteString(`${this.committeeId}_${this.BrowserService.getUserRoleId()}`),false,false,this.filterId,this.filterCase).subscribe((result) => {
      if(result && result.data){
        this.inComingContainer = scroll ? [...this.inComingContainer,...result.data] : result.data;
        this.count = result.count
      }
      this.layoutService.toggleSpinner(false);
      this.loadingData = false;
    })

  }
}
