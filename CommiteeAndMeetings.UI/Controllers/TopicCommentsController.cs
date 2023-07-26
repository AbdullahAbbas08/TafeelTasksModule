using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicCommentsController : _BaseController<TopicComment, TopicCommentDTO>
    {
        private readonly ITopicCommentService _topicCommentService;
        public TopicCommentsController(ITopicCommentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._topicCommentService = businessService;
        }
    }
}