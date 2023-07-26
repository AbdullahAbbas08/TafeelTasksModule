using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICategoryService : IBusinessService<Category, CategoryDTO>
    {
    }
}

