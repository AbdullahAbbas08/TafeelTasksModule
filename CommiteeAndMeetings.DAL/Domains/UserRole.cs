using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(RoleId), nameof(UserId), nameof(OrganizationId), Name = "IX_MultipleColumns", IsUnique = true)]
    [Index(nameof(CreatedBy), Name = "IX_UserRoles_CreatedBy")]
    [Index(nameof(OrganizationId), Name = "IX_UserRoles_OrganizationId")]
    [Index(nameof(UpdatedBy), Name = "IX_UserRoles_UpdatedBy")]
    [Index(nameof(UserId), Name = "IX_UserRoles_UserId")]
    [Index(nameof(UserId), nameof(OrganizationId), nameof(EnabledUntil), Name = "_dta_index_UserRoles_11_1209875477__K2_K4_K7")]
    [Index(nameof(OrganizationId), nameof(UserRoleId), Name = "_dta_index_UserRoles_45_1209875477__K4_K1_2")]
    [Index(nameof(OrganizationId), nameof(EnabledUntil), nameof(UserId), nameof(UserRoleId), Name = "_dta_index_UserRoles_45_1209875477__K4_K7_K2_K1_8066")]
    public partial class UserRole
    {
        public UserRole()
        {
            MasarExceptions = new HashSet<MasarException>();
            Searches = new HashSet<Search>();
            TransactionActionRecipientStatuses = new HashSet<TransactionActionRecipientStatus>();
            TransactionActions = new HashSet<TransactionAction>();
            TransactionAttachments = new HashSet<TransactionAttachment>();
            TransactionDetailLogs = new HashSet<TransactionDetailLog>();
        }

        [Key]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public bool? RoleOverridesUserPermissions { get; set; }
        public DateTimeOffset? EnabledSince { get; set; }
        public DateTimeOffset? EnabledUntil { get; set; }
        public string Notes { get; set; }
        public bool LastSelected { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? SyncTypeId { get; set; }
        public int? Order { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("UserRoleCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("UserRoles")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("UserRoles")]
        public virtual Role Role { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("UserRoleUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserRoleUsers")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(MasarException.UserRole))]
        public virtual ICollection<MasarException> MasarExceptions { get; set; }
        [InverseProperty(nameof(Search.CreatedByUserRole))]
        public virtual ICollection<Search> Searches { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientStatus.UserRole))]
        public virtual ICollection<TransactionActionRecipientStatus> TransactionActionRecipientStatuses { get; set; }
        [InverseProperty(nameof(TransactionAction.CreatedByUserRole))]
        public virtual ICollection<TransactionAction> TransactionActions { get; set; }
        [InverseProperty(nameof(TransactionAttachment.UserRole))]
        public virtual ICollection<TransactionAttachment> TransactionAttachments { get; set; }
        [InverseProperty(nameof(TransactionDetailLog.UserRole))]
        public virtual ICollection<TransactionDetailLog> TransactionDetailLogs { get; set; }
    }
}
