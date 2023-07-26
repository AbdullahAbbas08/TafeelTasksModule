using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("NotificationLog")]
    public partial class NotificationLog
    {
        [Key]
        public int NotificationLogId { get; set; }
        public DateTimeOffset SendingDate { get; set; }
        public string Subject { get; set; }
        public string Reciever { get; set; }
        public string TransactionNumber { get; set; }
    }
}
