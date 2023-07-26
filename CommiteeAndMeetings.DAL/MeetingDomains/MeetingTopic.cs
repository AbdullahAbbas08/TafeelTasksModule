using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MeetingTopics", Schema = "Meeting")]
    public class MeetingTopic : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public int TopicTypeId { get; set; }
        public virtual TopicType TopicType { get; set; }
        public string TopicTitle { get; set; }
        public string TopicPoints { get; set; }
        public DateTimeOffset TopicDate { get; set; }
        public DateTimeOffset TopicFromDateTime { get; set; }
        public DateTimeOffset TopicToDateTime { get; set; }
        public DateTimeOffset? TopicAcualStartDateTime { get; set; }
        public DateTimeOffset? TopicAcualEndDateTime { get; set; }
        public virtual List<TopicPauseDate> TopicPauseDates { get; set; }
        public virtual List<TopicComment> TopicComments { get; set; }
        public virtual List<Survey> TopicSurveies { get; set; }
        public TopicState TopicState { get; set; } = TopicState.New;
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
    public enum TopicState
    {
        New = 1,
        InProgress = 2,
        Completed = 3,
        InProgressPaused = 4,
        Cancled = 5
    }
    public enum TopicType
    {
        Break = 2,
        Discussion = 1
    }
    public enum PauseResume
    {
        Pause = 1,
        Resume = 2
    }
    public enum StartStop
    {
        Start = 1,
        Stop = 2
    }
}