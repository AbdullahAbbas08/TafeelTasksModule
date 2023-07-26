import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { GroupDto, LookUpDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { BehaviorSubject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { LookupService } from 'src/app/core/_services/lookup.service';
import { TasksService } from '../../tasks.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { takeUntil } from 'rxjs/operators';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
  providers: [DestroyService]
})
export class GroupComponent implements OnInit {
  taskgroupForm: FormGroup;
  currentLang: string;
  lookupTypes = SettingControllers;
  AllUsers:LookUpDTO[] =[];
  AllUsersChanged$ = new BehaviorSubject('');
  isLoading:Boolean = false;
  groupId:any;
  groupCreate:boolean = false;
  group:GroupDto;
  constructor(private destroyServ: DestroyService,private route: ActivatedRoute,private router: Router,private notificationService: NzNotificationService,public translateService: TranslateService,public lookupService: LookupService,public formBuilder: FormBuilder, private taskService: TasksService) {}
  ngOnInit(): void {
    this.route.params
    .pipe(takeUntil(this.destroyServ.subDestroyed))
    .subscribe((r) => {
      if (!r.GroupId) {
        this.groupCreate = true;
      } else {
        this.groupId = r.GroupId;
        this.getGroupById();
      }
    });
    this.currentLang = this.translateService.currentLang;
    this.initAllUserUsers();
    this.initTaskGroupForm()
    
  }
  getGroupById(){
    this.taskService.getGroupById(this.groupId).subscribe((res) => {
     if(res){
      this.group = res;
      this.editTaskGroupForm()
     }
    })
  }
  initTaskGroupForm() {
    this.taskgroupForm = this.formBuilder.group({
      NameAr: [null, [Validators.required,Validators.pattern('^[\u0621-\u064A\u0660-\u0669 ]+$')]],
      NameEn: [null, [Validators.required,Validators.pattern('^[a-zA-Z \-\']+')]],
      allUsers: [[], [Validators.required]],

    });
  }
  editTaskGroupForm(){
    this.group.groupUsers.map((y,index) => {
      if(!this.AllUsers.some(x => x.id == y.userId)){
        this.AllUsers.push(new LookUpDTO({id:y.userId,name:y.userDetailsDTO.fullNameAr}))
      }
    })
   
    this.taskgroupForm.patchValue({
      NameAr:this.group.groupNameAr,
      NameEn:this.group.groupNameEn,
      allUsers:this.group.groupUsers.map((user) => user.userId)
    })
  }
  get m(){
    return this.taskgroupForm.controls;
  }
  initAllUserUsers() {
    this.AllUsersChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.lookupService.getUsersLookup(
            20,
            0,
            false,
            text
              ? [{ field: 'name', operator: 'contains', value: text }]
              : undefined,
              undefined
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.AllUsers = data;
        this.isLoading = false;
      });
  }
  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.AllUsersChanged$.next(value);
        break;
      default:
        break;
    }
  }
  editGroup(){
    if (this.taskgroupForm && !this.taskgroupForm.valid) return;
    this.taskService.editTaskGroup(this.taskgroupForm.value,this.groupId).subscribe((res) => {
      if(res){
        this.translateService
        .get('Groupupdated')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
      this.router.navigate(['/tasks/taskgroup']);
      }else {
        this.translateService
        .get('cannotUpdateGroup')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
      }
    })
  }
  submitNewGroup(){
    if (this.taskgroupForm && !this.taskgroupForm.valid) return;
    this.taskService.saveNewGroup(this.taskgroupForm.value).subscribe((res) => {
      if(res){
        this.translateService
        .get('GroupCreated')
        .subscribe((translateValue) =>
          this.notificationService.success(translateValue, '')
        );
      this.router.navigate(['/tasks/taskgroup']);
      }else {
        this.translateService
        .get('cannotCreateGroup')
        .subscribe((translateValue) =>
          this.notificationService.error(translateValue, '')
        );
      }
    })
  }
}