using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using LinqHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IGroupService: IBusinessService<Group,GroupDto>
    {
        public Group InsertGroup(GroupDto entity);
        public bool DeleteGroupFromCreatedUser(int groupId);
        DataSourceResult<GroupDto> GetAllForUser(DataSourceRequest dataSourceRequest, int createdUserId, bool WithTracking = true);
       
        public GroupDto GetByGroupId(int groupId);
    }
    
}
