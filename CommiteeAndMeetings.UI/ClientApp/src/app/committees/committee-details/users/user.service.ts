import { Injectable } from '@angular/core';
import {LookUpDTO,CommiteeMemberDTODataSourceResult, SwaggerClient, CommiteeMemberDTO, DataSourceRequest, CommiteeMemberDataSourceResult, CommiteeUsersRoleDTO} from '../../../core/_services/swagger/SwaggerClient.service'
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';


export class ExtentedCommiteeMemberDTO extends  CommiteeMemberDTO {
  mainUserName?: string;
}
export class ExtendedCommiteeMemberDTODataSourceResult extends CommiteeMemberDTODataSourceResult {
   data?: ExtentedCommiteeMemberDTO[];
}
@Injectable({
  providedIn: 'root'
})


export class UserService {

  userMemberId:BehaviorSubject<any> = new BehaviorSubject<any>(null);
  userDelegate:BehaviorSubject<CommiteeUsersRoleDTO> = new BehaviorSubject<CommiteeUsersRoleDTO>(null);
  constructor(private swaggerServce: SwaggerClient,private translateService: TranslateService) { }


   getExternalUser(take:number,skip:number,searchName:string):Observable<LookUpDTO[]>{
     return this.swaggerServce.apiCommiteeUsersGetExternalUsersGet(take,skip,searchName);
   }
   getInternalUser(take:number,skip:number,searchName:string):Observable<LookUpDTO[]>{
     return this.swaggerServce.apiCommiteeUsersGetInternalUsersGet(take,skip,searchName);
   }
   getUserRole(take:number,skip:number):Observable<LookUpDTO[]> {
     return this.swaggerServce.apiCommiteeUsersGetRolesGet(take,skip);
   }
   getCommitteUsers(take: number,skip: number,committeeId: any,filters: any[] = [] ,filter_Logic:string,searchText:string):Observable<CommiteeMemberDTODataSourceResult>{
     return this.swaggerServce.apiCommiteeUsersGetAllWithCountsGet(take,skip,undefined,'CommiteeId','eq',`${committeeId}`,filter_Logic,filters,false,searchText
      ).pipe(
        map((result) => {
        if(result && result.data.length){
          return {
          ...result,
           data:result.data.map((item) => {
             return {
             ...item,
             mainUserName: this.translateService.currentLang === 'ar' ?
              item?.user?.fullNameAr : item?.user?.fullNameEn
             } as ExtentedCommiteeMemberDTO
           })
          } as ExtendedCommiteeMemberDTODataSourceResult;
        }else {
          return result;
       }
       }))
   }

   //  Replace User/Member
   createUsers(body:CommiteeMemberDTO[]){
     return this.swaggerServce.apiCommiteeUsersInsertPost(body);
   }
   getActiveUser(committeId:string,body:DataSourceRequest):Observable<CommiteeMemberDataSourceResult>{
      return this.swaggerServce.apiCommiteeUsersGetActiveUsersByCommitteeIdGet(committeId,body);
   }
   ActiveDisactiveUser(active:boolean,memberState:number,body:string):Observable<string>{
     return this.swaggerServce.apiCommiteeUsersActiveDisactiveMemberPost(active,memberState,body);
   }
   delegateUser(userId:number,committeId:string,committeMemberId:number,note:string,toDate:Date):Observable<CommiteeUsersRoleDTO>{
     return this.swaggerServce.apiCommiteeUsersDelegateUserGet(userId,committeId,committeMemberId,note,toDate);
   }
   disableDelegateUser(userRoleID: number):Observable<boolean>{
     return this.swaggerServce.apiCommiteeUsersDisableDelegateUserGet(userRoleID);
   }
   getAlldelegatedUsers(committeid:string):Observable<LookUpDTO[]>{
     return this.swaggerServce.apiCommiteeUsersGetDeleatedUsersGet(committeid)
   }
}
