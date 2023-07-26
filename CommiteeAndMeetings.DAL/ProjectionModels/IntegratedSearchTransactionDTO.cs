namespace Models.ProjectionModels
{
    public class IntegratedSearchTransactionDTO
    {
        public string Subject { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string LastAction { get; set; }
        public string transactionStatus { get; set; }
        public string OrganizationName { get; set; }
        public string TransactionDate_string { get; set; }
    }
}