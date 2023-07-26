namespace Models.ProjectionModels
{
    public class SystemSearchOldTransactionDTO
    {
        public string TransactionNumber { get; set; }
        public string TransactionSubject { get; set; }
        public string IncomingLetterNumber { get; set; }
        public string RegisteredByOrganizationName { get; set; }
        public string Year { get; set; }
        public string IncomingFromOrganizationName { get; set; }
        public int? @SIZE { get; set; }
    }
}
