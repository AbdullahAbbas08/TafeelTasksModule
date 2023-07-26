using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class DeliveryStatus
    {
        public DeliveryStatus()
        {
            DeliverySheetItems = new HashSet<DeliverySheetItem>();
            DeliverySheets = new HashSet<DeliverySheet>();
        }

        [Key]
        public int DeliveryStatusId { get; set; }
        [StringLength(400)]
        public string DeliveryStatusCode { get; set; }
        [StringLength(400)]
        public string DeliveryStatusNameAr { get; set; }
        [StringLength(400)]
        public string DeliveryStatusNameEn { get; set; }
        public string DeliveryStatusNameFn { get; set; }

        [InverseProperty(nameof(DeliverySheetItem.DeliveryStatus))]
        public virtual ICollection<DeliverySheetItem> DeliverySheetItems { get; set; }
        [InverseProperty(nameof(DeliverySheet.DeliverySheetStatus))]
        public virtual ICollection<DeliverySheet> DeliverySheets { get; set; }
    }
}
