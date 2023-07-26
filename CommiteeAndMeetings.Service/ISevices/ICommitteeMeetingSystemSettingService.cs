using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommitteeMeetingSystemSettingService : IBusinessService<CommitteeMeetingSystemSetting, CommitteeMeetingSystemSettingDTO>
    {
        CommitteeMeetingSystemSettingDTO GetByCode(string code);
        ThemeDTO DefaultTheme();
        ThemeDTO DefaultTheme(string themeCode);
    }
}
