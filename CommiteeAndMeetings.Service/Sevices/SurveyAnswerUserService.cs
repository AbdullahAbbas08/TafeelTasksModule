using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeAndMeetings.UI.Helpers;
using CommiteeDatabase.Models.Domains;
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
    public class SurveyAnswerUserService : BusinessService<SurveyAnswerUser, SurveyAnswerUserDTO>, ISurveyAnswerUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelperServices.ISessionServices _sessionServices;
        SignalRHelper _signalR;
        public SurveyAnswerUserService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, IHubContext<SignalRHub> signalR, IDataProtectService dataProtectService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _signalR = new SignalRHelper(signalR, dataProtectService);
        }

        public UserDetailsDTO GetCommitteUser(int userId)
        {
            IQueryable query = _unitOfWork.GetRepository<User>().GetAll(false).Where(c => c.UserId == userId);
            return query.ProjectTo<UserDetailsDTO>(_Mapper.ConfigurationProvider).FirstOrDefault();
        }

        public void SurveyAnswerUserSignalR(IEnumerable<SurveyAnswerUserDTO> member)
        {
            //foreach (var item in member)
            //{
            var surveyId = _unitOfWork.GetRepository<SurveyAnswer>().Find(member.FirstOrDefault().SurveyAnswerId).SurveyId;
            var survey = _unitOfWork.GetRepository<Survey>().Find(surveyId);
            var meetingId = survey.MeetingId;
            var topicId = survey.MeetingTopicId;
            if (meetingId != null)
            {
                var meeting = _unitOfWork.GetRepository<Meeting>().Find(meetingId);
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
                    _signalR.SurveyAnswerUserSender(user, (int)meetingId, member, (int)_sessionServices.UserId);
                }
                foreach (var user in Coordinators)
                {
                    _signalR.SurveyAnswerUserSender(user, (int)meetingId, member, (int)_sessionServices.UserId);
                }
                //}
            }
            else if (topicId != null)
            {
                var topic = _unitOfWork.GetRepository<MeetingTopic>().Find(topicId);
                var meeting = _unitOfWork.GetRepository<Meeting>().Find(topic.MeetingId);
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
                    _signalR.SurveyAnswerUserSender(user, (int)topicId, member, (int)_sessionServices.UserId);
                }
                foreach (var user in Coordinators)
                {
                    _signalR.SurveyAnswerUserSender(user, (int)topicId, member, (int)_sessionServices.UserId);
                }
            }
        }
    }
}