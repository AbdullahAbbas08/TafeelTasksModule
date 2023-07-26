using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("TransactionAttachmentIndex")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionAttachmentIndex_CreatedBy")]
    [Index(nameof(TransactionAttachmentId), Name = "IX_TransactionAttachmentIndex_TransactionAttachmentId")]
    [Index(nameof(TransactionId), Name = "IX_TransactionAttachmentIndex_TransactionId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionAttachmentIndex_UpdatedBy")]
    public partial class TransactionAttachmentIndex
    {
        [Key]
        public long AttachementIndexId { get; set; }
        public long TransactionId { get; set; }
        public int TransactionAttachmentId { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        [StringLength(50)]
        public string FromPage { get; set; }
        [StringLength(50)]
        public string ToPage { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string FromOrg { get; set; }
        public DateTime? IndexDate { get; set; }
        public string IndexType { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionAttachmentIndexCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransactionAttachmentIndices")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey(nameof(TransactionAttachmentId))]
        [InverseProperty("TransactionAttachmentIndices")]
        public virtual TransactionAttachment TransactionAttachment { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionAttachmentIndexUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
