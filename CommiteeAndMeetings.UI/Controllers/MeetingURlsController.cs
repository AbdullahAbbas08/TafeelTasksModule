using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingURlsController : _BaseController<MeetingURl, MeetingURlDTO>
    {
        private readonly IMeetingURlService _meetingURlService;

        public MeetingURlsController(IMeetingURlService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._meetingURlService = businessService;
        }
    }
}