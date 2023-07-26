using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingActivityLookup
    {
        public string MeetingTitle { get; set; }
        public bool isCoordinator { get; set; }
        public bool isCreator { get; set; }

        public bool IsStarted { get; set; }
        public bool isClosed { get; set; }
        public DateTime MeettingDate { get; set; }
        public int MeetingId { get; set; }
        public int? SurveyId { get; set; }
        public List<MeetingTopicSurveysDTO> MeetingTopicSurveys { get; set; } = new List<MeetingTopicSurveysDTO>();
        public List<IEnumerable<IEnumerable<SurvAnswers>>> SurveyAnswers { get; set; } = new List<IEnumerable<IEnumerable<SurvAnswers>>>();
    }
}
