using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(SystemSettingCategoryId), Name = "IX_SystemSettings_SystemSettingCategoryId")]
    [Index(nameof(SystemSettingCode), Name = "NonClusteredIndex-20200301-164354", IsUnique = true)]
    public partial class SystemSetting
    {
        [Key]
        public int SystemSettingId { get; set; }
        [StringLength(400)]
        public string SystemSettingCode { get; set; }
        [StringLength(400)]
        public string SystemSettingNameAr { get; set; }
        [StringLength(400)]
        public string SystemSettingNameEn { get; set; }
        public string SystemSettingValue { get; set; }
        public int? SystemSettingCategoryId { get; set; }
        public bool IsClientSide { get; set; }
        public string Instructions { get; set; }
        public bool IsHidden { get; set; }
        public string SystemSettingNameFn { get; set; }

        [ForeignKey(nameof(SystemSettingCategoryId))]
        [InverseProperty("SystemSettings")]
        public virtual SystemSettingCategory SystemSettingCategory { get; set; }
    }
}
