using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommitee_CommiteePermissionService : IBusinessService<CommiteeUsersPermission, Commitee_CommiteePermissionDTO>
    {
    }
}
