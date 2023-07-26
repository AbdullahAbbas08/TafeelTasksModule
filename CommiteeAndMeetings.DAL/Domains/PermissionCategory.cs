using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class PermissionCategory
    {
        public PermissionCategory()
        {
            Permissions = new HashSet<Permission>();
        }

        [Key]
        public int PermissionCategoryId { get; set; }
        [Required]
        [StringLength(400)]
        public string PermissionCategoryNameAr { get; set; }
        [Required]
        [StringLength(400)]
        public string PermissionCategoryNameEn { get; set; }
        public bool IsEmployeeCategory { get; set; }
        public string PermissionCategoryNameFn { get; set; }

        [InverseProperty(nameof(Permission.PermissionCategory))]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
