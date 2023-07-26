using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VwTransactionInfo
    {
        public long Id { get; set; }
        [Column("TransactionID")]
        public long? TransactionId { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        [StringLength(50)]
        public string IdentificationNumber { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        public string TransactionStatus { get; set; }
        [Column("SSN")]
        [StringLength(50)]
        public string Ssn { get; set; }
        [StringLength(50)]
        public string IncomingLetterNumber { get; set; }
        public bool? IsSaved { get; set; }
        public string Instructions { get; set; }
        [Column("ActionID")]
        [StringLength(30)]
        public string ActionId { get; set; }
    }
}
