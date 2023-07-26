using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeDatabase.Models.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(Username), Name = "IX_Users_Username", IsUnique = true)]
    [Index(nameof(UserId), Name = "_dta_index_Users_45_1105647232__K1_7_10_11_1563")]
    [Index(nameof(UserId), Name = "_dta_index_Users_45_1105647232__K1_7_9910")]
    public partial class User
    {
        public User()
        {
            ActionRequiredActionCreatedByNavigations = new HashSet<ActionRequiredAction>();
            ActionRequiredActionUpdatedByNavigations = new HashSet<ActionRequiredAction>();
            AnnotationCreatedByNavigations = new HashSet<Annotation>();
            AnnotationDeletedByNavigations = new HashSet<Annotation>();
            AnnotationSecurities = new HashSet<AnnotationSecurity>();
            AnnotationUpdatedByNavigations = new HashSet<Annotation>();
            AssignmentComments = new HashSet<AssignmentComment>();
            AttachmentCreatedByNavigations = new HashSet<Attachment>();
            AttachmentTags = new HashSet<AttachmentTag>();
            AttachmentUpdatedByNavigations = new HashSet<Attachment>();
            AttachmentVersionCreatedByNavigations = new HashSet<AttachmentVersion>();
            AttachmentVersionDeletedByNavigations = new HashSet<AttachmentVersion>();
            AttachmentVersionUpdatedByNavigations = new HashSet<AttachmentVersion>();
            ChatMessageCreatedByNavigations = new HashSet<ChatMessage>();
            ChatMessageDeletedByNavigations = new HashSet<ChatMessage>();
            ChatMessageSeens = new HashSet<ChatMessageSeen>();
            ChatRoomUserCreatedByNavigations = new HashSet<ChatRoomUser>();
            ChatRoomUserDeletedByNavigations = new HashSet<ChatRoomUser>();
            ChatRoomUserUsers = new HashSet<ChatRoomUser>();
            ChatRooms = new HashSet<ChatRoom>();
            ClassificationCreatedByNavigations = new HashSet<Classification>();
            ClassificationUpdatedByNavigations = new HashSet<Classification>();
            CommonGroupCreatedByNavigations = new HashSet<CommonGroup>();
            CommonGroupDeletedByNavigations = new HashSet<CommonGroup>();
            CommonGroupMemberCreatedByNavigations = new HashSet<CommonGroupMember>();
            CommonGroupMemberUsers = new HashSet<CommonGroupMember>();
            CommonGroupUpdatedByNavigations = new HashSet<CommonGroup>();
            DelegationCreatedByNavigations = new HashSet<Delegation>();
            DelegationReceipientCorrespondentUsers = new HashSet<DelegationReceipient>();
            DelegationReceipientDirectedToUsers = new HashSet<DelegationReceipient>();
            DelegationUpdatedByNavigations = new HashSet<Delegation>();
            DeliveryCorrespondentTransactionCreatedByNavigations = new HashSet<DeliveryCorrespondentTransaction>();
            DeliveryCorrespondentTransactionUpdatedByNavigations = new HashSet<DeliveryCorrespondentTransaction>();
            DeliveryCorrespondentTransactionWhoIsEmpSignNavigations = new HashSet<DeliveryCorrespondentTransaction>();
            DeliverySheetAttachmentCreatedByNavigations = new HashSet<DeliverySheetAttachment>();
            DeliverySheetAttachmentUpdatedByNavigations = new HashSet<DeliverySheetAttachment>();
            DeliverySheetCorrespondentUsers = new HashSet<DeliverySheet>();
            DeliverySheetCreatedByNavigations = new HashSet<DeliverySheet>();
            DeliverySheetItems = new HashSet<DeliverySheetItem>();
            DeliverySheetUpdatedByNavigations = new HashSet<DeliverySheet>();
            Ecmarchivings = new HashSet<ECMArchiving>();
            Ecmtemplates = new HashSet<Ecmtemplate>();
            FavoriteListFavoriteUsers = new HashSet<FavoriteList>();
            FavoriteListUsers = new HashSet<FavoriteList>();
            FollowUpChangeStatusByUsers = new HashSet<FollowUp>();
            FollowUpCreatedByNavigations = new HashSet<FollowUp>();
            FollowUpDeletedByNavigations = new HashSet<FollowUp>();
            FollowUpStatements = new HashSet<FollowUpStatement>();
            FollowUpStatuses = new HashSet<FollowUpStatus>();
            FollowUpUsers = new HashSet<FollowUp>();
            Genders = new HashSet<Gender>();
            HangFireJobSchedulings = new HashSet<HangFireJobScheduling>();
            ImportanceLevelCreatedByNavigations = new HashSet<ImportanceLevel>();
            ImportanceLevelUpdatedByNavigations = new HashSet<ImportanceLevel>();
            IncomingTypeCreatedByNavigations = new HashSet<IncomingType>();
            IncomingTypeUpdatedByNavigations = new HashSet<IncomingType>();
            IndividualRelationshipCreatedByNavigations = new HashSet<IndividualRelationship>();
            IndividualRelationshipUpdatedByNavigations = new HashSet<IndividualRelationship>();
            JobTitleCreatedByNavigations = new HashSet<JobTitle>();
            JobTitleUpdatedByNavigations = new HashSet<JobTitle>();
            LetterTemplateCreatedByNavigations = new HashSet<LetterTemplate>();
            LetterTemplateOrganizationCreatedByNavigations = new HashSet<LetterTemplateOrganization>();
            LetterTemplateOrganizationUpdatedByNavigations = new HashSet<LetterTemplateOrganization>();
            LetterTemplateUpdatedByNavigations = new HashSet<LetterTemplate>();
            LocalizationCreatedByNavigations = new HashSet<Localization>();
            LocalizationUpdatedByNavigations = new HashSet<Localization>();
            MasarSystemIntegratedUsers = new HashSet<MasarSystemIntegratedUser>();
            NationalityCreatedByNavigations = new HashSet<Nationality>();
            NationalityUpdatedByNavigations = new HashSet<Nationality>();
            NotificationCreatedByNavigations = new HashSet<Notification>();
            NotificationDismissedByNavigations = new HashSet<Notification>();

            
          
            NotificationUpdatedByNavigations = new HashSet<Notification>();
            NotificationUsers = new HashSet<Notification>();
            OrganizationGroups = new HashSet<OrganizationGroup>();
            PhysicalAttachmentTypeCreatedByNavigations = new HashSet<PhysicalAttachmentType>();
            PhysicalAttachmentTypeUpdatedByNavigations = new HashSet<PhysicalAttachmentType>();
            RelatedTransactionCreatedByNavigations = new HashSet<RelatedTransaction>();
            RelatedTransactionUpdatedByNavigations = new HashSet<RelatedTransaction>();
            ReportRequests = new HashSet<ReportRequest>();
            RequiredActionCreatedByNavigations = new HashSet<RequiredAction>();
            RequiredActionUpdatedByNavigations = new HashSet<RequiredAction>();
            RoleCreatedByNavigations = new HashSet<Role>();
            RolePermissionCreatedByNavigations = new HashSet<RolePermission>();
            RolePermissionUpdatedByNavigations = new HashSet<RolePermission>();
            RoleUpdatedByNavigations = new HashSet<Role>();
            SearchCreatedByNavigations = new HashSet<Search>();
            SearchUpdatedByNavigations = new HashSet<Search>();
            SignatureCreatedByNavigations = new HashSet<Signature>();
            SignatureUpdatedByNavigations = new HashSet<Signature>();
            SignatureUsers = new HashSet<Signature>();
            TagCreatedByNavigations = new HashSet<Tag>();
            TagUpdatedByNavigations = new HashSet<Tag>();
            TransactionActionCreatedByNavigations = new HashSet<TransactionAction>();
            TransactionActionDirectedFromUsers = new HashSet<TransactionAction>();
            TransactionActionRecipientAttachmentAttachmentStatusChangedByNavigations = new HashSet<TransactionActionRecipientAttachment>();
            TransactionActionRecipientAttachmentCreatedByNavigations = new HashSet<TransactionActionRecipientAttachment>();
            TransactionActionRecipientAttachmentUpdatedByNavigations = new HashSet<TransactionActionRecipientAttachment>();
            TransactionActionRecipientCorrespondentUsers = new HashSet<TransactionActionRecipient>();
            TransactionActionRecipientCreatedByNavigations = new HashSet<TransactionActionRecipient>();
            TransactionActionRecipientDirectedToUsers = new HashSet<TransactionActionRecipient>();
            TransactionActionRecipientRecipientStatusChangedByNavigations = new HashSet<TransactionActionRecipient>();
            TransactionActionRecipientStatuses = new HashSet<TransactionActionRecipientStatus>();
            TransactionActionRecipientUpdateStatuses = new HashSet<TransactionActionRecipientUpdateStatus>();
            TransactionActionRecipientUpdatedByNavigations = new HashSet<TransactionActionRecipient>();
            TransactionActionUpdatedByNavigations = new HashSet<TransactionAction>();
            TransactionAttachmentCreatedByNavigations = new HashSet<TransactionAttachment>();
            TransactionAttachmentDeletedByNavigations = new HashSet<TransactionAttachment>();
            TransactionAttachmentIndexCreatedByNavigations = new HashSet<TransactionAttachmentIndex>();
            TransactionAttachmentIndexUpdatedByNavigations = new HashSet<TransactionAttachmentIndex>();
            TransactionAttachmentUpdatedByNavigations = new HashSet<TransactionAttachment>();
            TransactionCreatedByNavigations = new HashSet<Transaction>();
            TransactionDeletedByNavigations = new HashSet<Transaction>();
            TransactionIndividualCreatedByNavigations = new HashSet<TransactionIndividual>();
            TransactionIndividualUpdatedByNavigations = new HashSet<TransactionIndividual>();
            TransactionIndividualUsers = new HashSet<TransactionIndividual>();
            TransactionRelationshipCreatedByNavigations = new HashSet<TransactionRelationship>();
            TransactionRelationshipUpdatedByNavigations = new HashSet<TransactionRelationship>();
            TransactionSubjectCreatedByNavigations = new HashSet<TransactionSubject>();
            TransactionSubjectUpdatedByNavigations = new HashSet<TransactionSubject>();
            TransactionSubjectUsers = new HashSet<TransactionSubject>();
            TransactionTags = new HashSet<TransactionTag>();
            TransactionTypeSerialAdjustmentCreatedByNavigations = new HashSet<TransactionTypeSerialAdjustment>();
            TransactionTypeSerialAdjustmentUpdatedByNavigations = new HashSet<TransactionTypeSerialAdjustment>();
            TransactionUpdatedByNavigations = new HashSet<Transaction>();
            TransactionWorkFlowProcessCreatedByNavigations = new HashSet<TransactionWorkFlowProcess>();
            TransactionWorkFlowProcessUpdatedByNavigations = new HashSet<TransactionWorkFlowProcess>();
            UserCorrespondentOrganizationCreatedByNavigations = new HashSet<UserCorrespondentOrganization>();
            UserCorrespondentOrganizationUpdatedByNavigations = new HashSet<UserCorrespondentOrganization>();
            UserCorrespondentOrganizationUsers = new HashSet<UserCorrespondentOrganization>();
            UserJobs = new HashSet<UserJob>();
            UserPermissionCreatedByNavigations = new HashSet<UserPermission>();
            UserPermissionUpdatedByNavigations = new HashSet<UserPermission>();
            UserPermissionUsers = new HashSet<UserPermission>();
            UserRoleCreatedByNavigations = new HashSet<UserRole>();
            UserRoleUpdatedByNavigations = new HashSet<UserRole>();
            UserRoleUsers = new HashSet<UserRole>();
            UserSignatureFactorAuths = new HashSet<UserSignatureFactorAuth>();
            UserSpecificOrganizationsAndEmployeeCreatedByNavigations = new HashSet<UserSpecificOrganizationsAndEmployee>();
            UserSpecificOrganizationsAndEmployeeSpecificUsers = new HashSet<UserSpecificOrganizationsAndEmployee>();
            UserSpecificOrganizationsAndEmployeeUpdatedByNavigations = new HashSet<UserSpecificOrganizationsAndEmployee>();
            UserSpecificOrganizationsAndEmployeeUsers = new HashSet<UserSpecificOrganizationsAndEmployee>();
            UserTokens = new HashSet<UserToken>();
            UserVacationStandByUsers = new HashSet<UserVacation>();
            UserVacationUsers = new HashSet<UserVacation>();
            WorkFlowProcessCreatedByNavigations = new HashSet<WorkFlowProcess>();
            WorkFlowProcessUpdatedByNavigations = new HashSet<WorkFlowProcess>();
            WorkFlowTransitionCreatedByNavigations = new HashSet<WorkFlowTransition>();
            WorkFlowTransitionUpdatedByNavigations = new HashSet<WorkFlowTransition>();
            WorkPlaceCreatedByNavigations = new HashSet<WorkPlace>();
            WorkPlaceUpdatedByNavigations = new HashSet<WorkPlace>();
            Groups = new HashSet<Group>();
            GroupUsers= new HashSet<GroupUsers>();
            CommiteeTaskMultiMissionUsers = new HashSet<CommiteeTaskMultiMissionUser>();
            CurrenHeadUnit = new HashSet<Commitee>();
            CommitteeSecretary = new HashSet<Commitee>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(450)]
        public string Password { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public DateTimeOffset? LastLoggedIn { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string SerialNumber { get; set; }
        [StringLength(100)]
        public string FullNameEn { get; set; }
        [StringLength(100)]
        public string FullNameAr { get; set; }
        public DateTimeOffset? EnabledUntil { get; set; }
        public int? ProfileImageFileId { get; set; }
        public int? SignatureFileId { get; set; }
        public int? TrkeenFileId { get; set; }
        public int? StampFileId { get; set; }
        [StringLength(50)]
        public string EmployeeNumber { get; set; }
        [Column("SSN")]
        [StringLength(50)]
        public string Ssn { get; set; }
        [StringLength(50)]
        public string PassportNumber { get; set; }
        [StringLength(50)]
        public string IqamaNumber { get; set; }
        [StringLength(50)]
        public string CompanyName { get; set; }
        public int? GenderId { get; set; }
        public int? NationalityId { get; set; }
        public int? JobTitleId { get; set; }
        public int? WorkPlaceId { get; set; }
        public string Address { get; set; }
        [StringLength(50)]
        public string WorkPhoneNumber { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        public bool NotificationByMail { get; set; }
        [Column("NotificationBySMS")]
        public bool NotificationBySms { get; set; }
        public bool IsCorrespondentForAllOrganizations { get; set; }
        public bool IsIndividual { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsCorrespondent { get; set; }
        [StringLength(5)]
        public string DefaultCulture { get; set; }
        [StringLength(10)]
        public string DefaultCalendar { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        [StringLength(450)]
        public string SignaturePassword { get; set; }
        public string ProfileImageMimeType { get; set; }
        [Column("isHijriDate")]
        public bool IsHijriDate { get; set; }
        [Column("showEvents")]
        public bool ShowEvents { get; set; }
        [Column("showStatistics")]
        public bool ShowStatistics { get; set; }
        [Column("userTheme")]
        public string UserTheme { get; set; }
        public int? IndividualAttachmentId { get; set; }
        public DateTimeOffset? FactorAuthDate { get; set; }
        public string FactorAuthValue { get; set; }
        [Column("RequestID")]
        public string RequestId { get; set; }
        [Column ("IsMobileUser")]
        public bool IsMobileuser { get; set; }
        [Column("AuditUser")]
        public bool AuditUser { get; set; } = false;
        public bool IsShowCalender { get; set; }
        public bool IsShowPeriodStatistics { get; set; }
        public bool IsShowTask { get; set; }
        [Column("DelegationDefaultID")]
        public int DelegationDefaultId { get; set; }
        public bool IsShowTransactionOwner { get; set; }
        public bool IsShowTransactionRelated { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsLocked { get; set; }
        public DateTimeOffset? PasswordUpdatedOn { get; set; }
        public bool? IsGeneral { get; set; }
        [StringLength(20)]
        public string CaptchaCode { get; set; }
        public DateTimeOffset? CaptchaExpireDate { get; set; }
        public string FullNameFn { get; set; }
        public int? DefaultInboxFilter { get; set; }
        public bool HasFactorAuth { get; set; }
        public string FaxUserId { get; set; }
        public string NormalizedUserName { get; set; }
        public bool HasSignatureFactorAuth { get; set; }
        public bool ExternalUser { get; set; } = false;

        [ForeignKey(nameof(NationalityId))]
        [InverseProperty("Users")]
        public virtual Nationality Nationality { get; set; }
        [InverseProperty(nameof(ActionRequiredAction.CreatedByNavigation))]
        public virtual ICollection<ActionRequiredAction> ActionRequiredActionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(ActionRequiredAction.UpdatedByNavigation))]
        public virtual ICollection<ActionRequiredAction> ActionRequiredActionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Annotation.CreatedByNavigation))]
        public virtual ICollection<Annotation> AnnotationCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Annotation.DeletedByNavigation))]
        public virtual ICollection<Annotation> AnnotationDeletedByNavigations { get; set; }
        [InverseProperty(nameof(AnnotationSecurity.User))]
        public virtual ICollection<AnnotationSecurity> AnnotationSecurities { get; set; }
        [InverseProperty(nameof(Annotation.UpdatedByNavigation))]
        public virtual ICollection<Annotation> AnnotationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(AssignmentComment.CreatedByNavigation))]
        public virtual ICollection<AssignmentComment> AssignmentComments { get; set; }
        [InverseProperty(nameof(Attachment.CreatedByNavigation))]
        public virtual ICollection<Attachment> AttachmentCreatedByNavigations { get; set; }
        [InverseProperty(nameof(AttachmentTag.CreatedByNavigation))]
        public virtual ICollection<AttachmentTag> AttachmentTags { get; set; }
        [InverseProperty(nameof(Attachment.UpdatedByNavigation))]
        public virtual ICollection<Attachment> AttachmentUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(AttachmentVersion.CreatedByNavigation))]
        public virtual ICollection<AttachmentVersion> AttachmentVersionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(AttachmentVersion.DeletedByNavigation))]
        public virtual ICollection<AttachmentVersion> AttachmentVersionDeletedByNavigations { get; set; }
        [InverseProperty(nameof(AttachmentVersion.UpdatedByNavigation))]
        public virtual ICollection<AttachmentVersion> AttachmentVersionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(ChatMessage.CreatedByNavigation))]
        public virtual ICollection<ChatMessage> ChatMessageCreatedByNavigations { get; set; }
        [InverseProperty(nameof(ChatMessage.DeletedByNavigation))]
        public virtual ICollection<ChatMessage> ChatMessageDeletedByNavigations { get; set; }
        [InverseProperty(nameof(ChatMessageSeen.CreatedByNavigation))]
        public virtual ICollection<ChatMessageSeen> ChatMessageSeens { get; set; }
        [InverseProperty(nameof(ChatRoomUser.CreatedByNavigation))]
        public virtual ICollection<ChatRoomUser> ChatRoomUserCreatedByNavigations { get; set; }
        [InverseProperty(nameof(ChatRoomUser.DeletedByNavigation))]
        public virtual ICollection<ChatRoomUser> ChatRoomUserDeletedByNavigations { get; set; }
        [InverseProperty(nameof(ChatRoomUser.User))]
        public virtual ICollection<ChatRoomUser> ChatRoomUserUsers { get; set; }
        [InverseProperty(nameof(ChatRoom.CreatedByNavigation))]
        public virtual ICollection<ChatRoom> ChatRooms { get; set; }
        [InverseProperty(nameof(Classification.CreatedByNavigation))]
        public virtual ICollection<Classification> ClassificationCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Classification.UpdatedByNavigation))]
        public virtual ICollection<Classification> ClassificationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(CommonGroup.CreatedByNavigation))]
        public virtual ICollection<CommonGroup> CommonGroupCreatedByNavigations { get; set; }
        [InverseProperty(nameof(CommonGroup.DeletedByNavigation))]
        public virtual ICollection<CommonGroup> CommonGroupDeletedByNavigations { get; set; }
        [InverseProperty(nameof(CommonGroupMember.CreatedByNavigation))]
        public virtual ICollection<CommonGroupMember> CommonGroupMemberCreatedByNavigations { get; set; }
        [InverseProperty(nameof(CommonGroupMember.User))]
        public virtual ICollection<CommonGroupMember> CommonGroupMemberUsers { get; set; }
        [InverseProperty(nameof(CommonGroup.UpdatedByNavigation))]
        public virtual ICollection<CommonGroup> CommonGroupUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Delegation.CreatedByNavigation))]
        public virtual ICollection<Delegation> DelegationCreatedByNavigations { get; set; }
        [InverseProperty(nameof(DelegationReceipient.CorrespondentUser))]
        public virtual ICollection<DelegationReceipient> DelegationReceipientCorrespondentUsers { get; set; }
        [InverseProperty(nameof(DelegationReceipient.DirectedToUser))]
        public virtual ICollection<DelegationReceipient> DelegationReceipientDirectedToUsers { get; set; }
        [InverseProperty(nameof(Delegation.UpdatedByNavigation))]
        public virtual ICollection<Delegation> DelegationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(DeliveryCorrespondentTransaction.CreatedByNavigation))]
        public virtual ICollection<DeliveryCorrespondentTransaction> DeliveryCorrespondentTransactionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(DeliveryCorrespondentTransaction.UpdatedByNavigation))]
        public virtual ICollection<DeliveryCorrespondentTransaction> DeliveryCorrespondentTransactionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(DeliveryCorrespondentTransaction.WhoIsEmpSignNavigation))]
        public virtual ICollection<DeliveryCorrespondentTransaction> DeliveryCorrespondentTransactionWhoIsEmpSignNavigations { get; set; }
        [InverseProperty(nameof(DeliverySheetAttachment.CreatedByNavigation))]
        public virtual ICollection<DeliverySheetAttachment> DeliverySheetAttachmentCreatedByNavigations { get; set; }
        [InverseProperty(nameof(DeliverySheetAttachment.UpdatedByNavigation))]
        public virtual ICollection<DeliverySheetAttachment> DeliverySheetAttachmentUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(DeliverySheet.CorrespondentUser))]
        public virtual ICollection<DeliverySheet> DeliverySheetCorrespondentUsers { get; set; }
        [InverseProperty(nameof(DeliverySheet.CreatedByNavigation))]
        public virtual ICollection<DeliverySheet> DeliverySheetCreatedByNavigations { get; set; }
        [InverseProperty(nameof(DeliverySheetItem.UpdatedByNavigation))]
        public virtual ICollection<DeliverySheetItem> DeliverySheetItems { get; set; }
        [InverseProperty(nameof(DeliverySheet.UpdatedByNavigation))]
        public virtual ICollection<DeliverySheet> DeliverySheetUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(ECMArchiving.User))]
        public virtual ICollection<ECMArchiving> Ecmarchivings { get; set; }
        [InverseProperty(nameof(Ecmtemplate.User))]
        public virtual ICollection<Ecmtemplate> Ecmtemplates { get; set; }
        [InverseProperty(nameof(FavoriteList.FavoriteUser))]
        public virtual ICollection<FavoriteList> FavoriteListFavoriteUsers { get; set; }
        [InverseProperty(nameof(FavoriteList.User))]
        public virtual ICollection<FavoriteList> FavoriteListUsers { get; set; }
        [InverseProperty(nameof(FollowUp.ChangeStatusByUser))]
        public virtual ICollection<FollowUp> FollowUpChangeStatusByUsers { get; set; }
        [InverseProperty(nameof(FollowUp.CreatedByNavigation))]
        public virtual ICollection<FollowUp> FollowUpCreatedByNavigations { get; set; }
        [InverseProperty(nameof(FollowUp.DeletedByNavigation))]
        public virtual ICollection<FollowUp> FollowUpDeletedByNavigations { get; set; }
        [InverseProperty(nameof(FollowUpStatement.User))]
        public virtual ICollection<FollowUpStatement> FollowUpStatements { get; set; }
        [InverseProperty(nameof(FollowUpStatus.CreatedBy))]
        public virtual ICollection<FollowUpStatus> FollowUpStatuses { get; set; }
        [InverseProperty(nameof(FollowUp.User))]
        public virtual ICollection<FollowUp> FollowUpUsers { get; set; }
        [InverseProperty(nameof(Gender.CreatedByNavigation))]
        public virtual ICollection<Gender> Genders { get; set; }
        [InverseProperty(nameof(HangFireJobScheduling.UpdatedByNavigation))]
        public virtual ICollection<HangFireJobScheduling> HangFireJobSchedulings { get; set; }
        [InverseProperty(nameof(ImportanceLevel.CreatedByNavigation))]
        public virtual ICollection<ImportanceLevel> ImportanceLevelCreatedByNavigations { get; set; }
        [InverseProperty(nameof(ImportanceLevel.UpdatedByNavigation))]
        public virtual ICollection<ImportanceLevel> ImportanceLevelUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(IncomingType.CreatedByNavigation))]
        public virtual ICollection<IncomingType> IncomingTypeCreatedByNavigations { get; set; }
        [InverseProperty(nameof(IncomingType.UpdatedByNavigation))]
        public virtual ICollection<IncomingType> IncomingTypeUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(IndividualRelationship.CreatedByNavigation))]
        public virtual ICollection<IndividualRelationship> IndividualRelationshipCreatedByNavigations { get; set; }
        [InverseProperty(nameof(IndividualRelationship.UpdatedByNavigation))]
        public virtual ICollection<IndividualRelationship> IndividualRelationshipUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(JobTitle.CreatedByNavigation))]
        public virtual ICollection<JobTitle> JobTitleCreatedByNavigations { get; set; }
        [InverseProperty(nameof(JobTitle.UpdatedByNavigation))]
        public virtual ICollection<JobTitle> JobTitleUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(LetterTemplate.CreatedByNavigation))]
        public virtual ICollection<LetterTemplate> LetterTemplateCreatedByNavigations { get; set; }
        [InverseProperty(nameof(LetterTemplateOrganization.CreatedByNavigation))]
        public virtual ICollection<LetterTemplateOrganization> LetterTemplateOrganizationCreatedByNavigations { get; set; }
        [InverseProperty(nameof(LetterTemplateOrganization.UpdatedByNavigation))]
        public virtual ICollection<LetterTemplateOrganization> LetterTemplateOrganizationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(LetterTemplate.UpdatedByNavigation))]
        public virtual ICollection<LetterTemplate> LetterTemplateUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Localization.CreatedByNavigation))]
        public virtual ICollection<Localization> LocalizationCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Localization.UpdatedByNavigation))]
        public virtual ICollection<Localization> LocalizationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(MasarSystemIntegratedUser.User))]
        public virtual ICollection<MasarSystemIntegratedUser> MasarSystemIntegratedUsers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Nationality> NationalityCreatedByNavigations { get; set; }
        [InverseProperty("UpdatedByNavigation")]
        public virtual ICollection<Nationality> NationalityUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Notification.CreatedByNavigation))]
        public virtual ICollection<Notification> NotificationCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Notification.DismissedByNavigation))]
        public virtual ICollection<Notification> NotificationDismissedByNavigations { get; set; }

        // for committe
       

        [InverseProperty(nameof(Notification.UpdatedByNavigation))]
        public virtual ICollection<Notification> NotificationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Notification.User))]
        public virtual ICollection<Notification> NotificationUsers { get; set; }
        [InverseProperty(nameof(OrganizationGroup.User))]
        public virtual ICollection<OrganizationGroup> OrganizationGroups { get; set; }
        [InverseProperty(nameof(PhysicalAttachmentType.CreatedByNavigation))]
        public virtual ICollection<PhysicalAttachmentType> PhysicalAttachmentTypeCreatedByNavigations { get; set; }
        [InverseProperty(nameof(PhysicalAttachmentType.UpdatedByNavigation))]
        public virtual ICollection<PhysicalAttachmentType> PhysicalAttachmentTypeUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(RelatedTransaction.CreatedByNavigation))]
        public virtual ICollection<RelatedTransaction> RelatedTransactionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(RelatedTransaction.UpdatedByNavigation))]
        public virtual ICollection<RelatedTransaction> RelatedTransactionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(ReportRequest.CreatedByNavigation))]
        public virtual ICollection<ReportRequest> ReportRequests { get; set; }
        [InverseProperty(nameof(RequiredAction.CreatedByNavigation))]
        public virtual ICollection<RequiredAction> RequiredActionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(RequiredAction.UpdatedByNavigation))]
        public virtual ICollection<RequiredAction> RequiredActionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Role.CreatedByNavigation))]
        public virtual ICollection<Role> RoleCreatedByNavigations { get; set; }
        [InverseProperty(nameof(RolePermission.CreatedByNavigation))]
        public virtual ICollection<RolePermission> RolePermissionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(RolePermission.UpdatedByNavigation))]
        public virtual ICollection<RolePermission> RolePermissionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Role.UpdatedByNavigation))]
        public virtual ICollection<Role> RoleUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Search.CreatedByNavigation))]
        public virtual ICollection<Search> SearchCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Search.UpdatedByNavigation))]
        public virtual ICollection<Search> SearchUpdatedByNavigations { get; set; }
        //[InverseProperty(nameof(Signature.CreatedByNavigation))]
        [NotMapped]
        public virtual ICollection<Signature> SignatureCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Signature.UpdatedByNavigation))]
        public virtual ICollection<Signature> SignatureUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Signature.User))]
        public virtual ICollection<Signature> SignatureUsers { get; set; }
        [InverseProperty(nameof(Tag.CreatedByNavigation))]
        public virtual ICollection<Tag> TagCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Tag.UpdatedByNavigation))]
        public virtual ICollection<Tag> TagUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAction.CreatedByNavigation))]
        public virtual ICollection<TransactionAction> TransactionActionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAction.DirectedFromUser))]
        public virtual ICollection<TransactionAction> TransactionActionDirectedFromUsers { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientAttachment.AttachmentStatusChangedByNavigation))]
        public virtual ICollection<TransactionActionRecipientAttachment> TransactionActionRecipientAttachmentAttachmentStatusChangedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientAttachment.CreatedByNavigation))]
        public virtual ICollection<TransactionActionRecipientAttachment> TransactionActionRecipientAttachmentCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientAttachment.UpdatedByNavigation))]
        public virtual ICollection<TransactionActionRecipientAttachment> TransactionActionRecipientAttachmentUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.CorrespondentUser))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipientCorrespondentUsers { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.CreatedByNavigation))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipientCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.DirectedToUser))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipientDirectedToUsers { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.RecipientStatusChangedByNavigation))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipientRecipientStatusChangedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientStatus.CreatedByNavigation))]
        public virtual ICollection<TransactionActionRecipientStatus> TransactionActionRecipientStatuses { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientUpdateStatus.CreatedByNavigation))]
        public virtual ICollection<TransactionActionRecipientUpdateStatus> TransactionActionRecipientUpdateStatuses { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.UpdatedByNavigation))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipientUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAction.UpdatedByNavigation))]
        public virtual ICollection<TransactionAction> TransactionActionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAttachment.CreatedByNavigation))]
        public virtual ICollection<TransactionAttachment> TransactionAttachmentCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAttachment.DeletedByNavigation))]
        public virtual ICollection<TransactionAttachment> TransactionAttachmentDeletedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAttachmentIndex.CreatedByNavigation))]
        public virtual ICollection<TransactionAttachmentIndex> TransactionAttachmentIndexCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAttachmentIndex.UpdatedByNavigation))]
        public virtual ICollection<TransactionAttachmentIndex> TransactionAttachmentIndexUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionAttachment.UpdatedByNavigation))]
        public virtual ICollection<TransactionAttachment> TransactionAttachmentUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Transaction.CreatedByNavigation))]
        public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Transaction.DeletedByNavigation))]
        public virtual ICollection<Transaction> TransactionDeletedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionIndividual.CreatedByNavigation))]
        public virtual ICollection<TransactionIndividual> TransactionIndividualCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionIndividual.UpdatedByNavigation))]
        public virtual ICollection<TransactionIndividual> TransactionIndividualUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionIndividual.User))]
        public virtual ICollection<TransactionIndividual> TransactionIndividualUsers { get; set; }
        [InverseProperty(nameof(TransactionRelationship.CreatedByNavigation))]
        public virtual ICollection<TransactionRelationship> TransactionRelationshipCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionRelationship.UpdatedByNavigation))]
        public virtual ICollection<TransactionRelationship> TransactionRelationshipUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionSubject.CreatedByNavigation))]
        public virtual ICollection<TransactionSubject> TransactionSubjectCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionSubject.UpdatedByNavigation))]
        public virtual ICollection<TransactionSubject> TransactionSubjectUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionSubject.User))]
        public virtual ICollection<TransactionSubject> TransactionSubjectUsers { get; set; }
        [InverseProperty(nameof(TransactionTag.CreatedByNavigation))]
        public virtual ICollection<TransactionTag> TransactionTags { get; set; }
        [InverseProperty(nameof(TransactionTypeSerialAdjustment.CreatedByNavigation))]
        public virtual ICollection<TransactionTypeSerialAdjustment> TransactionTypeSerialAdjustmentCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionTypeSerialAdjustment.UpdatedByNavigation))]
        public virtual ICollection<TransactionTypeSerialAdjustment> TransactionTypeSerialAdjustmentUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(Transaction.UpdatedByNavigation))]
        public virtual ICollection<Transaction> TransactionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionWorkFlowProcess.CreatedByNavigation))]
        public virtual ICollection<TransactionWorkFlowProcess> TransactionWorkFlowProcessCreatedByNavigations { get; set; }
        [InverseProperty(nameof(TransactionWorkFlowProcess.UpdatedByNavigation))]
        public virtual ICollection<TransactionWorkFlowProcess> TransactionWorkFlowProcessUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(UserCorrespondentOrganization.CreatedByNavigation))]
        public virtual ICollection<UserCorrespondentOrganization> UserCorrespondentOrganizationCreatedByNavigations { get; set; }
        [InverseProperty(nameof(UserCorrespondentOrganization.UpdatedByNavigation))]
        public virtual ICollection<UserCorrespondentOrganization> UserCorrespondentOrganizationUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(UserCorrespondentOrganization.User))]
        public virtual ICollection<UserCorrespondentOrganization> UserCorrespondentOrganizationUsers { get; set; }
        [InverseProperty(nameof(UserJob.User))]
        public virtual ICollection<UserJob> UserJobs { get; set; }
        [InverseProperty(nameof(UserPermission.CreatedByNavigation))]
        public virtual ICollection<UserPermission> UserPermissionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(UserPermission.UpdatedByNavigation))]
        public virtual ICollection<UserPermission> UserPermissionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(UserPermission.User))]
        public virtual ICollection<UserPermission> UserPermissionUsers { get; set; }
        [InverseProperty(nameof(UserRole.CreatedByNavigation))]
        public virtual ICollection<UserRole> UserRoleCreatedByNavigations { get; set; }
        [InverseProperty(nameof(UserRole.UpdatedByNavigation))]
        public virtual ICollection<UserRole> UserRoleUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(UserRole.User))]
        public virtual ICollection<UserRole> UserRoleUsers { get; set; }
        [InverseProperty(nameof(UserSignatureFactorAuth.User))]
        public virtual ICollection<UserSignatureFactorAuth> UserSignatureFactorAuths { get; set; }
        [InverseProperty(nameof(UserSpecificOrganizationsAndEmployee.CreatedByNavigation))]
        public virtual ICollection<UserSpecificOrganizationsAndEmployee> UserSpecificOrganizationsAndEmployeeCreatedByNavigations { get; set; }
        [InverseProperty(nameof(UserSpecificOrganizationsAndEmployee.SpecificUser))]
        public virtual ICollection<UserSpecificOrganizationsAndEmployee> UserSpecificOrganizationsAndEmployeeSpecificUsers { get; set; }
        [InverseProperty(nameof(UserSpecificOrganizationsAndEmployee.UpdatedByNavigation))]
        public virtual ICollection<UserSpecificOrganizationsAndEmployee> UserSpecificOrganizationsAndEmployeeUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(UserSpecificOrganizationsAndEmployee.User))]
        public virtual ICollection<UserSpecificOrganizationsAndEmployee> UserSpecificOrganizationsAndEmployeeUsers { get; set; }
        [InverseProperty(nameof(UserToken.User))]
        public virtual ICollection<UserToken> UserTokens { get; set; }
        [InverseProperty(nameof(UserVacation.StandByUser))]
        public virtual ICollection<UserVacation> UserVacationStandByUsers { get; set; }
        [InverseProperty(nameof(UserVacation.User))]
        public virtual ICollection<UserVacation> UserVacationUsers { get; set; }
        [InverseProperty(nameof(WorkFlowProcess.CreatedByNavigation))]
        public virtual ICollection<WorkFlowProcess> WorkFlowProcessCreatedByNavigations { get; set; }
        [InverseProperty(nameof(WorkFlowProcess.UpdatedByNavigation))]
        public virtual ICollection<WorkFlowProcess> WorkFlowProcessUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(WorkFlowTransition.CreatedByNavigation))]
        public virtual ICollection<WorkFlowTransition> WorkFlowTransitionCreatedByNavigations { get; set; }
        [InverseProperty(nameof(WorkFlowTransition.UpdatedByNavigation))]
        public virtual ICollection<WorkFlowTransition> WorkFlowTransitionUpdatedByNavigations { get; set; }
        [InverseProperty(nameof(WorkPlace.CreatedByNavigation))]
        public virtual ICollection<WorkPlace> WorkPlaceCreatedByNavigations { get; set; }
        [InverseProperty(nameof(WorkPlace.UpdatedByNavigation))]
        public virtual ICollection<WorkPlace> WorkPlaceUpdatedByNavigations { get; set; }

        // one user Can Create More than One Group
        public virtual ICollection<Group> Groups { get; set; }
        //one user can exist in more than one group
        public virtual ICollection<GroupUsers> GroupUsers { get; set; }
        public virtual ICollection<CommiteeTaskMultiMissionUser> CommiteeTaskMultiMissionUsers { get; set; }
        public virtual ICollection<Commitee> CommitteeSecretary { get; set; }
        public virtual ICollection<Commitee> CurrenHeadUnit { get; set; }
    }
}
