using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_ReportRequests_CreatedBy")]
    public partial class ReportRequest
    {
        [Key]
        [StringLength(36)]
        public string ReportRequestId { get; set; }
        public string ReportName { get; set; }
        public string ReportParameters { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.ReportRequests))]
        public virtual User CreatedByNavigation { get; set; }
    }
}
