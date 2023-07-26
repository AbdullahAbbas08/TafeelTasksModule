using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentId), Name = "IX_TransactionAttachments_AttachmentId")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionAttachments_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_TransactionAttachments_DeletedBy")]
    [Index(nameof(TransactionId), Name = "IX_TransactionAttachments_TransactionId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionAttachments_UpdatedBy")]
    [Index(nameof(UserRoleId), Name = "IX_TransactionAttachments_UserRoleId")]
    [Index(nameof(TransactionAttachmentId), nameof(AttachmentId), nameof(TransactionId), Name = "_dta_index_TransactionAttachments_11_1843641761__K1_K3_K2")]
    [Index(nameof(AttachmentId), nameof(TransactionAttachmentId), nameof(TransactionId), Name = "_dta_index_TransactionAttachments_11_1843641761__K3_K1_K2")]
    public partial class TransactionAttachment
    {
        public TransactionAttachment()
        {
            TransactionActionAttachments = new HashSet<TransactionActionAttachment>();
            TransactionActionRecipientAttachments = new HashSet<TransactionActionRecipientAttachment>();
            TransactionAttachmentIndices = new HashSet<TransactionAttachmentIndex>();
        }

        [Key]
        public int TransactionAttachmentId { get; set; }
        public long TransactionId { get; set; }
        public int AttachmentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool IsShared { get; set; }
        public int? UserRoleId { get; set; }

        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("TransactionAttachments")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionAttachmentCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty(nameof(User.TransactionAttachmentDeletedByNavigations))]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransactionAttachments")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionAttachmentUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserRoleId))]
        [InverseProperty("TransactionAttachments")]
        public virtual UserRole UserRole { get; set; }
        [InverseProperty(nameof(TransactionActionAttachment.TransactionAttachment))]
        public virtual ICollection<TransactionActionAttachment> TransactionActionAttachments { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientAttachment.TransactionAttachment))]
        public virtual ICollection<TransactionActionRecipientAttachment> TransactionActionRecipientAttachments { get; set; }
        [InverseProperty(nameof(TransactionAttachmentIndex.TransactionAttachment))]
        public virtual ICollection<TransactionAttachmentIndex> TransactionAttachmentIndices { get; set; }
    }
}
