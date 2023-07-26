using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(PermissionCategoryId), Name = "IX_Permissions_PermissionCategoryId")]
    public partial class Permission
    {
        public Permission()
        {
            EcmarchivingPermissions = new HashSet<EcmarchivingPermission>();
            RolePermissions = new HashSet<RolePermission>();
            UserPermissions = new HashSet<UserPermission>();
        }

        [Key]
        public int PermissionId { get; set; }
        [StringLength(200)]
        public string PermissionCode { get; set; }
        [StringLength(200)]
        public string PermissionNameAr { get; set; }
        [StringLength(200)]
        public string PermissionNameEn { get; set; }
        [Column("URL")]
        [StringLength(300)]
        public string Url { get; set; }
        [StringLength(20)]
        public string Method { get; set; }
        public bool Enabled { get; set; }
        public int PermissionCategoryId { get; set; }
        public bool ForDelegate { get; set; }
        public string PermissionNameFn { get; set; }
        public bool IsCommittePermission { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(PermissionCategoryId))]
        [InverseProperty("Permissions")]
        public virtual PermissionCategory PermissionCategory { get; set; }
        [InverseProperty(nameof(EcmarchivingPermission.Permission))]
        public virtual ICollection<EcmarchivingPermission> EcmarchivingPermissions { get; set; }
        [InverseProperty(nameof(RolePermission.Permission))]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        [InverseProperty(nameof(UserPermission.Permission))]
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}
