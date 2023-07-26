import {
  AttachmentSummaryDTO,
  DelegationDTO,
  Lookup,
  SwaggerClient,
  TransactionActionAttachmentDTO,
  TransactionActionRecipientAttachmentDTO,
  TransactionActionRecipientsDTO,
  TransactionAttachmentDTO,
  TransactionDetailsDTO,
} from './../../../../core/_services/swagger/SwaggerClient.service';
import { CreateTransactionService } from './create-transaction.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, of } from 'rxjs';
import { catchError, debounceTime, switchMap, tap } from 'rxjs/operators';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import {
  LookUpDTO,
  OrganizationDetailsDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { mimeTypeValidator } from 'src/app/shared/mime-type';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { BrowserStorageService } from 'src/app/shared/_services/browser-storage.service';
import { CommitteeService } from 'src/app/committees/committee.service';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss'],
})
export class CreateTransactionComponent implements OnInit {
  committeeId: string;
  departmentLink:number
  currentLang;
  transactionForm: FormGroup;
  organizationsChanged$ = new BehaviorSubject('');
  employeesChanged$ = new BehaviorSubject('');
  requirdActionChange$ = new BehaviorSubject('');
  correspondentChange$ = new BehaviorSubject('');
  users: LookUpDTO[] = [];
  organizations: OrganizationDetailsDTO[] = [];
  requirdActions: Lookup[] = [];
  correspondents: Lookup[] = [];
  files: File[] = [];
  isLoading = false;
  lookupTypes = SettingControllers;
  transaction: TransactionDetailsDTO = new TransactionDetailsDTO();
  transactionId = undefined;
  visible;
  transactionSaved = false;
  attachments: TransactionAttachmentDTO[];
  enableDecisions: boolean;
  toEmployee = false;
  isCcEmp = false;
  isCcOrg = false;
  isSecret = false;
  isDecision = false;

  saving = false;
  saved = false;
  canSendMinutesOfCommitte:boolean=false;
  @ViewChild('fileInput') fileInput: any;
  committeAttachment:boolean;
  constructor(
    public translate: TranslateService,
    private lookupService: LookupService,
    private modalService: SharedModalService,
    private createTransactionService: CreateTransactionService,
    private swagger: SwaggerClient,
    private browserService: BrowserStorageService,
    private committeSvr:CommitteeService
  ) {}

  ngOnInit(): void {
    this.canSendMinutesOfCommitte=this.committeSvr.checkPermission('sendMinutesOfCommitte');
    this.initForm();
    this.currentLang = this.translate.currentLang;
    this.subscribeToFormChanges();
    // init lookups ...
    this.initOrganizations();
    this.initEmployees();
    this.initCorrespondents();
    this.initRequiredActions();
  }

  initForm() {
    this.transactionForm = new FormGroup({
      subject: new FormControl('', [Validators.required]),
      explanation: new FormControl(''),
      referralTo: new FormControl('org'),
      toOrganization: new FormControl(null),
      toEmployee: new FormControl(null),
      toOrganizationList: new FormControl([]),
      toEmployeeList: new FormControl([]),
      requiredAction: new FormControl(null, [Validators.required]),
      correspondent: new FormControl(null),
      ccOrganaizations: new FormControl([]),
      ccEmployees: new FormControl([]),
      isSecret: new FormControl(null),
      isDecision: new FormControl(false),
      committeMom: new FormControl(null),
      files: new FormControl(null, {
        asyncValidators: [mimeTypeValidator],
      }),
    });
    // if(this.canSendMinutesOfCommitte){
    //   this.transactionForm.addControl('sendMinutesOfCommitte',new FormControl(false))
    // }
  }

