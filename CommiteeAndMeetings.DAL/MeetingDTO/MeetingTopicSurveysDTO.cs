using CommiteeAndMeetings.DAL.MeetingDomains;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingTopicSurveysDTO
    {
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }
        public TopicState TopicState { get; set; }

        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public List<IEnumerable<SurvAnswers>> SurveyAnswers { get; set; } = new List<IEnumerable<SurvAnswers>>();

    }
    public class SurvAnswers
    {
        public int UserId { get; set; }
    }
}
