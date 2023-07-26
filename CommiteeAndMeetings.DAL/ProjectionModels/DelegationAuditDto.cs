using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class DelegationAuditDto
    {
        public int TransactionActionId { get; set; }
        public string TransactionActionNumber { get; set; }
        public long TransactionId { get; set; }
        public int ActionId { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public int? ReferrerTransactionActionRecipientId { get; set; }
        public string ActionNumber { get; set; }
        public int? DirectedFromUserId { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int? CreatedByUserRoleId { get; set; }
        public string CreatedByUserRoleName { get; set; }
        public string OutgoingTransactionNumber { get; set; }
        public int? OutgoingTransactionTypeId { get; set; }
        public int? OutgoingImportanceLevelId { get; set; }
        public bool OutgoingIsConfidential { get; set; }
        public bool isEmployee { get; set; }
        public int TransactionTypeId { get; set; }
        public bool? IsTransaction { get; set; } = true;
        public bool acceptPreviousTRAR { get; set; } = false;
        public List<TransactionActionRecipientsDTO> transactionActionRecipientsDTO { get; set; }
        public List<TransactionActionAttachmentDTO> transactionActionAttachmentDTO { get; set; }
        public List<AttachmentViewDTO> transactionActionAttachmentViewDTO { get; set; }
    }
}
