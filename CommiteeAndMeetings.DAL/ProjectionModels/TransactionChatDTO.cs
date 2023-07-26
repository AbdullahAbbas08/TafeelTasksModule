using System;

namespace Models.ProjectionModels
{
    public class TransactionChatDTO
    {
        public string TransactionSubject { get; set; }
        public long? TransactionId { get; set; }
        public DateTimeOffset? Date { get; set; }
        public int? CreatedBy { get; set; }
        public bool state { get; set; }
        public string ProfileImage { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? LastMessageDate { get; set; }
    }
}
