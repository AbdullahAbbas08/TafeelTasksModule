using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class TopicPauseDateDTO
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public virtual MeetingTopicDTO Topic { get; set; }
        public DateTimeOffset PauseDateTime { get; set; }
        public DateTimeOffset? ContinueDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}