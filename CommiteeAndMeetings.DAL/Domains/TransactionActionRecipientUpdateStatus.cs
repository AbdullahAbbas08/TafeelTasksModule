using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_TransactionActionRecipientUpdateStatuses_CreatedBy")]
    [Index(nameof(CurrentRecipientStatusId), Name = "IX_TransactionActionRecipientUpdateStatuses_CurrentRecipientStatusId")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_TransactionActionRecipientUpdateStatuses_TransactionActionRecipientId")]
    [Index(nameof(UpdateRecipientStatusId), Name = "IX_TransactionActionRecipientUpdateStatuses_UpdateRecipientStatusId")]
    public partial class TransactionActionRecipientUpdateStatus
    {
        [Key]
        public int TransactionActionRecipientUpdateStatusId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int CurrentRecipientStatusId { get; set; }
        public int UpdateRecipientStatusId { get; set; }
        public string UpdateReason { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool UpdateIsDone { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientUpdateStatuses))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(CurrentRecipientStatusId))]
        [InverseProperty(nameof(RecipientStatus.TransactionActionRecipientUpdateStatusCurrentRecipientStatuses))]
        public virtual RecipientStatus CurrentRecipientStatus { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("TransactionActionRecipientUpdateStatuses")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(UpdateRecipientStatusId))]
        [InverseProperty(nameof(RecipientStatus.TransactionActionRecipientUpdateStatusUpdateRecipientStatuses))]
        public virtual RecipientStatus UpdateRecipientStatus { get; set; }
    }
}
