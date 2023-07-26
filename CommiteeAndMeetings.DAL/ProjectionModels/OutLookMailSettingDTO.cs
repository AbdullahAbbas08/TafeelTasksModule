namespace Models.ProjectionModels
{
    public class OutLookMailSettingDTO
    {
        public long transactionId { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string TransactionSubject { get; set; }
        public string Subject { get; set; }
        public string createdOn { get; set; }
        public string Type { get; set; }
        public string directedToName { get; set; }
        public int? recipientStatusId { get; set; }
    }
}
