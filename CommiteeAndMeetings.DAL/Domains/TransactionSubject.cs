using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("TransactionSubject")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionSubject_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionSubject_UpdatedBy")]
    [Index(nameof(UserId), Name = "IX_TransactionSubject_UserId")]
    public partial class TransactionSubject
    {
        [Key]
        public int TransactionSubjectId { get; set; }
        public string TransactionSubjectText { get; set; }
        public int? UserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("TransactionSubjectCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("TransactionSubjectUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("TransactionSubjectUsers")]
        public virtual User User { get; set; }
    }
}
