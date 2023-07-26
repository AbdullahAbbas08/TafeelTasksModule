import { SearchService } from 'src/app/shared/_services/search.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { LayoutService } from './../../../shared/_services/layout.service';
import { TimeLineService } from './time-line.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ValidityPeriodDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { CommitteeService } from '../../committee.service';
import { ActivatedRoute } from '@angular/router';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-time-line',
  templateUrl: './time-line.component.html',
  styleUrls: ['./time-line.component.scss'],
})
export class TimeLineComponent implements OnInit, OnDestroy {
  take = 5;
  skip = 0;
  loadingData = false;
  committeeId;
  stateSubscription: Subscription;
  validityPeriod: ValidityPeriodDTO;
  timeLineItems = [];
  currentCount = 0;
  count = 0;
  subscription;
  constructor(
    public timeLineService: TimeLineService,
    private layoutService: LayoutService,
    private storeService: StoreService,
    private committeeService: CommitteeService,
    private route: ActivatedRoute,
    private searchService: SearchService,
    private browserService:BrowserStorageService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      (params) => (this.committeeId = params.get('id'))
    );
    this.refreshTimelineItems();

    // Validity Period
    this.stateSubscription =
      this.committeeService.committeePeriodChange$.subscribe((period) => {
        this.validityPeriod = period;
        this.getTimeLineItems();
      });

    // Refresh
    this.subscription = this.searchService.searchcriteria.subscribe((word) => {
      this.getTimeLineItems(false, word);
    });
  }

  ngOnDestroy() {
    this.stateSubscription.unsubscribe();
    this.subscription.unsubscribe();
  }

  onScroll() {
    if (this.currentCount < this.count) {
      this.skip += this.take;
      this.getTimeLineItems(true);
    }
  }

  refreshTimelineItems() {
    this.storeService.refreshTimelineItems$.subscribe((val) => {
      if (val) {
        this.timeLineItems.unshift(val);
      }
    });
  }

  getTimeLineItems(scroll: boolean = false, searchText: string = undefined) {
    if (!scroll) {
      this.loadingData = true;
      this.layoutService.toggleSpinner(true);
      this.skip = 0;
    }
    this.timeLineService
      .getCommiteWallItems(
        this.take,
        this.skip,
        this.validityPeriod.validityPeriodFrom,
        this.validityPeriod.validityPeriodTo,
        this.browserService.encryptCommitteId(this.committeeId),
        searchText
      )
      .subscribe((data) => {
        if (data.result) {
          this.timeLineItems = scroll
            ? [...this.timeLineItems, ...data.result]
            : data.result;
          this.count = data.count;

          if (scroll) {
            this.currentCount += data.result.length;
          } else {
            this.currentCount = data.result.length;
          }
        }
        this.layoutService.toggleSpinner(false);
        this.loadingData = false;
      });
  }
}
