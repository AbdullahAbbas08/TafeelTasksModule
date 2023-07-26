using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingProjectsController : _BaseController<MeetingProject, MeetingProjectDTO>
    {
        private readonly IMeetingProjectService _meetingProjectService;

        public MeetingProjectsController(IMeetingProjectService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._meetingProjectService = businessService;
        }
    }
}