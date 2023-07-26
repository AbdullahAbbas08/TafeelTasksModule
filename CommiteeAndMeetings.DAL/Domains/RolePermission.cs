using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_RolePermissions_CreatedBy")]
    [Index(nameof(PermissionId), Name = "IX_RolePermissions_PermissionId")]
    [Index(nameof(UpdatedBy), Name = "IX_RolePermissions_UpdatedBy")]
    public partial class RolePermission
    {
        [Key]
        public int RoleId { get; set; }
        [Key]
        public int PermissionId { get; set; }
        public bool Enabled { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.RolePermissionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(PermissionId))]
        [InverseProperty("RolePermissions")]
        public virtual Permission Permission { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("RolePermissions")]
        public virtual Role Role { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.RolePermissionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
