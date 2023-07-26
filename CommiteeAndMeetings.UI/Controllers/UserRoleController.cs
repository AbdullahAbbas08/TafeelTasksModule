using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : _BaseController<UserRole, UserRoleDTO>
    {
        IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService businessService,
                                 IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            _userRoleService = businessService;
        }
        
        [HttpPost, Route("InsertUserRoleFromUser")]
        public bool InsertUserRoleFromUser([FromBody] IEnumerable<UserRoleDTO> entities)
        {
            return _userRoleService.InsertUserRoleFromUser(entities);
        }
    }
}
