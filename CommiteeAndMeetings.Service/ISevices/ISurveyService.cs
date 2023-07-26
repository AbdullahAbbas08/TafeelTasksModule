using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using System;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ISurveyService : IBusinessService<Survey, SurveyDTO>
    {
        // IEnumerable<SurveyDTO> InsertWithAttachment(IEnumerable<SurveyDTO> entities);
        AllSurveyDTO GetAllServey(int take, int skip, int committeId, DateTime? dateFrom, DateTime? dateTo,string SearchText);
    }
}
