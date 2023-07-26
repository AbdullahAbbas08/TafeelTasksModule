using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MinuteOfMeetingTopicDTO
    {
        public int Id { get; set; }
        public int MinuteOfMeetingId { get; set; }
        // public virtual MinuteOfMeeting MinuteOfMeeting { get; set; }
        public int MeetingTopicId { get; set; }
        public virtual MeetingTopicLookupDTO MeetingTopic { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
