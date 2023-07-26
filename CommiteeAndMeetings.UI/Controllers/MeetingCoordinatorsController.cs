using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingCoordinatorsController : _BaseController<MeetingCoordinator, MeetingCoordinatorDTO>
    {
        private readonly IMeetingCoordinatorService _meetingCoordinatorService;
        public MeetingCoordinatorsController(IMeetingCoordinatorService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._meetingCoordinatorService = businessService;
        }
    }
}
