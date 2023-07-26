using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Services.ISevices;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IMeetingTopicService : IBusinessService<MeetingTopic, MeetingTopicDTO>
    {
        TopicActivitityDTO GetTopicActivities(int topicId);
        List<MeetingTopicLookupDTO> GetMeettingTopicLookup(int meetingId);
        TopicDateDTO TopicStartEnd(int topicId, StartStop startStop);
        bool TopicPauseResume(int topicId, PauseResume pauseResume);
        TopicDateDTO NextTopic(int currentTopic, int nextTopic, int currentIndex);
        //  ReminingDurationDTO GetPausesSeconds(MeetingTopicDTO topic);
        List<MeetingTopicDTO> GetAllTopics(int meetingId);
        bool ChangeTopicState(int topicId, TopicState state);
    }
}
