using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IRolesService : IBusinessService<Role, RoleDTO>
    {
    }
}
