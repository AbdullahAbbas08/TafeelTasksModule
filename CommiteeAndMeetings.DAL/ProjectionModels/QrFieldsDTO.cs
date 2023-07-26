using System;

namespace Models.ProjectionModels
{
    public class QrFieldsDTO
    {
        public string TransactionTypeValue { get; set; }
        public string TransactionNumberValue { get; set; }
        public DateTimeOffset? TransactionDateValue { get; set; }
        public int? PhysicalAttachmentsCount { get; set; }
        public DateTimeOffset? TransactionDelegationDateValue { get; set; }
        public string PhysicalAttachmentsDetails { get; set; }
        public int TransactionTypeId { get; set; }
        public int ActionId { get; set; }
        public string CompanyLogo { get; set; }
        public DateTimeOffset? AttachmentDateValue { get; set; }

    }
}
