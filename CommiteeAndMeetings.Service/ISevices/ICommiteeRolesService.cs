using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeRolesService : IBusinessService<DAL.CommiteeDomains.CommiteeRole, CommiteeRoleDTO>
    {
    }
}
