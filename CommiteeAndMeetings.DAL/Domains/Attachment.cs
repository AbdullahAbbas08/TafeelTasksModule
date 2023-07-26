using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentTypeId), Name = "IX_Attachments_AttachmentTypeId")]
    [Index(nameof(CreatedBy), Name = "IX_Attachments_CreatedBy")]
    [Index(nameof(PhysicalAttachmentTypeId), Name = "IX_Attachments_PhysicalAttachmentTypeId")]
    [Index(nameof(ReferenceAttachmentId), Name = "IX_Attachments_ReferenceAttachmentId")]
    [Index(nameof(UpdatedBy), Name = "IX_Attachments_UpdatedBy")]
    [Index(nameof(AttachmentId), nameof(PhysicalAttachmentTypeId), nameof(AttachmentTypeId), Name = "_dta_index_Attachments_11_1906105831__K1_K10_K3_2_4_5_6_7_8_11_12_13_17")]
    [Index(nameof(AttachmentId), nameof(AttachmentTypeId), nameof(PhysicalAttachmentTypeId), Name = "_dta_index_Attachments_11_1906105831__K1_K3_K10_2_4_5_6_7_8_11_12_13_17")]
    public partial class Attachment
    {
        public Attachment()
        {
            Annotations = new HashSet<Annotation>();
            AttachmentTags = new HashSet<AttachmentTag>();
            AttachmentVersions = new HashSet<AttachmentVersion>();
            ChatMessages = new HashSet<ChatMessage>();
            DeliveryCorrespondentTransactions = new HashSet<DeliveryCorrespondentTransaction>();
            DeliverySheetAttachments = new HashSet<DeliverySheetAttachment>();
            InverseReferenceAttachment = new HashSet<Attachment>();
            TransactionAttachments = new HashSet<TransactionAttachment>();
        }

        [Key]
        public int AttachmentId { get; set; }
        [StringLength(500)]
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        [StringLength(500)]
        public string OriginalName { get; set; }
        [StringLength(500)]
        public string MimeType { get; set; }
        public int? Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? PhysicalAttachmentTypeId { get; set; }
        public int? PagesCount { get; set; }
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [Column("LFEntryId")]
        [StringLength(100)]
        public string LfentryId { get; set; }
        public bool IsDisabled { get; set; }
        public int? ReferenceAttachmentId { get; set; }

        [ForeignKey(nameof(AttachmentTypeId))]
        [InverseProperty("Attachments")]
        public virtual AttachmentType AttachmentType { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.AttachmentCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(PhysicalAttachmentTypeId))]
        [InverseProperty("Attachments")]
        public virtual PhysicalAttachmentType PhysicalAttachmentType { get; set; }
        [ForeignKey(nameof(ReferenceAttachmentId))]
        [InverseProperty(nameof(Attachment.InverseReferenceAttachment))]
        public virtual Attachment ReferenceAttachment { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.AttachmentUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Annotation.Attachment))]
        public virtual ICollection<Annotation> Annotations { get; set; }
        [InverseProperty(nameof(AttachmentTag.Attachment))]
        public virtual ICollection<AttachmentTag> AttachmentTags { get; set; }
        [InverseProperty(nameof(AttachmentVersion.Attachment))]
        public virtual ICollection<AttachmentVersion> AttachmentVersions { get; set; }
        [InverseProperty(nameof(ChatMessage.Attachment))]
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        [InverseProperty(nameof(DeliveryCorrespondentTransaction.Attachment))]
        public virtual ICollection<DeliveryCorrespondentTransaction> DeliveryCorrespondentTransactions { get; set; }
        [InverseProperty(nameof(DeliverySheetAttachment.Attachment))]
        public virtual ICollection<DeliverySheetAttachment> DeliverySheetAttachments { get; set; }
        [InverseProperty(nameof(Attachment.ReferenceAttachment))]
        public virtual ICollection<Attachment> InverseReferenceAttachment { get; set; }
        [InverseProperty(nameof(TransactionAttachment.Attachment))]
        public virtual ICollection<TransactionAttachment> TransactionAttachments { get; set; }
    }
}
