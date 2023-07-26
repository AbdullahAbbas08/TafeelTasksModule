namespace Models.ProjectionModels
{
    public class DeliverySheetDetailsDTO
    {
        public int DeliverySheetId { get; set; }

        public string DeliverySheetNumber { get; set; }

        public int CorrespondentUserId { get; set; }
        public int DeliverySheetStatusId { get; set; }

        public bool IsExternal { get; set; }

        public virtual UserDetailsDTO CorrespondentUser { get; set; }

    }
}
