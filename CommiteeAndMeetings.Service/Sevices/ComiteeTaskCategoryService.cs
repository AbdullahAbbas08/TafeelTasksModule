using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class ComiteeTaskCategoryService : BusinessService<ComiteeTaskCategory, ComiteeTaskCategoryDTO>, IComiteeTaskCategoryService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;

        public ComiteeTaskCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
        }


    }
}
