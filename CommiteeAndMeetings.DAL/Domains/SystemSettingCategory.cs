using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class SystemSettingCategory
    {
        public SystemSettingCategory()
        {
            SystemSettings = new HashSet<SystemSetting>();
        }

        [Key]
        public int SystemSettingCategoryId { get; set; }
        [StringLength(400)]
        public string SystemSettingCategoryCode { get; set; }
        [StringLength(400)]
        public string SystemSettingCategoryNameAr { get; set; }
        [StringLength(400)]
        public string SystemSettingCategoryNameEn { get; set; }
        public string SystemSettingCategoryNameFn { get; set; }

        [InverseProperty(nameof(SystemSetting.SystemSettingCategory))]
        public virtual ICollection<SystemSetting> SystemSettings { get; set; }
    }
}
