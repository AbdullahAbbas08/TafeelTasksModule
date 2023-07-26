using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class MigratedTransactionDetailsDTO
    {

        public long MigratedTransactionId { get; set; }

        public string TransactionNumber { get; set; }

        public string HijriYear { get; set; }

        public string TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public string OldTransactionType { get; set; }

        public string TransactionSubject { get; set; }

        public string Remarks { get; set; }

        public string Classification { get; set; }

        public string ConfidentialityLevel { get; set; }

        public string CreationDate { get; set; }

        public string RegisteredByOrganizationName { get; set; }

        public string ByEmployeeName { get; set; }

        public string ByDepartmentName { get; set; }

        public string IncomingNumber { get; set; }

        public string IncomingDate { get; set; }

        public string IncomingFromOrganizationName { get; set; }

        public string IncomingReceiveMode { get; set; }
        public bool? IsActive { get; set; }
        public IEnumerable<MigratedAttachmentViewDTO> migratedTransactionAttachments { get; set; }
        public MigratedTransactionActionDTO MigratedTransactionActionDTO { get; set; }
        public MigratedTransactionActionRecipientDTO MigratedTransactionActionRecipientDTO { get; set; }
    }
}
