using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionInboxDTO
    {
        public DateTimeOffset? createdOn;
        public IEnumerable<AttachmentViewDTO> transactionActionRecipientAttachment;
        public string physicalAttachment;

        public long TransactionId { get; set; }
        public int TransactionActionId { get; set; }
        public int? CorrespondentUserId { get; set; }
        public string CorrespondentUserName { get; set; }
        public int? DirectedToId { get; set; }
        public string DirectedToName { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }
    }
}
