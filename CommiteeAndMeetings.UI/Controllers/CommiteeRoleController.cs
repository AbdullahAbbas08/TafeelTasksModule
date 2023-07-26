using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeRoleController : _BaseController<DAL.CommiteeDomains.CommiteeRole, CommiteeRoleDTO>
    {
        private readonly ICommiteeRolesService _comitteRoleService;
        public CommiteeRoleController(ICommiteeRolesService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._comitteRoleService = businessService;
        }
    }
}
