using CommiteeAndMeetings.DAL.CommiteeDTO;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class TopicActivitityDTO
    {
        public int TopicId { get; set; }
        public List<SurveyDTO> Surveys { get; set; }
        public List<TopicCommentDTO> Comments { get; set; }
    }
}
