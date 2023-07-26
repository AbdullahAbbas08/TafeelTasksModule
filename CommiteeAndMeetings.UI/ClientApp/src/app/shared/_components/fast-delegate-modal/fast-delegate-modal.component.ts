import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs-compat';
import { CommitteeService } from 'src/app/committees/committee.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { GroupReferralDTO, Lookup, LookUpDTO, OrganizationDetailsDTO, SwaggerClient, TransactionBoxDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SettingControllers } from '../../_enums/AppEnums';
import { catchError, debounceTime, switchMap, tap } from 'rxjs/operators';
import { of } from 'rxjs/internal/observable/of';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { BrowserStorageService } from '../../_services/browser-storage.service';
import { SearchService } from '../../_services/search.service';
@Component({
  selector: 'app-fast-delegate-modal',
  templateUrl: './fast-delegate-modal.component.html',
  styleUrls: ['./fast-delegate-modal.component.scss']
})
export class FastDelegateModalComponent implements OnInit {
  fastDelegateForm: FormGroup;
  visible: boolean;
  currentLang: string;
  toEmployee: boolean = false;
  isCcEmp: boolean = false;
  isCcOrg: boolean = false;
  isLoading: boolean = false;
  users: LookUpDTO[] = [];
  organizations: OrganizationDetailsDTO[] = [];
  lookupTypes = SettingControllers;
  correspondents: Lookup[] = [];
  requirdActions: Lookup[] = [];
  organizationsChanged$ = new BehaviorSubject('');
  employeesChanged$ = new BehaviorSubject('');
  requirdActionChange$ = new BehaviorSubject('');
  correspondentChange$ = new BehaviorSubject('');
  saving: boolean = false;
  metaData: TransactionBoxDTO;
  isDelegateLoading: boolean = false;
  constructor(private committeSvr: CommitteeService, private translate: TranslateService
    , private modalService: SharedModalService, private browserService: BrowserStorageService,
    private swagger: SwaggerClient, private lookupService: LookupService, private searchService: SearchService,) {
    this.currentLang = this.translate.currentLang;
  }

  ngOnInit() {
    this.initForm();
    this.updateFormValidation();
    this.initOrganizations();
    this.initEmployees();
    this.initCorrespondents();
    this.initRequiredActions();
  }

  initForm() {
    this.fastDelegateForm = new FormGroup({
      explanation: new FormControl(''),
      referralTo: new FormControl('org'),
      toOrganization: new FormControl(null, [Validators.required]),
      toEmployee: new FormControl(null),
      requiredAction: new FormControl(null, [Validators.required]),
      correspondent: new FormControl(null),
      ccOrganaizations: new FormControl([]),
      ccEmployees: new FormControl([]),
      fromCommiteeId: new FormControl(null),
    });
  }

  updateFormValidation() {
    this.fastDelegateForm.get('referralTo').valueChanges.subscribe((val) => {
      if (val === 'emp') {
        this.fastDelegateForm
          .get('toEmployee')
          .setValidators(Validators.required);
        this.fastDelegateForm.get('toOrganization').clearValidators();
        this.toEmployee = true;
      } else if (val === 'org') {
        this.fastDelegateForm
          .get('toOrganization')
          .setValidators(Validators.required);
        this.fastDelegateForm.get('toEmployee').clearValidators();
        this.toEmployee = false;
      }
    });
  }

  referralTo(value: string) {
    this.fastDelegateForm.get('referralTo').patchValue(value);
    this.visible = false;

    if ((value = 'org')) this.fastDelegateForm.patchValue({ toEmployee: null });
    if ((value = 'emp'))
      this.fastDelegateForm.patchValue({ toOrganization: null });
  }

  initOrganizations() {
    this.organizationsChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.swagger
            .apiTransactionsGetOrganizationToReferralGet(
              false,
              false,
              true,
              text ? text : '',
              this.browserService.encrypteString(this.committeSvr.CommitteId),
              undefined,
              this.committeSvr.DepartmentLinkId
            )
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => (this.isLoading = false))
            )
        )
      )
      .subscribe((res: Lookup[]) => {
        this.organizations = res;
        this.isLoading = false;
      });
  }

  initEmployees() {
    this.employeesChanged$
      .asObservable()
      .pipe(
        debounceTime(500),

        switchMap((text: string) =>
          this.swagger
            .apiTransactionsGetEmployeesToReferralGet(
              false,
              false,
              text ? text : ''
            )
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => (this.isLoading = false))
            )
        )
      )
      .subscribe((res: Lookup[]) => {
        this.users = res;
        this.isLoading = false;
      });
  }

  initRequiredActions() {
    this.requirdActionChange$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getLookup(
            'requiredactions',
            undefined,
            text ? text : undefined,
            [
              {
                field: 'AllowedInTo',
                operator: 'eq',
                value: 'true',
              },
            ],
            undefined,
            20
          )
        )
      )
      .subscribe((res: Lookup[]) => {
        this.requirdActions = res;
        this.isLoading = false;
      });
  }

  initCorrespondents() {
    this.correspondentChange$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this.lookupService.getLookup(
            'correspondents',
            undefined,
            text ? text : undefined,
            undefined,
            undefined,
            20
          )
        )
      )
      .subscribe((res: Lookup[]) => {
        this.correspondents = res;
        this.isLoading = false;
      });
  }

  submitFastDelegate() {
    if (this.fastDelegateForm.valid) {
      this.isDelegateLoading = true;
      let GroupReferralDTO: GroupReferralDTO = {
        tranActionRecIds: this.metaData.transactionIdEncrypt + '_' + this.metaData.transactionActionRecipientIdEncrypt,
        tranActionIds: null,
        requiredActionId: this.fastDelegateForm.get('requiredAction').value,
        note: this.fastDelegateForm.get('explanation').value,
        empCCIds: this.fastDelegateForm.get('ccEmployees').value ? this.fastDelegateForm.get('ccEmployees').value.join(',') : "",
        orgCCIds: this.fastDelegateForm.get('ccOrganaizations').value ? this.fastDelegateForm.get('ccOrganaizations').value.join(',') : "",
        directToEmpId: this.fastDelegateForm.get('toEmployee').value,
        directToOrgId: this.fastDelegateForm.get('toOrganization').value,
        actionId: this.metaData.actionId,
        isEmp: false,
        correspondentUserId: this.metaData.correspondentUserId,
        fromCommiteeId: this.browserService.encrypteString(this.committeSvr.CommitteId)
      } as GroupReferralDTO;
      this.swagger.apiTransactionsGroupReferralPost(GroupReferralDTO).subscribe(res => {
        this.searchService.search(undefined);
        this.modalService.createMessage('success', 'ProcessSuccessfully');
        this.close();
        this.isDelegateLoading = false
      }, err => {
        this.isDelegateLoading = false
      })
    }
  }

  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.ORGANIZATION:
        this.organizationsChanged$.next(value);
        break;
      case SettingControllers.USERS:
        this.employeesChanged$.next(value);
        break;
      case SettingControllers.REQUIRED_ACTIONS:
        this.requirdActionChange$.next(value);
        break;
      case SettingControllers.CORRESPONDENT:
        this.correspondentChange$.next(value);
        break;
      default:
        break;
    }
  }

  close() {
    this.fastDelegateForm.reset();
    this.modalService.destroyModal();
  }

}
