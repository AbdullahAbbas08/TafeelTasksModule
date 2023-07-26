using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using Models;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ISurveyAnswerUserService : IBusinessService<SurveyAnswerUser, SurveyAnswerUserDTO>
    {
        UserDetailsDTO GetCommitteUser(int userId);
        void SurveyAnswerUserSignalR(IEnumerable<SurveyAnswerUserDTO> member);
    }
}
