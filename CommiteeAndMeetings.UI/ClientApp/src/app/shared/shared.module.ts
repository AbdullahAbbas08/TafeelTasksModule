import { AttachmentItemFilesComponent } from './_components/attachment-item-files/attachment-item-files.component';
import { CommentItemComponent } from './_components/comment-item/comment-item.component';
import { AddCommentComponent } from './_components/add-comment/add-comment.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng2PanZoomModule } from "ng2-panzoom";
import { NZZorroWrapperModule } from './_modules/nz-zorro-wrapper.module';
import { TranslateModule } from '@ngx-translate/core';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import {
  PerfectScrollbarConfigInterface,
  PerfectScrollbarModule,
  PERFECT_SCROLLBAR_CONFIG,
} from 'ngx-perfect-scrollbar';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { MaxLengthPipe } from './_pipes/max-text-length.pipe';
import { HasAuthUserViewPermissionDirective } from './_directives/has-auth-user-view-permission.directive';
import { DateInputValidationDirective } from './_directives/date-input-validation.directive';
import { NgbdDatepickerIslamicumalqura } from './_components/datepicker/datepicker-islamicumalqura';
import { CustomDatePipe } from './_pipes/custom-date.pipe';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NgxEchartsModule } from 'ngx-echarts';
import { RouterOutletComponent } from './_components/router-outlet.component';
import { SettingsBusinessListComponent } from './_components/setting-business-list/setting-business-list.component';
import { HttpClientModule } from '@angular/common/http';
import { BothCalendarsComponent } from './_components/datepicker/both-calendars/both-calendars.component';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { DropdownDirective } from './_directives/dropdown.directive';
import { NgxHijriGregorianDatepickerModule } from 'ngx-hijri-gregorian-datepicker';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzCommentModule } from 'ng-zorro-antd/comment';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzTableModule } from 'ng-zorro-antd/table';
import { DeleteModelComponent } from './_components/setting-business-list/delete-model/delete-model.component';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzTimePickerModule } from 'ng-zorro-antd/time-picker';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { CkEditorComponent } from './_components/ck-editor/ck-editor.component';
import { NzTimelineModule } from 'ng-zorro-antd/timeline';
import { CountdownModule } from 'ngx-countdown';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { CreateVotingComponent } from './_components/create-voting/create-voting.component';
import { VoteUsersComponent } from './_components/vote-users/vote-users.component';
import { VoteUsersListComponent } from './_components/vote-users/vote-users-list/vote-users-list.component';
import { DynamicImagesComponent } from './_components/dynamic-images/dynamic-images.component';
import { DownloadComponent } from './_components/download/download.component';
import { CdTimerModule } from 'angular-cd-timer';
import { NgxTimerModule } from 'ngx-timer';
import { SanitizeHtmlPipe } from './_pipes/sanitize-html.pipe';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { NzSliderModule } from 'ng-zorro-antd/slider';
import { CreateMeetingComponent } from './_components/create-meeting/create-meeting.component';
import { RichTextEditorModule } from '@syncfusion/ej2-angular-richtexteditor';
import { ScannerComponent } from './_components/scanner/scanner.component';
import { CalenderComponent } from './_components/system-calender/calender.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { CommentsComponent } from '../committees/committee-details/comments/comments.component';
import { GlobalControlsComponent } from './_components/global-controls/global-controls.component';
import { ConfirmArchivingComponent } from './_components/global-controls/confirm-archiving/confirm-archiving.component';
import { ExtendCommitteeComponent } from './_components/global-controls/extend-committee/extend-committee.component';
import { PrintTaskComponent } from './_components/global-controls/print-task/print-task.component';
import { TasksChartComponent } from './_components/tasks-chart/tasks-chart.component';
import { EmailDirective } from './_directives/email.directive';
import { MinLengthDirective } from './_directives/min-length.directive';
import { NumberDirective } from './_directives/number.directive';
import { PhoneDirective } from './_directives/phone.directive';
import { PasswordCriteriaDirective } from './_directives/password-criteria.directive';
import { LanguageDirective } from './_directives/language.directive';
import { HijriDatePipe } from './_pipes/hijri-date.pipe';
import { CommittesMinutesComponent } from './_components/global-controls/committes-minutes/committes-minutes.component';
import { CreateTaskComponent } from '../tasks/tasks-dashboard/create-task/create-task.component';
import { FastDelegateModalComponent } from './_components/fast-delegate-modal/fast-delegate-modal.component';
import { CloseMeetingModalComponent } from './_components/close-meeting-modal/close-meeting-modal.component';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NgxGaugeModule } from 'ngx-gauge';
export const modules = [
  CommonModule,
  NZZorroWrapperModule,
  RouterModule,
  HttpClientModule,
  PerfectScrollbarModule,
  ReactiveFormsModule,
  FormsModule,
  InfiniteScrollModule,
  NgbModule,
  NgxChartsModule,
  TranslateModule,
  NzCheckboxModule,
  NzUploadModule,
  NzRadioModule,
  NzSpinModule,
  NgxHijriGregorianDatepickerModule,
  NzTagModule,
  NgxSliderModule,
  NzCommentModule,
  NzNotificationModule,
  NzTableModule,
  NzMessageModule,
  NzTimePickerModule,
  NzInputNumberModule,
  NzTimelineModule,
  CountdownModule,
  NzCollapseModule,
  CdTimerModule,
  NgxTimerModule,
  NzSliderModule,
  RichTextEditorModule,
  Ng2PanZoomModule,
  NzPaginationModule,
  NgxGaugeModule
];

