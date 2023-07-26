using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class MaxReceipient
    {
        public int? DirectedFromUserId { get; set; }
        public int ActionId { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int TransactionActionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public long? RowNum { get; set; }
    }
}
