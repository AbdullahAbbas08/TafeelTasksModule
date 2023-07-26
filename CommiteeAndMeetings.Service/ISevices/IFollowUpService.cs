using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using Models.ProjectionModels;
using System.Collections.Generic;
namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IFollowUpService : IBusinessService<FollowUp, FollowUpDTO>
    {
        AddFollowUpDelegationDTO AddFollowUp(List<FollowUpDTO> objectsToAdd, bool from_delegation);
    }
}
