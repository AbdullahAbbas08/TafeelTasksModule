using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("FollowUpDateModified")]
    [Index(nameof(FollowUpId), Name = "IX_FollowUpDateModified_FollowUpId")]
    public partial class FollowUpDateModified
    {
        [Key]
        public int FollowUpDateModifiedId { get; set; }
        public int FollowUpId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }

        [ForeignKey(nameof(FollowUpId))]
        [InverseProperty("FollowUpDateModifieds")]
        public virtual FollowUp FollowUp { get; set; }
    }
}
