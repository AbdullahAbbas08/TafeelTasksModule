using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : _BaseController<Role, RoleDTO>
    {
        private readonly IRolesService _comitteRoleService;
        public RolesController(IRolesService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._comitteRoleService = businessService;
        }
    }
}