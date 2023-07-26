namespace DbContexts.MasarContext.ProjectionModels
{
    public class TransactionSummaryDTO
    {
        public long TransactionId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }

    }
}
