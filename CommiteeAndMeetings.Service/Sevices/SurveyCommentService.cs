using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class SurveyCommentService : BusinessService<SurveyComment, SurveyCommentDTO>, ISurveyCommentService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        ICommitteeNotificationService _committeeNotificationService;
        ICommiteeLocalizationService _commiteeLocalizationService;
        public SurveyCommentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _committeeNotificationService = committeeNotificationService;
            _commiteeLocalizationService = commiteeLocalizationService;
        }
        public override IEnumerable<SurveyCommentDTO> Insert(IEnumerable<SurveyCommentDTO> entities)
        {
            var Result = base.Insert(entities);
            foreach (var newSurvey in Result)
            {
                var Survey = _unitOfWork.GetRepository<Survey>().GetAll().FirstOrDefault(x => x.SurveyId == newSurvey.SurveyId);
                newSurvey.Survey = _Mapper.Map(Survey, typeof(Survey), typeof(SurveyDTO)) as SurveyDTO;
                var loc = _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewCommentNotificationText");
                if (Survey.CreatedBy != _sessionServices.UserId)
                {
                    CommitteeNotificationDTO committeeNotification3 = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)Survey.CreatedBy,
                        TextAR = loc.CommiteeLocalizationAr + " " + newSurvey.Comment.Text,
                        TextEn = loc.CommiteeLocalizationEn + " " + newSurvey.Comment.Text,
                        CommentId = newSurvey.CommentId,
                        CommiteeId = Survey.CommiteeId
                    };
                    List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification3 };
                    _committeeNotificationService.Insert(committeeNotifications);
                }
                if (Survey.IsShared)
                {
                    var committemebers = _UnitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == Survey.CommiteeId).ToList();
                    foreach (var item in committemebers)
                    {

                        if (item.UserId != _sessionServices.UserId)
                        {
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = (int)item.UserId,
                                TextAR = loc.CommiteeLocalizationAr + " " + newSurvey.Comment.Text,
                                TextEn = loc.CommiteeLocalizationEn + " " + newSurvey.Comment.Text,
                                SurveyId = newSurvey.SurveyId,
                                CommiteeId = Survey.CommiteeId
                            };
                            List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                            _committeeNotificationService.Insert(committeeNotifications);
                        }
                    }
                }
                else
                {
                    foreach (var item in Survey.SurveyUsers)
                    {
                        if (item.UserId != _sessionServices.UserId)
                        {
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = (int)item.UserId,
                                TextAR = loc.CommiteeLocalizationAr + " " + newSurvey.Comment.Text,
                                TextEn = loc.CommiteeLocalizationEn + " " + newSurvey.Comment.Text,
                                SurveyId = newSurvey.SurveyId,
                                CommiteeId = Survey.CommiteeId
                            };
                            List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                            _committeeNotificationService.Insert(committeeNotifications);
                        }
                    }
                }
            }

            return Result;
        }
    }
}