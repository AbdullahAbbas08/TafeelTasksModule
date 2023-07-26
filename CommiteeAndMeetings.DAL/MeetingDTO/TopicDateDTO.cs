using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class TopicDateDTO
    {
        public DateTimeOffset? CurrentStartDate { get; set; }
        public DateTimeOffset? CurrentEndDate { get; set; }
        public DateTimeOffset? NextStartDate { get; set; }
        public int? NextTopicId { get; set; }
    }
}
