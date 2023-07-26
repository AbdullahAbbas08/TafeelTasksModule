using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AssignmentTransactionId), Name = "IX_AssignmentComments_AssignmentTransactionId")]
    [Index(nameof(CreatedBy), Name = "IX_AssignmentComments_CreatedBy")]
    public partial class AssignmentComment
    {
        [Key]
        public int AssignmentCommentId { get; set; }
        public string Comment { get; set; }
        public int AssignmentId { get; set; }
        public long? AssignmentTransactionId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(AssignmentTransactionId))]
        [InverseProperty(nameof(Transaction.AssignmentComments))]
        public virtual Transaction AssignmentTransaction { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.AssignmentComments))]
        public virtual User CreatedByNavigation { get; set; }
    }
}
