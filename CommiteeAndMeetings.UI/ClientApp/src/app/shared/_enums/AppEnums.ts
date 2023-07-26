export enum HierarchyRadioButtons {
  All = 1,
  Internal,
  External,
}

export enum GerogorianMonthsAr {
  'يناير' = 1,
  'فبراير',
  'مارس',
  'ابريل',
  'مايو',
  'يونيو',
  'يوليو',
  'اغسطس',
  'سبتمبر',
  'اكتوبر',
  'نوفمبر',
  'ديسمبر',
}

export enum GerogorianMonthsEn {
  'January' = 1,
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
}

export enum hijriMonthsAr {
  'محرّم' = 1,
  'صفر',
  'ربيع الأول',
  'ربيع الآخر',
  'جمادى الأول',
  'جمادى الآخر',
  'رجب',
  'شعبان',
  'رمضان',
  'شوّال',
  'ذو القعدة',
  'ذو الحجة',
}

export enum hijriMonthsEn {
  'Muharram' = 1,
  'Safar',
  'Rabi I',
  'Rabi II',
  'Jumada I',
  'Jumada II',
  'Rajab',
  'Shaaban',
  'Ramadan',
  'Shawwal',
  'Dhu al-Qidah',
  'Dhu al-Hijjah',
}

export enum SystemCalendarAndLocal {
  CALENDAR = 1,
  LOCALIZATION,
}

export enum SystemUsers {
  ADMIN = 1,
}

export enum SettingControllers {
  PERMISSION = 'CommiteePermissions',
  LOCALIZATION = 'CommiteeLocalizations',
  ROLE = 'CommiteeRole',
  TYPE = 'CommiteeTypes',
  STATUS = 'CurrentStatus',
  CATEGORY = 'Categories',
  PARENTCOMMITTEE = 'ParentCommittee',
  ORGANIZATION = 'Organization',
  LETTERTEMPLATES = 'MeetingHeaderAndFooters',
  USERS = 'Users',
  COMMITTEESECRETARY ='CommitteeSecretary',
  ASSISTANTUSERS='AssistantUsers',
  GROUPS='Group',
  REQUIRED_ACTIONS = 'RequiredActions',
  CORRESPONDENT = 'Correspondent',
  ExternalUsers = 'ExternalUsers',
  InternalUsers = 'InternalUsers',
  PROJECTS = 'Projects',
  MeetingsList = 'MeetingList',
  COMMITTEE = 'Committee',
  TaskClassification ='ComiteeTaskCategory',
  Escalation ='CommiteeTaskEscalation',
  ALLUSERS ='AllUsers',
  SYSTEMORG ='SystemOrg',
  ALLORGANIZATION='Allorganization'
}

export enum CommitteesFilterType {
  'Active' = 1,
  'UnActive',
  'Archived',
  'Cancelled',
}
export enum CommitteeActions {
  'CreateNewCommittee' = 1,
  'EditCommittee',
  'CancelCommittee',
  'ArchiveCommittee',
  'CreateAttachment',
  'EditAttachment',
  'RemoveAttachment',
  'CreateTask',
  'EditTask',
  'RemoveTask',
  'CreateVote',
  'EditVote',
  'RemoveVote',
  'CreateTransaction',
  'EditTransaction',
  'DelegateUser',
  'ExtendCommittee',
  'changeLang',
  'deleteModel',
  'CreateNewUser',
  'EditCommitteUserPermissions',
  'FastDelegation'
}

export enum MeetingActions {
  'CreateTopicVoting' = 101,
  'TopicVoting',
  'CreateMeetingVoting',
  'CreateNewMeeting',
}

export enum CommitteeDataTypes {
  'Attachment' = 'Attachment',
  'Vote' = 'Vote',
  'Task' = 'Task',
  'Transaction' = 'Transaction',
}

export enum TopicTimingState {
  'NotStarted' = 1,
  'Running',
  'Warning',
  'Alarm',
  'Completed',
}
export enum TasksFilterEnum {
  all = '1',
  late = '2',
  closedlate = '3',
  sendlate= '4',
  sent = '5',
  closed= '6',
  Underprocedure= '7',
  // ClosedTasks='8',
  // AssistantUserTasks = '9'
}