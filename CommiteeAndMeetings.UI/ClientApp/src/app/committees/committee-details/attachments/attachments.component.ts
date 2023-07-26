import { ActivatedRoute } from '@angular/router';
import { LayoutService } from './../../../shared/_services/layout.service';
import { Subscription } from 'rxjs';
import { AttachmentsService } from './attachments.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { StoreService } from 'src/app/shared/_services/store.service';
import {
  CommiteeAttachmentDTO,
  ValidityPeriodDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { CommitteeService } from '../../committee.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-attachments',
  templateUrl: './attachments.component.html',
  styleUrls: ['./attachments.component.scss'],
})
export class AttachmentsComponent implements OnInit, OnDestroy {
  documents: CommiteeAttachmentDTO[];
  committeeId: string;
  count: number = 0;
  take: number = 10;
  skip: number = 0;
  loadingData: boolean = false;
  filters: any[] = [];
  subscription: Subscription;
  stateSubscription: Subscription;
  validityPeriod: ValidityPeriodDTO;
  currentCount = 0;

  constructor(
    private attachmentsService: AttachmentsService,
    private storeService: StoreService,
    private searchService: SearchService,
    private layoutService: LayoutService,
    private committeeService: CommitteeService,
    private route: ActivatedRoute,
    private browserStorage:BrowserStorageService
  ) {}

  ngOnInit(): void {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );

    this.storeService.refreshDocuments$.subscribe((val) => {
      if (val) {
        this.documents.unshift(val);
      }
    });

    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
      this.getDocuments(false, word);
    });

    // Validity Period
    this.stateSubscription = this.committeeService.committeePeriodChange$.subscribe(
      (period) => {
        this.validityPeriod = period;
        this.getDocuments();
      }
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.stateSubscription.unsubscribe();
  }

  onScroll() {
    if (this.currentCount < this.count) {
      this.skip += this.take;
      this.getDocuments(true);
    }
  }

  getDocuments(scroll: boolean = false, searchWord: string = undefined) {
    if (!scroll) {
      this.loadingData = true;
      this.layoutService.toggleSpinner(true);
      this.skip = 0;
    }

    // let dateFromString = this.validityPeriod.validityPeriodFrom.getTime();
    // let dateToString = this.validityPeriod.validityPeriodTo.getTime();

    // let dateFrom = new Date(dateFromString).toISOString();
    // let dateTo = new Date(dateToString).toISOString();

    // if (searchWord) {
    //   this.filters = [
    //     { field: 'description', operator: 'contains', value: searchWord },
    //     {
    //       field: 'createdOn',
    //       operator: 'gte',
    //       value: dateFrom,
    //     },
    //     {
    //       field: 'createdOn',
    //       operator: 'lte',
    //       value: dateTo,
    //     },
    //     { field: 'commiteeId', operator: 'eq', value: this.committeeId },
    //   ];
    // } else {
    //   this.filters = [
    //     {
    //       field: 'createdOn',
    //       operator: 'gte',
    //       value: dateFrom,
    //     },
    //     {
    //       field: 'createdOn',
    //       operator: 'lte',
    //       value: dateTo,
    //     },
    //     { field: 'commiteeId', operator: 'eq', value: this.committeeId },
    //   ];
    // }

    this.attachmentsService
      .getDocuments(this.take, this.skip,this.browserStorage.encryptCommitteId(this.committeeId),this.validityPeriod.validityPeriodFrom,this.validityPeriod.validityPeriodTo,searchWord)
      .subscribe((res) => {
        if (res && res.attachments.data) {
          this.documents = scroll ? [...this.documents, ...res.attachments.data] : res.attachments.data;
          this.count = res.attachments.count;
          if (scroll) {
            this.currentCount += res.attachments.data.length;
          } else {
            this.currentCount = res.attachments.data.length;
          }
        }
        this.layoutService.toggleSpinner(false);
        this.loadingData = false;
      });
  }
}
