using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_UserPermissions_CreatedBy")]
    [Index(nameof(OrganizationId), Name = "IX_UserPermissions_OrganizationId")]
    [Index(nameof(PermissionId), Name = "IX_UserPermissions_PermissionId")]
    [Index(nameof(RoleId), Name = "IX_UserPermissions_RoleId")]
    [Index(nameof(UpdatedBy), Name = "IX_UserPermissions_UpdatedBy")]
    public partial class UserPermission
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int PermissionId { get; set; }
        [Key]
        public int RoleId { get; set; }
        [Key]
        public int OrganizationId { get; set; }
        public bool Enabled { get; set; }
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("UserPermissionCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("UserPermissions")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(PermissionId))]
        [InverseProperty("UserPermissions")]
        public virtual Permission Permission { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("UserPermissions")]
        public virtual Role Role { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("UserPermissionUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserPermissionUsers")]
        public virtual User User { get; set; }
        public UserPermission ShallowCopy()
        {
            return (UserPermission)this.MemberwiseClone();
        }
    }
}
