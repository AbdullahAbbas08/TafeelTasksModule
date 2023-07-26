using System;

namespace Models.ProjectionModels
{
    public class TransactionActionDTO
    {
        public int TransactionActionId { get; set; }
        public string TransactionActionNumber { get; set; }
        public long TransactionId { get; set; }
        public int ActionId { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public int? ReferrerTransactionActionRecipientId { get; set; }
        public int? DirectedFromUserId { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int? CreatedByUserRoleId { get; set; }
        public DateTimeOffset? DisabledUntil { get; set; }
        public string OutgoingTransactionNumber { get; set; }
        public int? OutgoingTransactionTypeId { get; set; }
        public int? OutgoingImportanceLevelId { get; set; }
        public bool OutgoingIsConfidential { get; set; }
    }
}
