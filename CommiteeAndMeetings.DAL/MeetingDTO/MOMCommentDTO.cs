using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using Models;
using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MOMCommentDTO
    {
        public int Id { get; set; }
        public int MinuteOfMeetingId { get; set; }
        public virtual MinuteOfMeetingDTO MinuteOfMeeting { get; set; }
        public int CommentId { get; set; }
        public virtual CommentDTO Comment { get; set; }
        public CommentType CommentType { get; set; }
        public int? CreatedBy { get; set; }
        public virtual UserDetailsDTO CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
