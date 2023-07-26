using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicPauseDatesController : _BaseController<TopicPauseDate, TopicPauseDateDTO>
    {
        private readonly ITopicPauseDateService _topicPauseDateService;

        public TopicPauseDatesController(ITopicPauseDateService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._topicPauseDateService = businessService;
        }
    }
}