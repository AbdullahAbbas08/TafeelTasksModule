using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using DbContexts.MasarContext.ProjectionModels;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class LookupService : BusinessService<Lookup, Lookup>, ILookupService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _Mapper;
        protected readonly IStringLocalizer _StringLocalizer;
        protected readonly IHelperServices.ISessionServices _SessionServices;


        public LookupService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
            : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _Mapper = mapper;
            _StringLocalizer = stringLocalizer;
            _SessionServices = sessionServices;
        }

        public IQueryable<User> userCorrespondent(int? organizationID)
        {
            return (from ur in _unitOfWork.GetRepository<User>().GetAll()
                    join uC in _unitOfWork.GetRepository<UserCorrespondentOrganization>().GetAll()
                      on ur.UserId equals uC.UserId into ps
                    from p in ps.DefaultIfEmpty()
                    where (organizationID == null || (ur.IsCorrespondentForAllOrganizations || p.OrganizationId == organizationID))
                    select ur).Distinct();
        }

        public IEnumerable<Lookup> Get(string Type, string SearchText, DataSourceRequest dataSourceRequest, Dictionary<string, object> args, object[] ids = null)
        {
            bool isArabic = /*_SessionServices.CultureIsArabic*/ _SessionServices.CultureIsArabic;
            args = args ?? new Dictionary<string, object>();
            int? OrganizationId = null;
            IEnumerable<Lookup> result = null;
            SearchText = SearchText == null ? "" : SearchText;
            SearchText = "%" + SearchText + "%";
            switch (Type.ToLower())
            {

                case "requiredactions":
                    var ActionId = args.GetValue<int?>("ActionId");
                    var res = _unitOfWork.GetRepository<RequiredAction>().GetAll().Where(x => !ActionId.HasValue || x.ActionRequiredActions.Any(ara => ara.ActionId == ActionId))
                        .ToDataSourceResult(dataSourceRequest, true).Data.ToList();
                       //result = res.Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.RequiredActionNameAr, SearchText) || EF.Functions.Like(x.RequiredActionNameEn, SearchText) || EF.Functions.Like(x.RequiredActionNameFn, SearchText))
                       result = res.Where(x => string.IsNullOrEmpty(SearchText.Replace("%", "")) || x.RequiredActionNameAr.Contains(SearchText.Replace("%", "")) || x.RequiredActionNameEn.Contains(SearchText.Replace("%","")) || x.RequiredActionNameFn.Contains(SearchText.Replace("%","")))

                        .Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.RequiredActionId))
                        .Select(x => new Lookup
                        {
                            Id = x.RequiredActionId,
                            Text = _SessionServices.Culture.ToLower() == "ar" ? (string.IsNullOrEmpty(x.RequiredActionNameAr) ? x.RequiredActionNameEn : x.RequiredActionNameAr)
                                 : _SessionServices.Culture.ToLower() == "en" ? (string.IsNullOrEmpty(x.RequiredActionNameEn) ? x.RequiredActionNameAr : x.RequiredActionNameEn)
                                 : (string.IsNullOrEmpty(x.RequiredActionNameFn) ? x.RequiredActionNameEn : x.RequiredActionNameFn)
                        }).OrderBy(x => x.Text);
                    break;
                case "correspondents":
                    OrganizationId = args.GetValue<int?>("OrganizationId") ?? _SessionServices.OrganizationId;
                    //if (OrganizationId > 0)
                    //{
                    var result2 = userCorrespondent(OrganizationId).ToDataSourceResult(dataSourceRequest,true).Data.ToList();
                    //.Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.FullNameAr, SearchText) || EF.Functions.Like(x.FullNameEn, SearchText) || EF.Functions.Like(x.FullNameFn, SearchText) || x.Email.StartsWith(SearchText) || EF.Functions.Like(x.WorkPhoneNumber, SearchText))
                    //.Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.UserId))
                    //.ToDataSourceResult(dataSourceRequest, true).Data
                    //.Select(x => new Lookup
                    //{
                    //    Id = x.UserId,
                    //    Text = _SessionServices.Culture.ToLower() == "ar" ? (string.IsNullOrEmpty(x.FullNameAr) ? x.FullNameEn : x.FullNameAr)
                    //         : _SessionServices.Culture.ToLower() == "en" ? (string.IsNullOrEmpty(x.FullNameEn) ? x.FullNameAr : x.FullNameEn)
                    //         : (string.IsNullOrEmpty(x.FullNameFn) ? x.FullNameEn : x.FullNameFn),
                    //    Additional = new LookupAdditional { ImageId = x.ProfileImageFileId }
                    //}).OrderBy(x => x.Text);
                    //}
                   // result = result2.Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.FullNameAr, SearchText) || EF.Functions.Like(x.FullNameEn, SearchText) || EF.Functions.Like(x.FullNameFn, SearchText) || x.Email.StartsWith(SearchText) || EF.Functions.Like(x.WorkPhoneNumber, SearchText))
                    result = result2.Where(x => string.IsNullOrEmpty(SearchText.Replace("%", "")) || x.FullNameAr.Contains(SearchText.Replace("%","")) || x.FullNameEn.Contains(SearchText.Replace("%", "")) || x.FullNameFn.Contains(SearchText.Replace("%", ""))/*||  x.Email.Contains(SearchText.Replace("%", "")) || x.WorkPhoneNumber.Contains(SearchText.Replace("%", ""))*/)
                    
                    .Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.UserId))
                    //.ToDataSourceResult(dataSourceRequest, true).Data.ToList()
                    .Select(x => new Lookup
                    {
                        Id = x.UserId,
                        Text = _SessionServices.Culture.ToLower() == "ar" ? (string.IsNullOrEmpty(x.FullNameAr) ? x.FullNameEn : x.FullNameAr)
                             : _SessionServices.Culture.ToLower() == "en" ? (string.IsNullOrEmpty(x.FullNameEn) ? x.FullNameAr : x.FullNameEn)
                             : (string.IsNullOrEmpty(x.FullNameFn) ? x.FullNameEn : x.FullNameFn),
                        Additional = new LookupAdditional { ImageId = x.ProfileImageFileId }
                    }).OrderBy(x => x.Text);
                    break;
                case "genders":
                    result = _unitOfWork.GetRepository<Gender>().GetAll()
                        .Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.GenderNameAr, SearchText) || EF.Functions.Like(x.GenderNameEn, SearchText))
                        .Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.GenderId))
                        .ToDataSourceResult(dataSourceRequest, true).Data
                        .Select(x => new Lookup
                        {
                            Id = x.GenderId,
                            Text = _SessionServices.Culture.ToLower() == "ar" ? (string.IsNullOrEmpty(x.GenderNameAr) ? x.GenderNameEn : x.GenderNameAr)
                                 : _SessionServices.Culture.ToLower() == "en" ? (string.IsNullOrEmpty(x.GenderNameEn) ? x.GenderNameAr : x.GenderNameEn)
                                 : (string.IsNullOrEmpty(x.GenderNameFn) ? x.GenderNameEn : x.GenderNameFn)
                        }).OrderBy(x => x.Text);
                    break;
                case "nationalities":
                    result = _unitOfWork.GetRepository<Nationality>().GetAll()
                        .Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.NationalityNameAr, SearchText) || EF.Functions.Like(x.NationalityNameEn, SearchText))
                        .Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.NationalityId))
                        .ToDataSourceResult(dataSourceRequest, true).Data
                        .Select(x => new Lookup
                        {
                            Id = x.NationalityId,
                            Text = _SessionServices.Culture.ToLower() == "ar" ? (string.IsNullOrEmpty(x.NationalityNameAr) ? x.NationalityNameEn : x.NationalityNameAr)
                                 : _SessionServices.Culture.ToLower() == "en" ? (string.IsNullOrEmpty(x.NationalityNameEn) ? x.NationalityNameAr : x.NationalityNameEn)
                                 : (string.IsNullOrEmpty(x.NationalityNameFn) ? x.NationalityNameEn : x.NationalityNameFn)
                        }).OrderBy(x => x.Text);
                    break;
                case "jobtitles":
                    result = _unitOfWork.GetRepository<JobTitle>().GetAll()
                        .Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.JobTitleNameAr, SearchText) || EF.Functions.Like(x.JobTitleNameEn, SearchText))
                        .Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.JobTitleId))
                        .ToDataSourceResult(dataSourceRequest, true).Data
                        .Select(x => new Lookup
                        {
                            Id = x.JobTitleId,
                            Text = _SessionServices.Culture.ToLower() == "ar" ? (string.IsNullOrEmpty(x.JobTitleNameAr) ? x.JobTitleNameEn : x.JobTitleNameAr)
                                 : _SessionServices.Culture.ToLower() == "en" ? (string.IsNullOrEmpty(x.JobTitleNameEn) ? x.JobTitleNameAr : x.JobTitleNameEn)
                                 : (string.IsNullOrEmpty(x.JobTitleNameFn) ? x.JobTitleNameEn : x.JobTitleNameFn)
                        }).OrderBy(x => x.Text);
                    break;
                case "organizations":
                    result = _unitOfWork.GetRepository<Organization>().GetAll()
                        .Where(x =>  x.IsActive)
                        .Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.OrganizationNameAr, SearchText) || EF.Functions.Like(x.OrganizationNameEn, SearchText) || EF.Functions.Like(x.OrganizationNameFn, SearchText) || EF.Functions.Like(x.Code, SearchText))
                        .Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.OrganizationId))
                        .ToDataSourceResult(dataSourceRequest, true).Data
                        .OrderBy(x => x.Code)
                        .Select(x => new Lookup
                        {
                            Id = x.OrganizationId,
                            Text = x.Code.ToString() + " - " + (_SessionServices.Culture.ToLower() == "ar" ? (string.IsNullOrEmpty(x.OrganizationNameAr) ? x.OrganizationNameEn : x.OrganizationNameAr)
                                                               : _SessionServices.Culture.ToLower() == "en" ? (string.IsNullOrEmpty(x.OrganizationNameEn) ? x.OrganizationNameAr : x.OrganizationNameEn)
                                                               : (string.IsNullOrEmpty(x.OrganizationNameFn) ? x.OrganizationNameEn : x.OrganizationNameFn)),
                            IsCategory = x.IsCategory,
                            Additional = new LookupAdditional
                            {
                                Description = _SessionServices.Culture.ToLower() == "ar" ? x.FullPathAr
                                            : _SessionServices.Culture.ToLower() == "en" ? x.FullPathEn
                                            : x.FullPathFn,
                            }
                        });
                    break;
                case "roles":
                    result = _unitOfWork.GetRepository<Role>().GetAll()
                        .Where(x => string.IsNullOrEmpty(SearchText) || EF.Functions.Like(x.RoleNameAr, SearchText) || EF.Functions.Like(x.RoleNameEn, SearchText) || EF.Functions.Like(x.RoleNameFn, SearchText))
                        .Where(x => ids == null || ids.Select(id => Convert.ToInt32(id)).Contains(x.RoleId))
                        .Where(x=>x.IsCommitteRole)
                        .ToDataSourceResult(dataSourceRequest, true).Data
                        .Select(x => new Lookup
                        {
                            Id = x.RoleId,
                            Text = _SessionServices.Culture.ToLower() == "ar" ? x.RoleNameAr
                                 : _SessionServices.Culture.ToLower() == "en" ? x.RoleNameEn
                                 : x.RoleNameFn,
                        }).OrderBy(x => x.Text);
                    break;
                default:
                    throw new BusinessException("No lookup defined for this type");
            }

            return result?.Distinct();
        }

    }
}
