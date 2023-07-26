using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmMigratedTransactionViewCase2
    {
        public long? Id { get; set; }
        [Required]
        [StringLength(1)]
        public string DelegationType { get; set; }
        public bool? IsPending { get; set; }
        [Required]
        [StringLength(1)]
        public string InstructionsRecipient { get; set; }
        [Column("IsCC")]
        public bool? IsCc { get; set; }
        public bool? IsSaved { get; set; }
        public long? MigratedTransactionActionId { get; set; }
        public long? MigratedTransactionActionRecipientId { get; set; }
        [Required]
        [StringLength(1)]
        public string ParticipantOrganizationName { get; set; }
        [Required]
        [StringLength(1)]
        public string ParticipantType { get; set; }
        [Required]
        [StringLength(1)]
        public string ParticipantUserName { get; set; }
        [Required]
        [StringLength(1)]
        public string ToDeptCode { get; set; }
        [Required]
        [StringLength(1)]
        public string ToUserName { get; set; }
        [StringLength(300)]
        public string FromOrganizationName { get; set; }
        [StringLength(100)]
        public string ApplicationType { get; set; }
        [StringLength(100)]
        public string DelegationDate { get; set; }
        [StringLength(100)]
        public string ImportanceLevel { get; set; }
        public string InstructionsAction { get; set; }
        [StringLength(100)]
        public string UrgencyLevel { get; set; }
        [StringLength(100)]
        public string FromUserName { get; set; }
        [StringLength(100)]
        public string FromOrganizationCode { get; set; }
        public long MigratedTransactionId { get; set; }
        [StringLength(100)]
        public string TransactionNumber { get; set; }
        [StringLength(100)]
        public string HijriYear { get; set; }
        [StringLength(100)]
        public string TransactionDate { get; set; }
        [StringLength(100)]
        public string TransactionType { get; set; }
        [StringLength(100)]
        public string OldTransactionType { get; set; }
        public string TransactionSubject { get; set; }
        public string Remarks { get; set; }
        [StringLength(100)]
        public string Classification { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevel { get; set; }
        [StringLength(100)]
        public string CreationDate { get; set; }
        [StringLength(300)]
        public string RegisteredByOrganizationName { get; set; }
        [StringLength(300)]
        public string ByEmployeeName { get; set; }
        [StringLength(300)]
        public string ByDepartmentName { get; set; }
        [StringLength(100)]
        public string IncomingNumber { get; set; }
        [StringLength(100)]
        public string IncomingDate { get; set; }
        [StringLength(300)]
        public string IncomingFromOrganizationName { get; set; }
        [StringLength(200)]
        public string IncomingReceiveMode { get; set; }
        public bool IsTransfered { get; set; }
        public bool? IsActive { get; set; }
    }
}
