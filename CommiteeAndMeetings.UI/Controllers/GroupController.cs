using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using LinqHelper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : _BaseController<Group, GroupDto>
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._groupService = businessService;
        }

        
        [HttpPost, Route("InsertGroup")]
        public Group InsertGroup([FromBody] GroupDto group)
        {
            return this._groupService.InsertGroup(group);
        }

        [HttpDelete, Route("DeleteGroupFromCreatedUser")]
        public bool DeleteGroupFromCreatedUser(int groupId)
        {
            return this._groupService.DeleteGroupFromCreatedUser(groupId);
        }
        
        [HttpGet("GetAllForUser")]
        public DataSourceResult<GroupDto> GetAllForUser([FromQuery] DataSourceRequest dataSourceRequest ,int createdUserId, bool withTracking = true)
        {
            return this._groupService.GetAllForUser(dataSourceRequest,createdUserId,withTracking);
        }
       
        
        [HttpGet,Route("GetByGroupId")]
        public GroupDto GetByGroupId (int groupId)
        {
            return this._groupService.GetByGroupId(groupId);
        }
    }
    
   
}
