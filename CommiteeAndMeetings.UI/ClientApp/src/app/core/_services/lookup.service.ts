import { Injectable } from '@angular/core';
import {
  CategoryDTO,
  CommiteeTypeDTO,
  CurrentStatusDTO,
  OrganizationDetailsDTO,
  SwaggerClient,
  LookUpDTO,
  LookUpDTODataSourceResult,
} from './swagger/SwaggerClient.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

export class ExtendedCategoryDTO extends CategoryDTO {
  categoryName?: string;
}

export class ExtendedCurrentStatusDTO extends CurrentStatusDTO {
  currentStatusName?: string;
}

export class ExtendedCommiteeTypeDTO extends CommiteeTypeDTO {
  commiteeTypeName?: string;
}

@Injectable({
  providedIn: 'root',
})
export class LookupService {
  constructor(
    private swaggerService: SwaggerClient,
    private translateService: TranslateService
  ) {}

  getLookup(lookup, args?, searchText?, filters?: any[], logic?, take?) {
    return this.swaggerService.apiLookupGet(
      lookup,
      searchText,
      take,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      logic,
      filters,
      undefined,
      args ? JSON.stringify(args) : undefined
    );
  }

  getLookupProjects(take: number, skip: number, searchText: string) {
    return this.swaggerService.apiProjectsGetProjectsLookupGet(
      take,
      skip,
      searchText
    );
  }

  getOrganizations(
    take = 20,
    skip = 0,
    countless = false,
    filters = []
  ): Observable<OrganizationDetailsDTO[]> {
    return this.swaggerService
      .apiOrganizationsGetAllGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless
      )
      .pipe(
        map((res) =>
          res.data.map((item) => {
            return {
              ...item,
              organizationName:
                this.translateService.currentLang === 'ar'
                  ? item?.organizationNameAr
                  : item?.organizationNameEn,
            } as OrganizationDetailsDTO;
          })
        )
      );
  }

  getCategories(
    take = 20,
    skip = 0,
    countless = false,
    filters = []
  ): Observable<ExtendedCategoryDTO[]> {
    return this.swaggerService
      .apiCategoriesGetAllGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless
      )
      .pipe(
        map((res) =>
          res.data.map((item) => {
            return {
              ...item,
              categoryName:
                this.translateService.currentLang === 'ar'
                  ? item?.categoryNameAr
                  : item?.categoryNameEn,
            } as ExtendedCategoryDTO;
          })
        )
      );
  }

  getStatuses(
    take = 20,
    skip = 0,
    countless = false,
    filters = []
  ): Observable<ExtendedCurrentStatusDTO[]> {
    return this.swaggerService
      .apiCurrentStatusGetAllGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless
      )
      .pipe(
        map((res) =>
          res.data.map((item) => {
            return {
              ...item,
              currentStatusName:
                this.translateService.currentLang === 'ar'
                  ? item.currentStatusNameAr
                  : item?.currentStatusNameEn,
            } as ExtendedCurrentStatusDTO;
          })
        )
      );
  }

  getTypes(
    take = 20,
    skip = 0,
    countless = false,
    filters = []
  ): Observable<CommiteeTypeDTO[]> {
    return this.swaggerService
      .apiCommiteeTypesGetAllGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless
      )
      .pipe(
        map((res) =>
          res.data.map((item) => {
            return {
              ...item,
              commiteeTypeName:
                this.translateService.currentLang === 'ar'
                  ? item.commiteeTypeNameAr
                  : item.commiteeTypeNameEn,
            } as ExtendedCommiteeTypeDTO;
          })
        )
      );
  }

  getParentCommittees(
    take = 20,
    skip = 0,
    countless = false,
    filters = [],
    id: string
  ): Observable<LookUpDTO[]> {
    return this.swaggerService
      .apiCommiteesGetCommitteeLookupGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless,
        id
      )
      .pipe(map((res: LookUpDTODataSourceResult) => res.data));
  }

  getUsersLookup(
    take = 20,
    skip = 0,
    countless = false,
    filters = [],
    orgId:number
  ): Observable<LookUpDTO[]> {
    return this.swaggerService
      .apiCommiteesGetCommitteeHeadUnitLookupGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless,
        orgId
      )
      .pipe(map((res: LookUpDTODataSourceResult) => res.data));
  }
  getUsersWithOrgLookup(
    take = 20,
    skip = 0,
    countless = false,
    filters = [],
    orgId:number
  ): Observable<LookUpDTO[]> {
    return this.swaggerService
      .apiCommiteesGetCommitteeHeadUnitLookupUserAndOrganizationGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless,
        orgId
      )
      .pipe(map((res: LookUpDTODataSourceResult) => res.data));
  }
  getOrgsLookup(
    take = 20,
    skip = 0,
    countless = false,
    filters = [],
    fromAttendee:boolean
  ): Observable<LookUpDTO[]> {
    return this.swaggerService
      .apiCommiteesGetOrganizationLookupGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless,
        fromAttendee
      )
      .pipe(map((res: LookUpDTODataSourceResult) => res.data));
  }
  getMembersLookup(
    take = 10,
    skip = 0,
    committeeId,
    searchName,
    filters = []
  ): Observable<LookUpDTO[]> {
    return this.swaggerService
      .apiCommiteeUsersGetAllWithCountsGet(
        take,
        skip,
        undefined,
        'CommiteeId',
        'eq',
        `${committeeId}`,
        'or',
        filters,
        false,
        searchName
      )
      .pipe(map((res) => res.data));
  }
}
