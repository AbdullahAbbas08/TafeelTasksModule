using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class EmailTransaction
    {
        [Key]
        public int Id { get; set; }
        public long TransactionActionRecipientId { get; set; }
        public int UserId { get; set; }
        public DateTime SendDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
