using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionMapDTO
    {
        //Transactions
        public int TransactionActionId { get; set; }

        public int TransactionActionRecipientId { get; set; }
        public string RequiredActionName { get; set; }
        public DateTimeOffset? createdOn { get; set; }
        public string old_createdOn { get; set; }
        public string DirectedFromName { get; set; }
        public bool IsCC { get; set; }
        public string TransactionTypeName { get; set; }
        public string createdBy { get; set; }
        public string Notes { get; set; }
        public string TransactionActionRecipientReceivedBy { get; set; }
        public DateTimeOffset? TransactionActionRecipientReceivedDate { get; set; }
        public string DirectedToName { get; set; }
        public bool IsRecipientEmployee { get; set; }
        public string fromUserProfileImage { get; set; }

        public string correspondentUserName { get; set; }

        public string subject { get; set; }
        public string physicalAttachment { get; set; }
        public string requiredAction { get; set; }
        public long transactionid { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string ImportanceLevelName { get; set; }
        public int? DirectedToId { get; set; }
        public string recipientstatusName { get; set; }
        // isCC 
        public string delegationTypeName { get; set; }

        public List<AttachmentViewDTO> TransactionActionRecipientAttachments { get; set; }
        public bool isLate { get; set; }
        public int WorkDays { get; set; }
        public bool IsNoteHidden { get; set; } = false;
    }
}
