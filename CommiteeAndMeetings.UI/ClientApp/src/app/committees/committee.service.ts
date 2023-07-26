import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import {
  CommiteeDTO,
  CommiteeDTODataSourceResult,
  CommiteeUserPermissionDTO,
  CommiteeUsersRoleDTO,
  CountResultDTO,
  CurrentStatusDTODataSourceResult,
  PeriodState,
  SwaggerClient,
  ValidityPeriodDTO,
} from '../core/_services/swagger/SwaggerClient.service';
import { map, tap } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { BrowserStorageService } from '../shared/_services/browser-storage.service';

export class ExtendedCommitteeDTO extends CommiteeDTO {
  committeeConfidentiality?: string;
  categoryName?: string;
  relatedDepartment?: string;
  datefrom?: Date;
  dateTo?: Date;
  commiteeTypeName?: string;
  commiteeImage?: string;
  currentStatusName?: string;
  committeeAdminName?: string;
}

export class ExtendedCommiteeDTODataSourceResult extends CommiteeDTODataSourceResult {
  data?: ExtendedCommitteeDTO[];
}
@Injectable({
  providedIn: 'root',
})
export class CommitteeService {
  treeView: boolean = false;
  editedCommittee: Subject<ExtendedCommitteeDTO> =
    new Subject<ExtendedCommitteeDTO>();
  committeeFilters: BehaviorSubject<number> = new BehaviorSubject<number>(null);
  filterFlag: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
  validityPeriod: ValidityPeriodDTO;
  committeePeriodChange$ = new BehaviorSubject(null);
  archiveCurrentPeriod$ = new Subject();
  extendCommittee$ = new Subject<Date>();
  headUnit:any;
  committeName:any;
  committeeUserRoles: CommiteeUsersRoleDTO[] = [];
  // committeeUserRoles: CommiteeUserPermissionDTO[] = [];
  delegatedRole: CommiteeUsersRoleDTO;
  committeeState;
  committeStaus:boolean;
  committees: ExtendedCommitteeDTO[] = [];
  committeId:number;
  headUnitId:any;
  committeMembers:any
  private departmentLinkId:any;
  constructor(
    private translateService: TranslateService,
    private swaggerServce: SwaggerClient,
    private router: Router,
    private bs: BrowserStorageService,
  ) {}

