using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class EventReminder
    {
        [Key]
        public int EventReminderId { get; set; }
        [StringLength(150)]
        public string Subject { get; set; }
        [StringLength(150)]
        public string Url { get; set; }
        public DateTimeOffset? StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
