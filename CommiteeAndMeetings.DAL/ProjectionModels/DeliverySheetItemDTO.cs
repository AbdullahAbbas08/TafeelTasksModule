namespace Models.ProjectionModels
{
    public class DeliverySheetItemDTO
    {
        public int DeliverySheetItemId { get; set; }
        public int DeliverySheetId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int DeliveryTypeId { get; set; }
        public int DeliveryStatusId { get; set; }
        public bool IsExternal { get; set; }

    }
}
