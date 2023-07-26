using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("FollowUpStatement")]
    [Index(nameof(FollowUpId), Name = "IX_FollowUpStatement_FollowUpId")]
    [Index(nameof(FollowUpMessagingTypeId), Name = "IX_FollowUpStatement_FollowUpMessagingTypeId")]
    [Index(nameof(FollowUpStatementTypeId), Name = "IX_FollowUpStatement_FollowUpStatementTypeId")]
    [Index(nameof(UserId), Name = "IX_FollowUpStatement_UserId")]
    public partial class FollowUpStatement
    {
        [Key]
        public int FollowUpStatementId { get; set; }
        public int FollowUpId { get; set; }
        public int FollowUpStatementTypeId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsIndividual { get; set; }
        public int? FollowUpMessagingTypeId { get; set; }

        [ForeignKey(nameof(FollowUpId))]
        [InverseProperty("FollowUpStatements")]
        public virtual FollowUp FollowUp { get; set; }
        [ForeignKey(nameof(FollowUpMessagingTypeId))]
        [InverseProperty("FollowUpStatements")]
        public virtual FollowUpMessagingType FollowUpMessagingType { get; set; }
        [ForeignKey(nameof(FollowUpStatementTypeId))]
        [InverseProperty("FollowUpStatements")]
        public virtual FollowUpStatementType FollowUpStatementType { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("FollowUpStatements")]
        public virtual User User { get; set; }
    }
}
