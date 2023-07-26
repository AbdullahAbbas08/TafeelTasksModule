using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class DeliverySheetDTO
    {
        public int DeliverySheetId { get; set; }

        public string DeliverySheetNumber { get; set; }

        public int CorrespondentUserId { get; set; }
        public int DeliverySheetStatusId { get; set; }

        public virtual UserDetailsDTO CorrespondentUser { get; set; }
        public List<DeliverySheetItemDTO> DeliverySheetItems { get; set; }
    }
}
