using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class ConfidentialityLevel
    {
        public ConfidentialityLevel()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int ConfidentialityLevelId { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameAr { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameEn { get; set; }
        public bool IsConfidential { get; set; }
        [Column("code")]
        public string Code { get; set; }
        public string ConfidentialityLevelNameFn { get; set; }

        [InverseProperty(nameof(Transaction.ConfidentialityLevel))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
