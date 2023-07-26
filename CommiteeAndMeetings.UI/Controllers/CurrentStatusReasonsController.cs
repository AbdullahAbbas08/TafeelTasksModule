using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentStatusReasonsController : _BaseController<CurrentStatusReason, CurrentStatusReasonDTO>
    {
        private readonly ICurrentStatusReasonService _currentStatusReasonService;
        public CurrentStatusReasonsController(ICurrentStatusReasonService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._currentStatusReasonService = businessService;
        }
    }
}