const directivesPipesDeclaration = [
  HasAuthUserViewPermissionDirective,
  MaxLengthPipe,
  DateInputValidationDirective,
  CustomDatePipe,
  RouterOutletComponent,
  SettingsBusinessListComponent,
  BothCalendarsComponent,
  NgbdDatepickerIslamicumalqura,
  SanitizeHtmlPipe,
  HijriDatePipe
];

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
};

@NgModule({
  declarations: [
    ...directivesPipesDeclaration,
    DropdownDirective,
    DeleteModelComponent,
    CkEditorComponent,
    AddCommentComponent,
    CommentItemComponent,
    CreateVotingComponent,
    VoteUsersComponent,
    VoteUsersListComponent,
    AttachmentItemFilesComponent,
    DynamicImagesComponent,
    DownloadComponent,
    SanitizeHtmlPipe,
    CreateMeetingComponent,
    ScannerComponent,
    CalenderComponent,
    TasksChartComponent,
    CommentsComponent,
    GlobalControlsComponent,
    PrintTaskComponent,
    ExtendCommitteeComponent,
    ConfirmArchivingComponent,
    EmailDirective,
    MinLengthDirective,
    NumberDirective,
    PhoneDirective,
    PasswordCriteriaDirective,
    LanguageDirective,
    CommittesMinutesComponent,
    CreateTaskComponent,
    FastDelegateModalComponent,
    CloseMeetingModalComponent
  ],
  imports: [
    NgxEchartsModule.forRoot({ echarts: () => import('echarts') }),
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    ...modules,
  ],
  exports: [
    ...modules,
    NgxEchartsModule,
    ...directivesPipesDeclaration,
    CkEditorComponent,
    AddCommentComponent,
    CommentItemComponent,
    CreateVotingComponent,
    VoteUsersComponent,
    VoteUsersListComponent,
    AttachmentItemFilesComponent,
    DynamicImagesComponent,
    DownloadComponent,
    CreateMeetingComponent,
    ScannerComponent,
    CalenderComponent,
    TasksChartComponent,
    CommentsComponent,
    GlobalControlsComponent,
    PrintTaskComponent,
    ExtendCommitteeComponent,
    ConfirmArchivingComponent,
    EmailDirective,
    MinLengthDirective,
    NumberDirective,
    PhoneDirective,
    PasswordCriteriaDirective,
    LanguageDirective,
    FastDelegateModalComponent,
    CloseMeetingModalComponent
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG,
    },
  ],
})
export class SharedModule {}
