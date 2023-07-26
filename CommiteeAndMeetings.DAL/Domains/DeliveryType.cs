using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class DeliveryType
    {
        public DeliveryType()
        {
            DeliverySheetItems = new HashSet<DeliverySheetItem>();
        }

        [Key]
        public int DeliveryTypeId { get; set; }
        [StringLength(400)]
        public string DeliveryTypeNameAr { get; set; }
        [StringLength(400)]
        public string DeliveryTypeNameEn { get; set; }
        public string DeliveryTypeNameFn { get; set; }

        [InverseProperty(nameof(DeliverySheetItem.DeliveryType))]
        public virtual ICollection<DeliverySheetItem> DeliverySheetItems { get; set; }
    }
}
