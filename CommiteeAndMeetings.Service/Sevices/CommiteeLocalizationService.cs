using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{

    public class CommiteeLocalizationService : BusinessService<CommiteeLocalization, CommiteeLocalizationDetailsDTO>, ICommiteeLocalizationService
    {
        public CommiteeLocalizationService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
        }
        public string GetJson(string culture)
        {
            //var list = _mainDbContext.Localizations.Include(x => x.LocalizationCategory).AsNoTracking().ToList();
            //var result = "{" + string.Join(", ", list.GroupBy(x => x.LocalizationCategory).Select(x => "\"" + x.Key.Code + "\":{" + string.Join(", ", x.Select(s => "\"" + s.Key + "\":\"" + s.Value + "\"")) + "}")) + "}";
            var list = _UnitOfWork.GetRepository<CommiteeLocalization>()
                .GetAll()
                .Select(x => new
                {
                    x.Key,
                    Value = culture.ToLower().StartsWith("ar") ? x.CommiteeLocalizationAr
                : culture.ToLower().StartsWith("en") ? x.CommiteeLocalizationEn
                : x.CommiteeLocalizationAr
                })
                .ToList();
            var result = "{" + string.Join(", ", list.Select(x => "\"" + x.Key + "\":\"" + x.Value + "\"")) + "}";
            //StringBuilder resultx = new StringBuilder("{");
            //foreach (var x in list)
            //{
            //    resultx.Append('"').Append(x.Key.TrimEnd().TrimStart()).Append('"').Append(":").Append('"').Append(x.Value.TrimEnd().TrimStart()).Append('"').Append(",");
            //}
            //var xx = resultx.Append("}").ToString();
            //var result = xx.Remove(xx.LastIndexOf(","), 1);

            //return JsonConvert.SerializeObject(result);
            return result;
        }

        public DateTime GetLastLocalizationUpdateTime()
        {
            return DateTime.Parse(_UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().Select(x => x.CreatedOn)
                .Union(_UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().Select(x => x.UpdatedOn)).Max().GetValueOrDefault().ToString());
        }

        public string GetLocaliztionByCode(string code, bool cultureIsArabic)
        {
            return cultureIsArabic ? _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == code).CommiteeLocalizationAr :
                 _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == code).CommiteeLocalizationEn;
        }
    }
}
