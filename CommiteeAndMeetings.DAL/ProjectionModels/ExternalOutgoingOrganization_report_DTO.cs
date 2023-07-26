using System;

namespace Models.ProjectionModels
{
    public class ExternalOutgoingOrganization_report_DTO
    {
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }
        public int? FromOrganizationId { get; set; }
        public string FromAnyAr { get; set; }
        public string FromAnyEn { get; set; }
        public int DirectedToOrganizationId { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public DateTimeOffset TransactionCreatedOn { get; set; }
        public DateTimeOffset TransactionActionCreatedOn { get; set; }
        public string StatusName { get; set; }
        public long? count { get; set; }

        public long? TransactionId { get; set; }
        public string TransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionId.ToString()); } }
        public int? RelatedTransactionsCount { get; set; }

    }
}
