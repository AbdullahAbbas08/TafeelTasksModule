using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(OrganizationId), Name = "IX_ExternalOrgnaiztionStatuses_OrganizationId")]
    public partial class ExternalOrgnaiztionStatus
    {
        [Key]
        public int ExternalOrgnaiztionStatuseId { get; set; }
        public int TransactionActionId { get; set; }
        public int OrganizationId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("ExternalOrgnaiztionStatuses")]
        public virtual Organization Organization { get; set; }
    }
}
