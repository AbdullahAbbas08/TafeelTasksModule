using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class IntegratedSystemSearchDTO
    {
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string IncomingLetterNumber { get; set; }
        public string transactionTypeNameAr { get; set; }
        public string transactionTypeNameEn { get; set; }
        public string SystemTypeAr { get; set; }
        public string SystemTypeEn { get; set; }
        public string TransactionDate_string { get; set; }
        public string IncomingLetterDate_string { get; set; }
        public string IncomingOrganizationNameAr { get; set; }
        public string IncomingOrganizationNameEn { get; set; }
        public string CreatedByNameAr { get; set; }
        public string CreatedByNameEn { get; set; }
        public string CreatedByOrganizationNameAr { get; set; }
        public string CreatedByOrganizationNameEn { get; set; }

        public IEnumerable<AttachmentViewDTO> transactionActionRecipientAttachment { get; set; }

        public IEnumerable<MigratedAttachmentViewDTO> migratedTransactionAttachments { get; set; }

    }
}
