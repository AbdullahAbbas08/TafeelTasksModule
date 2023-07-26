using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class TransactionSource
    {
        public TransactionSource()
        {
            RelatedTransactions = new HashSet<RelatedTransaction>();
        }

        [Key]
        public int TransactionSourceId { get; set; }
        [StringLength(400)]
        public string TransactionSourceCode { get; set; }
        [StringLength(400)]
        public string TransactionSourceNameAr { get; set; }
        [StringLength(400)]
        public string TransactionSourceNameEn { get; set; }
        public string TransactionSourceNameFn { get; set; }

        [InverseProperty(nameof(RelatedTransaction.TransactionSource))]
        public virtual ICollection<RelatedTransaction> RelatedTransactions { get; set; }
    }
}
