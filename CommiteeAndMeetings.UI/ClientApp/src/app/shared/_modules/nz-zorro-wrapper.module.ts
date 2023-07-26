import { NgModule } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { NzCommentModule } from 'ng-zorro-antd/comment';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSwitchModule } from 'ng-zorro-antd/switch';

import { IconDefinition } from '@ant-design/icons-angular';
import {
  UserOutline,
  BellOutline,
  SettingOutline,
  PlusOutline,
  SearchOutline,
  FilterOutline,
  SmallDashOutline,
  EllipsisOutline,
  LinkOutline,
  LockOutline,
  UploadOutline,
  ClockCircleOutline,
  CheckOutline,
  MenuFoldOutline,
  MenuUnfoldOutline,
  ContainerOutline,
  LineChartOutline,
  UsergroupAddOutline,
  InboxOutline,
  SolutionOutline,
  PaperClipOutline,
  FormOutline,
  CloseOutline,
  QuestionOutline,
  CommentOutline,
  CalendarOutline,
  DotChartOutline,
  EditOutline,
  PlusCircleOutline,
  DeleteOutline,
  CheckCircleOutline,
  CloseCircleFill,
  LoadingOutline
} from '@ant-design/icons-angular/icons';

const icons: IconDefinition[] = [
  UserOutline,
  BellOutline,
  SettingOutline,
  PlusOutline,
  SearchOutline,
  FilterOutline,
  SmallDashOutline,
  EllipsisOutline,
  LinkOutline,
  LockOutline,
  UploadOutline,
  ClockCircleOutline,
  CheckOutline,
  MenuFoldOutline,
  MenuUnfoldOutline,
  ContainerOutline,
  LineChartOutline,
  UsergroupAddOutline,
  InboxOutline,
  SolutionOutline,
  PaperClipOutline,
  FormOutline,
  CloseOutline,
  QuestionOutline,
  CommentOutline,
  CalendarOutline,
  DotChartOutline,
  EditOutline,
  PlusCircleOutline,
  DeleteOutline,
  CheckCircleOutline,
  CloseCircleFill,
  LoadingOutline
];

export const modulesList = [
  NzButtonModule,
  NzAvatarModule,
  NzImageModule,
  NzModalModule,
  NzNotificationModule,
  NzToolTipModule,
  NzPopoverModule,
  NzBadgeModule,
  NzEmptyModule,
  NzDividerModule,
  NzInputModule,
  NzMenuModule,
  NzDropDownModule,
  NzDrawerModule,
  NzMessageModule,
  NzSelectModule,
  NzListModule,
  NzProgressModule,
  NzCommentModule,
  NzDescriptionsModule,
  NzTabsModule,
  NzTableModule,
  NzFormModule,
  NzSwitchModule
];

@NgModule({
  imports: [...modulesList, NzIconModule.forChild(icons)],
  exports: [...modulesList, NzIconModule],
  providers: [],
})
export class NZZorroWrapperModule {}
