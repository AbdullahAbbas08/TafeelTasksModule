using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingTopicDTO
    {

        public MeetingTopicDTO()
        {
            TopicPauseDates = new List<TopicPauseDateDTO>();
            TopicComments = new List<TopicCommentDTO>();
            TopicSurveies = new List<SurveyDTO>();
        }
        public int Id { get; set; }
        public int TopicTypeId { get; set; }
        public int MeetingId { get; set; }
        // public virtual MeetingDTO Meeting { get; set; }
        public virtual TopicType TopicType { get; set; }
        public string TopicTitle { get; set; }
        public string TopicPoints { get; set; }
        public DateTimeOffset TopicDate { get; set; }
        public DateTimeOffset TopicFromDateTime { get; set; }
        public DateTimeOffset TopicToDateTime { get; set; }
        public DateTimeOffset? TopicAcualStartDateTime { get; set; }
        public DateTimeOffset? TopicAcualEndDateTime { get; set; }
        public virtual List<TopicPauseDateDTO> TopicPauseDates { get; set; }
        public virtual List<TopicCommentDTO> TopicComments { get; set; }
        public virtual List<SurveyDTO> TopicSurveies { get; set; }
        public TopicState TopicState { get; set; } = TopicState.New;
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public ReminingDurationDTO ReminingDuration { get; set; }
    }
}