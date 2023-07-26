using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeRolePermissionsController : _BaseController<CommiteeRolePermission, CommiteeRolePermissionDTO>
    {
        private readonly ICommiteeRolePermissionService _commiteeRolePermissionService;

        public CommiteeRolePermissionsController(ICommiteeRolePermissionService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteeRolePermissionService = businessService;
        }
    }
}
