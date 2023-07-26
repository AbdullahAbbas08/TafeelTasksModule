using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class Commitee_CommiteePermissionService : BusinessService<CommiteeUsersPermission, Commitee_CommiteePermissionDTO>, ICommitee_CommiteePermissionService
    {
        public Commitee_CommiteePermissionService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
        }
    }
}