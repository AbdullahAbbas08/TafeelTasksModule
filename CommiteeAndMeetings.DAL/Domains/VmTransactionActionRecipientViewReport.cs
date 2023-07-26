using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionActionRecipientViewReport
    {
        public int? TransactionActionRecipientId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? ActionId { get; set; }
        [Column("archieveDate")]
        public int? ArchieveDate { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int? DirectedFromUserId { get; set; }
        public long TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public bool? IsConfidential { get; set; }
        [Column("IsCC")]
        public bool? IsCc { get; set; }
        public int? RecipientStatusId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        [Column("TARCreationOn")]
        public DateTimeOffset? TarcreationOn { get; set; }
        public DateTimeOffset? RecipientStatusChangedOn { get; set; }
    }
}
