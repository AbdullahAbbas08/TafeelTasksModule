using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(DeliverySheetId), Name = "IX_DeliverySheetItems_DeliverySheetId")]
    [Index(nameof(DeliveryStatusId), Name = "IX_DeliverySheetItems_DeliveryStatusId")]
    [Index(nameof(DeliveryTypeId), Name = "IX_DeliverySheetItems_DeliveryTypeId")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_DeliverySheetItems_TransactionActionRecipientId")]
    [Index(nameof(UpdatedBy), Name = "IX_DeliverySheetItems_UpdatedBy")]
    public partial class DeliverySheetItem
    {
        [Key]
        public int DeliverySheetItemId { get; set; }
        public int DeliverySheetId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int DeliveryTypeId { get; set; }
        public int? DeliveryStatusId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(DeliverySheetId))]
        [InverseProperty("DeliverySheetItems")]
        public virtual DeliverySheet DeliverySheet { get; set; }
        [ForeignKey(nameof(DeliveryStatusId))]
        [InverseProperty("DeliverySheetItems")]
        public virtual DeliveryStatus DeliveryStatus { get; set; }
        [ForeignKey(nameof(DeliveryTypeId))]
        [InverseProperty("DeliverySheetItems")]
        public virtual DeliveryType DeliveryType { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("DeliverySheetItems")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.DeliverySheetItems))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
