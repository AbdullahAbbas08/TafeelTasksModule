using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionActionRecipientViewInboxEva
    {
        public int Id { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        public long? TransactionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? RecipientStatusChangedBy { get; set; }
        public int? RecipientStatusId { get; set; }
        public int? TransactionTypeId { get; set; }
        [Column("archieveDate")]
        public int? ArchieveDate { get; set; }
        public int? ActionId { get; set; }
        public int IsLate { get; set; }
        public int? MainRecipientStatusId { get; set; }
        public int? ClassificationId { get; set; }
    }
}
