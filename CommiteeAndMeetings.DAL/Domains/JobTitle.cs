using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_JobTitles_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_JobTitles_UpdatedBy")]
    public partial class JobTitle
    {
        [Key]
        public int JobTitleId { get; set; }
        [StringLength(100)]
        public string JobTitleNameAr { get; set; }
        [StringLength(100)]
        public string JobTitleNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string JobTitleNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.JobTitleCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.JobTitleUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
