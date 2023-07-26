using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeAndMeetings.UI.Helpers;
using HelperServices.Hubs;
using IHelperServices.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using Models.ProjectionModels;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    // insert Get Creator
    public class TopicCommentService : BusinessService<TopicComment, TopicCommentDTO>, ITopicCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelperServices.ISessionServices _sessionServices;
        SignalRHelper _signalR;
        public TopicCommentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, IHubContext<SignalRHub> signalR, IDataProtectService dataProtectService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _signalR = new SignalRHelper(signalR, dataProtectService);
        }
        public override IEnumerable<TopicCommentDTO> Insert(IEnumerable<TopicCommentDTO> entities)
        {
            foreach (var item in entities)
            {
                item.Comment.CreatedBy = _sessionServices.UserId;
            }
            var newEntities = base.Insert(entities);
            foreach (var item in newEntities)
            {
                item.Comment.CreatedBy = item.CreatedBy;
                item.Comment.CreatedByUser = _unitOfWork.GetRepository<User>().GetAll().Where(c => c.UserId == item.CreatedBy)
                    .Select(x => new UserDetailsDTO
                    {
                        UserId = x.UserId,
                        FullNameAr = x.FullNameAr,
                        FullNameEn = x.FullNameEn,
                        ProfileImage = x.ProfileImage,
                    }).FirstOrDefault();
            }
            var meetingId = _unitOfWork.GetRepository<MeetingTopic>().Find(newEntities.First().TopicId).MeetingId;
            var meeting = _unitOfWork.GetRepository<Meeting>().GetAll().FirstOrDefault(x => x.Id == meetingId);
            var Attendees = meeting.MeetingAttendees.Select(c => new UserChatDTO
            {
                Id = c.AttendeeId,
                UserId = c.AttendeeId,
                UserName = c.Attendee.Username

            }).ToList();
            var Coordinators = meeting.MeetingCoordinators.Select(c => new UserChatDTO
            {
                Id = c.CoordinatorId,
                UserId = c.CoordinatorId,
                UserName = c.Coordinator.Username

            }).ToList();
            foreach (var user in Attendees)
            {
                _signalR.InsertTopicCommentSender(user, newEntities, (int)_sessionServices.UserId);
            }
            foreach (var user in Coordinators)
            {
                _signalR.InsertTopicCommentSender(user, newEntities, (int)_sessionServices.UserId);
            }
            return newEntities;
        }
    }
}
