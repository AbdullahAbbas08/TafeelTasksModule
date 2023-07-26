import { LayoutService } from './../../shared/_services/layout.service';
import {
  CommiteeTaskDTO,
  CommiteeUsersRoleDTO,
  SwaggerClient,
  ValidityPeriodDTO,
} from './../../core/_services/swagger/SwaggerClient.service';
import { Component, OnInit,Output,EventEmitter } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { CommiteeDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { CommitteeService } from '../committee.service';
import { TranslateService } from '@ngx-translate/core';
import { NgbDateService } from 'src/app/shared/_services/ngb-date.service';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';

@Component({
  selector: 'app-committee-details',
  templateUrl: './committee-details.component.html',
  styleUrls: ['./committee-details.component.scss'],
})
export class CommitteeDetailsComponent implements OnInit {
  menuTogglled: boolean = false;
  committeeId: string;
  committeDetails: CommiteeDTO;
  dataLoading: boolean = false;
  openDialog: boolean = false;
  currentUrl: string;
  hideControlsAndUsers = false;
  hidetransActionControls: boolean = false;
  hideonlineUserTasks:boolean = false;
  validityPeriods: ValidityPeriodDTO[] = [];
  selectedPeriod: ValidityPeriodDTO;
  committeeUserRoles: CommiteeUsersRoleDTO[];
  committeeEndDate: Date;
  enableTransaction:boolean;
  enableDescision:boolean;
  currentLang: string;
  loadingData = false;
  tasksSummary:CommiteeTaskDTO[] = [];
  constructor(
    private route: ActivatedRoute,
    private committeeService: CommitteeService,
    private router: Router,
    private swagger: SwaggerClient,
    public translateService: TranslateService,
    private dateService: NgbDateService,
    private layoutService: LayoutService,
    private bs: BrowserStorageService,
  ) {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.committeeId = id;
        this.getCommitteePermisssions();
      }
    });
  }

  ngOnInit(): void {
    this.langChange();
    this.getCommitteDetails();
    this.checkCurrentComponent();
    this.onPeriodArchivedUpdate();
    this.onExtendUpdate();
  }

  changeSeletedPeriod(
    periodState: number,
    validityPeriodId: number,
    dateFrom: Date,
    dateTo: Date
  ) {
    if(dateFrom.getFullYear() < 1900 || dateTo === undefined || dateTo.getFullYear() < 1900){
      this.selectedPeriod = new ValidityPeriodDTO({
        periodState: periodState,
        validityPeriodId: validityPeriodId,
        validityPeriodFrom: dateFrom,
        validityPeriodTo: undefined,
      });
    } else {
      this.selectedPeriod = new ValidityPeriodDTO({
        periodState: periodState,
        validityPeriodId: validityPeriodId,
        validityPeriodFrom: dateFrom,
        validityPeriodTo: dateTo,
      });
    }

    this.committeeService.committeePeriodChange$.next(this.selectedPeriod);
  }

  onPeriodArchivedUpdate() {
    this.committeeService.archiveCurrentPeriod$.subscribe(() => {
    if(this.validityPeriods[0].validityPeriodFrom.getFullYear() < 1900 || this.validityPeriods[0].validityPeriodTo.getFullYear() < 1900){
      let closedPeriod = new ValidityPeriodDTO({
        periodState: 2,
        validityPeriodId:
          this.validityPeriods[this.validityPeriods.length - 1]
            .validityPeriodId,
        validityPeriodFrom:this.committeDetails.createdOn,
        validityPeriodTo: new Date(),
      });
      let startedPeriod = new ValidityPeriodDTO({
        periodState: 1,
        validityPeriodId:
          this.validityPeriods[this.validityPeriods.length - 1]
            .validityPeriodId,
        validityPeriodFrom: new Date(),
        validityPeriodTo:new Date("0001-01-01T00:00:00")
      });
      this.validityPeriods.splice(this.validityPeriods.length - 1);
      this.validityPeriods.push(closedPeriod, startedPeriod);
      this.selectedPeriod =
        this.validityPeriods[this.validityPeriods.length - 2];
    }else {
      let closedPeriod = new ValidityPeriodDTO({
        periodState: 2,
        validityPeriodId:
          this.validityPeriods[this.validityPeriods.length - 1]
            .validityPeriodId,
        validityPeriodFrom:
          this.validityPeriods[this.validityPeriods.length - 1]
            .validityPeriodFrom,
        validityPeriodTo: new Date(),
      });
      let startedPeriod = new ValidityPeriodDTO({
        periodState: 1,
        validityPeriodId:
          this.validityPeriods[this.validityPeriods.length - 1]
            .validityPeriodId,
        validityPeriodFrom: new Date(),
        validityPeriodTo:
          this.validityPeriods[this.validityPeriods.length - 1]
            .validityPeriodTo,
      });
       
      this.validityPeriods.splice(this.validityPeriods.length - 1);
      this.validityPeriods.push(closedPeriod, startedPeriod);
      this.selectedPeriod =
        this.validityPeriods[this.validityPeriods.length - 2];
    }
    });
  }

  onExtendUpdate() {
    this.committeeService.extendCommittee$.subscribe((newDate) => {
      this.validityPeriods.find(
        (period) => period.periodState === 1 || period.periodState === 3
      ).validityPeriodTo = newDate;
      this.committeeEndDate = newDate;
    });
    this.selectedPeriod = this.validityPeriods[this.validityPeriods.length - 1];
  }

  getCommitteDetails() {
    this.loadingData = true;
    this.layoutService.toggleSpinner(true);

    this.committeeService
      .getCommitteeDetails(this.committeeId)
      .subscribe((committee) => {
        this.committeDetails = committee;
        this.enableTransaction = committee.enableTransactions;
        this.enableDescision = committee.enableDecisions
        this.validityPeriods = committee.validityPeriod;
        this.selectedPeriod =
          this.validityPeriods[this.validityPeriods.length - 1];
        this.committeeEndDate =
          this.validityPeriods[
            this.validityPeriods.length - 1
          ].validityPeriodTo;

        this.loadingData = false;
        this.layoutService.toggleSpinner(false);
      });
  }

  getCommitteePermisssions() {
    this.swagger
      .apiCommiteesGetCommitteeRolesGet(this.bs.encryptCommitteId(this.committeeId),0)
      .subscribe((res) => {
        this.committeeService.setCommitteeUserRole(res);
      });
  }

  showFullData() {
    this.dataLoading = true;
    this.openDialog = !this.openDialog;
    // setTimeout(() => {
    this.dataLoading = false;
    // }, 400);
  }

  checkCurrentComponent() {
    this.currentUrl = this.router.routerState.snapshot.url;
    if (
      this.currentUrl.includes('users') ||
      this.currentUrl.includes('statistics') ||
      this.currentUrl.includes('transactions') 
    ) {
      this.hideControlsAndUsers = true;
    } else {
      this.hideControlsAndUsers = false;
    }
    if (this.currentUrl.includes('transactions')) {
      this.hidetransActionControls = true;
    } else {
      this.hidetransActionControls = false;
    }
    if(this.currentUrl.includes('tasks')){
      this.hideonlineUserTasks = true;
    } else {
      this.hideonlineUserTasks = false;
    }
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = this.router.routerState.snapshot.url;
        if (
          this.currentUrl.includes('users') ||
          this.currentUrl.includes('statistics') ||
          this.currentUrl.includes('transactions')
        ) {
          this.hideControlsAndUsers = true;
        } else {
          this.hideControlsAndUsers = false;
        }
        if (this.currentUrl.includes('transactions')) {
          this.hidetransActionControls = true;
        } else {
          this.hidetransActionControls = false;
        }
        if(this.currentUrl.includes('tasks')){
          this.hideonlineUserTasks = true;
        } else {
          this.hideonlineUserTasks = false;
        }
      }
    });
  }

  langChange() {
    this.currentLang = this.translateService.currentLang;
    this.translateService.onLangChange.subscribe(() => {
      this.currentLang = this.translateService.currentLang;
    });
  }

  getHijriDate(date: Date) {
    return this.dateService.creatStructObjectFromDate(date);
  }

}
