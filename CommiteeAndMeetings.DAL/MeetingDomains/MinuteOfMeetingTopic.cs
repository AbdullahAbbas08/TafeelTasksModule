using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MinuteOfMeetingTopics", Schema = "Meeting")]
    public class MinuteOfMeetingTopic : _BaseEntity, IAuditableInsertNoRole
    {
        public int Id { get; set; }
        public int MinuteOfMeetingId { get; set; }
        public virtual MinuteOfMeeting MinuteOfMeeting { get; set; }
        public int MeetingTopicId { get; set; }
        public virtual MeetingTopic MeetingTopic { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}