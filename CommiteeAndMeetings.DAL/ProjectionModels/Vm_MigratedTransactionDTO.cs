using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.ProjectionModels
{
    public class Vm_MigratedTransactionDTO
    {
        //-----------MigratedTransactionActionRecipient
        public long Id { get; set; }

        public long MigratedTransactionId { get; set; }

        public long? MigratedTransactionActionId { get; set; }

        public long MigratedTransactionActionRecipientId { get; set; }

        public string DelegationType { get; set; }

        public string ParticipantUserName { get; set; }

        public string ParticipantOrganizationName { get; set; }

        public string ParticipantType { get; set; }

        public string InstructionsRecipient { get; set; }

        public bool? IsSaved { get; set; }

        public bool? IsCC { get; set; }


        public string ToDeptCode { get; set; }

        public bool? IsPending { get; set; }

        //----------------------------------- MigratedTransactionAction
        public string FromOrganizationName { get; set; }

        public string UrgencyLevel { get; set; }

        public string ImportanceLevel { get; set; }

        public string DelegationDate { get; set; }

        public string InstructionsAction { get; set; }

        public string ApplicationType { get; set; }

        //Migrated TransactionsactionRecipients


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
        [NotMapped]
        public string MigratedTransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.MigratedTransactionId.ToString()); } }
        [NotMapped]
        public string MigratedTransactionActionIdEncrypt { get { return EncryptHelper.Encrypt(this.MigratedTransactionActionId.ToString()); } }
        [NotMapped]
        public string MigratedTransactionActionRecipientIdEncrypt { get { return EncryptHelper.Encrypt(this.MigratedTransactionActionRecipientId.ToString()); } }
        public IEnumerable<MigratedAttachmentViewDTO> migratedTransactionAttachments { get; set; }
    }
}
