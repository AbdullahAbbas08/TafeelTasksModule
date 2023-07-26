using System;

namespace Models.ProjectionModels
{
    public class TransactionMapParam
    {
        public long transactionId { get; set; }
        public int transactionActionId { get; set; }
        public int transactionActionRecipientId { get; set; }
        public string directedFrom { get; set; }
        public string directedTo { get; set; }
        public string requiredActionName { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public bool? isCC { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
