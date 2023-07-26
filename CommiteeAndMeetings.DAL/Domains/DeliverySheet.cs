using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CorrespondentUserId), Name = "IX_DeliverySheets_CorrespondentUserId")]
    [Index(nameof(CreatedBy), Name = "IX_DeliverySheets_CreatedBy")]
    [Index(nameof(DeliverySheetStatusId), Name = "IX_DeliverySheets_DeliverySheetStatusId")]
    [Index(nameof(UpdatedBy), Name = "IX_DeliverySheets_UpdatedBy")]
    public partial class DeliverySheet
    {
        public DeliverySheet()
        {
            DeliverySheetAttachments = new HashSet<DeliverySheetAttachment>();
            DeliverySheetItems = new HashSet<DeliverySheetItem>();
        }

        [Key]
        public int DeliverySheetId { get; set; }
        [StringLength(25)]
        public string DeliverySheetNumber { get; set; }
        public int CorrespondentUserId { get; set; }
        public int? DeliverySheetStatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsExternal { get; set; }

        [ForeignKey(nameof(CorrespondentUserId))]
        [InverseProperty(nameof(User.DeliverySheetCorrespondentUsers))]
        public virtual User CorrespondentUser { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.DeliverySheetCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeliverySheetStatusId))]
        [InverseProperty(nameof(DeliveryStatus.DeliverySheets))]
        public virtual DeliveryStatus DeliverySheetStatus { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.DeliverySheetUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(DeliverySheetAttachment.DeliverySheet))]
        public virtual ICollection<DeliverySheetAttachment> DeliverySheetAttachments { get; set; }
        [InverseProperty(nameof(DeliverySheetItem.DeliverySheet))]
        public virtual ICollection<DeliverySheetItem> DeliverySheetItems { get; set; }
    }
}
