using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitePermissionsController : _BaseController<CommitePermission, CommitePermissionDTO>
    {
        private readonly ICommiteePermissionService _commitePermissionService;
        public CommitePermissionsController(ICommiteePermissionService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commitePermissionService = businessService;
        }
        [HttpGet("GetPermissionLookUps")]
        public List<LookUpDTO> GetPermissionLookUps()
        {
            return _commitePermissionService.GetPermissionLookUps();
        }
    }
}
