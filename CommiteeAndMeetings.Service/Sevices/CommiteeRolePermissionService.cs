using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeRolePermissionService : BusinessService<CommiteeRolePermission, CommiteeRolePermissionDTO>, ICommiteeRolePermissionService
    {
        private readonly IUnitOfWork unitOfWork;

        public CommiteeRolePermissionService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<CommiteeRolePermission> GetAllPermission()
        {
            return unitOfWork.GetRepository<CommiteeRolePermission>().GetAll();
        }
    }
}