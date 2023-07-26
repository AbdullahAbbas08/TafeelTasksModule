using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingURlDTO
    {
        public int Id { get; set; }
        public string OnlineUrl { get; set; }

        public int MeetingId { get; set; }
        public virtual MeetingDTO Meeting { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}