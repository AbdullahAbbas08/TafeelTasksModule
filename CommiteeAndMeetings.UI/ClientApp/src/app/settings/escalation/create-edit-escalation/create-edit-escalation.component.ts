import { TasksService } from 'src/app/tasks/tasks.service';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { debounceTime, switchMap } from 'rxjs/operators';
import { LookupService } from 'src/app/core/_services/lookup.service';
import {
  ComiteeTaskCategoryDTO,
  CommiteeTaskEscalationDTO,
  LookUpDTO,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import { NgForm } from '@angular/forms';
import { EscalationService } from '../escalation.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-create-edit-escalation',
  templateUrl: './create-edit-escalation.component.html',
  styleUrls: ['./create-edit-escalation.component.scss'],
})
export class CreateEditEscalationComponent implements OnInit {
  isLoading: boolean;
  usersChanged$ = new BehaviorSubject('');
  newAssigendUserChanged$ = new BehaviorSubject('');
  users: LookUpDTO[] = [];
  newAssisgendUsers: LookUpDTO[] = []
  lookupTypes = SettingControllers;

  mainAssinedUserId: number;
  newAssinedUserId: number;
  delayPeriod: number;
  classificationId: number;

  taskCategories: ComiteeTaskCategoryDTO[] = [];

  escalationId: number;

  constructor(
    public lookupService: LookupService,
    private tasksService: TasksService,
    private escalationService: EscalationService,
    private router: Router,
    private activeRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.initUsers();
    this.initnewAssigendUsers()
    this.checkEditMode();
    this.tasksService.getTaskCategories().subscribe((res) => {
      this.taskCategories = res.data;
    });
  }

  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.USERS:
        this.usersChanged$.next(value);
        break;
        case SettingControllers.ASSISTANTUSERS:
          this.newAssigendUserChanged$.next(value);
          break;
      default:
        break;
    }
  }

  initUsers() {
    this.usersChanged$
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
              : undefined,undefined
          );
        })
      )
      .subscribe((data: LookUpDTO[]) => {
        this.users = data;
        this.isLoading = false;
      });
  }
  initnewAssigendUsers() {
    this.newAssigendUserChanged$
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
        this.newAssisgendUsers = data;
        this.isLoading = false;
      });
  }
  checkEditMode() {
    this.escalationId = +this.activeRoute.snapshot.paramMap.get('escalationId');

    if (this.escalationId) {
      this.getEscalationById();
    }
  }

  submitForm(form: NgForm) {
    if (form.invalid) return;
    this.escalationId ? this.editEscalation() : this.addEscalation();
  }

  addEscalation() {
    this.escalationService
      .addEscalation(
        new CommiteeTaskEscalationDTO({
          mainAssinedUserId: this.mainAssinedUserId,
          delayPeriod: this.delayPeriod,
          newMainAssinedUserId: this.newAssinedUserId,
          comiteeTaskCategoryId: this.classificationId,
        })
      )
      .subscribe(() => {
        this.router.navigateByUrl('/settings/escalation');
      });
  }

  editEscalation() {
    this.escalationService
      .editEscalation(
        new CommiteeTaskEscalationDTO({
          mainAssinedUserId: this.mainAssinedUserId,
          delayPeriod: this.delayPeriod,
          newMainAssinedUserId: this.newAssinedUserId,
          comiteeTaskCategoryId: this.classificationId,
          commiteeTaskEscalationIndex: this.escalationId
        })
      )
      .subscribe(() => {
        this.router.navigateByUrl('/settings/escalation');
      });
  }

  getEscalationById() {
    this.escalationService
      .getEscalation(`${this.escalationId}`)
      .subscribe((escalation) => {
        this.mainAssinedUserId = escalation.mainAssinedUserId;
        this.newAssinedUserId = escalation.newMainAssinedUserId;
        this.delayPeriod = escalation.delayPeriod;
        this.classificationId = escalation.comiteeTaskCategoryId;
        if(!this.users.some((x) => x.id == escalation.mainAssinedUserId)){
          this.users.push(new LookUpDTO({id:escalation.mainAssinedUserId,name:escalation.mainAssinedUserFullNameAr}))
        }
        if(!this.newAssisgendUsers.some((x) => x.id == escalation.newMainAssinedUserId)){
          this.newAssisgendUsers.push(new LookUpDTO({id:escalation.newMainAssinedUserId,name:escalation.newMainAssinedUserFullNameAr}))
        }
      });

  }
}
