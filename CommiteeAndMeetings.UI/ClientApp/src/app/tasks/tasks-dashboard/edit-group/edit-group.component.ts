import { Component, OnInit,  OnDestroy } from '@angular/core';
import { CommiteeTaskDTO, CommiteetaskMultiMissionDTO, GroupDto, LookUpDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';
import {
    FormArray,
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
    
  } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { TasksService } from '../../tasks.service';
import { AuthService } from 'src/app/auth/auth.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { SharedModalService } from 'src/app/core/_services/modal.service';
import { StoreService } from 'src/app/shared/_services/store.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.scss']
})
export class EditGroup implements OnInit,OnDestroy {
  task: CommiteeTaskDTO;
  editgroup: FormGroup;
  currentLang: string;
  isLoading = false;
  groupsChanged$ = new BehaviorSubject('');
  groupList: GroupDto[] = [];
  saving:boolean = false;
  lookupTypes = SettingControllers;
  subscription: Subscription;
  constructor( public storeService: StoreService,public modalService: SharedModalService,private authService:AuthService,private taskService: TasksService,public formBuilder: FormBuilder,public translateService: TranslateService){}

  ngOnInit(): void {
    if(this.task){
      this.currentLang = this.translateService.currentLang;
      this.initTaskForm();
      this.editGroupTask();
      this.initGroups();
      if(this.task.taskGroups.length > 0){
        this.task.taskGroups.map((val) => {
         this.groupList.push( new GroupDto({
           groupId:val.groupId,
           groupNameAr:val.group.groupNameAr,
           groupNameEn:val.group.groupNameEn
         }))
        })
     }
    }
  }
  ngOnDestroy() {
    if (this.subscription) this.subscription.unsubscribe();
  }
  initTaskForm() {
    this.editgroup = this.formBuilder.group({
      groups: [[], []],
    });
  }
  editGroupTask(){
      this.editgroup.patchValue({groups:this.task.taskGroups.map((group) => group.groupId)})
  }
  isNotGroupSelected(value): boolean {
    return this.editgroup.controls['groups'].value.indexOf(value) === -1
  }
  initGroups() {
    this.groupsChanged$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) => {
          return this.taskService.getAllGroups(
            20,
            0,
            false,
            text
              ? [{ field: 'groupNameAr', operator: 'contains', value: text},
              { field: 'groupNameEn', operator: 'contains', value: text }]
              : undefined,
              this.authService.getUser().userId
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.groupList = data;
        this.isLoading = false;
      });
  }
  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case  SettingControllers.GROUPS:
        this.groupsChanged$.next(value);
        break;
      default:
        break;
    }
  }
  submitGroupData(){
    this.saving = true;
      this.taskService.updategroupTask(this.editgroup.value,this.task).subscribe((res) => {
        if(res && res.length){
          this.storeService.refresh$.next(res[0]);
          this.saving = false;
          this.close();
        }
      })
  }
  close() {
    this.editgroup.reset();
    this.modalService.destroyModal();
  }
}
