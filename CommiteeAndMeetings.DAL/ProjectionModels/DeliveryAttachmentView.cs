namespace Models.ProjectionModels
{
    public class DeliveryAttachmentView
    {
        public int Id { get; set; }
        public int DeliverySheetId { get; set; }
        public int DeliverySheetAttachmentId { get; set; }
        public int AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        public string OriginalName { get; set; }
        public string MimeType { get; set; }
        public int? Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string LFEntryId { get; set; }
        public int? PagesCount { get; set; }
        public string Notes { get; set; }
    }
}
