import { TranslateService } from '@ngx-translate/core';
import { map } from 'rxjs/operators';
import { Inject, Injectable } from '@angular/core';
import {
  CommiteeTaskDTO,
  CommiteeTaskDTODataSourceResult,
  SwaggerClient,
  UserTaskDTO,
  UpdateTaskLogDTO,
  API_BASE_URL,
  CommitteeTaskAttachmentDTO,
  CommiteetaskMultiMissionDTO,
  CountResultDTO,
  GroupDto,
  GroupUsersDto,
  Group,
  GroupDtoDataSourceResult,
  TaskGroupDto,
  UserDetailsDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BrowserStorageService } from '../shared/_services/browser-storage.service';
import { AuthService } from '../auth/auth.service';
import { LayoutService } from '../shared/_services/layout.service';


export class ExtendedTaskDTO extends CommiteeTaskDTO {
  mainAssignedUserName?: string;
  expand?: boolean;

}
export class ExtendedCountDTO extends CountResultDTO{
  permissionValue?:string
}
export class ExtendedTaskDTODataSourceResult extends CommiteeTaskDTODataSourceResult {
  data?: ExtendedTaskDTO[];
}
export interface TaskFilters {
  typeId?: any;
  classificationId?: number;
  searchtxt?:string;
  body?:any
}
export interface StatsArray{
  index:number;
  value:number;
  name:any;
}
@Injectable({
  providedIn: 'root',
})
export class TasksService {
  baseUrl: string;
  tasksFilters: BehaviorSubject<TaskFilters> = new BehaviorSubject<TaskFilters>(
    null
  );
  toggleTasksFilter: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    null
  );
  filterEnumChanged: BehaviorSubject<number> = new BehaviorSubject<number>(
    null
  );
  fromNotifications:BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null)
  filterWithClick: BehaviorSubject<number> = new BehaviorSubject<number>(null);
  toggleCompeleteTask:BehaviorSubject<any> = new BehaviorSubject<any>(null);
  shareTaskStats:BehaviorSubject<CountResultDTO[]> = new BehaviorSubject<CountResultDTO[]>(null);
  constructor(
    private http: HttpClient,
    private swaggerService: SwaggerClient,
    @Inject(API_BASE_URL) baseUrl: string,
    private translateService: TranslateService,
    private BrowserService:BrowserStorageService,
    private authService:AuthService,
    private layoutService: LayoutService
  ) {
    this.baseUrl = baseUrl;
  }
  getFilteredTasks(
    take: number,
    skip: number,
    committeeId: number,
    filters: any[] = [],
    filteLogic = undefined,
    filterNum: number,
    userId:string,
    orgId:string,
    fromDate?:Date,
    toDate?:Date,
    mainUserId?:number,
    assistantUser?:number,
    validityPeriodFrom?:Date,
    validityPeriodTo?:Date,
    selectedCommitee?:number,
    selectedMeeting?:number
  ) {
    return this.swaggerService.apiCommiteeTasksGetAllwithFiltersGet(
      take,
      skip,
      undefined,
      undefined,
      undefined,
      undefined,
      filteLogic,
      filters,
      false,
      filterNum,
      userId,
      orgId,
      fromDate,
      toDate,
      mainUserId,
      assistantUser,
      validityPeriodFrom,
      validityPeriodTo,
      selectedCommitee,
      selectedMeeting
    ).pipe(map((res) => {
      if(res && res.data.length){
        return {
          ...res,
          data:res.data.map((item) => {
            return {
              ...item,
              expand:false
            } as ExtendedTaskDTO
          }),
        } as ExtendedTaskDTODataSourceResult
      } else {
        return res;
      }
    }))
  }


  getAllUserTasks(userId:string) {
    return this.swaggerService.apiCommiteeTasksGetAllForPrintGet(
      1,
      undefined,
      undefined,
      undefined,
      userId,
      undefined,undefined,undefined,undefined,undefined,undefined,undefined
    );
  }
  getAllMeetingUsers(meetingId:number):Observable<UserDetailsDTO[]>{
    return this.swaggerService.apiMeetingsUsersInMeetingPost(meetingId)
  }
  getOrganizationByOrganizationName(){
    return this.swaggerService.apiCommiteesGetOrgnaztionFromSessionGet()
  }
  saveNewGroup(values): Observable<Group>{
     const group = new GroupDto({
      groupNameAr:values['NameAr'],
      groupNameEn:values['NameEn'],
      groupUsers:values['allUsers'].map((id) => {
        return new GroupUsersDto({userId:id})
      })
     })
    return this.swaggerService.apiGroupInsertGroupPost(group)
  }
  editTaskGroup(values,groupId:number): Observable<GroupDto[]>{
    const group = new GroupDto({
    groupId:groupId,
     groupNameAr:values['NameAr'],
     groupNameEn:values['NameEn'],
     groupUsers:values['allUsers'].map((id) => {
       return new GroupUsersDto({userId:id})
     })
    })
   return this.swaggerService.apiGroupUpdatePut([group])
 }
  getGroupById(id:number): Observable<GroupDto>{
    return this.swaggerService.apiGroupGetByGroupIdGet(id)
  }
  updategroupTask(values,task:CommiteeTaskDTO){
    const taskGroup = new CommiteeTaskDTO({
      taskGroups:values['groups'].map((id) => {
        return new TaskGroupDto({groupId:id})
      }),   
      commiteeTaskId: task.commiteeTaskId,
      title:task.title,
      endDate:task.endDate,
      taskDetails:task.taskDetails,
      mainAssinedUserId:task.mainAssinedUserId,
      assistantUsers:task.assistantUsers,
      taskAttachments:task.taskAttachments,
      isShared:task.isShared,
      multiMission:task.multiMission,
      comiteeTaskCategoryId:task.comiteeTaskCategoryId,
      isEmail:task.isEmail,
      isSMS:task.isSMS,
      isNotification:task.isNotification,
      startDate:task.startDate
    });
    return this.swaggerService.apiCommiteeTasksUpdatePut([taskGroup]);
  }
  insertNewSubTask(taskId:string,CommiteetaskMultiMissionDTO:CommiteetaskMultiMissionDTO):Observable<CommiteetaskMultiMissionDTO>{
    return this.swaggerService.apiCommiteeTasksInsertMultiMissionToTaskPost(taskId,CommiteetaskMultiMissionDTO)
  }
  saveCommitteeTask(
    values,
    startDate:Date,
    endDate: Date,
    committeeId,
    taskId?,
    existingAttachments: CommitteeTaskAttachmentDTO[] = [],
    multiTasks: CommiteetaskMultiMissionDTO[] = [],
    meetingId?:number
  ) {
    const task = new CommiteeTaskDTO({
      title: values['title'],
      startDate:startDate,
      endDate: endDate, // Active status
      taskDetails:values['details'],
      mainAssinedUserId: values['mainAssinedUserId'],
      // assistantUsers: values['assistantUsers'].map((id) => {
      //   return new UserTaskDTO({ userId: id, commiteeTaskId: taskId });
      // }),
      taskGroups:values['groups'].map((id) => {
        return new TaskGroupDto({groupId:id})
      }),
      commiteeIdEncrypted: committeeId,
      taskComments: [],
      taskAttachments: existingAttachments,
      isShared: values['isShared'],
      commiteeTaskId: taskId,
      multiMission: multiTasks,
      comiteeTaskCategoryId: values['category'],
      isEmail: values['emailNotify'],
      isSMS: values['smsNotify'],
      isNotification: values['appNotify'],
      meetingId:meetingId
    });

    if (taskId) {
      return this.swaggerService.apiCommiteeTasksUpdatePut([task]);
    }

    return this.swaggerService.apiCommiteeTasksInsertPost([task]);
  }
  getAllGroups(
    take = 20,
    skip = 0,
    countless = false,
    filters = [],
    userId
  ): Observable<GroupDtoDataSourceResult[]> {
    return this.swaggerService
      .apiGroupGetAllGet(
        take,
        skip,
        undefined,
        undefined,
        undefined,
        undefined,
        'or',
        filters,
        countless,
      )
      .pipe(map((res: GroupDtoDataSourceResult) => res.data));
  }
  postAttachments(files: File[], taskId) {
    const postData = new FormData();
    if (files) files.forEach((file) => postData.append(file.name, file));

    postData.append('commiteeTaskId', taskId);

    return this.http.post(
      `${this.baseUrl}/api/Document/UploadAttachmentToCommitteeTask`,
      postData
    );
  }
  postCommentAttachment(files:File[],commentId){
    const postData = new FormData();
    if(files) files.forEach((file) => postData.append(file.name, file))
    postData.append('commentId',this.BrowserService.encrypteString(commentId));

    return this.http.post(
      `${this.baseUrl}/api/Document/UploadAttachmentToComment`,
      postData
    );
  }
  updateMutiTasksForTask(multiMissionId:string) {
    return this.swaggerService.apiCommiteeTasksChangeStateForMissionPut(multiMissionId);
  }

  getTaskCategories() {
    return this.swaggerService.apiComiteeTaskCategoryGetAllGet(
      10,
      0,
      [],
      undefined,
      undefined,
      undefined,
      undefined,
      [],
      false
    );
  }

  editTaskHistory(committeTaskId: string): Observable<UpdateTaskLogDTO[]> {
    return this.swaggerService.apiUpdateTaskLogGetTaskUpdateslogGet(
      committeTaskId
    );
  }

  getStatistisTasksNumber(orgId:string,userId:any,committeId?: string,validityPeriodFrom?:Date,validityPeriodTo?:Date): Observable<CountResultDTO[]> {
    return this.swaggerService.apiCommiteeTasksGetStatisticsGet(orgId,userId,committeId,validityPeriodFrom,validityPeriodTo);
  }
  getTaskDetails(id:string): Observable<CommiteeTaskDTO>{
    return this.swaggerService.apiCommiteeTasksGetDetailsByIdGet(id);
  }
  exportDocument(requiredTasks,userIdEncrpted,exportType){
    this.layoutService.toggleSpinner(true);
    var accessToken = this.authService.getToken()
    fetch(
      `${this.baseUrl}/api/CommiteeTasks/Export?requiredTasks=${requiredTasks}&userIdEncrpted=${userIdEncrpted}&exportType=${exportType}`,
      {
        method: "POST",
        body: JSON.stringify({}),
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${accessToken}`,
        },
      }
    )
      .then((response) => {
       if(response){
        return response.blob()
       }
      }).then(blob => {
        switch(requiredTasks){
          case 1:
            var downloadURL = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
           link.href = downloadURL;
           link.download = 'كل المهام';
           link.click();
          break;
          case 2:
            var downloadURL = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
           link.href = downloadURL;
           link.download = 'المهام المتاخرة';
           link.click();
          break;
          case 7:
            var downloadURL = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
           link.href = downloadURL;
           link.download = ' تحت الإجراء';
           link.click();
          break;
          case 9:
            var downloadURL = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
           link.href = downloadURL;
           link.download = ' المهام المغلقة ';
           link.click();
           break;
           case 8:
            var downloadURL = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
           link.href = downloadURL;
           link.download = ' تكليف فرعي';
           link.click();
           break;
           case 10:
            var downloadURL = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
           link.href = downloadURL;
           link.download = 'مهام للإطلاع';
           link.click();
           break;
        }
        this.layoutService.toggleSpinner(false);
      })
      .catch((error) => {
        this.layoutService.toggleSpinner(false);
      });
  }
}
