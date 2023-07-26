import { Component, OnInit, OnDestroy } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { SettingControllers } from 'src/app/shared/_enums/AppEnums';
import {
  SwaggerClient,
  LookUpDTO,
  MeetingHeaderAndFooterDTO,
  Meeting_Meeting_HeaderAndFooterDTO,
  Lookup,
} from 'src/app/core/_services/swagger/SwaggerClient.service';
import { TranslateService } from '@ngx-translate/core';
import { DashboardService } from 'src/app/meetings/meetings-dashboard/dashboard.service';
import { NzSelectSizeType } from 'ng-zorro-antd/select';
import { ActivatedRoute, Router } from '@angular/router';
import { DestroyService } from 'src/app/shared/_services/destroy.service';
import { takeUntil } from 'rxjs/operators';
import { Subscription } from 'rxjs/index';
import { NzNotificationService } from 'ng-zorro-antd/notification';
@Component({
  selector: 'app-letter-template',
  templateUrl: './letter-template.component.html',
  styleUrls: ['./letter-template.component.scss'],
  providers: [DestroyService],
})
export class LetterTemplateComponent implements OnInit, OnDestroy {
  editor: any;
  editorConfig = { usePaging: false, height: 500, width: 'auto' ,removePlugins:'font'};
  size: NzSelectSizeType = 'default';
  meetingList: number[] = [];
  letterTemplate: MeetingHeaderAndFooterDTO = new MeetingHeaderAndFooterDTO();
  headerAndFooter: boolean = true;
  MeetingListLookup$: LookUpDTO[] = [];
  routerSubscription: Subscription;
  meetingsList$ = new BehaviorSubject('');
  lookupTypes = SettingControllers;
  isLoading = false;
  letterCreate: boolean = false;
  letterId: any;
  meetingListNames: string[];
  constructor(
    private swagger: SwaggerClient,
    public translate: TranslateService,
    private _dashBoardService: DashboardService,
    private route: ActivatedRoute,
    private router: Router,
    private destroyServ: DestroyService,
    private notificationService: NzNotificationService
  ) {}

  ngOnInit(): void {
    this.routerSubscription = this.route.params
      .pipe(takeUntil(this.destroyServ.subDestroyed))
      .subscribe((r) => {
        if (!r.letterId) {
          this.letterCreate = true;
          this.letterTemplate.meetings = []
          this.letterTemplate = new MeetingHeaderAndFooterDTO();
        } else {
          this.letterId = r.letterId;
          this.swagger
            .apiMeetingHeaderAndFootersGetByIdGet(r.letterId)
            .subscribe((result) => {
              this.meetingList = result.meetings.map(x => x.meetingId)
              this.meetingListNames = result.meetings.map(x => x.title)
              this.letterTemplate = result;
            });
        }
      });
    this.getMeetingList();
  }
  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
  }
  getMeetingList() {
    this.meetingsList$
      .asObservable()
      .pipe(
        debounceTime(500),
        switchMap((text: string) =>
          this._dashBoardService.getMeetingsNameList(
            10,
            0,
            text ? text : undefined
          )
        )
      )
      .subscribe((meeting) => {
        this.MeetingListLookup$ = meeting;
        this.isLoading = false;
        if(this.letterId){
          this.meetingList.map((y,index)=> {
            if (!meeting.some(x=>x.id == y)){
              meeting.push(new LookUpDTO({id:y,name:this.meetingListNames[index]}))
            }
          })
        }
      });
  }
  onSearch(type: string, value: string): void {
    this.isLoading = true;
    switch (type) {
      case SettingControllers.MeetingsList:
        this.meetingsList$.next(value);
        break;
      default:
        break;
    }
  }
  initEditor(editor) {
    if (!this.editor) {
      this.editor = editor;
      this.editor.setData(this.letterTemplate.html);
    }
  }
  insertHeaderAndFooter() {
    this.letterTemplate.html = this.editor.getData();
    let meeting: Meeting_Meeting_HeaderAndFooterDTO[] = [];
    this.meetingList.map((ids) => {
      meeting.push(
        new Meeting_Meeting_HeaderAndFooterDTO({
          meetingId: +ids,
        })
      );
    });
    this.letterTemplate.meetings = meeting;
    this.swagger
      .apiMeetingHeaderAndFootersInsertPost([this.letterTemplate])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('LetterTemplateCreated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/letter-templates']);
        } else {
          this.translate
            .get('letterTemplateCreatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }
  editHeaderAndFooter() {
    this.letterTemplate.html = this.editor.getData();
    let meeting: Meeting_Meeting_HeaderAndFooterDTO[] = [];
    this.meetingList.map((ids) => {
      meeting.push(
        new Meeting_Meeting_HeaderAndFooterDTO({
          meetingId: +ids,
        })
      );
    });
    this.letterTemplate.meetings = meeting;
    this.swagger
      .apiMeetingHeaderAndFootersUpdatePut([this.letterTemplate])
      .subscribe((value) => {
        if (value) {
          this.translate
            .get('LetterTemplateUpdated')
            .subscribe((translateValue) =>
              this.notificationService.success(translateValue, '')
            );
          this.router.navigate(['/settings/letter-templates']);
        } else {
          this.translate
            .get('letterTemplateUpdatedError')
            .subscribe((translateValue) =>
              this.notificationService.error(translateValue, '')
            );
        }
      });
  }
  onChangeMeetingList(changedMeetingList:number[]){
    let tempSelectedNames:LookUpDTO[] = []
    tempSelectedNames = [...this.MeetingListLookup$]

    setTimeout(() => {
      this.MeetingListLookup$.map(x=>{
        if (!x.name){
          x.name = tempSelectedNames.find(y=>y.id == x.id).name;
        }
      })
    },600);
    
    
  }
}
