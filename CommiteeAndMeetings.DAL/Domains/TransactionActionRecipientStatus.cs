using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_TransactionActionRecipientStatuses_CreatedBy")]
    [Index(nameof(RecipientStatusId), Name = "IX_TransactionActionRecipientStatuses_RecipientStatusId")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_TransactionActionRecipientStatuses_TransactionActionRecipientId")]
    [Index(nameof(UserRoleId), Name = "IX_TransactionActionRecipientStatuses_UserRoleId")]
    public partial class TransactionActionRecipientStatus
    {
        [Key]
        public int TransactionActionRecipientStatusId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int RecipientStatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string Notes { get; set; }
        public int? ArchiveReasonId { get; set; }
        public int? UserRoleId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientStatuses))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(RecipientStatusId))]
        [InverseProperty("TransactionActionRecipientStatuses")]
        public virtual RecipientStatus RecipientStatus { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("TransactionActionRecipientStatuses")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(UserRoleId))]
        [InverseProperty("TransactionActionRecipientStatuses")]
        public virtual UserRole UserRole { get; set; }
    }
}
