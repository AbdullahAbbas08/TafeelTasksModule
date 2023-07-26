using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MinuteOfMeetingDTO
    {
        public MinuteOfMeetingDTO()
        {
            Topics = new List<MinuteOfMeetingTopicDTO>();
            MOMComment = new List<MOMCommentDTO>();
        }
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public bool FromTopic { get; set; }
        public virtual List<MinuteOfMeetingTopicDTO> Topics { get; set; }
        public virtual List<MOMCommentDTO> MOMComment { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool Colsed { get; set; } = false;
    }
}