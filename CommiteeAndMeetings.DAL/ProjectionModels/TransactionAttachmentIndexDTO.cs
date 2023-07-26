using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionAttachmentIndexDTO
    {
        public long AttachementIndexID { get; set; }
        public long TransactionID { get; set; }
        public int TransactionAttachmentId { get; set; }
        public string Subject { get; set; }
        public string FromPage { get; set; }
        public string ToPage { get; set; }
        public DateTime? IndexDate { get; set; }
        public string IndexType { get; set; }
        public string FromOrg { get; set; }
        public string DateAsString { get; set; }
    }

    public class AttachmentWithTransactionAttachmentIndexDTO
    {
        public int? PagesCount { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string TransactionSubject { get; set; }
        public string AttachmentName { get; set; }
        public List<TransactionAttachmentIndexDTO> AttachmentIndexes { get; set; }
    }

}