  getCommittees(
    take: number,
    skip: number,
    filters: any[] = [],
    filter_Logic: string
  ): Observable<ExtendedCommiteeDTODataSourceResult> {
    return this.swaggerServce
      .apiCommiteesGetAllGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        filter_Logic,
        filters,
        false
      )
      .pipe(
        map((res) => {
          if (res && res.data.length) {
            let committees = res.data.map((item) => {
              return {
                ...item,
                committeeConfidentiality: item?.isSecrete
                  ? 'SecretCommittee'
                  : 'NotSecretCommittee',
                categoryName:
                  this.translateService.currentLang === 'ar'
                    ? item?.category?.categoryNameAr
                    : item?.category?.categoryNameEn,
                relatedDepartment:
                  this.translateService.currentLang === 'ar'
                    ? item?.departmentLink?.organizationNameAr
                    : item?.departmentLink?.organizationNameEn,
                //  datefrom: item?.validityPeriod[item?.validityPeriod.length - 1].validityPeriodFrom,
                //  dateTo: item?.validityPeriod[item?.validityPeriod.length - 1].validityPeriodTo,
                datefrom: item?.validityPeriod.find(
                  (per) =>
                    [PeriodState._1, PeriodState._2].indexOf(per.periodState) >
                    -1
                )?.validityPeriodFrom,
                dateTo: item?.validityPeriod.find(
                  (per) =>
                    [PeriodState._1, PeriodState._2].indexOf(per.periodState) >
                    -1
                )?.validityPeriodTo,
                commiteeTypeName:
                  this.translateService.currentLang === 'ar'
                    ? item?.commiteeType?.commiteeTypeNameAr
                    : item?.commiteeType?.commiteeTypeNameEn,
                commiteeImage: item?.commiteeType.image,
                currentStatusName:
                  this.translateService.currentLang === 'ar'
                    ? item?.currentStatus?.currentStatusNameAr
                    : item?.currentStatus?.currentStatusNameEn,
                committeeAdminName:
                  this.translateService.currentLang === 'ar'
                    ? item?.currenHeadUnit?.fullNameAr
                    : item?.currenHeadUnit?.fullNameEn,
              } as ExtendedCommitteeDTO;
            });
            return {
              ...res,
              data: committees,
            } as ExtendedCommiteeDTODataSourceResult;
          } else {
            return res;
          }
        })
      );
  }

  fetchCommitteesTreeList() {
    return this.swaggerServce.apiCommiteesGetCommittesTreeGet().pipe(tap(committeesList => this.setCommittees(committeesList)));
  }

  getCommitteesTree() {
    return this.committees;
  }

  setCommittees(committees) {
    this.committees = [...committees];
  }
  saveCommitteeData(values, dateFrom: Date, dateTo: Date) {
    if (!values) {
      return;
    }
    const committee = new CommiteeDTO({
      name: values['name'],
      title: values['title'],
      description: values['description'],
      commiteeTypeId: values['commiteeTypeId'],
      categoryId: values['categoryId'],
      parentCommiteeId: values['parentCommiteeId']
        ? values['parentCommiteeId']
        : undefined,
      departmentLinkId: values['departmentLinkId']
        ? values['departmentLinkId']
        : undefined,
      currentStatusId: 1, // Active status
      enableTransactions: values['enableTransactions'],
      enableDecisions: values['enableDecisions'],
      currenHeadUnitId: values['currenHeadUnitId'],
      isSecrete: values['isSecrete'],
      committeeSecretaryId:values['committeeSecretary'],
      validityPeriod: [
        new ValidityPeriodDTO({
          periodState: 1,
          validityPeriodFrom: dateFrom,
          validityPeriodTo: dateTo,
        }),
      ],
    });
    return this.swaggerServce.apiCommiteesInsertPost([committee]).pipe(
      map((value) =>
        value.map((item) => {
          return {
            ...item,
            committeeConfidentiality: item?.isSecrete
              ? 'SecretCommittee'
              : 'NotSecretCommittee',
            categoryName:
              this.translateService.currentLang === 'ar'
                ? item?.category?.categoryNameAr
                : item?.category?.categoryNameEn,
            relatedDepartment:
              this.translateService.currentLang === 'ar'
                ? item?.departmentLink?.organizationNameAr
                : item?.departmentLink?.organizationNameEn,
            datefrom: item?.validityPeriod.find(
              (per) =>
                [PeriodState._1, PeriodState._2].indexOf(per.periodState) > -1
            )?.validityPeriodFrom,
            dateTo: item?.validityPeriod.find(
              (per) =>
                [PeriodState._1, PeriodState._2].indexOf(per.periodState) > -1
            )?.validityPeriodTo,
            commiteeTypeName:
              this.translateService.currentLang === 'ar'
                ? item?.commiteeType?.commiteeTypeNameAr
                : item?.commiteeType?.commiteeTypeNameEn,
            currentStatusName:
              this.translateService.currentLang === 'ar'
                ? item?.currentStatus?.currentStatusNameAr
                : item?.currentStatus?.currentStatusNameEn,
            committeeAdminName:
              this.translateService.currentLang === 'ar'
                ? item?.currenHeadUnit?.fullNameAr
                : item?.currenHeadUnit?.fullNameEn,
          } as ExtendedCommitteeDTO;
        })
      )
    );
  }

  editCommitteeData(values, committeId?: string) {
    if (!values) {
      return;
    }

    // const encryptedId:string = this.bs.encrypteString(`${committeId} + '_' + ${this.getUserRoleId()}`)
    const committee = new CommiteeDTO({
      commiteeIdEncrpt:committeId,
      name: values['name'],
      title: values['title'],
      description: values['description'],
      commiteeTypeId: values['commiteeTypeId'],
      categoryId: values['categoryId'],
      parentCommiteeId: values['parentCommiteeId']
        ? values['parentCommiteeId']
        : undefined,
      departmentLinkId: values['departmentLinkId']
        ? values['departmentLinkId']
        : undefined,
      currentStatusId: 1, // Active status
      enableTransactions: values['enableTransactions'],
      enableDecisions: values['enableDecisions'],
      currenHeadUnitId: values['currenHeadUnitId'],
      committeeSecretaryId:values['committeeSecretary'],
      isSecrete: values['isSecrete'],
    });
    return this.swaggerServce.apiCommiteesUpdatePut([committee]).pipe(
      map((value) =>
        value.map((item) => {
          return {
            ...item,
            committeeConfidentiality: item?.isSecrete
              ? 'SecretCommittee'
              : 'NotSecretCommittee',
            categoryName:
              this.translateService.currentLang === 'ar'
                ? item?.category?.categoryNameAr
                : item?.category?.categoryNameEn,
            relatedDepartment:
              this.translateService.currentLang === 'ar'
                ? item?.departmentLink?.organizationNameAr
                : item?.departmentLink?.organizationNameEn,
            datefrom: item?.validityPeriod.find(
              (per) =>
                [PeriodState._1, PeriodState._2].indexOf(per.periodState) > -1
            )?.validityPeriodFrom,
            dateTo: item?.validityPeriod.find(
              (per) =>
                [PeriodState._1, PeriodState._2].indexOf(per.periodState) > -1
            )?.validityPeriodTo,
            commiteeTypeName:
              this.translateService.currentLang === 'ar'
                ? item?.commiteeType?.commiteeTypeNameAr
                : item?.commiteeType?.commiteeTypeNameEn,
            currentStatusName:
              this.translateService.currentLang === 'ar'
                ? item?.currentStatus?.currentStatusNameAr
                : item?.currentStatus?.currentStatusNameEn,
            committeeAdminName:
              this.translateService.currentLang === 'ar'
                ? item?.currenHeadUnit?.fullNameAr
                : item?.currenHeadUnit?.fullNameEn,
          } as ExtendedCommitteeDTO;
        })
      )
    );
  }
  getCommitteeDetails(id: string) {
    let encripted = this.bs.decrypteString(id)
    return this.swaggerServce
      .apiCommiteesGetCommitteeDetailsWithValidityPeriodGet(this.bs.encrypteString(encripted))
      .pipe(
        tap((committee: CommiteeDTO) => {
          if (!committee.commiteeId) {
            this.router.navigate(['/committees']);
            return;
          }
          // Get last period
          let validityperiod =
            committee.validityPeriod[committee.validityPeriod.length - 1];
          // Check if Committee Expired
          if (
            validityperiod.validityPeriodTo.getTime() < new Date().getTime() &&
            validityperiod.validityPeriodTo.getTime() !== new Date("0001-01-01T00:00:00").getTime()
          ) {
            validityperiod.periodState = 2;
            const encryptedId:string = this.bs.encrypteString(`${committee.commiteeId}_${this.bs.getUserRoleId()}`)
            this.swaggerServce
              .apiCommiteesArchivePost(encryptedId)
              .subscribe();
          }
          if(validityperiod.validityPeriodFrom.getFullYear() > 1900 && validityperiod.validityPeriodTo.getFullYear() > 1900){
            this.committeePeriodChange$.next(validityperiod);
          }else {
            let validityPeriod = new ValidityPeriodDTO({
              validityPeriodFrom:undefined,
              validityPeriodTo:undefined,
              periodState:validityperiod.periodState
            })
            this.committeePeriodChange$.next(validityPeriod);
          }
          if (!this.validityPeriod) this.setValidityPeriod(validityperiod);

          // Check Committee State
          this.committeeState = committee.currentStatus.currentStatusId;
          this.CommitteHeadUnit = committee.currenHeadUnit
          this.CommitteName = committee.name;
          this.committeStaus = committee.isSecrete
          this.committeId = committee.commiteeId
          this.CommitteHeadUnitId = committee.currenHeadUnitId;
          this.committeMembers = committee.members
          this.departmentLinkId = committee.departmentLinkId
        })
      );
  }
  set CommitteId(value){
    this.committeId = value
  }
  get CommitteId(){
    return this.committeId
  }
  set DepartmentLinkId(value){
    this.departmentLinkId = value
  }
  get DepartmentLinkId(){
    return this.departmentLinkId
  }
  set CommitteHeadUnit(value){
    this.headUnit = value
  }
  get CommitteHeadUnit(){
    return this.headUnit
  }
  set CommitteHeadUnitId(value){
   this.headUnitId = value
  }
  get CommitteHeadUnitId(){
    return this.headUnitId
  }
  set CommitteMembers(value){
    this.committeMembers = value
  }
  get CommitteMembers(){
    return this.committeMembers
  }
  set CommitteName(value){
    this.committeName = value
  }
  get CommitteName(){
    return this.committeName
  }
  set CommitteStatus(value){
    this.committeStaus = value
  }
  get CommitteStatus(){
    return this.committeStaus
  }
  getCommitteeCurrentState(): boolean {
    if (this.committeeState === 1) return true;
    return false;
  }

  getValidityPeriod() {
    return this.validityPeriod;
  }

  setValidityPeriod(validityPeriod) {
    this.validityPeriod = validityPeriod;
  }
  getCommitteStatus(
    take: number,
    skip: number
  ): Observable<CurrentStatusDTODataSourceResult> {
    return this.swaggerServce.apiCurrentStatusGetAllGet(
      take,
      skip,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined
    );
  }

  setCommitteeUserRole(committeeUserRoles: CommiteeUsersRoleDTO[]) {
    this.committeeUserRoles = committeeUserRoles;
    this.delegatedRole = this.committeeUserRoles.find(
      (element) => element.delegated
    );
  }
  disactiveCommitte(id: string): Observable<boolean> {
    return this.swaggerServce.apiCommiteesDisactivePost(id);
  }

  checkPermission(role: string) {
    if (this.delegatedRole) {
      return !!this.delegatedRole?.role.rolePermissions.find((element) => {
        return (
          element?.permission.permissionCode === role &&
          element?.permission.enabled
        );
      });
    } else {
        return !!this.committeeUserRoles[0]?.role.rolePermissions.find(
          (element) => {
            return (
              element?.permission.permissionCode === role &&
              element?.permission.enabled
            );
          }
        );
    }
  }

}
