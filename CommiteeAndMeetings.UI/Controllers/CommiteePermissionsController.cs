using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteePermissionsController : _BaseController<CommitePermission, CommitePermissionDTO>
    {
        private readonly ICommiteePermissionService _commiteePermissionService;
        public CommiteePermissionsController(ICommiteePermissionService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteePermissionService = businessService;
        }
    }
}