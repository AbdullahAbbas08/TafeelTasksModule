using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentStatusController : _BaseController<CurrentStatus, CurrentStatusDTO>
    {
        private readonly ICurrentStatusService _currentStatusService;
        public CurrentStatusController(ICurrentStatusService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._currentStatusService = businessService;
        }
    }
}
