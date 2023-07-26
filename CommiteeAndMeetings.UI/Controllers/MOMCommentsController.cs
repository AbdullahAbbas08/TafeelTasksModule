using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MOMCommentsController : _BaseController<MOMComment, MOMCommentDTO>
    {
        private readonly IMOMCommentService _mOMCommentService;

        public MOMCommentsController(IMOMCommentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._mOMCommentService = businessService;
        }
    }
}
