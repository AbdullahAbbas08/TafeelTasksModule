using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("TopicPauseDates", Schema = "Meeting")]
    public class TopicPauseDate : _BaseEntity, IAuditableInsertNoRole
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public virtual MeetingTopic Topic { get; set; }
        public DateTimeOffset? PauseDateTime { get; set; }
        public DateTimeOffset? ContinueDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}