using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ISystemSettingsService : IBusinessService<SystemSetting, SystemSettingDTO>
    {
        SystemSettingDTO GetSystemSettingByCode(string Code);
        bool SetSystemUsers(int numberOfUsers);
        bool CheckCredential();
    }
}
