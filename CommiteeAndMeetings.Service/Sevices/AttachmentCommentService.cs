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
    public class AttachmentCommentService : BusinessService<AttachmentComment, AttachmentCommentDTO>, IAttachmentCommentService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        ICommitteeNotificationService _committeeNotificationService;
        ICommiteeLocalizationService _commiteeLocalizationService;
        public AttachmentCommentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _committeeNotificationService = committeeNotificationService;
            _commiteeLocalizationService = commiteeLocalizationService;
        }
        public override IEnumerable<AttachmentCommentDTO> Insert(IEnumerable<AttachmentCommentDTO> entities)
        {
            var result = base.Insert(entities);
            var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewAttachmentCommitteeNotificationText");
            foreach (var item in result)
            {

                List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO>();

                var attachment = _unitOfWork.GetRepository<CommiteeSavedAttachment>().Find(item.AttachmentId);
                if (attachment.CreatedBy != _sessionServices.UserId)
                {
                    CommitteeNotificationDTO committeeNotification3 = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)attachment.CreatedBy,
                        TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                        TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                        CommentId = item.CommentId,
                        CommiteeId = attachment.CommiteeId
                    };
                    committeeNotifications.Add(committeeNotification3);
                }
                //  item.Attachment = _Mapper.Map(attachment, typeof(SavedAttachment), typeof(SavedAttachmentDTO)) as SavedAttachmentDTO;
                if (attachment.AllUsers)
                {
                    var committemebers = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == attachment.CommiteeId).ToList();
                    foreach (var item2 in committemebers)
                    {
                        if (item2.UserId != _sessionServices.UserId)
                        {
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = (int)item2.UserId,
                                TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                                TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                                CommentId = item.CommentId,
                                CommiteeId = item2.CommiteeId
                            };
                            committeeNotifications.Add(committeeNotification);
                        }
                    }
                    _committeeNotificationService.Insert(committeeNotifications);
                }
                else
                {
                    foreach (var item2 in attachment.AttachmentUsers)
                    {
                        if (item2.UserId != _sessionServices.UserId)
                        {
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = (int)item2.UserId,
                                TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                                TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                                CommentId = item.CommentId,
                                CommiteeId = item2.CommiteeAttachment.CommiteeId
                            };
                            committeeNotifications.Add(committeeNotification);
                        }
                    }
                    _committeeNotificationService.Insert(committeeNotifications);
                }
            }
            return result;
        }
    }
}