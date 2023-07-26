using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommitePermissions", Schema = "Committe")]
    public class CommitePermission : _BaseEntity
    {
        public CommitePermission()
        {
            UserPermissions = new HashSet<CommiteeUsersPermission>();
            // RolePermissions = new HashSet<CommiteeRolePermission>();
        }
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int CommitePermissionId { get; set; }

        [MaxLength(200)]
        public string PermissionCode { get; set; }

        [MaxLength(200)]
        public string CommitePermissionNameAr { get; set; }

        [MaxLength(200)]
        public string CommitePermissionNameEn { get; set; }
        public string CommitePermissionNameFn { get; set; }


        [MaxLength(300)]
        public string URL { get; set; }
        [MaxLength(20)]
        public string Method { get; set; }
        public bool Enabled { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? PermissionCategoryId { get; set; }
        #region Navigation Properties
        [ForeignKey("PermissionCategoryId")]
        public virtual CommiteePermissionCategory PermissionCategory { get; set; }
        [InverseProperty("Permission")]
        public virtual ICollection<CommiteeUsersPermission> UserPermissions { get; set; }
        #endregion Navigation Properties
    }
}