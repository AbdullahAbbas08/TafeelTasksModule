using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IComiteeTaskCategoryService : IBusinessService<ComiteeTaskCategory, ComiteeTaskCategoryDTO>
    {
    }
}
