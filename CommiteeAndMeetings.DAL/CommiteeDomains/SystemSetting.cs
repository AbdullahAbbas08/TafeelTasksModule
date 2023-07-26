using CommiteeDatabase.Models.Domains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("CommitteeMeetingSystemSetting", Schema = "Committe")]
    public class CommitteeMeetingSystemSetting : _BaseEntity
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
    }
}
