using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteePermissionService : BusinessService<CommitePermission, CommitePermissionDTO>, ICommiteePermissionService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;

        public CommiteePermissionService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
        }


        public List<LookUpDTO> GetPermissionLookUps()
        {
            return _unitOfWork.GetRepository<CommitePermission>().GetAll().Where(x => x.Enabled).Select(x => new LookUpDTO
            {
                Id = x.CommitePermissionId,
                Name = _sessionServices.CultureIsArabic ? x.CommitePermissionNameAr : x.CommitePermissionNameEn,
                IsDeleted=x.IsDeleted
            }).ToList();
        }

        // override get all
        public override DataSourceResult<CommitePermissionDTO> GetAll<CommitePermissionDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            
            IQueryable query = _unitOfWork.GetRepository<CommitePermission>().GetAll(WithTracking).Where(x => !x.IsDeleted);

            return query.ProjectTo<CommitePermissionDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);

            
        }
    }
}
