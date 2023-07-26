using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using DbContexts.MasarContext.ProjectionModels;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class SystemSettingsService : BusinessService<SystemSetting, SystemSettingDTO>, ISystemSettingsService
    {
        private ISecurityService securityService;
        private IOptions<AppSettings> _appSettings;
        public SystemSettingsService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, IHelperServices.ISessionServices sessionServices, ISecurityService _securityService, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, _securityService, sessionServices, appSettings)
        {
            securityService = _securityService;
            _appSettings = appSettings;
        }

        public SystemSettingDTO GetSystemSettingByCode(string Code)
        {
            if (Code.ToUpper() == "SSO_LOGIN".ToUpper())
            {
                string app_setting_UsersystemValue = _appSettings.Value.SystemSettingOptions.SSO_LOGIN;

                var Setting = base._UnitOfWork.GetRepository<SystemSetting>().GetAll().Where(x => x.SystemSettingCode.ToUpper() == Code.ToUpper()).FirstOrDefault();
                if (Setting != null)
                {
                    Setting.SystemSettingValue = app_setting_UsersystemValue;
                    return base._Mapper.Map(Setting, typeof(SystemSetting), typeof(SystemSettingDTO)) as SystemSettingDTO;
                }
                return null;

            }
            else if (Code.ToUpper() == "JWTLogOffUQU".ToUpper())
            {
                string app_setting_UsersystemValue = _appSettings.Value.SystemSettingOptions.JWTLogOffUQU;

                var Setting = base._UnitOfWork.GetRepository<SystemSetting>().FirstOrDefaultAsync(x => x.SystemSettingCode.ToUpper() == Code.ToUpper()).Result;
                if (Setting != null)
                {
                    Setting.SystemSettingValue = app_setting_UsersystemValue;
                    return base._Mapper.Map(Setting, typeof(SystemSetting), typeof(SystemSettingDTO)) as SystemSettingDTO;
                }
                return null;

            }
            else if (Code.ToUpper() == "SendAttachmentForDelegation".ToUpper())
            {
                var Setting = new SystemSetting();
                Setting.SystemSettingValue = _appSettings.Value.SystemSettingOptions.SendAttachmentForDelegation;
                if (Setting != null)
                {
                    return base._Mapper.Map(Setting, typeof(SystemSetting), typeof(SystemSettingDTO)) as SystemSettingDTO;
                }
                return null;
            }
            else
            {
                var Setting = base._UnitOfWork.GetRepository<SystemSetting>().FirstOrDefaultAsync(x => x.SystemSettingCode.ToUpper() == Code.ToUpper()).Result;
                if (Setting != null)
                {
                    return base._Mapper.Map(Setting, typeof(SystemSetting), typeof(SystemSettingDTO)) as SystemSettingDTO;
                }
                return null;
            }
        }

        public bool SetSystemUsers(int NumberOfUsers)
        {
            // Check You Have Permission
            try
            {
                var Setting = base._UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(x => x.SystemSettingCode.ToUpper() == "SystemUsers".ToUpper()).FirstOrDefault();
                if (Setting != null)
                {
                    // Encrypt Number 
                    string EncryptedData = securityService.EncryptData(NumberOfUsers.ToString());
                    // Update New Value
                    Setting.SystemSettingValue = EncryptedData;
                    base._UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().Update(Setting);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool CheckCredential()
        {
            try
            {
                var Setting = base._UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(x => x.SystemSettingCode.ToUpper() == "SystemUsers".ToUpper()).FirstOrDefault();
                // get Number Of Valid Or Enabled Users
                var count_Enabled_users = base._UnitOfWork.GetRepository<User>().GetAll().Where(x => x.Enabled == true && x.IsEmployee == true).Count();
                if (Setting != null)
                {
                    // Decrept Number 
                    Setting.SystemSettingValue = String.IsNullOrEmpty(Setting.SystemSettingValue) ? String.Empty : Setting.SystemSettingValue;
                    string DecreptedData = securityService.DecryptData(Setting.SystemSettingValue);
                    string app_setting_UsersystemValue = _appSettings.Value.SystemSettingOptions.SystemUsers;
                    if ((long.Parse(app_setting_UsersystemValue) == 0 && long.Parse(DecreptedData) == 0) || (count_Enabled_users <= long.Parse(DecreptedData) && DecreptedData == app_setting_UsersystemValue))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
