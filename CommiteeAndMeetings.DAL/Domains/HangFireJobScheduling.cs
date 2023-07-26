using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(UpdatedBy), Name = "IX_HangFireJobSchedulings_UpdatedBy")]
    public partial class HangFireJobScheduling
    {
        [Key]
        public string HangFireJobSchedulingId { get; set; }
        public string CronDate { get; set; }
        public bool IsActive { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string HangFireJobNameAr { get; set; }
        public string HangFireJobNameEn { get; set; }
        public string HangFireJobNameFn { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.HangFireJobSchedulings))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
