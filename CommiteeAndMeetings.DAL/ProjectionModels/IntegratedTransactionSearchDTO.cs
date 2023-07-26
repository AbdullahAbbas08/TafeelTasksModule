namespace Models.ProjectionModels
{
    public class IntegratedTransactionSearchDTO
    {
        public long Id { get; set; }
        public long TransactionID { get; set; }
        public string TransactionNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string Mobile { get; set; }
        public string TransactionStatus { get; set; }
        public string SSN { get; set; }
        public string IncomingLetterNumber { get; set; }
        public bool IsSaved { get; set; }
        public string Instructions { get; set; }
        public string ActionID { get; set; }
    }
}
