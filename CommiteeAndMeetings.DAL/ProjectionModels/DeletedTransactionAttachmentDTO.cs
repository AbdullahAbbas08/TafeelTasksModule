namespace Models.ProjectionModels
{
    public class DeletedTransactionAttachmentDTO
    {
        public long transactionId { get; set; }
        public int transactionActionId { get; set; }
        public int transactionActionRecipientId { get; set; }
        public int transactionAttachmentId { get; set; }
    }
}
