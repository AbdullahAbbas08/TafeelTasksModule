using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentId), Name = "IX_DeliverySheetAttachments_AttachmentId")]
    [Index(nameof(CreatedBy), Name = "IX_DeliverySheetAttachments_CreatedBy")]
    [Index(nameof(DeliverySheetId), Name = "IX_DeliverySheetAttachments_DeliverySheetId")]
    [Index(nameof(UpdatedBy), Name = "IX_DeliverySheetAttachments_UpdatedBy")]
    public partial class DeliverySheetAttachment
    {
        [Key]
        public int DeliverySheetAttachmentId { get; set; }
        public int DeliverySheetId { get; set; }
        public int AttachmentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("DeliverySheetAttachments")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.DeliverySheetAttachmentCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeliverySheetId))]
        [InverseProperty("DeliverySheetAttachments")]
        public virtual DeliverySheet DeliverySheet { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.DeliverySheetAttachmentUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
