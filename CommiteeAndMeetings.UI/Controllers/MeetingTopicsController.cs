using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingTopicsController : _BaseController<MeetingTopic, MeetingTopicDTO>
    {
        private readonly IMeetingTopicService _meetingTopicsService;

        public MeetingTopicsController(IMeetingTopicService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._meetingTopicsService = businessService;
        }
        [HttpGet("GetTopicActivities")]
        public TopicActivitityDTO GetTopicActivities(int topicId)
        {
            return _meetingTopicsService.GetTopicActivities(topicId);
        }
        [HttpGet("GetMeettingTopicLookup")]
        public List<MeetingTopicLookupDTO> GetMeettingTopicLookup(int meetingId)
        {
            return _meetingTopicsService.GetMeettingTopicLookup(meetingId);
        }
        [HttpPost("TopicStartEnd")]

        // 1 Start , 2 End
        public TopicDateDTO TopicStartEnd(int topicId, StartStop startStop)
        {
            return _meetingTopicsService.TopicStartEnd(topicId, startStop);
        }
        [HttpPost("TopicPauseResume")]
        // 1 Pause , 2 Resume
        public bool TopicPauseResume(int topicId, PauseResume pauseResume)
        {
            return _meetingTopicsService.TopicPauseResume(topicId, pauseResume);
        }
        [HttpPost("NextTopic")]
        public TopicDateDTO NextTopic(int currentTopic, int nextTopic, int currentIndex)
        {
            return _meetingTopicsService.NextTopic(currentTopic, nextTopic, currentIndex);
        }
        [HttpGet("GetAllTopics")]
        public List<MeetingTopicDTO> GetAllTopics(int meetingId)
        {
            return _meetingTopicsService.GetAllTopics(meetingId);
        }
        [HttpGet("ChangeTopicState")]
        public bool ChangeTopicState(int topicId, TopicState state)
        {
            return _meetingTopicsService.ChangeTopicState(topicId, state);
        }
    }
}