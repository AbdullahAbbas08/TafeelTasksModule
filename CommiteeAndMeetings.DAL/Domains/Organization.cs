using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AdminOrganizationId), Name = "IX_Organizations_AdminOrganizationId")]
    [Index(nameof(CreatedBy), Name = "IX_Organizations_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_Organizations_DeletedBy")]
    [Index(nameof(FollowUpOrganizationId), Name = "IX_Organizations_FollowUpOrganizationId")]
    [Index(nameof(ManagerUserId), Name = "IX_Organizations_ManagerUserId")]
    [Index(nameof(ParentOrganizationId), Name = "IX_Organizations_ParentOrganizationId")]
    [Index(nameof(UpdatedBy), Name = "IX_Organizations_UpdatedBy")]
    [Index(nameof(OrganizationId), nameof(IsOuterOrganization), Name = "_dta_index_Organizations_45_1991938418__K1_K9_2_3_8066")]
    [Index(nameof(IsOuterOrganization), nameof(OrganizationId), Name = "_dta_index_Organizations_45_1991938418__K9_K1_7271")]
    public partial class Organization
    {
        public Organization()
        {
            CommonGroupMembers = new HashSet<CommonGroupMember>();
            DelegationReceipients = new HashSet<DelegationReceipient>();
            Ecmarchivings = new HashSet<ECMArchiving>();
            Ecmtemplates = new HashSet<Ecmtemplate>();
            ExternalOrgnaiztionStatuses = new HashSet<ExternalOrgnaiztionStatus>();
            ExternalUsers = new HashSet<ExternalUser>();
            FavoriteLists = new HashSet<FavoriteList>();
            FollowUps = new HashSet<FollowUp>();
            InverseAdminOrganization = new HashSet<Organization>();
            InverseFollowUpOrganization = new HashSet<Organization>();
            InverseParentOrganization = new HashSet<Organization>();
            LetterTemplateOrganizations = new HashSet<LetterTemplateOrganization>();
            Notifications = new HashSet<Notification>();
            OfficeTempleteOrganizations = new HashSet<OfficeTempleteOrganization>();
            OrganizationGroupMembers = new HashSet<OrganizationGroupMember>();
            Signatures = new HashSet<Signature>();
            TransactionActionRecipients = new HashSet<TransactionActionRecipient>();
            TransactionActions = new HashSet<TransactionAction>();
            Transactions = new HashSet<Transaction>();
            UserCorrespondentOrganizations = new HashSet<UserCorrespondentOrganization>();
            UserPermissions = new HashSet<UserPermission>();
            UserRoles = new HashSet<UserRole>();
            UserSpecificOrganizationsAndEmployeeOrganizations = new HashSet<UserSpecificOrganizationsAndEmployee>();
            UserSpecificOrganizationsAndEmployeeSpecificOrganizations = new HashSet<UserSpecificOrganizationsAndEmployee>();
            WorkFlowProcessFromOrganizations = new HashSet<WorkFlowProcess>();
            WorkFlowProcessToOrganizations = new HashSet<WorkFlowProcess>();
            WorkFlowTransitions = new HashSet<WorkFlowTransition>();
        }

        [Key]
        public int OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public int? ParentOrganizationId { get; set; }
        public int? RootOrganizationId { get; set; }
        public int? AdminOrganizationId { get; set; }
        public bool IsAdminOrganization { get; set; }
        public bool IsCategory { get; set; }
        public bool IsOuterOrganization { get; set; }
        public bool IsMainOrganization { get; set; }
        public bool DelegateOnlyToSiblingsAndChildren { get; set; }
        [Column("SMSAllowed")]
        public bool Smsallowed { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string FaxNumber { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public int? DisplayOrder { get; set; }
        [StringLength(450)]
        public string Code { get; set; }
        public int? ManagerUserId { get; set; }
        public byte[] StampFile { get; set; }
        public string Color { get; set; }
        public string FullPathAr { get; set; }
        public string FullPathEn { get; set; }
        [StringLength(100)]
        public string ArchFolderEntryId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int? GroupId { get; set; }
        public bool? IsMinOrganization { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? FollowUpOrganizationId { get; set; }
        public int? OrganizationWeight { get; set; }
        public bool IsBlackBox { get; set; }
        public bool? IsGeneral { get; set; }
        public bool DelegateToAllChildrenExceptChildrenOfMain { get; set; }
        public bool DelegateToItSelf { get; set; }
        public bool? IsRelatedNeed { get; set; }
        public string FullPathFn { get; set; }
        public string OrganizationNameFn { get; set; }
        public int? DepCode { get; set; }
        public bool AdminCanEnableDisableUsers { get; set; }

        [ForeignKey(nameof(AdminOrganizationId))]
        [InverseProperty(nameof(Organization.InverseAdminOrganization))]
        public virtual Organization AdminOrganization { get; set; }
        [ForeignKey(nameof(FollowUpOrganizationId))]
        [InverseProperty(nameof(Organization.InverseFollowUpOrganization))]
        public virtual Organization FollowUpOrganization { get; set; }
        [ForeignKey(nameof(ParentOrganizationId))]
        [InverseProperty(nameof(Organization.InverseParentOrganization))]
        public virtual Organization ParentOrganization { get; set; }
        [InverseProperty(nameof(CommonGroupMember.Organization))]
        public virtual ICollection<CommonGroupMember> CommonGroupMembers { get; set; }
        [InverseProperty(nameof(DelegationReceipient.DirectedToOrganization))]
        public virtual ICollection<DelegationReceipient> DelegationReceipients { get; set; }
        [InverseProperty(nameof(ECMArchiving.Organization))]
        public virtual ICollection<ECMArchiving> Ecmarchivings { get; set; }
        [InverseProperty(nameof(Ecmtemplate.Organization))]
        public virtual ICollection<Ecmtemplate> Ecmtemplates { get; set; }
        [InverseProperty(nameof(ExternalOrgnaiztionStatus.Organization))]
        public virtual ICollection<ExternalOrgnaiztionStatus> ExternalOrgnaiztionStatuses { get; set; }
        [InverseProperty(nameof(ExternalUser.Organization))]
        public virtual ICollection<ExternalUser> ExternalUsers { get; set; }
        [InverseProperty(nameof(FavoriteList.FavoriteOrganization))]
        public virtual ICollection<FavoriteList> FavoriteLists { get; set; }
        [InverseProperty(nameof(FollowUp.Organization))]
        public virtual ICollection<FollowUp> FollowUps { get; set; }
        [InverseProperty(nameof(Organization.AdminOrganization))]
        public virtual ICollection<Organization> InverseAdminOrganization { get; set; }
        [InverseProperty(nameof(Organization.FollowUpOrganization))]
        public virtual ICollection<Organization> InverseFollowUpOrganization { get; set; }
        [InverseProperty(nameof(Organization.ParentOrganization))]
        public virtual ICollection<Organization> InverseParentOrganization { get; set; }
        [InverseProperty(nameof(LetterTemplateOrganization.Organization))]
        public virtual ICollection<LetterTemplateOrganization> LetterTemplateOrganizations { get; set; }
        [InverseProperty(nameof(Notification.Organization))]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty(nameof(OfficeTempleteOrganization.Organization))]
        public virtual ICollection<OfficeTempleteOrganization> OfficeTempleteOrganizations { get; set; }
        [InverseProperty(nameof(OrganizationGroupMember.Organization))]
        public virtual ICollection<OrganizationGroupMember> OrganizationGroupMembers { get; set; }
        [InverseProperty(nameof(Signature.Organization))]
        public virtual ICollection<Signature> Signatures { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.DirectedToOrganization))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipients { get; set; }
        [InverseProperty(nameof(TransactionAction.DirectedFromOrganization))]
        public virtual ICollection<TransactionAction> TransactionActions { get; set; }
        [InverseProperty(nameof(Transaction.IncomingOrganization))]
        public virtual ICollection<Transaction> Transactions { get; set; }
        [InverseProperty(nameof(UserCorrespondentOrganization.Organization))]
        public virtual ICollection<UserCorrespondentOrganization> UserCorrespondentOrganizations { get; set; }
        [InverseProperty(nameof(UserPermission.Organization))]
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        [InverseProperty(nameof(UserRole.Organization))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        [InverseProperty(nameof(UserSpecificOrganizationsAndEmployee.Organization))]
        public virtual ICollection<UserSpecificOrganizationsAndEmployee> UserSpecificOrganizationsAndEmployeeOrganizations { get; set; }
        [InverseProperty(nameof(UserSpecificOrganizationsAndEmployee.SpecificOrganization))]
        public virtual ICollection<UserSpecificOrganizationsAndEmployee> UserSpecificOrganizationsAndEmployeeSpecificOrganizations { get; set; }
        [InverseProperty(nameof(WorkFlowProcess.FromOrganization))]
        public virtual ICollection<WorkFlowProcess> WorkFlowProcessFromOrganizations { get; set; }
        [InverseProperty(nameof(WorkFlowProcess.ToOrganization))]
        public virtual ICollection<WorkFlowProcess> WorkFlowProcessToOrganizations { get; set; }
        [InverseProperty(nameof(WorkFlowTransition.Organization))]
        public virtual ICollection<WorkFlowTransition> WorkFlowTransitions { get; set; }
    }
}
