using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_Roles_CreatedBy")]
    [Index(nameof(RoleNameAr), Name = "IX_Roles_RoleNameAr", IsUnique = true)]
    [Index(nameof(RoleNameEn), Name = "IX_Roles_RoleNameEn", IsUnique = true)]
    [Index(nameof(UpdatedBy), Name = "IX_Roles_UpdatedBy")]
    public partial class Role
    {
        public Role()
        {
            RolePermissions = new HashSet<RolePermission>();
            UserPermissions = new HashSet<UserPermission>();
            UserRoles = new HashSet<UserRole>();
        }

        [Key]
        public int RoleId { get; set; }
        [Required]
        [StringLength(100)]
        public string RoleNameAr { get; set; }
        [Required]
        [StringLength(100)]
        public string RoleNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsEmployeeRole { get; set; }
        public bool IsCommitteRole { get; set; } = false;
        public bool IsDelegatedEmployeeRole { get; set; }
        public string RoleNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.RoleCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.RoleUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(RolePermission.Role))]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        [InverseProperty(nameof(UserPermission.Role))]
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        [InverseProperty(nameof(UserRole.Role))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
