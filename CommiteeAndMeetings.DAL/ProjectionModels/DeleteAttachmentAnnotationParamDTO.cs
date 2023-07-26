namespace Models.ProjectionModels
{
    public class DeleteAttachmentAnnotationParamDTO
    {
        public int annotationId { get; set; }
        public long? transactionId { get; set; }
        public int? transactionActionId { get; set; }
        public int? transactionActionRecipientId { get; set; }
        public int transactionAttachmentId { get; set; }
    }
}
