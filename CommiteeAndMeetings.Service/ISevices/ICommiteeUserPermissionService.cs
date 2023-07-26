using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeUserPermissionService : IBusinessService<CommiteeUsersPermission, CommiteeUserPermissionDTO>
    {
        void CustomInsert(int RoleId, int UserId, string CommiteeIdEncrypt, bool IsDelegated = false);
        void DelegateUserAddPermissions(int userId, string committeId,bool IsDelegated = false); 
    }
}

