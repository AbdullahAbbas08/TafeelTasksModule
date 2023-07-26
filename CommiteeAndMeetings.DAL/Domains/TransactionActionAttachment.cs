using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("TransactionActionAttachment")]
    [Index(nameof(TransactionActionId), Name = "IX_TransactionActionAttachment_TransactionActionId")]
    [Index(nameof(TransactionAttachmentId), Name = "IX_TransactionActionAttachment_TransactionAttachmentId")]
    public partial class TransactionActionAttachment
    {
        [Key]
        public int TransactionActionAttachmentId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionAttachmentId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(TransactionActionId))]
        [InverseProperty("TransactionActionAttachments")]
        public virtual TransactionAction TransactionAction { get; set; }
        [ForeignKey(nameof(TransactionAttachmentId))]
        [InverseProperty("TransactionActionAttachments")]
        public virtual TransactionAttachment TransactionAttachment { get; set; }
    }
}
