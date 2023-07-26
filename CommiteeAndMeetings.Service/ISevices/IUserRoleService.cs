using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IUserRoleService:IBusinessService<UserRole, UserRoleDTO>
    {
        bool InsertUserRoleFromUser(IEnumerable<UserRoleDTO> entities);
        bool InsertUserRole(UserRoleDTO enttiy);
    }
}
