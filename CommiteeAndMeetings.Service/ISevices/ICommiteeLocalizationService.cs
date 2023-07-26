using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using System;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeLocalizationService : IBusinessService<CommiteeLocalization, CommiteeLocalizationDetailsDTO>
    {
        string GetJson(string culture);
        DateTime GetLastLocalizationUpdateTime();
        string GetLocaliztionByCode(string v, bool cultureIsArabic);
    }
}
