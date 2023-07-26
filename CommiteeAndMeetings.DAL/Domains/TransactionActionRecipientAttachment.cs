using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentStatusChangedBy), Name = "IX_TransactionActionRecipientAttachments_AttachmentStatusChangedBy")]
    [Index(nameof(AttachmentStatusId), Name = "IX_TransactionActionRecipientAttachments_AttachmentStatusId")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionActionRecipientAttachments_CreatedBy")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_TransactionActionRecipientAttachments_TransactionActionRecipientId")]
    [Index(nameof(TransactionAttachmentId), Name = "IX_TransactionActionRecipientAttachments_TransactionAttachmentId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionActionRecipientAttachments_UpdatedBy")]
    [Index(nameof(TransactionActionRecipientId), nameof(TransactionActionRecipientAttachmentId), Name = "_dta_index_TransactionActionRecipientAttach_11_2046630334__K2_K1_3_5_6_8_9_10_11")]
    [Index(nameof(TransactionActionRecipientId), nameof(TransactionAttachmentId), nameof(TransactionActionRecipientAttachmentId), Name = "_dta_index_TransactionActionRecipientAttach_11_2046630334__K2_K3_K1")]
    public partial class TransactionActionRecipientAttachment
    {
        [Key]
        public int TransactionActionRecipientAttachmentId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int TransactionAttachmentId { get; set; }
        public int? AttachmentStatusId { get; set; }
        public int? AttachmentStatusChangedBy { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(AttachmentStatusId))]
        [InverseProperty("TransactionActionRecipientAttachments")]
        public virtual AttachmentStatus AttachmentStatus { get; set; }
        [ForeignKey(nameof(AttachmentStatusChangedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientAttachmentAttachmentStatusChangedByNavigations))]
        public virtual User AttachmentStatusChangedByNavigation { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientAttachmentCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("TransactionActionRecipientAttachments")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(TransactionAttachmentId))]
        [InverseProperty("TransactionActionRecipientAttachments")]
        public virtual TransactionAttachment TransactionAttachment { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientAttachmentUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
