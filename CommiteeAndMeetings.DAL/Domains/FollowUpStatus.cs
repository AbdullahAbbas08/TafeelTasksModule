using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("FollowUpStatus")]
    [Index(nameof(CreatedById), Name = "IX_FollowUpStatus_CreatedById")]
    [Index(nameof(FollowStatusTypeId), Name = "IX_FollowUpStatus_FollowStatusTypeId")]
    [Index(nameof(FollowUpId), Name = "IX_FollowUpStatus_FollowUpId")]
    public partial class FollowUpStatus
    {
        [Key]
        public int FollowUpStatusId { get; set; }
        public int FollowUpId { get; set; }
        public int FollowStatusTypeId { get; set; }
        public int CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [InverseProperty(nameof(User.FollowUpStatuses))]
        public virtual User CreatedBy { get; set; }
        [ForeignKey(nameof(FollowStatusTypeId))]
        [InverseProperty(nameof(FollowUpStatusType.FollowUpStatuses))]
        public virtual FollowUpStatusType FollowStatusType { get; set; }
        [ForeignKey(nameof(FollowUpId))]
        [InverseProperty("FollowUpStatuses")]
        public virtual FollowUp FollowUp { get; set; }
    }
}
