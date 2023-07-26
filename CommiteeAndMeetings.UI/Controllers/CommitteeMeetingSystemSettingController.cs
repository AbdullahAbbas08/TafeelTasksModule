using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Service.Sevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteeMeetingSystemSettingController : _BaseController<CommitteeMeetingSystemSetting, CommitteeMeetingSystemSettingDTO>
    {
        ICommitteeMeetingSystemSettingService _committeeMeetingSystemSettingService;
        private readonly ISystemSettingsService _systemSettingsService;

        public CommitteeMeetingSystemSettingController(ICommitteeMeetingSystemSettingService committeeMeetingSystemSettingService, ISystemSettingsService systemSettingsService , IHelperServices.ISessionServices sessionSevices)
            : base(committeeMeetingSystemSettingService, sessionSevices)
        {
            _committeeMeetingSystemSettingService = committeeMeetingSystemSettingService;
            _systemSettingsService = systemSettingsService;


        }
        [HttpGet("GetByCode")]
        public CommitteeMeetingSystemSettingDTO GetByCode(string code)
        {
            return _committeeMeetingSystemSettingService.GetByCode(code);
        }

        [HttpGet("DefaultTheme")]
        public ThemeDTO getDefaultTheme()
        {
            return _committeeMeetingSystemSettingService.DefaultTheme();
        }
        
        [HttpGet("SpecificTheme")]
        public ThemeDTO getSpecificTheme(string themeCode)
        {
            return _committeeMeetingSystemSettingService.DefaultTheme(themeCode);
        }

        [HttpPost, Route("SetSystemUsers")]
        public bool SetSystemUsers([FromBody] int NumberOfUsers)
        {
            return _systemSettingsService.SetSystemUsers(NumberOfUsers);
        }
        [HttpPost, Route("CheckCredential")]
        public bool CheckCredential()
        {
            return _systemSettingsService.CheckCredential();
        }

    }
}
