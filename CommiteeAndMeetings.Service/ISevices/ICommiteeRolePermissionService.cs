using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using System.Linq;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeRolePermissionService : IBusinessService<CommiteeRolePermission, CommiteeRolePermissionDTO>
    {

        public IQueryable<CommiteeRolePermission> GetAllPermission();
    }
}
