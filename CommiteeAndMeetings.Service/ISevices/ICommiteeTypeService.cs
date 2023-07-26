using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeTypeService : IBusinessService<CommiteeType, CommiteeTypeDTO>
    {
        CommiteeTypeDTO AddCommiteeTypeImage(int id, byte[] profileImage, string profileImageMimeType);
    }
}
