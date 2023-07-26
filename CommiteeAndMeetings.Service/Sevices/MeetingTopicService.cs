using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class MeetingTopicService : BusinessService<MeetingTopic, MeetingTopicDTO>, IMeetingTopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelperServices.ISessionServices _sessionServices;
        protected readonly IMapper _Mapper;
        SignalRHelper _signalR;
        ICommitteeNotificationService _committeeNotificationService;
        public MeetingTopicService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, IHubContext<SignalRHub> signalR, IDataProtectService dataProtectService, ICommitteeNotificationService committeeNotificationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _signalR = new SignalRHelper(signalR, dataProtectService);
            _Mapper = mapper;
            _sessionServices = sessionServices;
            _committeeNotificationService = committeeNotificationService;
        }
        public TopicActivitityDTO GetTopicActivities(int topicId)
        {
            var topicActivies = _unitOfWork.GetRepository<MeetingTopic>().GetAll()
                  .Select(x => new TopicActivitityDTO
                  {
                      TopicId = x.Id,
                      Comments = x.TopicComments.Select(y => new TopicCommentDTO
                      {
                          CommentId = y.CommentId,
                          CommentType = y.CommentType,
                          CreatedBy = y.CreatedBy,
                          Comment = new CommentDTO
                          {
                              CommentId = y.CommentId,
                              Text = y.Comment.Text,
                              CreatedBy = y.CreatedBy,
                              CreatedByUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Where(c => c.UserId == y.CreatedBy)
                                             .Select(z => new UserDetailsDTO
                                             {
                                                 UserId = z.UserId,
                                                 FullNameAr = z.FullNameAr,
                                                 FullNameEn = z.FullNameEn,
                                                 ProfileImage = z.ProfileImage,
                                             }).FirstOrDefault()
                          },
                          CreatedOn = y.CreatedOn,
                          Id = y.Id,
                          TopicId = y.TopicId
                      }).ToList(),
                      Surveys = x.TopicSurveies.Select(y => new SurveyDTO
                      {
                          Attachments = y.Attachments.Select(z => new SurveyAttachmentDTO
                          {
                              Attachment = new SavedAttachmentDTO
                              {
                                  AttachmentName = z.Attachment.AttachmentName,
                                  AttachmentTypeId = z.Attachment.AttachmentTypeId,
                                  BinaryContent = z.Attachment.BinaryContent,
                                  Height = z.Attachment.Height,
                                  LFEntryId = z.Attachment.LFEntryId,
                                  PagesCount = z.Attachment.PagesCount,
                                  Width = z.Attachment.Width,
                                  MimeType = z.Attachment.MimeType,
                                  SavedAttachmentId = z.Attachment.SavedAttachmentId
                              },
                              AttachmentId = z.AttachmentId,
                              SurveyAttachmentId = z.SurveyAttachmentId,
                              SurveyId = z.SurveyId
                          }).ToList(),
                          IsShared = y.IsShared,
                          CreatedOn = y.CreatedOn,
                          CreatedBy = y.CreatedBy,
                          CreatedByUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Where(c => c.UserId == (int)y.CreatedBy)
                                        .Select(z => new UserDetailsDTO
                                        {
                                            UserId = z.UserId,
                                            FullNameAr = z.FullNameAr,
                                            FullNameEn = z.FullNameEn,
                                            UserName = z.FullNameEn,
                                            ProfileImage = z.ProfileImage,
                                        }).FirstOrDefault(),
                          MeetingId = y.MeetingId,
                          Multi = y.Multi,
                          Subject = y.Subject,
                          SurveyAnswers = y.SurveyAnswers.Select(x => new SurveyAnswerDTO
                          {
                              Answer = x.Answer,
                              SurveyAnswerId = x.SurveyAnswerId,
                              SurveyAnswerUsers = x.SurveyAnswerUsers.Select(z => new SurveyAnswerUserDTO
                              {
                                  UserId = z.UserId,
                                  SurveyAnswerId = z.SurveyAnswerId,
                                  SurveyAnswerUserId = z.SurveyAnswerUserId,
                                  User = new Models.UserDetailsDTO
                                  {
                                      UserId = z.User.UserId,
                                      FullNameAr = z.User.FullNameAr,
                                      FullNameEn = z.User.FullNameEn,
                                      UserName = z.User.FullNameEn,
                                      ProfileImage = z.User.ProfileImage,

                                  }
                              }).ToList()
                          }).ToList(),
                          SurveyId = y.SurveyId,
                          SurveyUsers = y.SurveyUsers.Select(x => new SurveyUserDTO { UserId = x.UserId }).ToList(),
                          CreatedByRoleId = y.CreatedByRoleId,
                          SurveyEndDate = y.SurveyEndDate

                      }).ToList(),
                  }).FirstOrDefault(t => t.TopicId == topicId);
            return topicActivies;
        }
        public List<MeetingTopicLookupDTO> GetMeettingTopicLookup(int meetingId)
        {
            return _unitOfWork.GetRepository<MeetingTopic>().GetAll().Where(x => x.MeetingId == meetingId && x.TopicType == TopicType.Discussion && x.TopicState != TopicState.Cancled)
                              .Select(x => new MeetingTopicLookupDTO
                              {
                                  Id = x.Id,
                                  Points = x.TopicPoints,
                                  Title = x.TopicTitle

                              }).ToList();
        }
        public TopicDateDTO TopicStartEnd(int topicId, StartStop startStop)
        {
            var topic = _unitOfWork.GetRepository<MeetingTopic>().Find(topicId);
            try
            {
                var meeting = _unitOfWork.GetRepository<Meeting>().GetAll().FirstOrDefault(x => x.Id == topic.MeetingId);
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

                switch (startStop)
                {
                    case StartStop.Start:
                        topic.TopicAcualStartDateTime = DateTime.Now;
                        topic.TopicState = TopicState.InProgress;
                        _unitOfWork.GetRepository<MeetingTopic>().Update(topic);
                        foreach (var user in Attendees)
                        {
                            var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "BeginTopicNotificationText");
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = user.Id,
                                TextAR = loc.CommiteeLocalizationAr + " " + meeting.Title,
                                TextEn = loc.CommiteeLocalizationEn + " " + meeting.Title,
                                MeetingId = meeting.Id
                            };
                            List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                            _committeeNotificationService.Insert(committeeNotifications);
                            _signalR.BeginTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        foreach (var user in Coordinators)
                        {
                            var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "BeginTopicNotificationText");
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = user.Id,
                                TextAR = loc.CommiteeLocalizationAr + " " + meeting.Title,
                                TextEn = loc.CommiteeLocalizationEn + " " + meeting.Title,
                                MeetingId = meeting.Id,

                            };
                            List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                            _committeeNotificationService.Insert(committeeNotifications);
                            _signalR.BeginTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        break;
                    case StartStop.Stop:
                        topic.TopicAcualEndDateTime = DateTime.Now;
                        topic.TopicState = TopicState.Completed;
                        _unitOfWork.GetRepository<MeetingTopic>().Update(topic);
                        foreach (var user in Attendees)
                        {
                            _signalR.EndTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        foreach (var user in Coordinators)
                        {
                            _signalR.EndTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        break;
                    default:
                        break;
                }

                return new TopicDateDTO { CurrentStartDate = topic.TopicAcualStartDateTime, CurrentEndDate = topic.TopicAcualEndDateTime };
            }
            catch (Exception ex)
            {
                return new TopicDateDTO();
            }
        }
        public bool TopicPauseResume(int topicId, PauseResume pauseResume)
        {
            try
            {
                var topic = _unitOfWork.GetRepository<MeetingTopic>().Find(topicId);
                var meeting = _unitOfWork.GetRepository<Meeting>().GetAll().FirstOrDefault(x => x.Id == topic.MeetingId);
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
                switch (pauseResume)
                {
                    case PauseResume.Pause:
                        topic.TopicState = TopicState.InProgressPaused;
                        _unitOfWork.GetRepository<MeetingTopic>().Update(topic);
                        _unitOfWork.GetRepository<TopicPauseDate>().Insert(new TopicPauseDate
                        {
                            TopicId = topicId,
                            PauseDateTime = DateTime.Now,
                        });
                        foreach (var user in Attendees)
                        {
                            _signalR.PauseTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        foreach (var user in Coordinators)
                        {
                            _signalR.PauseTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        break;
                    case PauseResume.Resume:
                        topic.TopicState = TopicState.InProgress;
                        _unitOfWork.GetRepository<MeetingTopic>().Update(topic);
                        var PauseTopic = _unitOfWork.GetRepository<TopicPauseDate>().GetAll().FirstOrDefault(x => x.TopicId == topicId && x.PauseDateTime != null && x.ContinueDateTime == null);
                        PauseTopic.ContinueDateTime = DateTime.Now;
                        _unitOfWork.GetRepository<TopicPauseDate>().Update(PauseTopic);
                        foreach (var user in Attendees)
                        {
                            _signalR.ResumeTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        foreach (var user in Coordinators)
                        {
                            _signalR.ResumeTopicSender(user, topicId, (int)_sessionServices.UserId);
                        }
                        break;
                    default:
                        break;
                }
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
        public TopicDateDTO NextTopic(int currentTopicId, int nextTopicId, int currentIndex)
        {
            var currentTopic = _unitOfWork.GetRepository<MeetingTopic>().Find(currentTopicId);
            var nextTopic = _unitOfWork.GetRepository<MeetingTopic>().Find(nextTopicId);
            if (nextTopic.TopicState == TopicState.Cancled)
            {
                nextTopic = _unitOfWork.GetRepository<MeetingTopic>().GetAll()
                    .Where(c => c.TopicState != TopicState.Cancled && c.Id > nextTopicId && c.MeetingId == currentTopic.MeetingId).FirstOrDefault();
            }
            if (nextTopic == null)
            {
                return new TopicDateDTO();
            }
            var currentTopicPause = _unitOfWork.GetRepository<TopicPauseDate>().GetAll()
                .Where(x => x.ContinueDateTime == null).ToList();
            try
            {
                currentTopic.TopicState = TopicState.Completed;
                currentTopic.TopicAcualEndDateTime = DateTime.Now;
                nextTopic.TopicAcualStartDateTime = DateTime.Now;
                nextTopic.TopicState = TopicState.InProgress;

                _unitOfWork.GetRepository<MeetingTopic>().Update(currentTopic);
                _unitOfWork.GetRepository<MeetingTopic>().Update(nextTopic);
                foreach (var item in currentTopicPause)
                {
                    item.ContinueDateTime = DateTime.Now;
                    _unitOfWork.GetRepository<TopicPauseDate>().Update(item);
                }
                var meeting = _unitOfWork.GetRepository<Meeting>().GetAll().FirstOrDefault(x => x.Id == currentTopic.MeetingId);
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
                    _signalR.NextTopicSender(user, new TopicTimeLineDTO
                    {
                        CurrentTopicId = currentTopicId,
                        NextTopicId = nextTopic.Id,
                        CurrentIndex = currentIndex
                    }, (int)_sessionServices.UserId);
                }
                foreach (var user in Coordinators)
                {
                    _signalR.NextTopicSender(user, new TopicTimeLineDTO
                    {
                        CurrentTopicId = currentTopicId,
                        NextTopicId = nextTopic.Id,
                        CurrentIndex = currentIndex
                    }, (int)_sessionServices.UserId);
                }
                return new TopicDateDTO { CurrentStartDate = currentTopic.TopicAcualStartDateTime, CurrentEndDate = currentTopic.TopicAcualEndDateTime, NextStartDate = nextTopic.TopicAcualStartDateTime, NextTopicId = nextTopic.Id };
            }
            catch (Exception)
            {
                return new TopicDateDTO();

            }
        }
        public List<MeetingTopicDTO> GetAllTopics(int meetingId)
        {
            var topics = _unitOfWork.GetRepository<MeetingTopic>().GetAll().Where(x => x.MeetingId == meetingId).Select(x => new MeetingTopicDTO
            {
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                DeletedBy = x.DeletedBy,
                DeletedOn = x.DeletedOn,
                Id = x.Id,
                MeetingId = x.MeetingId,
                TopicAcualEndDateTime = x.TopicAcualEndDateTime,
                TopicAcualStartDateTime = x.TopicAcualStartDateTime,
                TopicDate = x.TopicDate,
                TopicFromDateTime = x.TopicFromDateTime,
                TopicState = x.TopicState,
                TopicTitle = x.TopicTitle,
                TopicToDateTime = x.TopicToDateTime,
                TopicType = x.TopicType,
                TopicTypeId = x.TopicTypeId,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = x.UpdatedOn,
                TopicComments = x.TopicComments.Select(y => new TopicCommentDTO
                {
                    Comment = new CommentDTO
                    {
                        CommentId = y.CommentId,
                        CreatedBy = y.Comment.CreatedBy,
                        Text = y.Comment.Text,
                        CreatedByUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Where(x => x.UserId == (int)y.Comment.CreatedBy)
                                        .Select(x => new UserDetailsDTO
                                        {
                                            UserId = x.UserId,
                                            FullNameAr = x.FullNameAr,
                                            FullNameEn = x.FullNameEn,
                                            ProfileImage = x.ProfileImage,
                                            UserName = x.Username
                                        }).FirstOrDefault(),
                    },
                    CommentId = y.CommentId,
                    CommentType = y.CommentType,
                    CreatedBy = y.CreatedBy,
                    Id = y.Id,
                    TopicId = y.TopicId,
                }).ToList(),
                TopicPauseDates = x.TopicPauseDates.Select(y => new TopicPauseDateDTO
                {
                    ContinueDateTime = y.ContinueDateTime,
                    CreatedBy = y.CreatedBy,
                    CreatedOn = y.CreatedOn,
                    Id = y.Id,
                    PauseDateTime = (DateTimeOffset)y.PauseDateTime,
                    TopicId = y.TopicId
                }).ToList(),
                TopicPoints = x.TopicPoints,
                TopicSurveies = x.TopicSurveies.Select(y => new SurveyDTO
                {
                    Attachments = y.Attachments.Select(z => new SurveyAttachmentDTO
                    {
                        // TODO Get Attachment
                        Attachment = new SavedAttachmentDTO
                        {
                            AttachmentName = z.Attachment.AttachmentName,
                            AttachmentTypeId = z.Attachment.AttachmentTypeId,
                            BinaryContent = z.Attachment.BinaryContent,
                            Height = z.Attachment.Height,
                            LFEntryId = z.Attachment.LFEntryId,
                            PagesCount = z.Attachment.PagesCount,
                            Width = z.Attachment.Width,
                            MimeType = z.Attachment.MimeType,
                            SavedAttachmentId = z.Attachment.SavedAttachmentId
                        },
                        // Attachment = _Mapper.Map(z.Attachment, typeof(SavedAttachment), typeof(SavedAttachmentDTO)) as SavedAttachmentDTO,
                        AttachmentId = z.AttachmentId,
                        SurveyAttachmentId = z.SurveyAttachmentId,
                        SurveyId = z.SurveyId,
                        


                    }).ToList(),
                    IsShared = y.IsShared,
                    CreatedOn = y.CreatedOn,
                    CreatedBy = y.CreatedBy,
                    SurveyEndDate = y.SurveyEndDate,
                    MeetingTopicId = (int)y.MeetingTopicId,
                    Multi = y.Multi,
                    Subject = y.Subject,
                    SurveyAnswers = y.SurveyAnswers.Select(x => new SurveyAnswerDTO
                    {
                        Answer = x.Answer,
                        SurveyAnswerId = x.SurveyAnswerId,
                        SurveyAnswerUsers = x.SurveyAnswerUsers.Select(y => new SurveyAnswerUserDTO
                        {
                            SurveyAnswerId = y.SurveyAnswerId,
                            SurveyAnswerUserId = y.SurveyAnswerUserId,
                            UserId = y.UserId,
                            User = new UserDetailsDTO
                            {
                                UserId = y.User.UserId,
                                FullNameAr = y.User.FullNameAr,
                                FullNameEn = y.User.FullNameEn,
                                UserName = y.User.FullNameEn,
                                ProfileImage = y.User.ProfileImage,
                            }
                        }).ToList()
                    }).ToList(),
                    SurveyId = y.SurveyId,
                    SurveyUsers = y.SurveyUsers.Select(x => new SurveyUserDTO { UserId = x.UserId }).ToList(),
                    CreatedByRoleId = y.CreatedByRoleId,
                    CreatedByUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Where(x => x.UserId == (int)y.CreatedBy)
                                        .Select(x => new UserDetailsDTO
                                        {
                                            UserId = x.UserId,
                                            FullNameAr = x.FullNameAr,
                                            FullNameEn = x.FullNameEn,
                                            ProfileImage = x.ProfileImage,
                                            UserName = x.Username
                                        }).FirstOrDefault(),

                }).ToList()
            }).ToList();
            //.AsQueryable().ProjectTo<MeetingTopicDTO>(_Mapper.ConfigurationProvider).ToList();
            foreach (var item in topics.Where(x => x.TopicState == TopicState.InProgress || x.TopicState == TopicState.InProgressPaused))
            {
                item.ReminingDuration = GetPausesSeconds(item);
            }
            return topics.ToList();
        }
        public static ReminingDurationDTO GetPausesSeconds(MeetingTopicDTO topic)
        {
            var Pauses = topic.TopicPauseDates
                .Select(x =>
                (x.ContinueDateTime == null) ?
                (DateTime.Now - x.PauseDateTime).TotalSeconds :
                (x.ContinueDateTime - x.PauseDateTime).Value.TotalSeconds).ToList();
            var duration = (topic.TopicToDateTime - topic.TopicFromDateTime).TotalSeconds;
            var expectedEnd = (topic.TopicAcualStartDateTime == null) ? topic.TopicFromDateTime.AddSeconds(duration) : topic.TopicAcualStartDateTime.Value.AddSeconds(duration);
            var remining = Math.Abs((expectedEnd - DateTime.Now).TotalSeconds) + Pauses.Sum();
            return new ReminingDurationDTO { Remining = remining, Up = expectedEnd > DateTime.Now };
        }
        public bool ChangeTopicState(int topicId, TopicState state)
        {
            try
            {
                var topic = _unitOfWork.GetRepository<MeetingTopic>().Find(topicId);
                topic.TopicState = state;
                _unitOfWork.GetRepository<MeetingTopic>().Update(topic);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}