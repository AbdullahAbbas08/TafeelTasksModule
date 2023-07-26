using System;

namespace Models.ProjectionModels
{
    public class TransactionCorrespondentDTO
    {
        public int Id { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int? TransactionActionId { get; set; }
        public long? TransactionId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public string NormalizedSubject { get; set; }
        public string DirectedToOrganizationNameAr { get; set; }
        public string DirectedToOrganizationNameEn { get; set; }
        public string DirectedFromOrganizationNameAr { get; set; }
        public string DirectedFromOrganizationNameEn { get; set; }
        public bool? IsOuterOrganization { get; set; }
        public int? DeliveryCorrespondentTransactionID { get; set; }
        public int? CorrespondentUserId { get; set; }
        public bool hasDeliveryCorresponden { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
    }
}
