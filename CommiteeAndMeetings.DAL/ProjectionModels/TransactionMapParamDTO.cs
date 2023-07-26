namespace Models.ProjectionModels
{
    public class TransactionMapParamDTO
    {
        public long MigratedTransactionId { get; set; }
        public int MigratedtransactionActionId { get; set; }
        public int MigratedtransactionActionRecipientId { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
