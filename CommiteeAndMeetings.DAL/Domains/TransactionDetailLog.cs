using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(TransactionActionId), Name = "IX_TransactionDetailLogs_TransactionActionId")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_TransactionDetailLogs_TransactionActionRecipientId")]
    [Index(nameof(TransactionId), Name = "IX_TransactionDetailLogs_TransactionId")]
    [Index(nameof(UserRoleId), Name = "IX_TransactionDetailLogs_UserRoleId")]
    public partial class TransactionDetailLog
    {
        [Key]
        public int Id { get; set; }
        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int UserRoleId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public bool? FromSearch { get; set; }

        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransactionDetailLogs")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey(nameof(TransactionActionId))]
        [InverseProperty("TransactionDetailLogs")]
        public virtual TransactionAction TransactionAction { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("TransactionDetailLogs")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(UserRoleId))]
        [InverseProperty("TransactionDetailLogs")]
        public virtual UserRole UserRole { get; set; }
    }
}
