using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class TopicCommentDTO
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public virtual MeetingTopicDTO Topic { get; set; }
        public int CommentId { get; set; }
        public virtual CommentDTO Comment { get; set; }
        public CommentType CommentType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}