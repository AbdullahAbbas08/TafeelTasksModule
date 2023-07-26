using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentId), Name = "IX_DeliveryCorrespondentTransactions_AttachmentId")]
    [Index(nameof(CreatedBy), Name = "IX_DeliveryCorrespondentTransactions_CreatedBy")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_DeliveryCorrespondentTransactions_TransactionActionRecipientId")]
    [Index(nameof(UpdatedBy), Name = "IX_DeliveryCorrespondentTransactions_UpdatedBy")]
    [Index(nameof(WhoIsEmpSign), Name = "IX_DeliveryCorrespondentTransactions_WhoIsEmpSign")]
    public partial class DeliveryCorrespondentTransaction
    {
        [Key]
        [Column("DeliveryCorrespondentTransactionID")]
        public int DeliveryCorrespondentTransactionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int? AttachmentId { get; set; }
        public int? WhoIsEmpSign { get; set; }
        public string WhoIsExternalSign { get; set; }
        public string Signature { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Mobile { get; set; }
        [Column("SSN")]
        public string Ssn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("DeliveryCorrespondentTransactions")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.DeliveryCorrespondentTransactionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("DeliveryCorrespondentTransactions")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.DeliveryCorrespondentTransactionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(WhoIsEmpSign))]
        [InverseProperty(nameof(User.DeliveryCorrespondentTransactionWhoIsEmpSignNavigations))]
        public virtual User WhoIsEmpSignNavigation { get; set; }
    }
}
