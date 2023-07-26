using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class TransactionType
    {
        public TransactionType()
        {
            ActionReferralModes = new HashSet<ActionReferralMode>();
            TransactionTypeClassifications = new HashSet<TransactionTypeClassification>();
            TransactionTypeSerialAdjustments = new HashSet<TransactionTypeSerialAdjustment>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int TransactionTypeId { get; set; }
        [StringLength(400)]
        public string TransactionTypeCode { get; set; }
        [StringLength(400)]
        public string TransactionTypeNameAr { get; set; }
        [StringLength(400)]
        public string TransactionTypeNameEn { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsExternal { get; set; }
        public bool IsInternal { get; set; }
        public bool IsDecision { get; set; }
        public string TransactionTypeCodeForSerial { get; set; }
        public bool AllowDelegateToMulti { get; set; }
        public string TransactionTypeNameFn { get; set; }
        public bool AddOrgCodeToSerial { get; set; }

        [InverseProperty(nameof(ActionReferralMode.TransactionType))]
        public virtual ICollection<ActionReferralMode> ActionReferralModes { get; set; }
        [InverseProperty(nameof(TransactionTypeClassification.TransactionType))]
        public virtual ICollection<TransactionTypeClassification> TransactionTypeClassifications { get; set; }
        [InverseProperty(nameof(TransactionTypeSerialAdjustment.TransactionType))]
        public virtual ICollection<TransactionTypeSerialAdjustment> TransactionTypeSerialAdjustments { get; set; }
        [InverseProperty(nameof(Transaction.TransactionType))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
