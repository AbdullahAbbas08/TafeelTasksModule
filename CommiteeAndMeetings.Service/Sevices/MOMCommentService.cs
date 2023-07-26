using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class MOMCommentService : BusinessService<MOMComment, MOMCommentDTO>, IMOMCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelperServices.ISessionServices _sessionServices;

        public MOMCommentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
        }
        public override IEnumerable<MOMCommentDTO> Insert(IEnumerable<MOMCommentDTO> entities)
        {
            var entity = base.Insert(entities);
            foreach (var item in entity)
            {
                item.CreatedByUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == item.CreatedBy)
                    .Select(x => new UserDetailsDTO
                    {
                        UserId = x.UserId,
                        FullNameAr = x.FullNameAr,
                        FullNameEn = x.FullNameEn,
                        ProfileImage = x.ProfileImage,
                        //JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                        ExternalUser = x.ExternalUser
                    }).FirstOrDefault();
            }
            return entity;
        }
    }
}