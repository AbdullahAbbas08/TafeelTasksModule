using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AnnotationTypeId), Name = "IX_Annotations_AnnotationTypeId")]
    [Index(nameof(AttachmentId), Name = "IX_Annotations_AttachmentId")]
    [Index(nameof(CreatedBy), Name = "IX_Annotations_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_Annotations_DeletedBy")]
    [Index(nameof(ReferrerTransactionActionId), Name = "IX_Annotations_ReferrerTransactionActionId")]
    [Index(nameof(ReferrerTransactionActionRecipientId), Name = "IX_Annotations_ReferrerTransactionActionRecipientId")]
    [Index(nameof(ReferrerTransactionId), Name = "IX_Annotations_ReferrerTransactionId")]
    [Index(nameof(SignatureId), Name = "IX_Annotations_SignatureId")]
    [Index(nameof(UpdatedBy), Name = "IX_Annotations_UpdatedBy")]
    public partial class Annotation
    {
        public Annotation()
        {
            AnnotationSecurities = new HashSet<AnnotationSecurity>();
        }

        [Key]
        public int AnnotationId { get; set; }
        public int AnnotationTypeId { get; set; }
        public int AttachmentId { get; set; }
        public int? PageNumber { get; set; }
        public float? X { get; set; }
        public float? Y { get; set; }
        public string Content { get; set; }
        public long? ReferrerTransactionId { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public int? ReferrerTransactionActionRecipientId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public float? Scale { get; set; }
        public int? SignatureId { get; set; }
        public int NaturalPageWidth { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool AnnotationScope { get; set; }
        public bool IsDelegated { get; set; }
        public double? Hight { get; set; }
        public double? Width { get; set; }

        [ForeignKey(nameof(AnnotationTypeId))]
        [InverseProperty("Annotations")]
        public virtual AnnotationType AnnotationType { get; set; }
        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("Annotations")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.AnnotationCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty(nameof(User.AnnotationDeletedByNavigations))]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey(nameof(ReferrerTransactionId))]
        [InverseProperty(nameof(Transaction.Annotations))]
        public virtual Transaction ReferrerTransaction { get; set; }
        [ForeignKey(nameof(ReferrerTransactionActionId))]
        [InverseProperty(nameof(TransactionAction.Annotations))]
        public virtual TransactionAction ReferrerTransactionAction { get; set; }
        [ForeignKey(nameof(ReferrerTransactionActionRecipientId))]
        [InverseProperty(nameof(TransactionActionRecipient.Annotations))]
        public virtual TransactionActionRecipient ReferrerTransactionActionRecipient { get; set; }
        [ForeignKey(nameof(SignatureId))]
        [InverseProperty("Annotations")]
        public virtual Signature Signature { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.AnnotationUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(AnnotationSecurity.Annotation))]
        public virtual ICollection<AnnotationSecurity> AnnotationSecurities { get; set; }
    }
}
