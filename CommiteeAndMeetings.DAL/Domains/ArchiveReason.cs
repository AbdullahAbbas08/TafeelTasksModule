using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("ArchiveReason")]
    public partial class ArchiveReason
    {
        [Key]
        public int ArchiveReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonNameAr { get; set; }
        public string ReasonNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public bool IsDefault { get; set; }
        [Column("IsForVIP")]
        public bool? IsForVip { get; set; }
        public string ReasonNameFn { get; set; }
    }
}
