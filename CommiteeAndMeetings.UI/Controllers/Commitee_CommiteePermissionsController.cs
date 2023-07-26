using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Commitee_CommiteePermissionsController : _BaseController<CommiteeUsersPermission, Commitee_CommiteePermissionDTO>
    {
        private readonly ICommitee_CommiteePermissionService _commitee_CommiteePermissionService;
        public Commitee_CommiteePermissionsController(ICommitee_CommiteePermissionService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commitee_CommiteePermissionService = businessService;
        }
    }
}
