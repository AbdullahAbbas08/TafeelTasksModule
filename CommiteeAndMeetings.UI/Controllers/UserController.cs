using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : _BaseController<User, UserDetailsDTO>
    {
        private readonly IUsersService _userService;
        public UserController(IUsersService userService, IHelperServices.ISessionServices sessionSevices) : base(userService, sessionSevices)
        {
            _userService = userService;
        }

        [HttpGet, Route("getUsersByOrganization")]
        public DataSourceResult<UserSummaryDTO> getUsersByOrganization([FromQuery] DataSourceRequestForDelegated dataSourceRequestDelegated)
        {
            //TempData["EditFromList"] = true;
            DataSourceRequest dataSourceRequest = new DataSourceRequest
            {
                Take = dataSourceRequestDelegated.Take,
                Skip = dataSourceRequestDelegated.Skip,
                Sort = dataSourceRequestDelegated.Sort,
                Filter = dataSourceRequestDelegated.Filter,
                Countless = dataSourceRequestDelegated.Countless
            };

            return _userService.getUsersByOrganization(dataSourceRequest, dataSourceRequestDelegated.OrganizationID, dataSourceRequestDelegated.RoleID);
        }
        [HttpGet("GetById")]
        public override UserDetailsDTO Get(string id)
        {
            return _userService.GetDetails2(id);
        }

        [HttpGet]
        [Route("GetUserRolePermissionsByUserNameST")]
        [ProducesResponseType(200, Type = typeof(UserRolePermissionsDTO))]
        public UserRolePermissionsDTO GetUserRolePermissionsByUserNameST(string username, bool isDelegated)
        {
            return _userService.getuserrolepermissionsbyusername_st(username, isDelegated);
        }

        [HttpPost, Route("InsertNewUsers")]
        public async Task<IEnumerable<object>> InsertNewUsers([FromBody] List<UserDetailsDTO> entities)
        {
            return _userService.InsertNewUsers(entities);
        }

        [HttpPut, Route("UpdateUsers")]
        public IEnumerable<UserDetailsDTO> UpdateUsers([FromBody] IEnumerable<UserDetailsDTO> entities)
        {
            return this._userService.UpdateUsers(entities);
        }

        [HttpGet]
        [Route("GetuserrolepermissionsbyValues")]
        [ProducesResponseType(200, Type = typeof(UserRolePermissionsDTO))]
        public UserRolePermissionsDTO GetuserrolepermissionsbyValues(int userId, int UserRoleId, int OrganizationId, int roleId, bool IsEmployeeRole, bool ForDelegate)
        {
            return _userService.GetUserRolePermissionsByValues(userId, UserRoleId, OrganizationId, roleId, IsEmployeeRole, ForDelegate);
        }

        [HttpPost]
        [Route("EditUserRolePermissionByValues")]
        public bool EditUserRolePermissionByValues(int UserId, int PermissionId, int RoleId, int OrganizationId,int? permission_case)
        {
            return _userService.EditUserRolePermissionByValues(UserId, PermissionId, RoleId, OrganizationId, permission_case);
        }
    }
}
