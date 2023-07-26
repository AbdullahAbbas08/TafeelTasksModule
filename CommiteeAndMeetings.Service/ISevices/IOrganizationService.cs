using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IOrganizationService : IBusinessService<Organization, OrganizationDetailsDTO>
    {
    }
}
