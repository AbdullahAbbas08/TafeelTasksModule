using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingAttendeesController : _BaseController<MeetingAttendee, MeetingAttendeeDTO>
    {
        private readonly IMeetingAttendeeService _meetingAttendeeService;
        public MeetingAttendeesController(IMeetingAttendeeService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._meetingAttendeeService = businessService;
        }
    }
}