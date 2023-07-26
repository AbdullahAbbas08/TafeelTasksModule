using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using DbContexts.MasarContext.ProjectionModels;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class OrganizationService : BusinessService<Organization, OrganizationDetailsDTO>, IOrganizationService
    {
        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
        }
        public override DataSourceResult<OrganizationDetailsDTO> GetAll<OrganizationDetailsDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = this._UnitOfWork.GetRepository<Organization>().GetAll(WithTracking).Where(x => !x.IsOuterOrganization);

            return query.ProjectTo<OrganizationDetailsDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
        }
    }
}