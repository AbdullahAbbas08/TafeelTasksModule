using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommitteeMeetingSystemSettingService : BusinessService<CommitteeMeetingSystemSetting, CommitteeMeetingSystemSettingDTO>, ICommitteeMeetingSystemSettingService
    {
        //readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;
        public CommitteeMeetingSystemSettingService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _Mapper = mapper;
        }

        public CommitteeMeetingSystemSettingDTO GetByCode(string code)
        {
            var x = _UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(x => x.SystemSettingCode == code).FirstOrDefault();
            return _Mapper.Map<CommitteeMeetingSystemSettingDTO>(x);
        }
        public ThemeDTO DefaultTheme()
        {
            var x = _UnitOfWork.GetRepository<SystemSetting>().GetAll().Where(x => x.SystemSettingCode == "DefaultTheme").FirstOrDefault()?.SystemSettingValue;
            if (string.IsNullOrEmpty(x))
            {
                return _Mapper.Map<ThemeDTO>(_UnitOfWork.GetRepository<CommitteeTheme>().GetAll().FirstOrDefault());
            }
            return _Mapper.Map<ThemeDTO>(_UnitOfWork.GetRepository<CommitteeTheme>().GetAll().Where(theme => theme.Name == x).FirstOrDefault());

        }

        public ThemeDTO DefaultTheme(string? themeCode)
        {
            ThemeDTO Result = new();
            var SystemSettingValue = _UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(x => x.SystemSettingCode == themeCode).FirstOrDefault()?.SystemSettingValue;
            if (string.IsNullOrEmpty(SystemSettingValue))
            {
                SystemSettingValue = _UnitOfWork.GetRepository<SystemSetting>().GetAll().Where(x => x.SystemSettingCode == "DefaultTheme").FirstOrDefault()?.SystemSettingValue;
                if (string.IsNullOrEmpty(SystemSettingValue))
                {
                    Result = _Mapper.Map<ThemeDTO>(_UnitOfWork.GetRepository<CommitteeTheme>().GetAll().FirstOrDefault());
                    Result.IsGradientTheme = isGradientTheme();
                    Result.GradientType = GradientType();
                    return Result;
                }
            }
            Result = _Mapper.Map<ThemeDTO>(_UnitOfWork.GetRepository<CommitteeTheme>().GetAll().Where(theme => theme.Name == SystemSettingValue).FirstOrDefault());
            Result.IsGradientTheme = isGradientTheme();
            Result.GradientType = GradientType();
            return Result;
        }

        public string isGradientTheme()
        {
            var res = _UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(x => x.SystemSettingCode == "AllowGradientTheme").FirstOrDefault()?.SystemSettingValue;
            if (string.IsNullOrEmpty(res)) res = "0";
            return res;
        }

        public string GradientType()
        {
            var GradientType = _UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(x => x.SystemSettingCode == "GradientType").FirstOrDefault()?.SystemSettingValue;
            if (string.IsNullOrEmpty(GradientType)) GradientType = "left";
            return GradientType.ToLower();
        }

        //override GetAll
        public override DataSourceResult<CommitteeMeetingSystemSettingDTO> GetAll<CommitteeMeetingSystemSettingDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = _UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll(WithTracking).Where(x => !x.IsHidden);

            return query.ProjectTo<CommitteeMeetingSystemSettingDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
        }
    }
}
