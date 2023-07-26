using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class MigratedTransaction
    {
        public MigratedTransaction()
        {
            MigratedRelatedTransactionMigratedTransactions = new HashSet<MigratedRelatedTransaction>();
            MigratedRelatedTransactionRelatedMigratedTransactions = new HashSet<MigratedRelatedTransaction>();
            MigratedTransactionActions = new HashSet<MigratedTransactionAction>();
            MigratedTransactionAttachments = new HashSet<MigratedTransactionAttachment>();
            RelatedTransactions = new HashSet<RelatedTransaction>();
        }

        [Key]
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
        public bool? IsActive { get; set; }
        public bool IsTransfered { get; set; }
        [Column("Document_ID")]
        public string DocumentId { get; set; }

        [InverseProperty(nameof(MigratedRelatedTransaction.MigratedTransaction))]
        public virtual ICollection<MigratedRelatedTransaction> MigratedRelatedTransactionMigratedTransactions { get; set; }
        [InverseProperty(nameof(MigratedRelatedTransaction.RelatedMigratedTransaction))]
        public virtual ICollection<MigratedRelatedTransaction> MigratedRelatedTransactionRelatedMigratedTransactions { get; set; }
        [InverseProperty(nameof(MigratedTransactionAction.MigratedTransaction))]
        public virtual ICollection<MigratedTransactionAction> MigratedTransactionActions { get; set; }
        [InverseProperty(nameof(MigratedTransactionAttachment.MigratedTransaction))]
        public virtual ICollection<MigratedTransactionAttachment> MigratedTransactionAttachments { get; set; }
        [InverseProperty(nameof(RelatedTransaction.ChildOldTransaction))]
        public virtual ICollection<RelatedTransaction> RelatedTransactions { get; set; }
    }
}
