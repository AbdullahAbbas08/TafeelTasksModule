import {
  SurveyDTO,
  ValidityPeriodDTO,
} from './../../../core/_services/swagger/SwaggerClient.service';
import { VotingService } from './voting.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { StoreService } from 'src/app/shared/_services/store.service';
import { SearchService } from 'src/app/shared/_services/search.service';
import { LayoutService } from 'src/app/shared/_services/layout.service';
import { CommitteeService } from '../../committee.service';
import { ActivatedRoute } from '@angular/router';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-votes',
  templateUrl: './votes.component.html',
  styleUrls: ['./votes.component.scss'],
})
export class VotesComponent implements OnInit, OnDestroy {
  votings: SurveyDTO[];
  committeeId: string;
  loadingData: boolean = false;
  count: number = 0;
  take: number = 10;
  skip: number = 0;
  filters: any[] = [];
  subscription: Subscription;
  stateSubscription: Subscription;
  votingSubscription:Subscription;
  validityPeriod: ValidityPeriodDTO;
  currentCount = 0;

  constructor(
    private votingService: VotingService,
    private storeService: StoreService,
    private searchService: SearchService,
    private layoutService: LayoutService,
    private committeeService: CommitteeService,
    private route: ActivatedRoute,
    private BrowserService:BrowserStorageService
  ) {}

  ngOnInit(): void {
    this.route.parent.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );

   this.votingSubscription = this.storeService.refreshVotings$.subscribe((val) => {
      if (val && val !== undefined && this.votings) {
        this.votings.unshift(val);
      }
    });

    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
      this.getVotings(false, word);
    });

    // Validity Period
    this.stateSubscription = this.committeeService.committeePeriodChange$.subscribe(
      (period) => {
        this.validityPeriod = period;
        this.getVotings();
      }
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.stateSubscription.unsubscribe();
    this.votingSubscription.unsubscribe()
  }

  onScroll() {
    if (this.currentCount < this.count) {
      this.skip += this.take;
      this.getVotings(true);
    }
  }

  getVotings(scroll: boolean = false, searchWord: string = undefined) {
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
    //     { field: 'subject', operator: 'contains', value: searchWord },
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
    this.votingService
      .getVotings(this.take, this.skip,this.BrowserService.encryptCommitteId(this.committeeId),this.validityPeriod.validityPeriodFrom,
        this.validityPeriod.validityPeriodTo,searchWord)
      .subscribe((res) => {
        if (res && res.surveys.data) {
          this.votings = scroll ? [...this.votings, ...res.surveys.data] : res.surveys.data;
          this.count = res.surveys.count;

          if (scroll) {
            this.currentCount += res.surveys.data.length;
          } else {
            this.currentCount = res.surveys.data.length;
          }
        }

        this.layoutService.toggleSpinner(false);
        this.loadingData = false;
      });
  }
}
