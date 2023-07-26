using System;

namespace Models.ProjectionModels
{
    public class OldTransactionSummary
    {
        public int OldTransactionId { get; set; }

        public string TransactionNumber { get; set; }

        public DateTimeOffset? TransactionDate { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Remarks { get; set; }
        public string TransactionBasis { get; set; }
    }
}
