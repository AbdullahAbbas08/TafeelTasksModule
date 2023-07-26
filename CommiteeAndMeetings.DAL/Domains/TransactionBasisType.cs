using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class TransactionBasisType
    {
        public TransactionBasisType()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int TransactionBasisTypeId { get; set; }
        [StringLength(400)]
        public string TransactionBasisTypeNameAr { get; set; }
        [StringLength(400)]
        public string TransactionBasisTypeNameEn { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; }
        public string TransactionBasisTypeNameFn { get; set; }

        [InverseProperty(nameof(Transaction.TransactionBasisType))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
