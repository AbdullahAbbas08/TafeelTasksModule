using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommiteePermissionCategories", Schema = "Committe")]
    public class CommiteePermissionCategory : _BaseEntity
    {
        [Key]
        public int CommiteePermissionCategoryId { get; set; }

        [Required]
        [MaxLength(400)]
        public string PermissionCategoryNameAr { get; set; }

        [Required]
        [MaxLength(400)]
        public string PermissionCategoryNameEn { get; set; }
        public string PermissionCategoryNameFn { get; set; }

        public virtual ICollection<CommiteeTask> CommiteeTasks { get; set; }


        [Required]
        public bool? IsEmployeeCategory { get; set; }

        public virtual ICollection<CommitePermission> Permissions { get; set; }

    }
}
