using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeTaskMultiMissionController : _BaseController<CommiteeTaskMultiMission, CommiteetaskMultiMissionDTO>
    {
        private readonly ICommiteeTaskMultiMissionService _ICommiteeTaskMultiMissionService;

        public CommiteeTaskMultiMissionController(ICommiteeTaskMultiMissionService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            _ICommiteeTaskMultiMissionService = businessService;
        }
    }
}
