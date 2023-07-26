namespace Models.ProjectionModels
{
    public class TransactionActionRecipientStatusDTO
    {
        public int TransactionActionRecipientStatusId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int RecipientStatusId { get; set; }
        public string Notes { get; set; }

    }
}