  initOrganizations() {
    const id = this.browserService.decrypteString(this.committeeId)
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
              this.browserService.encrypteString(id),
              undefined,
              this.departmentLink
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

  subscribeToFormChanges() {
    this.transactionForm.get('referralTo').valueChanges.subscribe((val) => {
      if (!this.isDecision) {
        if (val === 'emp') {
          this.transactionForm
            .get('toEmployee')
            .setValidators(Validators.required);
          this.transactionForm.get('toOrganization').clearValidators();
          this.toEmployee = true;
        } else if (val === 'org') {
          this.transactionForm
            .get('toOrganization')
            .setValidators(Validators.required);
          this.transactionForm.get('toEmployee').clearValidators();
          this.toEmployee = false;
        }
      } else {
        if (val === 'emp') {
          this.transactionForm
            .get('toEmployeeList')
            .setValidators(Validators.required);
          this.transactionForm.get('toOrganizationList').clearValidators();
          this.toEmployee = true;
        } else if (val === 'org') {
          this.transactionForm
            .get('toOrganizationList')
            .setValidators(Validators.required);
          this.transactionForm.get('toEmployeeList').clearValidators();
          this.toEmployee = false;
        }
      }

      this.transactionForm.get('toEmployee').updateValueAndValidity();
      this.transactionForm.get('toOrganization').updateValueAndValidity();
      this.transactionForm.get('toEmployeeList').updateValueAndValidity();
      this.transactionForm.get('toOrganizationList').updateValueAndValidity();
      this.transactionForm.get('toEmployee').patchValue(undefined);
      this.transactionForm.get('toOrganization').patchValue(undefined);
      this.transactionForm.get('toEmployeeList').patchValue([]);
      this.transactionForm.get('toOrganizationList').patchValue([]);
    });

    this.transactionForm.get('isSecret').valueChanges.subscribe((val) => {
      if (val === true) {
        this.transactionForm.get('files').clearValidators();
      }
      if (val === false) {
        this.transactionForm.get('files').setValidators(Validators.required);
      }
      this.transactionForm.get('files').updateValueAndValidity();
      this.isSecret = val;
    });
    this.transactionForm.get('committeMom').valueChanges.subscribe((val) => {
     if(val === true){
      this.transactionForm.get('files').clearValidators()
     }
     if(val === false){
       this.transactionForm.get('files').setValidators(Validators.required);
     }
     this.transactionForm.get('files').updateValueAndValidity();
    })
    this.transactionForm.get('isDecision').valueChanges.subscribe((val) => {
      this.isDecision = val;

      if (this.toEmployee) {
        if (this.isDecision) {
          this.transactionForm
            .get('toEmployeeList')
            .setValidators(Validators.required);
          this.transactionForm.get('toEmployee').clearValidators();
        } else {
          this.transactionForm
            .get('toEmployee')
            .setValidators(Validators.required);
          this.transactionForm.get('toEmployeeList').clearValidators();
        }
      } else {
        if (this.isDecision) {
          this.transactionForm
            .get('toOrganizationList')
            .setValidators(Validators.required);
          this.transactionForm.get('toOrganization').clearValidators();
        } else {
          this.transactionForm
            .get('toOrganization')
            .setValidators(Validators.required);
          this.transactionForm.get('toOrganizationList').clearValidators();
        }
      }

      this.transactionForm.get('toEmployee').updateValueAndValidity();
      this.transactionForm.get('toOrganization').updateValueAndValidity();
      this.transactionForm.get('toEmployeeList').updateValueAndValidity();
      this.transactionForm.get('toOrganizationList').updateValueAndValidity();
      this.transactionForm.get('toEmployee').patchValue(undefined);
      this.transactionForm.get('toOrganization').patchValue(undefined);
      this.transactionForm.get('toEmployeeList').patchValue([]);
      this.transactionForm.get('toOrganizationList').patchValue([]);
    });

    this.transactionForm.get('isSecret').patchValue(false);
  }
  checkValues(event){
    if(event === true){
      this.transactionForm.get('files').clearAsyncValidators();
    } else {
      this.transactionForm.get('files').setValidators(Validators.required);
    }
    this.committeAttachment = event
    this.transactionForm.get('files').updateValueAndValidity();
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

  onSubmitTransaction() {
    let toId = this.toEmployee
      ? this.transactionForm.value.toEmployee
      : this.transactionForm.value.toOrganization;
    let toEmployeesIds: [] = this.transactionForm.value.toEmployeeList;
    let toOrganaizationsIds: [] = this.transactionForm.value.toOrganizationList;
    let ccEmployeesIds: [] = this.transactionForm.value.ccEmployees;
    let ccOrganaizationsIds: [] = this.transactionForm.value.ccOrganaizations;

    let correspondentId = this.transactionForm.value.correspondent;
    let attachments: TransactionActionAttachmentDTO[] = [];
    let recepients: TransactionActionRecipientsDTO[] = [];
    let recepientAttachments: TransactionActionRecipientAttachmentDTO[] = [];
    let requiredActionId = this.transactionForm.value.requiredAction;

    const encryptedId:string = this.browserService.encryptCommitteId(`${this.committeeId}_${this.browserService.getUserRoleId()}`)
    // Delegation Attachments
    if (this.attachments) {
      attachments = this.attachments.map((item) => {
        return new TransactionActionAttachmentDTO({
          transactionAttachmentId: item.transactionAttachmentId,
        });
      });

      recepientAttachments = this.attachments.map((item) => {
        return new TransactionActionRecipientAttachmentDTO({
          transactionAttachmentId: item.transactionAttachmentId,
        });
      });
    }

    if (!this.toEmployee && !this.isDecision) {
      recepients.push(
        new TransactionActionRecipientsDTO({
          correspondentUserId: correspondentId,
          directedToOrganizationId: toId,
          isCC: false,
          isNoteHidden: false,
          notes: null,
          requiredActionId: requiredActionId,
          transactionActionRecipientAttachmentDTO: recepientAttachments,
        })
      );
    } else if (this.toEmployee && !this.isDecision) {
      recepients.push(
        new TransactionActionRecipientsDTO({
          correspondentUserId: correspondentId,
          directedToUserId: toId,
          isCC: false,
          isNoteHidden: false,
          notes: null,
          requiredActionId: requiredActionId,
          transactionActionRecipientAttachmentDTO: recepientAttachments,
        })
      );
    }

    // Push Employees List if Decision
    if (this.isDecision) {
      toEmployeesIds.forEach((empId) =>
        recepients.push(
          new TransactionActionRecipientsDTO({
            correspondentUserId: correspondentId,
            directedToUserId: empId,
            isCC: false,
            isNoteHidden: false,
            notes: null,
            requiredActionId: 2,
            transactionActionRecipientAttachmentDTO: recepientAttachments,
          })
        )
      );
    }

    // Push Org List if Decision
    if (this.isDecision) {
      toOrganaizationsIds.forEach((orgId) =>
        recepients.push(
          new TransactionActionRecipientsDTO({
            correspondentUserId: correspondentId,
            directedToOrganizationId: orgId,
            isCC: false,
            isNoteHidden: false,
            notes: null,
            requiredActionId: 2,
            transactionActionRecipientAttachmentDTO: recepientAttachments,
          })
        )
      );
    }

    // Push CC Employees
    if (ccEmployeesIds.length > 0) {
      ccEmployeesIds.forEach((empId) =>
        recepients.push(
          new TransactionActionRecipientsDTO({
            correspondentUserId: correspondentId,
            directedToUserId: empId,
            isCC: true,
            isNoteHidden: false,
            notes: null,
            requiredActionId: 2,
            transactionActionRecipientAttachmentDTO: recepientAttachments,
          })
        )
      );
    }

    // Push CC Organizations
    if (ccOrganaizationsIds.length > 0) {
      ccOrganaizationsIds.forEach((orgId) =>
        recepients.push(
          new TransactionActionRecipientsDTO({
            correspondentUserId: correspondentId,
            directedToOrganizationId: orgId,
            isCC: true,
            isNoteHidden: false,
            notes: null,
            requiredActionId: 2,
            transactionActionRecipientAttachmentDTO: recepientAttachments,
          })
        )
      );
    }

    const referralObj = new DelegationDTO({
      committeId: encryptedId,
      actionId: 1,
      isEmployee: false,
      transactionActionAttachmentDTO: attachments,
      transactionActionRecipientsDTO: recepients,
      transactionId: this.transaction.transactionId,
      isSaveCommitteeMinutes: this.transactionForm.value.committeMom ? this.transactionForm.value.committeMom : false
    });

    this.swagger
      .apiTransactionsInsertDelegationPost(referralObj)
      .subscribe((res) => {
        this.modalService.createNotification('success', 'DelegationSuccess');
        this.saving = false;
        this.close();
      });
  }

  referralTo(value: string) {
    this.transactionForm.get('referralTo').patchValue(value);
    this.visible = false;

    if ((value = 'org')) this.transactionForm.patchValue({ toEmployee: null });
    if ((value = 'emp'))
      this.transactionForm.patchValue({ toOrganization: null });
  }

  onSelectFile(event: Event) {
    const selectedFiles = (event.target as HTMLInputElement).files;

    for (let i = 0; i < selectedFiles.length; i++) {
      this.files.push(selectedFiles[`${i}`]);
    }

    this.transactionForm.patchValue({ files: this.files });
    this.fileInput.nativeElement.value = '';
    this.transactionForm.get('files').updateValueAndValidity();
  }

  removeSelectedFile(index) {
    this.files.splice(index, 1);
    this.transactionForm.patchValue({ files: this.files });
    if(this.committeAttachment && this.files.length === 0){
      this.transactionForm.get('files').clearAsyncValidators();
    } else {
      this.transactionForm.get('files').setValidators(Validators.required);
    }
    this.transactionForm.get('files').updateValueAndValidity();
  }

  close() {
    this.transactionForm.reset();
    this.modalService.destroyModal();
  }

  saveTransaction() {
    if (this.transaction.transactionId || !this.transactionForm.valid) return;

    this.saving = true;
    let subject = this.transactionForm.value.subject;
    let explanation = this.transactionForm.value.explanation;
    
    let transActionActionType = this.isDecision ? 2 : 4;
    this.createTransactionService
      .saveTransaction(
        subject,
        explanation,
        this.isSecret,
        transActionActionType
      )
      .subscribe((res) => {
        if (res) {
          this.transaction = res;
          this.transactionForm.get('subject').disable();
          this.transactionForm.get('explanation').disable();
          this.transactionForm.get('isSecret').disable();
          const encryptedId:string = this.browserService.encrypteString(`${this.committeeId}_${this.browserService.getUserRoleId()}`)
          this.createTransactionService
            .uploadTransactionAttachments(
              this.files,
              this.browserService.encrypteString(`${this.transaction.transactionId}_${this.browserService.getUserRoleId()}`)
            )
            .subscribe((attachments) =>
              this.createTransactionService
                .insertTransactionAttachments(
                  <AttachmentSummaryDTO[]>attachments,
                  this.transaction.transactionId
                )
                .subscribe((data) => {
                  this.attachments = data;
                  this.saved = true;
                  // this.modalService.createNotification(
                  //   'success',
                  //   'TransactionSaved'
                  // );
                  this.onSubmitTransaction();
                })
            );
        }
      });
  }

  addAttachments() {
    this.createTransactionService
      .uploadTransactionAttachments(this.files,this.browserService.encrypteString(`${this.transaction.transactionId}_${this.browserService.getUserRoleId()}`))
      .subscribe((attachments) =>
        this.createTransactionService
          .insertTransactionAttachments(
            <AttachmentSummaryDTO[]>attachments,
            this.transaction.transactionId
          )
          .subscribe((data: TransactionAttachmentDTO[]) => {
            this.attachments = data;
          })
      );
  }
}
