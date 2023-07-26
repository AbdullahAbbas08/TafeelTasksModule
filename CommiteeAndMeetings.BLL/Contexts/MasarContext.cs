using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.Views;
using CommiteeDatabase.Models.Domains;
using DbContexts.AuditDbContext;
using IHelperServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models.ProjectionModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CommiteeAndMeetings.BLL.Contexts
{
    public class MasarContext : DbContext
    {
        public static string connectionString;
        public static string AuditUrl;
        public static bool AuditEnabled;
        public static string DatabaseName;
        public static IHelperServices.ISessionServices SessionService { get; set; }
        public MasarContext(DbContextOptions options) : base(options)
        {
           
        }


        public MasarContext()
        {
             
        }

        #region DbSets
        public DbSet<Vw_ReturnGroupReferralDTO> Vw_ReturnGroupReferralDTO { get; set; }
        public virtual DbSet<DAL.Domains.Action> Actions { get; set; }
        public virtual DbSet<ActionReferralMode> ActionReferralModes { get; set; }
        public virtual DbSet<ActionRequiredAction> ActionRequiredActions { get; set; }
        public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; }
        public virtual DbSet<UserRoleRequest> UserRoleRequest { get; set; }
        public virtual DbSet<AllowedCountry> AllowedCountries { get; set; }
        public virtual DbSet<CommiteeTaskMultiMission> CommiteeTaskMultiMission { get; set; }
        // public virtual DbSet<CommiteeTaskMultiMissionUser> CommiteeTaskMultiMissionUsers { get; set; }
        public virtual DbSet<ComiteeTaskCategory> ComiteeTaskCategory { get; set; }
        public virtual DbSet<Annotation> Annotations { get; set; }
        public virtual DbSet<AnnotationSecurity> AnnotationSecurities { get; set; }
        public virtual DbSet<AnnotationType> AnnotationTypes { get; set; }
        public virtual DbSet<ArchiveReason> ArchiveReasons { get; set; }
        public virtual DbSet<ViewPermissionCategoryView> ViewPermissionCategoryView { get; set; }
        public virtual DbSet<AssignmentComment> AssignmentComments { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<CommitteeTheme> CommitteeTheme { get; set; }
        public virtual DbSet<SavedAttachment> SavedAttachments { get; set; }
        public virtual DbSet<AttachmentStatus> AttachmentStatuses { get; set; }
        public virtual DbSet<AttachmentTag> AttachmentTags { get; set; }
        public virtual DbSet<CommitteeTaskAttachment> CommitteeTaskAttachments { get; set; }
        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }
        public virtual DbSet<AttachmentVersion> AttachmentVersions { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<AutoResetTransctionNumberPattern> AutoResetTransctionNumberPatterns { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<ChatMessageSeen> ChatMessageSeens { get; set; }
        public virtual DbSet<ChatRoom> ChatRooms { get; set; }
        public virtual DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
        public virtual DbSet<Classification> Classifications { get; set; }
        public virtual DbSet<CommonGroup> CommonGroups { get; set; }
        public virtual DbSet<CommonGroupMember> CommonGroupMembers { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<ConfidentialityLevel> ConfidentialityLevels { get; set; }
        public virtual DbSet<Counter> Counters { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Delegation> Delegations { get; set; }
        public virtual DbSet<DelegationReceipient> DelegationReceipients { get; set; }
        public virtual DbSet<DeliveryCorrespondentTransaction> DeliveryCorrespondentTransactions { get; set; }
        public virtual DbSet<DeliverySheet> DeliverySheets { get; set; }
        public virtual DbSet<DeliverySheetAttachment> DeliverySheetAttachments { get; set; }
        public virtual DbSet<DeliverySheetItem> DeliverySheetItems { get; set; }
        public virtual DbSet<DeliveryStatus> DeliveryStatuses { get; set; }
        public virtual DbSet<DeliveryType> DeliveryTypes { get; set; }
        public virtual DbSet<DictionaryWord> DictionaryWords { get; set; }
        public virtual DbSet<DirectionType> DirectionTypes { get; set; }
        public virtual DbSet<ECMArchiving> Ecmarchivings { get; set; }
        public virtual DbSet<EcmarchivingPermission> EcmarchivingPermissions { get; set; }
        public virtual DbSet<Ecmtemplate> Ecmtemplates { get; set; }
        public virtual DbSet<EmailTransaction> EmailTransactions { get; set; }
        public virtual DbSet<EventReminder> EventReminders { get; set; }
        public virtual DbSet<ExternalOrgnaiztionStatus> ExternalOrgnaiztionStatuses { get; set; }
        public virtual DbSet<ExternalUser> ExternalUsers { get; set; }
        public virtual DbSet<CommitteeMeetingSystemSetting> CommitteeMeetingSystemSetting { get; set; }
        public virtual DbSet<FavoriteList> FavoriteLists { get; set; }
        public virtual DbSet<FollowUp> FollowUps { get; set; }
        public virtual DbSet<FollowUpDateModified> FollowUpDateModifieds { get; set; }
        public virtual DbSet<FollowUpMessagingType> FollowUpMessagingTypes { get; set; }
        public virtual DbSet<FollowUpStatement> FollowUpStatements { get; set; }
        public virtual DbSet<FollowUpStatementType> FollowUpStatementTypes { get; set; }
        public virtual DbSet<FollowUpStatus> FollowUpStatuses { get; set; }
        public virtual DbSet<FollowUpStatusType> FollowUpStatusTypes { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<HangFireJobScheduling> HangFireJobSchedulings { get; set; }
        public virtual DbSet<Hash> Hashes { get; set; }
        public virtual DbSet<HeaderAndFooter> HeaderAndFooters { get; set; }
        public virtual DbSet<Help> Helps { get; set; }
        public virtual DbSet<ImportanceLevel> ImportanceLevels { get; set; }
        public virtual DbSet<IncomingType> IncomingTypes { get; set; }
        public virtual DbSet<IndividualRelationship> IndividualRelationships { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobParameter> JobParameters { get; set; }
        public virtual DbSet<JobQueue> JobQueues { get; set; }
        public virtual DbSet<JobTitle> JobTitles { get; set; }
        public virtual DbSet<LetterTemplate> LetterTemplates { get; set; }
        public virtual DbSet<LetterTemplateOrganization> LetterTemplateOrganizations { get; set; }
        public virtual DbSet<List> Lists { get; set; }
        public virtual DbSet<Vw_String> STRINGS { get; set; }
        public virtual DbSet<Localization> Localizations { get; set; }
        public virtual DbSet<MasarException> MasarExceptions { get; set; }
        public virtual DbSet<MasarLookUpExternalOrganization> MasarLookUpExternalOrganizations { get; set; }
        public virtual DbSet<MasarLookUpMap> MasarLookUpMaps { get; set; }
        public virtual DbSet<MasarSystemIntegrated> MasarSystemIntegrateds { get; set; }
        public virtual DbSet<MasarSystemIntegratedUser> MasarSystemIntegratedUsers { get; set; }
        public virtual DbSet<MaxReceipient> MaxReceipients { get; set; }
        public virtual DbSet<MigratedRelatedTransaction> MigratedRelatedTransactions { get; set; }
        public virtual DbSet<MigratedTransaction> MigratedTransactions { get; set; }
        public virtual DbSet<MigratedTransactionAction> MigratedTransactionActions { get; set; }
        public virtual DbSet<MigratedTransactionActionRecipient> MigratedTransactionActionRecipients { get; set; }
        public virtual DbSet<MigratedTransactionAttachment> MigratedTransactionAttachments { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationLog> NotificationLogs { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<OfficeTemplete> OfficeTempletes { get; set; }
        public virtual DbSet<OfficeTempleteOrganization> OfficeTempleteOrganizations { get; set; }
        public virtual DbSet<OldTransaction> OldTransactions { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationGroup> OrganizationGroups { get; set; }
        public virtual DbSet<OrganizationGroupMember> OrganizationGroupMembers { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionCategory> PermissionCategories { get; set; }
        public virtual DbSet<PhysicalAttachmentType> PhysicalAttachmentTypes { get; set; }
        public virtual DbSet<RecipientStatus> RecipientStatuses { get; set; }
        public virtual DbSet<ReferalMode> ReferalModes { get; set; }
        public virtual DbSet<RelatedTransaction> RelatedTransactions { get; set; }
        public virtual DbSet<ReportRequest> ReportRequests { get; set; }
        public virtual DbSet<RequiredAction> RequiredActions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Schema> Schemas { get; set; }
        public virtual DbSet<Search> Searches { get; set; }
        public virtual DbSet<SearchTemplate> SearchTemplates { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<Set> Sets { get; set; }
        public virtual DbSet<Signature> Signatures { get; set; }
        public virtual DbSet<Smstemplate> Smstemplates { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<SystemSettingCategory> SystemSettingCategories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TicketClassification> TicketClassifications { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionAction> TransactionActions { get; set; }
        public virtual DbSet<TransactionActionAttachment> TransactionActionAttachments { get; set; }
        public virtual DbSet<TransactionActionRecipient> TransactionActionRecipients { get; set; }
        public virtual DbSet<TransactionActionRecipientAttachment> TransactionActionRecipientAttachments { get; set; }
        public virtual DbSet<TransactionActionRecipientStatus> TransactionActionRecipientStatuses { get; set; }
        public virtual DbSet<TransactionActionRecipientUpdateStatus> TransactionActionRecipientUpdateStatuses { get; set; }
        public virtual DbSet<TransactionAttachment> TransactionAttachments { get; set; }
        public virtual DbSet<TransactionAttachmentIndex> TransactionAttachmentIndices { get; set; }
        public virtual DbSet<TransactionBasisType> TransactionBasisTypes { get; set; }
        public virtual DbSet<TransactionDetailLog> TransactionDetailLogs { get; set; }
        public virtual DbSet<TransactionIndividual> TransactionIndividuals { get; set; }
        public virtual DbSet<TransactionRegisterationType> TransactionRegisterationTypes { get; set; }
        public virtual DbSet<TransactionRelationship> TransactionRelationships { get; set; }
        public virtual DbSet<TransactionSource> TransactionSources { get; set; }
        public virtual DbSet<TransactionSubject> TransactionSubjects { get; set; }
        public virtual DbSet<TransactionTag> TransactionTags { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<TransactionTypeClassification> TransactionTypeClassifications { get; set; }
        public virtual DbSet<TransactionTypeSerialAdjustment> TransactionTypeSerialAdjustments { get; set; }
        public virtual DbSet<TransactionWorkFlowProcess> TransactionWorkFlowProcesses { get; set; }
        public virtual DbSet<TransfaredFaxis> TransfaredFaxes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserCorrespondentOrganization> UserCorrespondentOrganizations { get; set; }
        public virtual DbSet<UserJob> UserJobs { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserSignatureFactorAuth> UserSignatureFactorAuths { get; set; }
        public virtual DbSet<UserSpecificOrganizationsAndEmployee> UserSpecificOrganizationsAndEmployees { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<UserVacation> UserVacations { get; set; }
        public virtual DbSet<UsersDashboardStatistic> UsersDashboardStatistics { get; set; }
        public virtual DbSet<UsersSso> UsersSsos { get; set; }
        public virtual DbSet<ViewDeiverySheetViewVm> ViewDeiverySheetViewVms { get; set; }
        public virtual DbSet<ViewPoint> ViewPoints { get; set; }
        public virtual DbSet<VmActionView> VmActionViews { get; set; }
        public virtual DbSet<VmAnnouncementsView> VmAnnouncementsViews { get; set; }
        public virtual DbSet<VmArchiveDeliverySheet> VmArchiveDeliverySheets { get; set; }
        public virtual DbSet<VmAssignmentsView> VmAssignmentsViews { get; set; }
        public virtual DbSet<VmAttachmentView> VmAttachmentViews { get; set; }
        public virtual DbSet<VmDeliveryAttachmentView> VmDeliveryAttachmentViews { get; set; }
        public virtual DbSet<VmDraftTransaction> VmDraftTransactions { get; set; }
        public virtual DbSet<VmExternalDelegationView> VmExternalDelegationViews { get; set; }
        public virtual DbSet<VmFollowUpSpecialReportView> VmFollowUpSpecialReportViews { get; set; }
        public virtual DbSet<VmFollowUpView> VmFollowUpViews { get; set; }
        public virtual DbSet<VmFunctionsIncomingOrganizationReportView> VmFunctionsIncomingOrganizationReportViews { get; set; }
        public virtual DbSet<VmGetLastStatusByTransactionIdView> VmGetLastStatusByTransactionIdViews { get; set; }
        public virtual DbSet<VmIncomingOrganizationReportView> VmIncomingOrganizationReportViews { get; set; }
        public virtual DbSet<VmMigratedAttachmentView> VmMigratedAttachmentViews { get; set; }
        public virtual DbSet<VmMigratedTransactionView> VmMigratedTransactionViews { get; set; }
        public virtual DbSet<VmMigratedTransactionViewCase2> VmMigratedTransactionViewCase2s { get; set; }
        public virtual DbSet<VmNotificationView> VmNotificationViews { get; set; }
        public virtual DbSet<VmOrganizationToReportVm> VmOrganizationToReportVms { get; set; }
        public virtual DbSet<VmOutGoingOrganizationInternallyReport> VmOutGoingOrganizationInternallyReports { get; set; }
        public virtual DbSet<VmOutGoingOrganizationReport> VmOutGoingOrganizationReports { get; set; }
        public virtual DbSet<VmReceiveOutgoingView> VmReceiveOutgoingViews { get; set; }
        public virtual DbSet<VmRecipientsName> VmRecipientsNames { get; set; }
        public virtual DbSet<VmSenderName> VmSenderNames { get; set; }
        public virtual DbSet<VmTransactionActionRecipientSavedView> VmTransactionActionRecipientSavedViews { get; set; }
        public virtual DbSet<VmTransactionActionRecipientView> VmTransactionActionRecipientViews { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewDelivirySheet> VmTransactionActionRecipientViewDelivirySheets { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewInbox> VmTransactionActionRecipientViewInboxes { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewInboxEva> VmTransactionActionRecipientViewInboxEvas { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewKpi> VmTransactionActionRecipientViewKpis { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewOutbox> VmTransactionActionRecipientViewOutboxes { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewOutboxEva> VmTransactionActionRecipientViewOutboxEvas { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewReport> VmTransactionActionRecipientViewReports { get; set; }
        public virtual DbSet<VmTransactionActionRecipientViewStatstic> VmTransactionActionRecipientViewStatstics { get; set; }
        public virtual DbSet<VmTransactionCorrespondent> VmTransactionCorrespondents { get; set; }
        public virtual DbSet<VmTransactionForLateFromDelegation> VmTransactionForLateFromDelegations { get; set; }
        public virtual DbSet<VmTransactionForLateFromReg> VmTransactionForLateFromRegs { get; set; }
        public virtual DbSet<VmTransactionForSavedForReport> VmTransactionForSavedForReports { get; set; }
        public virtual DbSet<VmTransactionForSearch> VmTransactionForSearches { get; set; }
        public virtual DbSet<VmTransactionForSearchForReport> VmTransactionForSearchForReports { get; set; }
        public virtual DbSet<VmTransactionMapView> VmTransactionMapViews { get; set; }
        public virtual DbSet<VmUserRoleCountOfUnDelivered> VmUserRoleCountOfUnDelivereds { get; set; }
        public virtual DbSet<VwGetEmployeeAllTransactionCount> VwGetEmployeeAllTransactionCounts { get; set; }
        public virtual DbSet<VwOrganizationsToReferral> VwOrganizationsToReferrals { get; set; }
        public virtual DbSet<VwLookUpReturnUser> VwLookUpReturnUsers { get; set; }
        public virtual DbSet<VwTransactionInfo> VwTransactionInfos { get; set; }
        public virtual DbSet<WorkFlowFilter> WorkFlowFilters { get; set; }
        public virtual DbSet<WorkFlowFilterEnum> WorkFlowFilterEnums { get; set; }
        public virtual DbSet<WorkFlowProcess> WorkFlowProcesses { get; set; }
        public virtual DbSet<WorkFlowProcessAction> WorkFlowProcessActions { get; set; }
        public virtual DbSet<WorkFlowTransition> WorkFlowTransitions { get; set; }
        public virtual DbSet<WorkPlace> WorkPlaces { get; set; }
        public DbSet<Vm_Permissions> Vm_Permissions { get; set; }
        public DbSet<Vm_EmpInOrgaHierarchy> vm_EmpInOrgaHierarchies { get; set; }
        public DbSet<Vm_OrgInOrgaHierarchy> Vm_OrgInOrgaHierarchies { get; set; }

        public DbSet<CommitteeNotification> CommitteeNotifications { get; set; }
        public virtual DbSet<UpdateTaskLog> UpdateTaskLog { get; set; }
        public virtual DbSet<UpdateTaskLogAssistantUser> UpdateTaskLogAssistantUser { get; set; }
        public virtual DbSet<UpdateTaskLogMainAssignedUser> UpdateTaskLogMainAssignedUser { get; set; }

        #region Commite Domains
        public DbSet<AttachmentComment> AttachmentComments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Commitee> Commitees { get; set; }
        public DbSet<CommiteeSavedAttachment> CommiteeSavedAttachments { get; set; }
        public DbSet<CommiteeUsersPermission> CommiteeUsersPermissions { get; set; }
        public DbSet<CommiteeRole> CommiteeRoles { get; set; }
        public DbSet<CommiteeTask> CommiteeTasks { get; set; }
        public DbSet<CommiteeTaskEscalation> CommiteeTaskEscalation { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }

        public DbSet<CommiteeType> CommiteeTypes { get; set; }
        public DbSet<CurrentStatus> CurrentStatus { get; set; }
        public DbSet<CurrentStatusReason> CurrentStatusReasons { get; set; }
        public DbSet<CommiteeLocalization> CommiteeLocalizations { get; set; }
        public DbSet<CommiteePermissionCategory> CommiteePermissionCategories { get; set; }
        public DbSet<CommiteeRolePermission> CommiteeRolePermissions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUsers> GroupUsers { get; set; }
        public DbSet<CommiteeTaskMultiMissionUser> CommiteeTaskMultiMissionUsers { get; set; }
        public DbSet<TaskGroups> TaskGroups { get; set; }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyComment> SurveyComments { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyUser> SurveyUsers { get; set; }
        public DbSet<SurveyAnswerUser> SurveyAnswerUsers { get; set; }
        public DbSet<SurveyAttachment> SurveyAttachments { get; set; }
        public DbSet<ValidityPeriod> ValidityPeriods { get; set; }
        public DbSet<CommiteeMember> CommiteeMembers { get; set; }
        public DbSet<CommiteeUsersRole> CommiteeUsersRoles { get; set; }
        public DbSet<Vw_TransactionBoxes_Inbox> Vw_TransactionBoxes_Inbox { get; internal set; }
        public DbSet<Vw_Count> COUNTS { get; set; }
        public DbSet<TransactionByTypeReportDTO> TransactionByTypeReportDTO { get; set; }
        public DbSet<Vw_TransactionBoxes> ViewTransactionsView { get; set; }
        public DbSet<Vw_Attachments> attachmentsViews { get; set; }
        // public DbSet<Vw_OrganizationToReferral> Vw_OrganizationsToReferral { get; set; }
        public DbSet<Vw_UserToReferral> Vw_UsersToReferral { get; set; }
        #endregion
        #region Mettings Domain
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingAttendee> MeetingAttendees { get; set; }
        public DbSet<MeetingCoordinator> MeetingCoordinators { get; set; }
        public DbSet<MeetingHeaderAndFooter> MeetingHeaderAndFooters { get; set; }
        public DbSet<MeetingURl> MeetingURls { get; set; }
        public DbSet<MeetingProject> MeetingProjects { get; set; }
        public DbSet<MeetingTopic> MeetingTopics { get; set; }
        public DbSet<MinuteOfMeeting> MinutesOfMeetings { get; set; }
        public DbSet<MeetingComment> MeetingComments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TopicComment> TopicComments { get; set; }
        public DbSet<TopicPauseDate> TopicPauseDates { get; set; }
        public DbSet<Meeting_Meeting_HeaderAndFooter> Meeting_Meeting_HeaderAndFooters { get; set; }
        public DbSet<MOMComment> MOMComments { get; set; }
        #endregion



        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //modelBuilder
            //   .Entity<CommiteeTaskMultiMission>()
            //   .HasOne(e => e.CommiteeTask)
            //   .WithMany(e => e.MultiMission).HasForeignKey(x => x.CommiteeTaskId);
            modelBuilder
               .Entity<CommiteeTaskMultiMission>()
               .HasMany(e => e.CommiteeTaskMultiMissionUsers)
               .WithOne(e => e.CommiteeTaskMultiMission)
               .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder
               .Entity<CommiteeTask>()
               .HasMany(e => e.MultiMission)
               .WithOne(e => e.CommiteeTask)
               .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder
                .Entity<CommiteeTask>()
                .HasMany(e => e.AssistantUsers)
                .WithOne(e => e.CommiteeTask)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder
               .Entity<CommiteeTask>()
               .HasMany(e => e.TaskGroups)
               .WithOne(e => e.CommiteeTask)
               .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder
               .Entity<MeetingComment>()
               .HasMany(e => e.SurveyAnswers)
               .WithOne(e => e.MeetingComment)
               .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder
                .Entity<CommiteeTask>()
                .HasMany(e => e.TaskAttachments)
                .WithOne(e => e.CommiteeTask)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder
                .Entity<CommiteeTaskEscalation>()
                .HasIndex(e=>new { e.ComiteeTaskCategoryId,e.MainAssinedUserId,e.NewMainAssinedUserId,e.DelayPeriod }).IsUnique();
            //modelBuilder
            //   .Entity<CommiteeTaskEscalation>().HasIndex(z => z.CommiteeTaskEscalationIndex).IsUnique();
            modelBuilder.Entity<CommiteeTaskEscalation>().Property(z => z.CommiteeTaskEscalationIndex).ValueGeneratedOnAdd();
           // modelBuilder.Entity<CommiteeTaskEscalation>().Property(z => z.CommiteeTaskEscalationIndex).H();

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasIndex(e => e.Code).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleNameEn).IsUnique();
                entity.HasIndex(e => e.RoleNameAr).IsUnique();
            });
            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.Property(ut => ut.RefreshTokenIdHash).IsRequired();
            });
            modelBuilder.Entity<User>(x =>
            {
                //x.HasMany<User>(m => m.Users).WithOne(n => n.Nationality).HasForeignKey(u => u.NationalityId);
                x.HasOne<Nationality>(m => m.Nationality).WithMany(u => u.Users).HasForeignKey(f => f.NationalityId);
            });
            //modelBuilder.Entity<UserCorrespondentOrganization>(e =>
            //{
            //    e.HasOne(u => u.User).WithMany(u => u.userCorrespondentOrganization).HasForeignKey(u => u.UserId );
            //    e.HasOne(o => o.Organization).WithMany(o => o.userCorrespondentOrganization).HasForeignKey(o => o.OrganizationId);
            //});
            modelBuilder.Entity<UserTask>().HasQueryFilter(p => p.DeletedBy == null);
            modelBuilder.Entity<CommiteeTaskMultiMissionUser>().HasQueryFilter(p => p.DeletedBy == null);
            modelBuilder.Entity<Transaction>().HasIndex(u => u.TransactionNumber).IsUnique(); ;
            modelBuilder.Entity<RolePermission>()
                .HasKey(x => new { x.RoleId, x.PermissionId });
            modelBuilder.Entity<Permission>()
                .HasIndex(x => x.PermissionCode)
                .IsUnique();
            //modelBuilder.Entity<UserRole>()
            //    .HasKey(x => new { x.RoleId, x.UserId, x.OrganizationId });
            //modelBuilder.Entity<UserRole>()
            //    .HasIndex(x => new { x.RoleId, x.UserId, x.OrganizationId }).IsUnique();

            modelBuilder.Entity<RolePermission>()
                .HasKey(x => new { x.RoleId, x.PermissionId });
            modelBuilder.Entity<UserPermission>()
                .HasKey(x => new { x.UserId, x.PermissionId, x.OrganizationId, x.RoleId });

            modelBuilder.Entity<Localization>()
                .HasIndex(x => new { x.Key })
                .IsUnique();
            modelBuilder.Entity<TransactionTypeClassification>()
                .HasKey(x => new { x.ClassificationId, x.TransactionTypeId });

            modelBuilder.Entity<User>(e => {
                e.HasMany(x => x.CurrenHeadUnit).WithOne(x => x.CurrenHeadUnit).HasForeignKey(x => x.CurrenHeadUnitId).IsRequired(false);
                e.HasMany(x => x.CommitteeSecretary).WithOne(x => x.CommitteeSecretary).HasForeignKey(x => x.CommitteeSecretaryId).IsRequired(false);
            });
            //modelBuilder.Entity<AnnotationType>()
            //    .HasKey(x => x.AnnotationTypeId);
            //modelBuilder.Entity<TransactionRegisterationType>().HasData(
            //    new TransactionRegisterationType() {  TransactionRegisterationTypeAr = "داخلي", TransactionRegisterationTypeEn = "Internal" },
            //    new TransactionRegisterationType() {  TransactionRegisterationTypeAr = "خارجي", TransactionRegisterationTypeEn = "External" }
            //);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
        .UseLazyLoadingProxies(true)
        .UseSqlServer(connectionString);
        }

        // Audit
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (AuditEnabled)
            {
                List<AuditEntry> auditEntries = OnBeforeSaveChanges();
                int result = base.SaveChanges(acceptAllChangesOnSuccess);
                OnAfterSaveChanges(auditEntries);
                return result;
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (AuditEnabled)
                {
                    List<AuditEntry> auditEntries = OnBeforeSaveChanges();
                    Task<int> result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                    OnAfterSaveChanges(auditEntries);
                    return result;
                }
                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            List<AuditEntry> auditEntries = new List<AuditEntry>();
            List<AuditEntry> auditRelations = new List<AuditEntry>();
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                string changeType = entry.State.ToString();
                AuditEntry auditEntry = new AuditEntry(entry)
                {
                    State = changeType,
                    TableName = entry.Metadata.GetTableName()
                };
                auditEntries.Add(auditEntry);
                foreach (PropertyEntry property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }
                    //if (property.Metadata.IsForeignKey())
                    //{
                    //    auditEntry.TemporaryProperties.Add(property);
                    //    auditEntry.ForignKeys[propertyName] = property.CurrentValue;
                    //}

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                //{
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                if (entry.Metadata.GetTableName().Equals("Organizations"))
                                {
                                    if (property.Metadata.Name.Equals("AdminOrganizationId"))
                                    {

                                    }
                                }
                            }

                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            ///}
                            //else
                            //{
                            //    auditEntry.OldValues[propertyName] = property.OriginalValue;
                            //    auditEntry.NewValues[propertyName] = property.CurrentValue;
                            //}
                            break;
                    }
                }

            }

            // Save audit entities that have all the modifications
            //foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            //{
            //    Audits.Add(auditEntry.ToAudit());
            //}

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.ToList();
        }

        private int OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
            {
                return 0;
            }

            User user = new User();
            foreach (AuditEntry auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (PropertyEntry prop in auditEntry.TemporaryProperties)
                {
                    //if (prop.Metadata.IsPrimaryKey())
                    //{
                    //    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    //}

                    //else
                    //{
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;

                    //   }

                }

                // Save the Audit entry

                CommitteAudit audittril = auditEntry.ToAudit();
                audittril.UserName = SessionService?.EmployeeFullNameAr; //_contextAccessor.HttpContext.User?.Identity?.Name; 
                if (!string.IsNullOrEmpty(audittril.UserName))
                {
                    audittril.IP = SessionService.ClientIP;// HttpContext.Connection.RemoteIpAddress.ToString();
                    audittril.OrganizationName = SessionService.OrganizationNameAr;
                    audittril.RoleName = SessionService.RoleNameAr;
                    audittril.ApplicationType = SessionService.ApplicationType;
                    audittril.LoginName = SessionService.UserName;
                    audittril.NewValue = JsonConvert.SerializeObject(auditEntry.NewValues);
                    audittril.OldValue = JsonConvert.SerializeObject(auditEntry.OldValues);
                    if (auditEntry.NewValues.ContainsKey("TransactionId"))
                    {
                        audittril.TransactionId = long.Parse(auditEntry.NewValues["TransactionId"].ToString());
                    }

                    //audittril.OrganizationName = _dataProtectService.Decrypt(audittril.OrganizationName);
                    //audittril.RoleName = _dataProtectService.Decrypt(audittril.RoleName);
                    using (AuditContext cont = new AuditContext())
                    {
                        cont.Audits.Add(audittril);
                        cont.SaveChanges();
                    }
                }
                else
                    return 0;

            }

            return SaveChanges();
        }
    }


    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
        public Dictionary<string, object> ForignKeys { get; set; } = new Dictionary<string, object>();
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public string State { get; set; }

        public CommitteAudit ToAudit()
        {
            CommitteAudit audit = new CommitteAudit
            {
                //var auditEntities = (int)Enum.Parse(typeof(AuditEntities), TableName); ;
                //audit.ActionId = auditEntities;
                ActionName = State,
                TableName = TableName,
                CreatedDate = DateTime.UtcNow,
                ForignKeys = ForignKeys.Count == 0 ? null : JsonConvert.SerializeObject(ForignKeys),
                OldValue = null,
                NewValue = null

            };
            return audit;
        }

    }
}
