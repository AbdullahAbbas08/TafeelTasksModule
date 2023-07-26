namespace Models
{
    public class AnnotationSummaryDTO
    {
        public int AnnotationId { get; set; }
        public int AnnotationTypeId { get; set; }
        public int AttachmentId { get; set; }
        public int? PageNumber { get; set; }
        public float? X { get; set; }
        public float? Y { get; set; }
        public float? Scale { get; set; }
        public string Content { get; set; }
        public int SignatureId { get; set; }
        public double? Width { get; set; }
        public double? Hight { get; set; }
        public long? ReferrerTransactionId { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public int? ReferrerTransactionActionRecipientId { get; set; }
    }
}
