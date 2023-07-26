using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteePermissionService : IBusinessService<CommitePermission, CommitePermissionDTO>
    {
        List<LookUpDTO> GetPermissionLookUps();
    }
}
