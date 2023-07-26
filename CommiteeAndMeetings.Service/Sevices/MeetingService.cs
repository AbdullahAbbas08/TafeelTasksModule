using AutoMapper;
using AutoMapper.QueryableExtensions;
using Castle.Core.Internal;
using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.Helpers;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeAndMeetings.UI.Helpers;
using CommiteeDatabase.Models.Domains;
using Hangfire;
using Hangfire.Storage.Monitoring;
using HelperServices;
using HelperServices.LinqHelpers;
using IHelperServices;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
//using ZXing;
//using static iTextSharp.text.pdf.AcroFields;
//using static Microsoft.AspNetCore.Internal.AwaitableThreadPool;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class MeetingService : BusinessService<Meeting, MeetingDTO>, IMeetingService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IHelperServices.ISessionServices _sessionServices;
        private readonly IMailServices _MailServices;
        ICommiteeLocalizationService _commiteeLocalizationService;
        ICommitteeNotificationService _committeeNotificationService;
        private readonly IMeetingCommentService meetingCommentService;
        private readonly ICommitteeMeetingSystemSettingService _systemSettingsService;
        ISmsServices smsServices;
        public MeetingService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ISmsServices _smsServices,
            ICommitteeMeetingSystemSettingService systemSettingsService,
            IStringLocalizer stringLocalizer,
            ISecurityService securityService,
            IHelperServices.ISessionServices sessionServices,
            IOptions<AppSettings> appSettings,
            IMailServices MailServices,
            ICommiteeLocalizationService commiteeLocalizationService,
            ICommitteeNotificationService committeeNotificationService,
            IMeetingCommentService _meetingCommentService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
            _sessionServices = sessionServices;
            _MailServices = MailServices;
            _commiteeLocalizationService = commiteeLocalizationService;
            _committeeNotificationService = committeeNotificationService;
            meetingCommentService = _meetingCommentService;
            smsServices = _smsServices;
            _systemSettingsService = systemSettingsService;
        }

        public List<MeetingUserAttendationDTO> UsersForSpectificMeetings(int meetingId)
        {
            var result = new List<MeetingUserAttendationDTO>();
            var coordinatorsShouldAttend = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll().Where(x => x.MeetingId == meetingId && x.ConfirmeAttendance).Select(x => new MeetingUserAttendationDTO
            {
                UserId = x.CoordinatorId,
                Attended = x.Attended == null || x.Attended == false ? false : true,
                FullNameAr = x.Coordinator.FullNameAr,
                FullNameEn = x.Coordinator.FullNameEn,
                ProfileImage = x.Coordinator.ProfileImage,
                type = UserType.Coordinator,
                UserDelegate = x.UserDelegate != null ? new Models.UserDetailsDTO
                {
                    UserId = x.UserDelegate.UserId,
                    FullNameAr = x.UserDelegate.FullNameAr,
                    FullNameEn = x.UserDelegate.FullNameEn,
                    ProfileImage = x.UserDelegate.ProfileImage,
                    ExternalUser = x.UserDelegate.ExternalUser
                } : null


            }).ToList();
            if (coordinatorsShouldAttend.Count() > 0)
                result.AddRange(coordinatorsShouldAttend);
            var attendees = _unitOfWork.GetRepository<MeetingAttendee>().GetAllWithoutInclude().Where(x => x.MeetingId == meetingId).Select(x => new MeetingUserAttendationDTO
            {
                UserId = x.AttendeeId,
                Attended = x.Attended == null || x.Attended == false ? false : true,
                FullNameAr = x.Attendee.FullNameAr,
                FullNameEn = x.Attendee.FullNameEn,
                ProfileImage = x.Attendee.ProfileImage,
                type = UserType.Attendee,
                UserDelegate = x.UserDelegate != null ? new Models.UserDetailsDTO
                {
                    UserId = x.UserDelegate.UserId,
                    FullNameAr = x.UserDelegate.FullNameAr,
                    FullNameEn = x.UserDelegate.FullNameEn,
                    ProfileImage = x.UserDelegate.ProfileImage,
                    ExternalUser = x.UserDelegate.ExternalUser
                } : null
            }).ToList();
            if (attendees.Count() > 0)
                result.AddRange(attendees);
            return result;
        }

        public MeetingUserAvailabilityDTO InsertMeetingAttendeesOrCoordinators(MeetingUserDTO attendeeDTO)
        {
            MeetingUserAvailabilityDTO meetingUser = new MeetingUserAvailabilityDTO();
            var meeting = _unitOfWork.GetRepository<Meeting>().GetById(attendeeDTO.MeetingId);
            //CheckAvailability(userId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, UserType.Coordinator)
            var UserAvailability = CheckAvailability(attendeeDTO.UserId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, attendeeDTO.UserType, true);
            if (attendeeDTO.UserType == UserType.Attendee)
            {
                if (!meeting.MeetingCoordinators.Any(x => x.CoordinatorId == attendeeDTO.UserId) &&
                    !meeting.MeetingAttendees.Any(x => x.AttendeeId == attendeeDTO.UserId))
                {
                    var attendee = _unitOfWork.GetRepository<MeetingAttendee>().Insert(new MeetingAttendee
                    {
                        AttendeeId = attendeeDTO.UserId,
                        Available = UserAvailability.Available,
                        ConfirmeAttendance = false,
                        MeetingId = attendeeDTO.MeetingId,
                        State = AttendeeState.New
                    });
                    attendee.Attendee = _unitOfWork.GetRepository<User>().GetById(attendeeDTO.UserId);
                    var jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == attendee.Attendee.JobTitleId).FirstOrDefault();
                    meetingUser = new MeetingUserAvailabilityDTO
                    {
                        Available = UserAvailability.Available,
                        Attended = attendee.Attended,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = attendee.AttendeeId,
                            FullNameAr = attendee.Attendee.FullNameAr,
                            FullNameEn = attendee.Attendee.FullNameEn,
                            ProfileImage = attendee.Attendee.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                            ExternalUser = attendee.Attendee.ExternalUser
                        },
                        UserId = attendee.AttendeeId,
                        CreatedOn = attendee.CreatedOn,
                        Meetings = UserAvailability.Meetings
                    };

                    return meetingUser;
                }
                else
                    return null;
            }
            else if (attendeeDTO.UserType == UserType.Coordinator)
            {
                if (!meeting.MeetingCoordinators.Any(x => x.CoordinatorId == attendeeDTO.UserId) &&
                    !meeting.MeetingAttendees.Any(x => x.AttendeeId == attendeeDTO.UserId))
                {
                    var coordinator = _unitOfWork.GetRepository<MeetingCoordinator>().Insert(new MeetingCoordinator
                    {

                        CoordinatorId = attendeeDTO.UserId,
                        Available = UserAvailability.Available,
                        ConfirmeAttendance = false,
                        MeetingId = attendeeDTO.MeetingId,
                        State = AttendeeState.New
                    });
                    coordinator.Coordinator = _unitOfWork.GetRepository<User>().GetById(attendeeDTO.UserId);
                    var jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == coordinator.Coordinator.JobTitleId).FirstOrDefault();
                    meetingUser = new MeetingUserAvailabilityDTO
                    {
                        Available = UserAvailability.Available,
                        Attended = coordinator.Attended,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = coordinator.CoordinatorId,
                            FullNameAr = coordinator.Coordinator.FullNameAr,
                            FullNameEn = coordinator.Coordinator.FullNameEn,
                            ProfileImage = coordinator.Coordinator.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName?.JobTitleNameAr : jobTitleName?.JobTitleNameEn,
                            ExternalUser = coordinator.Coordinator.ExternalUser
                        },
                        UserId = coordinator.CoordinatorId,
                        CreatedOn = coordinator.CreatedOn,
                        Meetings = UserAvailability.Meetings
                    };

                    return meetingUser;
                }
                else
                    return null;
            }
            else
            {
                return new MeetingUserAvailabilityDTO();
            }
        }

        public List<MeetingUserAvailabilityDTO> InsertMeetingMultiAttendeesOrCoordinators(ListOfMeetingUserDTO attendeeDTOs)
        {
            var result = new List<MeetingUserAvailabilityDTO>();
            foreach (var attendeeDTO in attendeeDTOs.UserDTO)
            {
                MeetingUserAvailabilityDTO meetingUser = new MeetingUserAvailabilityDTO();
                var meeting = _unitOfWork.GetRepository<Meeting>().GetById(attendeeDTO.MeetingId);
                string subject = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestSubject", _sessionServices.CultureIsArabic);

                var UserAvailability = CheckAvailability(attendeeDTO.UserId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, attendeeDTO.UserType, true);
                if (attendeeDTO.UserType == UserType.Attendee)
                {
                    string message = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestAttendeeMessage", _sessionServices.CultureIsArabic);

                    if (!meeting.MeetingCoordinators.Any(x => x.CoordinatorId == attendeeDTO.UserId) &&
                        !meeting.MeetingAttendees.Any(x => x.AttendeeId == attendeeDTO.UserId))
                    {
                        var attendee = _unitOfWork.GetRepository<MeetingAttendee>().Insert(new MeetingAttendee
                        {
                            AttendeeId = attendeeDTO.UserId,
                            Available = UserAvailability.Available,
                            ConfirmeAttendance = false,
                            MeetingId = attendeeDTO.MeetingId,
                            State = AttendeeState.New
                        });
                        attendee.Attendee = _unitOfWork.GetRepository<User>().GetById(attendeeDTO.UserId);
                        var jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == attendee.Attendee.JobTitleId).FirstOrDefault();
                        meetingUser = new MeetingUserAvailabilityDTO
                        {
                            Available = UserAvailability.Available,
                            Attended = attendee.Attended,
                            User = new Models.UserDetailsDTO
                            {
                                UserId = attendee.AttendeeId,
                                FullNameAr = attendee.Attendee.FullNameAr,
                                FullNameEn = attendee.Attendee.FullNameEn,
                                ProfileImage = attendee.Attendee.ProfileImage,
                                JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                                ExternalUser = attendee.Attendee.ExternalUser
                            },
                            UserId = attendee.AttendeeId,
                            CreatedOn = attendee.CreatedOn,
                            Meetings = UserAvailability.Meetings,
                            ConfirmeAttendance = attendee.ConfirmeAttendance
                        };

                        result.Add(meetingUser);

                        //   string Message = "";
                        //   string mailSubject = "";

                        //GetMailMessageForMeeting(attendeeDTO, ref Message, ref mailSubject, meeting.Title);
                        //   AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                        //   Task.Run(() =>
                        //   {
                        //       _MailServices.SendNotificationEmail(attendee.Attendee.Email, mailSubject,
                        //           null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                        //           );

                        //   });

                        //_MailServices.SendNotificationEmail(attendee.Attendee.Email, subject, message.Replace("{Title}", meeting.Title), true, null, "", "", null, "");
                    }
                }
                else if (attendeeDTO.UserType == UserType.Coordinator)
                {
                    string message = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestCoordinatorMessage", _sessionServices.CultureIsArabic);

                    if (!meeting.MeetingCoordinators.Any(x => x.CoordinatorId == attendeeDTO.UserId) &&
                        !meeting.MeetingAttendees.Any(x => x.AttendeeId == attendeeDTO.UserId))
                    {
                        var coordinator = _unitOfWork.GetRepository<MeetingCoordinator>().Insert(new MeetingCoordinator
                        {

                            CoordinatorId = attendeeDTO.UserId,
                            Available = UserAvailability.Available,
                            ConfirmeAttendance = false,
                            MeetingId = attendeeDTO.MeetingId,
                            State = AttendeeState.New,
                        });
                        coordinator.Coordinator = _unitOfWork.GetRepository<User>().GetById(attendeeDTO.UserId);
                        var jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == coordinator.Coordinator.JobTitleId).FirstOrDefault();
                        meetingUser = new MeetingUserAvailabilityDTO
                        {
                            Available = UserAvailability.Available,
                            Attended = coordinator.Attended,
                            User = new Models.UserDetailsDTO
                            {
                                UserId = coordinator.CoordinatorId,
                                FullNameAr = coordinator.Coordinator.FullNameAr,
                                FullNameEn = coordinator.Coordinator.FullNameEn,
                                ProfileImage = coordinator.Coordinator.ProfileImage,
                                JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName?.JobTitleNameAr : jobTitleName?.JobTitleNameEn,
                                ExternalUser = coordinator.Coordinator.ExternalUser
                            },
                            UserId = coordinator.CoordinatorId,
                            CreatedOn = coordinator.CreatedOn,
                            Meetings = UserAvailability.Meetings,
                            ConfirmeAttendance = coordinator.ConfirmeAttendance
                        };

                        result.Add(meetingUser);

                        //string Message = "";
                        //string mailSubject = "";

                        //GetMailMessageForMeeting(attendeeDTO, ref Message, ref mailSubject, meeting.Title);
                        //AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                        //Task.Run(() =>
                        //{
                        //    _MailServices.SendNotificationEmail(coordinator.Coordinator.Email, mailSubject,
                        //        null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                        //        );

                        //});
                        //_MailServices.SendNotificationEmail(coordinator.Coordinator.Email, subject, message.Replace("{Title}", meeting.Title), false, null, "", "", null, "");
                    }
                }
            }
            return result;
        }




        // TODO Check code
        public MeetingAvailabilityDTO CheckAvailability(int attendeeId, int meetingId, DateTimeOffset From, DateTimeOffset To, UserType userType, bool withConfict = false)
        {
            MeetingAvailabilityDTO meetingAvailability = new MeetingAvailabilityDTO();
            switch (userType)
            {
                case UserType.Coordinator:
                    var coordinatorMeetings = _unitOfWork.GetRepository<Meeting>().GetAll()
                        .Where(x => (withConfict) ? (
                            (From >= x.MeetingFromTime && From <= x.MeetingToTime)
                         || (To >= x.MeetingFromTime && To <= x.MeetingToTime) || (From <= x.MeetingFromTime && To >= x.MeetingToTime)
                         ) : x.MeetingFromTime.Date == From.Date)
                        .Where(x => (x.Id != meetingId &&
                                (x.MeetingCoordinators.Any(x => x.CoordinatorId == attendeeId)
                                || x.MeetingAttendees.Any(x => x.AttendeeId == attendeeId))));
                    meetingAvailability = new MeetingAvailabilityDTO
                    {
                        Meetings = coordinatorMeetings.Select(x => new MeetingDetailsDTO
                        {
                            Id = x.Id,
                            Date = x.Date,
                            MeetingFromTime = x.MeetingFromTime,
                            MeetingToTime = x.MeetingToTime,
                            ReminderBeforeMinutes = x.ReminderBeforeMinutes,
                            Repated = x.Repated,
                            Subject = x.Subject,
                            Title = x.Title,
                            MeetingAttendees = x.MeetingCoordinators.Select(y => new MeetingAttendeeDTO
                            {
                                Attendee = new Models.UserDetailsDTO
                                {
                                    UserId = y.CoordinatorId,
                                    FullNameAr = y.Coordinator.FullNameAr,
                                    FullNameEn = y.Coordinator.FullNameEn,
                                    ProfileImage = y.Coordinator.ProfileImage,
                                    ExternalUser = y.Coordinator.ExternalUser
                                },
                                AttendeeId = y.CoordinatorId,
                                Available = y.Available,
                                ConfirmeAttendance = y.ConfirmeAttendance,
                                State = y.State,
                            }).ToList()
                        }).ToList(),
                        Available = coordinatorMeetings.Where(c => (From >= c.MeetingFromTime && From <= c.MeetingToTime)
                         || (To >= c.MeetingFromTime && To <= c.MeetingToTime) || (From <= c.MeetingFromTime && To >= c.MeetingToTime)).Count() == 0
                    };
                    break;
                case UserType.Attendee:
                    var attendeeMeetings = _unitOfWork.GetRepository<Meeting>().GetAll()
                          .Where(x => (withConfict) ? (
                            (From >= x.MeetingFromTime && From <= x.MeetingToTime)
                         || (To >= x.MeetingFromTime && To <= x.MeetingToTime) || (From <= x.MeetingFromTime && To >= x.MeetingToTime)
                         ) : x.MeetingFromTime.Date == From.Date)
                        .Where(x => (x.Id != meetingId &&
                                (x.MeetingCoordinators.Any(x => x.CoordinatorId == attendeeId)
                                || x.MeetingAttendees.Any(x => x.AttendeeId == attendeeId))));
                    meetingAvailability = new MeetingAvailabilityDTO
                    {
                        Meetings = attendeeMeetings.Select(x => new MeetingDetailsDTO
                        {
                            Id = x.Id,
                            Date = x.Date,
                            MeetingFromTime = x.MeetingFromTime,
                            MeetingToTime = x.MeetingToTime,
                            ReminderBeforeMinutes = x.ReminderBeforeMinutes,
                            Repated = x.Repated,
                            Subject = x.Subject,
                            Title = x.Title,
                            MeetingAttendees = x.MeetingAttendees.Select(y => new MeetingAttendeeDTO
                            {
                                Attendee = new Models.UserDetailsDTO
                                {
                                    UserId = y.AttendeeId,
                                    FullNameAr = y.Attendee.FullNameAr,
                                    FullNameEn = y.Attendee.FullNameEn,
                                    ProfileImage = y.Attendee.ProfileImage,
                                    ExternalUser = y.Attendee.ExternalUser
                                },
                                AttendeeId = y.AttendeeId,
                                Available = y.Available,
                                ConfirmeAttendance = y.ConfirmeAttendance,
                                State = y.State,
                            }).ToList()
                        }).ToList(),
                        Available = attendeeMeetings.Where(c => (From >= c.MeetingFromTime && From <= c.MeetingToTime)
                          || (To >= c.MeetingFromTime && To <= c.MeetingToTime) || (From <= c.MeetingFromTime && To >= c.MeetingToTime)).Count() == 0
                    };
                    break;
            }
            return meetingAvailability;
        }
        public MeetingUserAvailabilityDTO ChangeMeetingAttendeesOrCoordinatorState(int userId, int meetingId, UserType userType, AttendeeState state)
        {
            MeetingUserAvailabilityDTO meetingAvailability = new MeetingUserAvailabilityDTO();
            var meeting = _unitOfWork.GetRepository<Meeting>().GetById(meetingId);
            string subject = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestSubject", _sessionServices.CultureIsArabic);
            // build MeetingUserDTO obj to to email
            MeetingUserDTO meetingUserDTO = new MeetingUserDTO()
            {
                MeetingId = meetingId,
                UserId = userId,
                UserType = userType,
                State = state,

            };
            switch (userType)
            {

                case UserType.Coordinator:
                    string message = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestCoordinatorMessage", _sessionServices.CultureIsArabic);
                    var coordinator = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll()
                                     .Where(x => x.MeetingId == meetingId && x.CoordinatorId == userId).FirstOrDefault();
                    coordinator.State = state;
                    if (state == AttendeeState.Pending)
                        coordinator.SendingDate = DateTimeOffset.Now;
                    _unitOfWork.GetRepository<MeetingCoordinator>().Update(coordinator);
                    coordinator.Coordinator = _unitOfWork.GetRepository<User>().GetById(userId);
                    var Availability = CheckAvailability(userId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, UserType.Coordinator);
                    var jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == coordinator.Coordinator.JobTitleId).FirstOrDefault();
                    meetingAvailability = new MeetingUserAvailabilityDTO
                    {
                        Available = Availability.Available,
                        Meetings = Availability.Meetings,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = coordinator.Coordinator.UserId,
                            FullNameAr = coordinator.Coordinator.FullNameAr,
                            FullNameEn = coordinator.Coordinator.FullNameEn,
                            ProfileImage = coordinator.Coordinator.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                            ExternalUser = coordinator.Coordinator.ExternalUser
                        },
                        UserId = userId,
                        ConfirmeAttendance = coordinator.ConfirmeAttendance,
                        CreatedOn = coordinator.CreatedOn,
                        SendingDate = coordinator.SendingDate
                    };
                    if (state != AttendeeState.Confirmed && state != AttendeeState.Reject)
                    {
                        //   var subject= _commiteeLocalizationService.GetAll().W
                        var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewcoordinatorNotificationText");
                        CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = coordinator.CoordinatorId,
                            TextAR = loc.CommiteeLocalizationAr + " " + meeting.Title,
                            TextEn = loc.CommiteeLocalizationEn + " " + meeting.Title,
                            MeetingId = meeting.Id
                        };
                        List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                        _committeeNotificationService.Insert(committeeNotifications);
                        // _MailServices.SendNotificationEmail(coordinator.Coordinator.Email, subject, message.Replace("{Title}", meeting.Title), false, null, "", "", null, "");

                        string Message = "";
                        string mailSubject = "";

                        GetMailMessageForMeeting(meetingUserDTO, ref Message, ref mailSubject, meeting.Title);
                        AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                        Task.Run(() =>
                        {
                            _MailServices.SendNotificationEmail(coordinator.Coordinator.Email, mailSubject,
                                null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                );

                        });


                        //Send SMS If This user has mobile Number
                        if (coordinator.Coordinator.NotificationBySms && !string.IsNullOrEmpty(coordinator.Coordinator.Mobile))
                        {
                            smsServices.SendSMS(coordinator.Coordinator.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + meeting.Title, null);
                        }
                    }

                    break;
                case UserType.Attendee:
                    message = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestAttendeeMessage", _sessionServices.CultureIsArabic);
                    var Attendee = _unitOfWork.GetRepository<MeetingAttendee>().GetAll()
                                   .Where(x => x.MeetingId == meetingId && x.AttendeeId == userId).FirstOrDefault();
                    Attendee.State = state;
                    if (state == AttendeeState.Pending)
                        Attendee.SendingDate = DateTimeOffset.Now;
                    _unitOfWork.GetRepository<MeetingAttendee>().Update(Attendee);
                    Attendee.Attendee = _unitOfWork.GetRepository<User>().GetById(userId);
                    var AttendeeAvailability = CheckAvailability(userId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, UserType.Coordinator);
                    jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == Attendee.Attendee.JobTitleId).FirstOrDefault();
                    meetingAvailability = new MeetingUserAvailabilityDTO
                    {
                        Available = AttendeeAvailability.Available,
                        Meetings = AttendeeAvailability.Meetings,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = Attendee.Attendee.UserId,
                            FullNameAr = Attendee.Attendee.FullNameAr,
                            FullNameEn = Attendee.Attendee.FullNameEn,
                            ProfileImage = Attendee.Attendee.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                            ExternalUser = Attendee.Attendee.ExternalUser
                        },
                        UserId = userId,
                        ConfirmeAttendance = Attendee.ConfirmeAttendance,
                        CreatedOn = Attendee.CreatedOn,
                        SendingDate = Attendee.SendingDate

                    };
                    if (state != AttendeeState.Confirmed && state != AttendeeState.Reject)
                    {
                        var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewAttendeeNotificationText");
                        CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = Attendee.AttendeeId,
                            TextAR = loc.CommiteeLocalizationAr + " " + meeting.Title,
                            TextEn = loc.CommiteeLocalizationEn + " " + meeting.Title,
                            MeetingId = meeting.Id
                        };
                        List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                        _committeeNotificationService.Insert(committeeNotifications);
                        //_MailServices.SendNotificationEmail(Attendee.Attendee.Email, subject, message.Replace("{Title}", meeting.Title), false, null, "", "", null, "");

                        //Send SMS If This user has mobile Number
                        if (Attendee.Attendee.NotificationBySms && !string.IsNullOrEmpty(Attendee.Attendee.Mobile))
                        {
                            smsServices.SendSMS(Attendee.Attendee.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + meeting.Title, null);
                        }
                        string Message = "";
                        string mailSubject = "";
                        GetMailMessageForMeeting(meetingUserDTO, ref Message, ref mailSubject, meeting.Title);
                        AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                        Task.Run(() =>
                        {
                            _MailServices.SendNotificationEmail(Attendee.Attendee.Email, mailSubject,
                                null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                );

                        });

                    }
                    break;
            }
            return meetingAvailability;
        }

        public MeetingUserAvailabilityDTO ConfirmMeetingAttendeesOrCoordinatorState(string userIdEncrpted, string meetingIdEncrpted, UserType userType, AttendeeState state)
        {
            // For userId
            string decodeduserid = HttpUtility.UrlDecode(userIdEncrpted);
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _sessionServices.UserIdAndRoleIdAfterDecrypt(decodeduserid, false);
            int userId = UserIdAndUserRoleId.Id;
            //for MeetingId
            string decodedMeetingId = HttpUtility.UrlDecode(meetingIdEncrpted);
            UserIdAndRoleIdAfterDecryptDTO meetingIdAfterDecrpt = _sessionServices.UserIdAndRoleIdAfterDecrypt(decodedMeetingId, false);
            int meetingId = meetingIdAfterDecrpt.Id;

            MeetingUserAvailabilityDTO meetingAvailability = new MeetingUserAvailabilityDTO();
            var meeting = _unitOfWork.GetRepository<Meeting>().GetById(meetingId);
            JobTitle jobTitleName = null;
            switch (userType)
            {

                case UserType.Coordinator:

                    var coordinator = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll()
                                     .Where(x => x.MeetingId == meetingId && x.CoordinatorId == userId).FirstOrDefault();
                    if (coordinator == null) // confirm or reject from replacing user
                    {
                        MeetingCoordinator meetingCoordinator = new MeetingCoordinator()
                        {
                            MeetingId = meetingId,
                            CoordinatorId = userId,
                            State = state
                        };
                        _unitOfWork.GetRepository<MeetingCoordinator>().Insert(meetingCoordinator);

                        var user = _unitOfWork.GetRepository<User>().GetById(userId);
                        jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == user.JobTitleId).FirstOrDefault();
                    }
                    else
                    {

                        coordinator.State = state; // update to Confirm
                        _unitOfWork.GetRepository<MeetingCoordinator>().Update(coordinator);
                        coordinator.Coordinator = _unitOfWork.GetRepository<User>().GetById(userId);
                        jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == coordinator.Coordinator.JobTitleId).FirstOrDefault();
                    }


                    var Availability = CheckAvailability(userId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, UserType.Coordinator);
                    meetingAvailability = new MeetingUserAvailabilityDTO
                    {
                        Available = Availability.Available,
                        Meetings = Availability.Meetings,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = coordinator.Coordinator.UserId,
                            FullNameAr = coordinator.Coordinator.FullNameAr,
                            FullNameEn = coordinator.Coordinator.FullNameEn,
                            ProfileImage = coordinator.Coordinator.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                            ExternalUser = coordinator.Coordinator.ExternalUser
                        },
                        UserId = userId,
                        ConfirmeAttendance = coordinator.ConfirmeAttendance,
                        CreatedOn = coordinator.CreatedOn,
                        SendingDate = coordinator.SendingDate
                    };


                    break;
                case UserType.Attendee:

                    var Attendee = _unitOfWork.GetRepository<MeetingAttendee>().GetAll()
                                   .Where(x => x.MeetingId == meetingId && x.AttendeeId == userId).FirstOrDefault();
                    if (Attendee == null) // confirm or reject from replacing user
                    {
                        MeetingAttendee meetingAttendee = new MeetingAttendee()
                        {
                            MeetingId = meetingId,
                            AttendeeId = userId,
                            State = state
                        };
                        _unitOfWork.GetRepository<MeetingAttendee>().Insert(meetingAttendee);
                        var user = _unitOfWork.GetRepository<User>().GetById(userId);
                        jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == user.JobTitleId).FirstOrDefault();

                    }
                    else
                    {

                        Attendee.State = state; // update to Confirm

                        _unitOfWork.GetRepository<MeetingAttendee>().Update(Attendee);
                        Attendee.Attendee = _unitOfWork.GetRepository<User>().GetById(userId);
                        jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == Attendee.Attendee.JobTitleId).FirstOrDefault();
                    }
                    var AttendeeAvailability = CheckAvailability(userId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, UserType.Coordinator);
                    meetingAvailability = new MeetingUserAvailabilityDTO
                    {
                        Available = AttendeeAvailability.Available,
                        Meetings = AttendeeAvailability.Meetings,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = Attendee.Attendee.UserId,
                            FullNameAr = Attendee.Attendee.FullNameAr,
                            FullNameEn = Attendee.Attendee.FullNameEn,
                            ProfileImage = Attendee.Attendee.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                            ExternalUser = Attendee.Attendee.ExternalUser
                        },
                        UserId = userId,
                        ConfirmeAttendance = Attendee.ConfirmeAttendance,
                        CreatedOn = Attendee.CreatedOn,
                        SendingDate = Attendee.SendingDate

                    };

                    break;
            }
            return meetingAvailability;


        }

        public MeetingUserAvailabilityDTO ToogleCoordinatorConfirmMeetingAttendance(int userId, int meetingId, UserType type)
        {
            MeetingUserAvailabilityDTO meetingAvailability = new MeetingUserAvailabilityDTO();
            if (type == UserType.Coordinator)
            {
                var coordinator = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll()
                                        .Where(x => x.MeetingId == meetingId && x.CoordinatorId == userId).FirstOrDefault();
                if (coordinator != null)
                {
                    var meeting = _unitOfWork.GetRepository<Meeting>().GetById(meetingId);
                    // string subject = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestSubject", _sessionServices.CultureIsArabic);


                    // case UserType.Coordinator:
                    //string message = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestCoordinatorMessage", _sessionServices.CultureIsArabic);

                    coordinator.ConfirmeAttendance = !coordinator.ConfirmeAttendance;

                    _unitOfWork.GetRepository<MeetingCoordinator>().Update(coordinator);
                    coordinator.Coordinator = _unitOfWork.GetRepository<User>().GetById(userId);
                    var Availability = CheckAvailability(userId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, UserType.Coordinator);
                    var jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == coordinator.Coordinator.JobTitleId).FirstOrDefault();
                    meetingAvailability = new MeetingUserAvailabilityDTO
                    {
                        Available = Availability.Available,
                        Meetings = Availability.Meetings,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = coordinator.Coordinator.UserId,
                            FullNameAr = coordinator.Coordinator.FullNameAr,
                            FullNameEn = coordinator.Coordinator.FullNameEn,
                            ProfileImage = coordinator.Coordinator.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName?.JobTitleNameAr : jobTitleName?.JobTitleNameEn,
                            ExternalUser = coordinator.Coordinator.ExternalUser
                        },
                        UserId = userId,
                        ConfirmeAttendance = coordinator.ConfirmeAttendance
                    };
                    //if (state != AttendeeState.Confirmed && state != AttendeeState.Reject)
                    //{
                    //    //   var subject= _commiteeLocalizationService.GetAll().W
                    //    var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewcoordinatorNotificationText");
                    //    CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                    //    {
                    //        IsRead = false,
                    //        UserId = coordinator.CoordinatorId,
                    //        TextAR = loc.CommiteeLocalizationAr + " " + meeting.Title,
                    //        TextEn = loc.CommiteeLocalizationEn + " " + meeting.Title,
                    //        MeetingId = meeting.Id
                    //    };
                    //    List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                    //    _committeeNotificationService.Insert(committeeNotifications);
                    //    _MailServices.SendNotificationEmail(coordinator.Coordinator.Email, subject, message.Replace("{Title}", meeting.Title), false, null, "", "", null, "");
                    //}  
                }
            }
            else if (type == UserType.Attendee)
            {
                var Attendee = _unitOfWork.GetRepository<MeetingAttendee>().GetAll()
                                       .Where(x => x.MeetingId == meetingId && x.AttendeeId == userId).FirstOrDefault();
                if (Attendee != null)
                {
                    var meeting = _unitOfWork.GetRepository<Meeting>().GetById(meetingId);
                    Attendee.ConfirmeAttendance = !Attendee.ConfirmeAttendance;
                    _unitOfWork.GetRepository<MeetingAttendee>().Update(Attendee);
                    Attendee.Attendee = _unitOfWork.GetRepository<User>().GetById(userId);
                    var Availability = CheckAvailability(userId, meeting.Id, meeting.MeetingFromTime, meeting.MeetingToTime, UserType.Attendee);
                    var jobTitleName = _unitOfWork.GetRepository<JobTitle>().GetAll().Where(x => x.JobTitleId == Attendee.Attendee.JobTitleId).FirstOrDefault();
                    meetingAvailability = new MeetingUserAvailabilityDTO
                    {
                        Available = Availability.Available,
                        Meetings = Availability.Meetings,
                        User = new Models.UserDetailsDTO
                        {
                            UserId = Attendee.Attendee.UserId,
                            FullNameAr = Attendee.Attendee.FullNameAr,
                            FullNameEn = Attendee.Attendee.FullNameEn,
                            ProfileImage = Attendee.Attendee.ProfileImage,
                            JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                            ExternalUser = Attendee.Attendee.ExternalUser
                        },
                        UserId = userId,
                        ConfirmeAttendance = Attendee.ConfirmeAttendance
                    };
                }
            }

            return meetingAvailability;
        }





        public override IEnumerable<MeetingDTO> Update(IEnumerable<MeetingDTO> Entities)
        {
            foreach (var item in Entities)
            {
                var meeting = _unitOfWork.GetRepository<Meeting>().GetAll(false).FirstOrDefault(x => x.Id == item.Id);
                meeting.ApproveManual = item.ApproveManual;
                meeting.Date = item.Date;
                meeting.IsSecret = item.IsSecret;
                meeting.MeetingFromTime = item.MeetingFromTime;
                meeting.MeetingToTime = item.MeetingToTime;
                meeting.MemberConfirmation = item.MemberConfirmation;
                meeting.PermitedToEnterMeeting = item.PermitedToEnterMeeting;
                meeting.PhysicalLocation = item.PhysicalLocation;
                meeting.ReferenceNumber = item.ReferenceNumber;
                meeting.ReminderBeforeMinutes = item.ReminderBeforeMinutes;
                meeting.Repated = item.Repated;
                meeting.Subject = item.Subject;
                meeting.Title = item.Title;
                meeting.CommitteId = item.CommitteId;
                meeting.ActualLocation = item.ActualLocation;
                var MeetingProjects = _unitOfWork.GetRepository<MeetingProject>().GetAll().Where(x => x.MeetingId == item.Id).ToList();
                var MeetingURLs = _unitOfWork.GetRepository<MeetingURl>().GetAll().Where(x => x.MeetingId == item.Id).ToList();
                if (MeetingProjects.Count() != 0)
                {
                    _unitOfWork.GetRepository<MeetingProject>().Delete(MeetingProjects);
                    _unitOfWork.SaveChanges();
                }
                if (MeetingURLs.Count() != 0)
                {
                    _unitOfWork.GetRepository<MeetingURl>().Delete(MeetingURLs);
                    _unitOfWork.SaveChanges();
                }
                meeting.MeetingProjects = item.MeetingProjects.Select(x => new MeetingProject
                {
                    MeetingId = item.Id,
                    ProjectId = x.ProjectId
                }).ToList();
                meeting.MeetingURls = item.MeetingURls.Select(x => new MeetingURl
                {
                    OnlineUrl = x.OnlineUrl,
                    MeetingId = item.Id
                }).ToList();
                _unitOfWork.GetRepository<Meeting>().Update(meeting);
                item.IsCreator = meeting.CreatedBy == _sessionServices.UserId;
                item.CreatedByUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == meeting.CreatedBy)
                    .Select(x => new UserDetailsDTO
                    {
                        UserId = x.UserId,
                        FullNameAr = x.FullNameAr,
                        FullNameEn = x.FullNameEn,
                        ProfileImage = x.ProfileImage,
                    }).FirstOrDefault();
                item.IsFinished = meeting.MeetingTopics.Count() > 0 && meeting.MeetingTopics.All(c => c.TopicState == TopicState.Completed || c.TopicState == TopicState.Cancled);
                item.MeetingTopics = meeting.MeetingTopics.AsQueryable().ProjectTo<MeetingTopicDTO>(_Mapper.ConfigurationProvider).ToList();
                item.MeetingCoordinators = meeting.MeetingCoordinators.AsQueryable().ProjectTo<MeetingCoordinatorDTO>(_Mapper.ConfigurationProvider).ToList();
                item.MeetingAttendees = meeting.MeetingAttendees.AsQueryable().ProjectTo<MeetingAttendeeDTO>(_Mapper.ConfigurationProvider).ToList();
                item.IsCoordinator = meeting.MeetingCoordinators.Any(x => x.CoordinatorId == _sessionServices.UserId);
            }
            return Entities;
        }
        public bool ChangeApproveManual(int mettingId, bool ApproveManual)
        {
            try
            {
                var meeting = _unitOfWork.GetRepository<Meeting>().GetById(mettingId);
                meeting.ApproveManual = ApproveManual;
                _unitOfWork.GetRepository<Meeting>().Update(meeting);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public List<MeetingDetailsDTO> DisplayMeetings(DateTimeOffset fromDate, DateTimeOffset? toDate, DisplayMeetingCallType displayMeetingCallType)
        {
            List<MeetingDetailsDTO> meetings;
            switch (displayMeetingCallType)
            {
                case DisplayMeetingCallType.FromBody:

                    #region FromBody Section
                    meetings = _unitOfWork.GetRepository<Meeting>().GetAll()
             .Where(x => x.Date >= fromDate &&
                   (x.Date <= toDate || toDate == null) &&
                   (x.MeetingAttendees.Any(c => c.AttendeeId == _sessionServices.UserId && c.State != AttendeeState.Reject) ||
                    x.MeetingCoordinators.Any(c => c.CoordinatorId == _sessionServices.UserId && c.State != AttendeeState.Reject))).OrderByDescending(x => x.Date)
             .Select(x => new MeetingDetailsDTO
             {
                 Id = x.Id,
                 Date = x.Date,
                 MeetingFromTime = x.MeetingFromTime,
                 MeetingToTime = x.MeetingToTime,
                 Repated = x.Repated,
                 ReminderBeforeMinutes = x.ReminderBeforeMinutes,
                 Subject = x.Subject,
                 Title = x.Title,
                 ReferenceNumber = x.ReferenceNumber,

                 //TODO Change State 
                 MeetingState = (x.Canceled) ? MeetingState.Canceled : (x.Colsed) ? MeetingState.Closed :
                 x.MeetingTopics.Where(c => c.TopicState != TopicState.Cancled && c.TopicType == TopicType.Discussion).All(c => c.TopicState == TopicState.Completed) && x.MeetingTopics.Count > 0 ? MeetingState.Finished : MeetingState.Active,
                 MeetingAttendees = x.MeetingAttendees.Select(z => new MeetingAttendeeDTO
                 {
                     Id = z.Id,
                     MeetingId = (int)z.MeetingId,
                     Attendee = new Models.UserDetailsDTO
                     {
                         UserId = z.AttendeeId,
                         FullNameAr = z.Attendee.FullNameAr,
                         FullNameEn = z.Attendee.FullNameEn,
                         ProfileImage = z.Attendee.ProfileImage,
                         // JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                         ExternalUser = z.Attendee.ExternalUser
                     },
                     AttendeeId = z.AttendeeId,
                     Available = z.Available,
                     ConfirmeAttendance = z.ConfirmeAttendance,
                     State = z.State,
                     UserDelegate = z.UserDelegate != null ? new Models.UserDetailsDTO
                     {
                         UserId = z.UserDelegate.UserId,
                         FullNameAr = z.UserDelegate.FullNameAr,
                         FullNameEn = z.UserDelegate.FullNameEn,
                         ProfileImage = z.UserDelegate.ProfileImage,
                         ExternalUser = z.UserDelegate.ExternalUser
                     } : null

                 }).ToList(),
                 MeetingCoordinator = x.MeetingCoordinators.Select(z => new MeetingCoordinatorDTO
                 {
                     Id = z.Id,
                     MeetingId = (int)z.MeetingId,
                     Coordinator = new Models.UserDetailsDTO
                     {
                         UserId = z.CoordinatorId,
                         FullNameAr = z.Coordinator.FullNameAr,
                         FullNameEn = z.Coordinator.FullNameEn,
                         ProfileImage = z.Coordinator.ProfileImage,
                         // JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                         ExternalUser = z.Coordinator.ExternalUser
                     },
                     CoordinatorId = z.CoordinatorId,
                     Available = z.Available,
                     ConfirmeAttendance = z.ConfirmeAttendance,
                     State = z.State,
                     IsCreator = (z.CoordinatorId == z.CreatedBy),
                     UserDelegate = z.UserDelegate != null ? new Models.UserDetailsDTO
                     {
                         UserId = z.UserDelegate.UserId,
                         FullNameAr = z.UserDelegate.FullNameAr,
                         FullNameEn = z.UserDelegate.FullNameEn,
                         ProfileImage = z.UserDelegate.ProfileImage,
                         ExternalUser = z.UserDelegate.ExternalUser
                     } : null

                 }).ToList()
             }).ToList();
                    return meetings;
                #endregion

                case DisplayMeetingCallType.FromSideBar:

                    #region From SideBar Section
                    meetings = _unitOfWork.GetRepository<Meeting>().GetAll().Where(x => !x.Canceled)
             .Where(x => x.Date >= fromDate && (x.Date <= toDate || toDate == null) &&
             (x.MeetingAttendees.Any(c => c.AttendeeId == _sessionServices.UserId && c.State != AttendeeState.Reject) ||
             x.MeetingCoordinators.Any(c => c.CoordinatorId == _sessionServices.UserId && c.State != AttendeeState.Reject))).OrderByDescending(x => x.Date)
             .Select(x => new MeetingDetailsDTO
             {
                 Id = x.Id,
                 Date = x.Date,
                 MeetingFromTime = x.MeetingFromTime,
                 MeetingToTime = x.MeetingToTime,
                 Repated = x.Repated,
                 ReminderBeforeMinutes = x.ReminderBeforeMinutes,
                 Subject = x.Subject,
                 Title = x.Title,
                 ReferenceNumber = x.ReferenceNumber,

                 //TODO Change State 
                 MeetingState = (x.Canceled) ? MeetingState.Canceled : (x.Colsed) ? MeetingState.Closed :
                 x.MeetingTopics.Where(c => c.TopicState != TopicState.Cancled && c.TopicType == TopicType.Discussion).All(c => c.TopicState == TopicState.Completed) && x.MeetingTopics.Count > 0 ? MeetingState.Finished : MeetingState.Active,
                 MeetingAttendees = x.MeetingAttendees.Select(z => new MeetingAttendeeDTO
                 {
                     Id = z.Id,
                     MeetingId = (int)z.MeetingId,
                     Attendee = new Models.UserDetailsDTO
                     {
                         UserId = z.AttendeeId,
                         FullNameAr = z.Attendee.FullNameAr,
                         FullNameEn = z.Attendee.FullNameEn,
                         ProfileImage = z.Attendee.ProfileImage,
                         // JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                         ExternalUser = z.Attendee.ExternalUser
                     },
                     AttendeeId = z.AttendeeId,
                     Available = z.Available,
                     ConfirmeAttendance = z.ConfirmeAttendance,
                     State = z.State,
                     UserDelegate = z.UserDelegate != null ? new Models.UserDetailsDTO
                     {
                         UserId = z.UserDelegate.UserId,
                         FullNameAr = z.UserDelegate.FullNameAr,
                         FullNameEn = z.UserDelegate.FullNameEn,
                         ProfileImage = z.UserDelegate.ProfileImage,
                         ExternalUser = z.UserDelegate.ExternalUser
                     } : null

                 }).ToList(),
                 MeetingCoordinator = x.MeetingCoordinators.Select(z => new MeetingCoordinatorDTO
                 {
                     Id = z.Id,
                     MeetingId = (int)z.MeetingId,
                     Coordinator = new Models.UserDetailsDTO
                     {
                         UserId = z.CoordinatorId,
                         FullNameAr = z.Coordinator.FullNameAr,
                         FullNameEn = z.Coordinator.FullNameEn,
                         ProfileImage = z.Coordinator.ProfileImage,
                         // JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                         ExternalUser = z.Coordinator.ExternalUser
                     },
                     CoordinatorId = z.CoordinatorId,
                     Available = z.Available,
                     ConfirmeAttendance = z.ConfirmeAttendance,
                     State = z.State,
                     IsCreator = (z.CoordinatorId == z.CreatedBy),
                     UserDelegate = z.UserDelegate != null ? new Models.UserDetailsDTO
                     {
                         UserId = z.UserDelegate.UserId,
                         FullNameAr = z.UserDelegate.FullNameAr,
                         FullNameEn = z.UserDelegate.FullNameEn,
                         ProfileImage = z.UserDelegate.ProfileImage,
                         ExternalUser = z.UserDelegate.ExternalUser
                     } : null

                 }).ToList()
             }).ToList();
                    return meetings;
                #endregion

                default:

                    #region Ather
                    meetings = _unitOfWork.GetRepository<Meeting>().GetAll()
            .Where(x => x.Date >= fromDate && (x.Date <= toDate || toDate == null) &&
            (x.MeetingAttendees.Any(c => c.AttendeeId == _sessionServices.UserId) ||
            x.MeetingCoordinators.Any(c => c.CoordinatorId == _sessionServices.UserId))).OrderByDescending(x => x.Date)
            .Select(x => new MeetingDetailsDTO
            {
                Id = x.Id,
                Date = x.Date,
                MeetingFromTime = x.MeetingFromTime,
                MeetingToTime = x.MeetingToTime,
                Repated = x.Repated,
                ReminderBeforeMinutes = x.ReminderBeforeMinutes,
                Subject = x.Subject,
                Title = x.Title,
                ReferenceNumber = x.ReferenceNumber,

                //TODO Change State 
                MeetingState = (x.Canceled) ? MeetingState.Canceled : (x.Colsed) ? MeetingState.Closed :
                x.MeetingTopics.Where(c => c.TopicState != TopicState.Cancled && c.TopicType == TopicType.Discussion).All(c => c.TopicState == TopicState.Completed) && x.MeetingTopics.Count > 0 ? MeetingState.Finished : MeetingState.Active,
                MeetingAttendees = x.MeetingAttendees.Select(z => new MeetingAttendeeDTO
                {
                    Id = z.Id,
                    MeetingId = (int)z.MeetingId,
                    Attendee = new Models.UserDetailsDTO
                    {
                        UserId = z.AttendeeId,
                        FullNameAr = z.Attendee.FullNameAr,
                        FullNameEn = z.Attendee.FullNameEn,
                        ProfileImage = z.Attendee.ProfileImage,
                        // JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                        ExternalUser = z.Attendee.ExternalUser
                    },
                    AttendeeId = z.AttendeeId,
                    Available = z.Available,
                    ConfirmeAttendance = z.ConfirmeAttendance,
                    State = z.State,
                    UserDelegate = z.UserDelegate != null ? new Models.UserDetailsDTO
                    {
                        UserId = z.UserDelegate.UserId,
                        FullNameAr = z.UserDelegate.FullNameAr,
                        FullNameEn = z.UserDelegate.FullNameEn,
                        ProfileImage = z.UserDelegate.ProfileImage,
                        ExternalUser = z.UserDelegate.ExternalUser
                    } : null

                }).ToList(),
                MeetingCoordinator = x.MeetingCoordinators.Select(z => new MeetingCoordinatorDTO
                {
                    Id = z.Id,
                    MeetingId = (int)z.MeetingId,
                    Coordinator = new Models.UserDetailsDTO
                    {
                        UserId = z.CoordinatorId,
                        FullNameAr = z.Coordinator.FullNameAr,
                        FullNameEn = z.Coordinator.FullNameEn,
                        ProfileImage = z.Coordinator.ProfileImage,
                        // JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                        ExternalUser = z.Coordinator.ExternalUser
                    },
                    CoordinatorId = z.CoordinatorId,
                    Available = z.Available,
                    ConfirmeAttendance = z.ConfirmeAttendance,
                    State = z.State,
                    IsCreator = (z.CoordinatorId == z.CreatedBy),
                    UserDelegate = z.UserDelegate != null ? new Models.UserDetailsDTO
                    {
                        UserId = z.UserDelegate.UserId,
                        FullNameAr = z.UserDelegate.FullNameAr,
                        FullNameEn = z.UserDelegate.FullNameEn,
                        ProfileImage = z.UserDelegate.ProfileImage,
                        ExternalUser = z.UserDelegate.ExternalUser
                    } : null

                }).ToList()
            }).ToList();
                    return meetings;
                    #endregion
            }

        }
        public DataSourceResult<MeetingDetailsDTO> DisplayClosedMeeting(DataSourceRequest dataSourceRequest)
        {

            var meetings = _unitOfWork.GetRepository<Meeting>().GetAll()
                //  .Where(x => x.Date >= fromDate && (x.Date <= toDate || toDate == null) &&
                .Where(x => x.MeetingAttendees.Any(c => c.AttendeeId == _sessionServices.UserId) || x.MeetingCoordinators.Any(c => c.CoordinatorId == _sessionServices.UserId))
                .Where(x => x.Colsed).OrderByDescending(x => x.Date)
                .Select(x => new MeetingDetailsDTO
                {
                    Id = x.Id,
                    Date = x.Date,
                    MeetingFromTime = x.MeetingFromTime,
                    MeetingToTime = x.MeetingToTime,
                    Repated = x.Repated,
                    ReminderBeforeMinutes = x.ReminderBeforeMinutes,
                    Subject = x.Subject,
                    Title = x.Title,
                    MeetingState = MeetingState.Closed
                }).ToDataSourceResult(dataSourceRequest);
            return meetings;
        }
        public DataSourceResult<MeetingDetailsDTO> DisplayFinishedMeeting(DataSourceRequest dataSourceRequest)
        {
            var meetings = _unitOfWork.GetRepository<Meeting>().GetAll()
                  .Where(x => !x.Colsed)
                  .Where(x => x.MeetingTopics.All(c => c.TopicState == TopicState.Completed || c.TopicState == TopicState.Cancled) && x.MeetingTopics.Count() > 0 && !x.MeetingTopics.All(z => z.TopicState == TopicState.Cancled))
                  .Where(x => x.MeetingAttendees.Any(c => c.AttendeeId == _sessionServices.UserId) || x.MeetingCoordinators.Any(c => c.CoordinatorId == _sessionServices.UserId)).OrderByDescending(x => x.Date)
                  .Select(x => new MeetingDetailsDTO
                  {
                      Id = x.Id,
                      Date = x.Date,
                      MeetingFromTime = x.MeetingFromTime,
                      MeetingToTime = x.MeetingToTime,
                      Repated = x.Repated,
                      ReminderBeforeMinutes = x.ReminderBeforeMinutes,
                      Subject = x.Subject,
                      Title = x.Title,
                      MeetingState = MeetingState.Finished
                  }).ToDataSourceResult(dataSourceRequest);
            return meetings;
        }
        public List<MeetingActivityLookup> GetAllActivities(DataSourceRequest dataSourceRequest)
        {
            //var e = 
            var Activities = _unitOfWork.GetRepository<Meeting>().GetAll()
                .Where(x => (x.Surveys.Count() > 0 || x.MeetingTopics.Any(m => m.TopicSurveies.Count > 0)) &&
                (x.MeetingAttendees.Any(c => c.AttendeeId == _sessionServices.UserId) ||
                x.MeetingCoordinators.Any(c => c.CoordinatorId == _sessionServices.UserId)))
                .Select(x => new MeetingActivityLookup
                {
                    MeetingId = x.Id,
                    isCoordinator = x.MeetingCoordinators.Any(x => x.CoordinatorId == _sessionServices.UserId),
                    isCreator = x.CreatedBy == _sessionServices.UserId,
                    MeetingTitle = x.Title,
                    isClosed = x.Colsed,
                    IsStarted = _unitOfWork.GetRepository<MeetingTopic>(false).GetAll(false).Where(m => m.MeetingId == x.Id && m.TopicState != TopicState.Cancled && m.TopicState != TopicState.New).Count() > 0,
                    MeettingDate = x.Date,
                    SurveyId = x.Surveys.FirstOrDefault().SurveyId,
                    SurveyAnswers = _unitOfWork.GetRepository<Survey>(false).GetAll(false)
                                            .Where(z => z.SurveyId == x.Surveys.FirstOrDefault().SurveyId).Select(x => x.SurveyAnswers.Select(z => z.SurveyAnswerUsers.Select(zz => new SurvAnswers { UserId = zz.UserId })).AsEnumerable()).ToList(),
                    MeetingTopicSurveys = _unitOfWork.GetRepository<Survey>(false).GetAll(false)
                                            .Where(z => z.MeetingTopic.MeetingId == x.Id)
                                            .Select(y => new MeetingTopicSurveysDTO
                                            {
                                                SurveyId = y.SurveyId,
                                                SurveyTitle = y.Subject,
                                                TopicId = (int)y.MeetingTopicId,
                                                TopicTitle = y.MeetingTopic.TopicTitle,
                                                TopicState = y.MeetingTopic.TopicState
                                            }).ToList()

                }).ToList();
            foreach (var item in Activities)
            {
                foreach (var top in item.MeetingTopicSurveys)
                {
                    top.SurveyAnswers = _unitOfWork.GetRepository<SurveyAnswer>().GetAll().Where(x => x.SurveyId == top.SurveyId).Select(x => x.SurveyAnswerUsers.Select(zz => new SurvAnswers { UserId = zz.UserId }).AsEnumerable()).ToList();
                }
            }
            return Activities;
        }
        public bool TakeAttendees(int meetingId, AttendeesList AttendedUsers)
        {
            var meeting = _unitOfWork.GetRepository<Meeting>().Find(meetingId);
            if (meeting.Id > 0)
            {

                var coordinatorsShouldAttend = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll().Where(x => x.ConfirmeAttendance && x.MeetingId == meetingId).ToList();
                var meetingAttendees = _unitOfWork.GetRepository<MeetingAttendee>().GetAll().Where(z => z.MeetingId == meetingId).ToList();
                foreach (var Attendee in AttendedUsers.Attendees)
                {
                    if (Attendee.Type == UserType.Attendee)
                    {
                        try
                        {
                            var res = meetingAttendees.Where(x => x.AttendeeId == Attendee.Id).FirstOrDefault();
                            res.Attended = true;
                            _unitOfWork.GetRepository<MeetingAttendee>().Update(res);
                        }
                        catch (Exception)
                        {
                            return false;
                        }


                    }
                    else if (Attendee.Type == UserType.Coordinator)
                    {
                        try
                        {
                            var res = coordinatorsShouldAttend.Where(x => x.CoordinatorId == Attendee.Id).FirstOrDefault();
                            res.Attended = true;
                            _unitOfWork.GetRepository<MeetingCoordinator>().Update(res);
                        }
                        catch (Exception)
                        {
                            return false;
                        }

                    }
                }
                return true;

            }
            return false;
        }
        public bool ColseMeeting(int meetingId)
        {


            var meeting = _unitOfWork.GetRepository<Meeting>().Find(meetingId);
            //meeting.Canceled = true;
            meeting.Colsed = true;
            try
            {
                _unitOfWork.GetRepository<Meeting>().Update(meeting);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CanceledMeeting(int meetingId)
        {


            var meeting = _unitOfWork.GetRepository<Meeting>().Find(meetingId);
            var MeetingAttendees = meeting.MeetingAttendees;
            var MeetingCoordinators = meeting.MeetingCoordinators;

            meeting.Canceled = true;
            meeting.IsCanceled = true;
            try
            {
                _unitOfWork.GetRepository<Meeting>().Update(meeting);
                foreach (var item in MeetingAttendees)
                {
                    string Message = "";
                    string mailSubject = "";

                    GetMailMessageForMeetingForCancelMeeting(new MeetingUserDTO
                    {
                        MeetingId = meeting.Id,
                        State = item.State,
                        UserType = UserType.Attendee,
                        User = mapper.Map<UserDetailsDTO>(item.Attendee)
                    }, ref Message, ref mailSubject, meeting.Title, _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", true),
                                                     _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", false));
                    AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                    Task.Run(() =>
                    {
                        _MailServices.SendNotificationEmail(item.Attendee.Email, mailSubject,
                            null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                            );

                    });
                    // send SMS
                    if (item.AlternativeAttendee is not null)
                    {

                        if (item.AlternativeAttendee.NotificationBySms  && !string.IsNullOrEmpty(item.AlternativeAttendee.Mobile))
                        {
                            smsServices.SendSMS(item.AlternativeAttendee.Mobile, new string[0] { }, "إلغاء الاجتماع" + " " + meeting.Title, null);
                        }
                    }

                }
                foreach (var item in MeetingCoordinators)
                {
                    string Message = "";
                    string mailSubject = "";
                    GetMailMessageForMeetingForCancelMeeting(new MeetingUserDTO
                    {
                        MeetingId = meeting.Id,
                        State = item.State,
                        UserType = UserType.Coordinator,
                        User = mapper.Map<UserDetailsDTO>(item.Coordinator)
                    }, ref Message, ref mailSubject, meeting.Title, _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", true),
                                                     _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", false));
                    AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                    Task.Run(() =>
                    {
                        _MailServices.SendNotificationEmail(item.Coordinator.Email, mailSubject,
                            null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                            );

                    });
                    if (item.Coordinator is not null)
                    {

                        if (item.Coordinator.NotificationBySms && !string.IsNullOrEmpty(item.Coordinator.Mobile))
                        {
                            smsServices.SendSMS(item.Coordinator.Mobile, new string[0] { }, "إلغاء الاجتماع" + " " + meeting.Title, null);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GettingReplacingCoordinateOrAttendee(int meetingId, string reason, int? userId)
        {
            try
            {
                int currentUser = (int)_sessionServices.UserId;
                var meet = _unitOfWork.GetRepository<Meeting>().GetAll().Where(x => x.Id == meetingId).FirstOrDefault();
                var IsCoordinator = meet.MeetingCoordinators.Any(x => x.CoordinatorId == userId);
                var UserAvailability = CheckAvailability(int.Parse(userId.ToString()), meet.Id, meet.MeetingFromTime, meet.MeetingToTime, IsCoordinator ? UserType.Coordinator : UserType.Attendee, true);

                var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewAttendeeNotificationText");
                MeetingUserDTO meetingUserDTO = new MeetingUserDTO()
                {
                    MeetingId = meetingId,
                    UserId = userId.HasValue ? userId.Value : 0,
                    //UserType = userType,
                    State = AttendeeState.Pending,

                };


                foreach (var meetingCoordinate in meet.MeetingCoordinators.ToList())
                {
                    meetingUserDTO.UserType = UserType.Coordinator;
                    if (meetingCoordinate.CoordinatorId == currentUser)
                    {
                        meetingCoordinate.ReasonForReplacing = reason;
                        meetingCoordinate.AlternativeCoordinatorId = userId;

                        _unitOfWork.GetRepository<MeetingCoordinator>().Update(meetingCoordinate);
                        //Send SMS If This user has mobile Number
                        if (userId.HasValue)
                        {
                            _unitOfWork.GetRepository<MeetingCoordinator>().Insert(new MeetingCoordinator()
                            {
                                CoordinatorId = (int)userId,
                                State = AttendeeState.Pending,
                                Available = UserAvailability.Available,
                                MeetingId = meetingId,
                                UserDelegate = _unitOfWork.GetRepository<User>().GetAll().FirstOrDefault(x => x.UserId == currentUser),
                            });

                            if (meetingCoordinate.AlternativeCoordinator.NotificationBySms && !string.IsNullOrEmpty(meetingCoordinate.AlternativeCoordinator.Mobile))
                            {
                                smsServices.SendSMS(meetingCoordinate.AlternativeCoordinator.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + meet.Title, null);
                            }
                            string Message = "";
                            string mailSubject = "";
                            GetMailMessageForMeetingGenReplacingCoordinate(meetingUserDTO,
                                                        ref Message,
                                                        ref mailSubject,
                                                        meet.Title,
                                                     _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", true),
                                                     _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", false));

                            AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                            Task.Run(() =>
                            {
                                _MailServices.SendNotificationEmail(meetingCoordinate.AlternativeCoordinator.Email, mailSubject,
                                    null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                    );

                            });
                        }
                    }


                }

                foreach (var meetingAttendee in meet.MeetingAttendees.ToList())
                {
                    meetingUserDTO.UserType = UserType.Attendee;
                    if (meetingAttendee.AttendeeId == currentUser)
                    {
                        meetingAttendee.ReasonForReplacing = reason;
                        meetingAttendee.AlternativeAttendeeId = userId;


                        _unitOfWork.GetRepository<MeetingAttendee>().Update(meetingAttendee);

                        if (userId.HasValue)
                        {

                            _unitOfWork.GetRepository<MeetingAttendee>().Insert(new MeetingAttendee()
                            {
                                AttendeeId = (int)userId,
                                State = AttendeeState.Pending,
                                Available = UserAvailability.Available,
                                MeetingId = meetingId,
                                UserDelegate = _unitOfWork.GetRepository<User>().GetAll().FirstOrDefault(x => x.UserId == currentUser)

                            });
                            //Send SMS If This user has mobile Number
                            if (meetingAttendee.AlternativeAttendee.NotificationBySms && !string.IsNullOrEmpty(meetingAttendee.AlternativeAttendee.Mobile))
                            {
                                smsServices.SendSMS(meetingAttendee.AlternativeAttendee.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + meet.Title, null);
                            }
                            string Message = "";
                            string mailSubject = "";
                            GetMailMessageForMeeting(meetingUserDTO, ref Message, ref mailSubject, meet.Title);
                            AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                            Task.Run(() =>
                            {
                                _MailServices.SendNotificationEmail(meetingAttendee.AlternativeAttendee.Email, mailSubject,
                                    null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                    );

                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }

            //Sending Mail To Alternative User

            return true;

        }
        public /*AllSurveyDTOINMeeting*/ List<SurveyDTO> GetSurviesByMeetingId(int meetingId)
        {
            //IQueryable query = this._UnitOfWork.GetRepository<Survey>().GetAll(false).Where(x => x.MeetingId == meetingId);
            //var res = query.ProjectTo<SurveyDTO>(_Mapper.ConfigurationProvider).ToList();
            return mapper.Map<List<SurveyDTO>>(this._UnitOfWork.GetRepository<Survey>().GetAll().Where(x => x.MeetingId == meetingId));
        }


        public override MeetingDTO GetDetails(object Id, bool WithTracking = true)
        {
            var meeting = base.GetDetails(Id, WithTracking);
            if (meeting.CommitteId != null)
                meeting.Commitee = _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == meeting.CommitteId).Select(z => new CommiteeDTO { Name = z.Name, CommiteeId = z.CommiteeId }).FirstOrDefault();
            foreach (var item in meeting.MeetingCoordinators)
            {
                if (item.Coordinator.JobTitleId != null)
                {
                    item.Coordinator.JobTitleName = _sessionServices.CultureIsArabic ?
                        _unitOfWork.GetRepository<JobTitle>().GetById(item.Coordinator.JobTitleId)?.JobTitleNameAr :
                        _unitOfWork.GetRepository<JobTitle>().GetById(item.Coordinator.JobTitleId)?.JobTitleNameEn;
                }
            }
            foreach (var item in meeting.MeetingAttendees)
            {
                if (item.Attendee.JobTitleId != null)
                {
                    item.Attendee.JobTitleName = _sessionServices.CultureIsArabic ?
                        _unitOfWork.GetRepository<JobTitle>().GetById(item.Attendee.JobTitleId).JobTitleNameAr :
                        _unitOfWork.GetRepository<JobTitle>().GetById(item.Attendee.JobTitleId).JobTitleNameEn;
                }
            }
            // To Load Active meetig 
            meeting.MeetingTopics = meeting.MeetingTopics.Where(x => x.TopicState == TopicState.InProgress || x.TopicState == TopicState.InProgressPaused).ToList();
            meeting.IsCoordinator = meeting.MeetingCoordinators.Any(x => x.CoordinatorId == _sessionServices.UserId);
            meeting.IsCreator = meeting.CreatedBy == _sessionServices.UserId;
            var meetingTopics = _unitOfWork.GetRepository<MeetingTopic>().GetAll().Where(c => c.MeetingId == meeting.Id).AsQueryable().ProjectTo<MeetingTopicDTO>(_Mapper.ConfigurationProvider);
            meeting.IsFinished = meetingTopics.Count() > 0 && meetingTopics.All(c => c.TopicState == TopicState.Completed || c.TopicState == TopicState.Cancled);
            // URL Guard 
            if (meeting.IsSecret && (meeting.MeetingAttendees.Any(x => x.AttendeeId == _sessionServices.UserId) || meeting.MeetingCoordinators.Any(x => x.CoordinatorId == _sessionServices.UserId) || meeting.CreatedBy == _sessionServices.UserId))
                return meeting;
            else if (!meeting.IsSecret)
                return meeting;
            else
                return null;
        }
        public override IEnumerable<MeetingDTO> Insert(IEnumerable<MeetingDTO> entities)
        {
            var entity = entities.FirstOrDefault();
            IEnumerable<MeetingDTO> newEntity;
            if (!entity.Repated)
            {
                newEntity = base.Insert(entities);
                _unitOfWork.GetRepository<MeetingCoordinator>().Insert(new MeetingCoordinator
                {
                    CoordinatorId = (int)_sessionServices.UserId,
                    Available = true,
                    ConfirmeAttendance = false,
                    MeetingId = newEntity.FirstOrDefault().Id,
                    State = AttendeeState.New,
                    IsCreator = true
                });
                newEntity.First().IsCoordinator = newEntity.First().MeetingCoordinators.Any(x => x.CoordinatorId == _sessionServices.UserId);
                newEntity.First().IsCreator = newEntity.First().CreatedBy == _sessionServices.UserId;
                newEntity.First().CreatedByUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == newEntity.FirstOrDefault().CreatedBy)
                    .Select(x => new UserDetailsDTO
                    {
                        UserId = x.UserId,
                        FullNameAr = x.FullNameAr,
                        FullNameEn = x.FullNameEn,
                        ProfileImage = x.ProfileImage,
                    }).FirstOrDefault();
            }
            else
            {
                List<MeetingDTO> meetings = new List<MeetingDTO>();
                entity.ReferenceNumber = _unitOfWork.GetRepository<Meeting>().GetAll().Max(x => x.Id) + 1;
                entity.IsCoordinator = entity.MeetingCoordinators.Any(x => x.CoordinatorId == _sessionServices.UserId);
                entity.IsCreator = entity.CreatedBy == _sessionServices.UserId;
                meetings.Add(entity);

                var numofdays = (int)entity.PeriodByDays;
                for (int i = 1; i < entity.RepatedTimes; i++)
                {
                    var Relatedentity = new MeetingDTO()
                    {
                        ApproveManual = entity.ApproveManual,
                        Colsed = entity.Colsed,
                        CommitteId = entity.CommitteId,
                        Date = entity.Date,
                        IsSecret = entity.IsSecret,
                        MeetingFromTime = entity.MeetingFromTime,
                        MeetingProjects = entity.MeetingProjects,
                        MeetingToTime = entity.MeetingToTime,
                        MeetingURls = entity.MeetingURls,
                        MemberConfirmation = entity.MemberConfirmation,
                        PeriodByDays = entity.PeriodByDays,
                        PermitedToEnterMeeting = entity.PermitedToEnterMeeting,
                        PhysicalLocation = entity.PhysicalLocation,
                        ReferenceNumber = entity.ReferenceNumber,
                        ReminderBeforeMinutes = entity.ReminderBeforeMinutes,
                        Repated = entity.Repated,
                        RepatedTimes = entity.RepatedTimes,
                        Subject = entity.Subject,
                        Title = entity.Title
                    };
                    Relatedentity.Date = meetings[i - 1].Date.AddDays(numofdays);
                    Relatedentity.MeetingFromTime = meetings[i - 1].MeetingFromTime.AddDays(numofdays);
                    Relatedentity.MeetingToTime = meetings[i - 1].MeetingToTime.AddDays(numofdays);
                    meetings.Add(Relatedentity);

                }
                newEntity = base.Insert(meetings);
                foreach (var item in newEntity)
                {
                    _unitOfWork.GetRepository<MeetingCoordinator>().Insert(new MeetingCoordinator
                    {
                        CoordinatorId = (int)_sessionServices.UserId,
                        Available = true,
                        ConfirmeAttendance = false,
                        MeetingId = item.Id,
                        State = AttendeeState.New,
                        IsCreator = true
                    });
                    item.CreatedByUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == item.CreatedBy)
                    .Select(x => new UserDetailsDTO
                    {
                        UserId = x.UserId,
                        FullNameAr = x.FullNameAr,
                        FullNameEn = x.FullNameEn,
                        ProfileImage = x.ProfileImage,
                    }).FirstOrDefault();
                }
            }

            return newEntity;
        }
        public ReletedMeetingListDTO GetRelatedMeetingsByReferenceNumber(int referenceNumber)
        {
            var meetings = _unitOfWork.GetRepository<Meeting>().GetAll()
                .Where(x => x.Repated && x.ReferenceNumber == referenceNumber).Select(x => new ReletedMeetingDTO
                {

                    Id = x.Id,
                    Date = x.Date,
                    MeetingFromTime = x.MeetingFromTime,
                    ReferenceNumber = x.ReferenceNumber,
                    MeetingToTime = x.MeetingToTime,
                    Subject = x.Subject,
                    Title = x.Title,
                    MeetingState = (x.Colsed) ? ReletedMeetingState.Colsed :
                     x.MeetingTopics.Count > 0 && (x.MeetingTopics.All(c => c.TopicState == TopicState.Completed)) ?
                    ReletedMeetingState.Finished :
                    (x.Date > DateTime.Now) ? ReletedMeetingState.New : ReletedMeetingState.InProgress,
                    CreatedBy = x.CreatedBy,
                    CreatedByUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Where(y => y.UserId == x.CreatedBy)
                   .Select(z => new UserDetailsDTO
                   {
                       UserId = z.UserId,
                       FullNameAr = z.FullNameAr,
                       FullNameEn = z.FullNameEn,
                       ProfileImage = z.ProfileImage,
                   }).FirstOrDefault()

                }).ToList();
            return new ReletedMeetingListDTO
            {
                ReletedMeetings = meetings,
                CreatedBy = meetings.FirstOrDefault().CreatedBy,
                CreatedByUser = meetings.FirstOrDefault().CreatedByUser,
            };
        }
        public bool DeleteMeetingAttendeesOrCoordinator(int userId, int meetingId, UserType userType)
        {
            switch (userType)
            {
                case UserType.Coordinator:
                    var meetingCoordinator = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll()
                        .Where(c => c.MeetingId == meetingId && c.CoordinatorId == userId).FirstOrDefault();
                    try
                    {
                        _unitOfWork.GetRepository<MeetingCoordinator>().Delete(meetingCoordinator);
                        _unitOfWork.SaveChanges();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        throw;
                    }
                case UserType.Attendee:
                    var meetingAttendee = _unitOfWork.GetRepository<MeetingAttendee>().GetAll()
                      .Where(c => c.MeetingId == meetingId && c.AttendeeId == userId).FirstOrDefault();
                    try
                    {
                        _unitOfWork.GetRepository<MeetingAttendee>().Delete(meetingAttendee);
                        _unitOfWork.SaveChanges();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        throw;
                    }
                default:
                    return false;
            }
        }

        public MeetingSummaryDTO GetMeetingSummary(int meetingId)
        {
            var coordinators = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll().Where(x => x.MeetingId == meetingId && ((x.Attended == null || x.Attended == false) ? false : true) == true).ToList();
            var topics = _unitOfWork.GetRepository<MeetingTopic>().GetAll().Where(x => x.MeetingId == meetingId).ToList();
            var meeting = _unitOfWork.GetRepository<Meeting>().GetAll()
                .Select(x => new MeetingSummaryDTO
                {
                    Id = x.Id,
                    Date = x.Date,
                    MeetingFromTime = x.MeetingFromTime,
                    MeetingToTime = x.MeetingToTime,
                    PhysicalLocation = x.PhysicalLocation,
                    Subject = x.Subject,
                    Title = x.Title,
                    MeetingAttendees = x.MeetingAttendees.Where(a => a.Attended == true).Select(z => new MeetingAttendeeDTO
                    {
                        Id = z.Id,
                        MeetingId = (int)z.MeetingId,
                        // TODO Check user data
                        Attendee = new Models.UserDetailsDTO
                        {
                            UserId = z.AttendeeId,
                            FullNameAr = z.Attendee.FullNameAr,
                            FullNameEn = z.Attendee.FullNameEn,
                            ProfileImage = z.Attendee.ProfileImage,
                            Email = z.Attendee.Email,
                            Mobile = z.Attendee.Mobile,
                            JobTitleName = _sessionServices.CultureIsArabic ?
                                               _unitOfWork.GetRepository<JobTitle>(false).Find(z.Attendee.JobTitleId).JobTitleNameAr
                                             : _unitOfWork.GetRepository<JobTitle>(false).Find(z.Attendee.JobTitleId).JobTitleNameEn,
                            ExternalUser = z.Attendee.ExternalUser
                        },
                        AttendeeId = z.AttendeeId,
                        Available = z.Available,
                        ConfirmeAttendance = z.ConfirmeAttendance,
                        State = z.State

                    }).ToList(),
                    MeetingCoordinators = coordinators.Select(coordinator => new MeetingCoordinatorDTO
                    {
                        Id = coordinator.Id,
                        MeetingId = (int)coordinator.MeetingId,
                        // TODO Check user data
                        Coordinator = new Models.UserDetailsDTO
                        {
                            UserId = coordinator.CoordinatorId,
                            FullNameAr = coordinator.Coordinator.FullNameAr,
                            FullNameEn = coordinator.Coordinator.FullNameEn,
                            ProfileImage = coordinator.Coordinator.ProfileImage,
                            Email = coordinator.Coordinator.Email,
                            Mobile = coordinator.Coordinator.Mobile,
                            JobTitleName = _sessionServices.CultureIsArabic ?
                                               _unitOfWork.GetRepository<JobTitle>(false).Find(coordinator.Coordinator.JobTitleId).JobTitleNameAr
                                             : _unitOfWork.GetRepository<JobTitle>(false).Find(coordinator.Coordinator.JobTitleId).JobTitleNameEn,
                            ExternalUser = coordinator.Coordinator.ExternalUser
                        },
                        CoordinatorId = coordinator.CoordinatorId,
                        Available = coordinator.Available,
                        ConfirmeAttendance = coordinator.ConfirmeAttendance,
                        State = coordinator.State
                    }).ToList(),

                    // topics in meeting
                    MeetingTopicDTOs = topics.Select(top => new MeetingTopicDTO
                    {
                        //CreatedBy = top.CreatedBy,
                        //CreatedOn = top.CreatedOn,
                        //DeletedBy = top.DeletedBy,
                        //DeletedOn = top.DeletedOn,
                        //Id = x.Id,
                        //MeetingId = top.MeetingId,
                        //TopicAcualEndDateTime = top.TopicAcualEndDateTime,
                        //TopicAcualStartDateTime = top.TopicAcualStartDateTime,
                        //TopicDate = top.TopicDate,
                        //TopicFromDateTime = top.TopicFromDateTime,
                        //TopicState = top.TopicState,
                        TopicTitle = top.TopicTitle,
                        //TopicToDateTime = top.TopicToDateTime,
                        //TopicType = top.TopicType,
                        //TopicTypeId = top.TopicTypeId,
                        //UpdatedBy = top.UpdatedBy,
                        //UpdatedOn = top.UpdatedOn,
                        //TopicComments = top.TopicComments.Select(y => new TopicCommentDTO
                        //{
                        //    Comment = new CommentDTO
                        //    {
                        //        CommentId = y.CommentId,
                        //        CreatedBy = y.Comment.CreatedBy,
                        //        Text = y.Comment.Text,
                        //        CreatedByUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Where(x => x.UserId == (int)y.Comment.CreatedBy)
                        //                        .Select(x => new UserDetailsDTO
                        //                        {
                        //                            UserId = x.UserId,
                        //                            FullNameAr = x.FullNameAr,
                        //                            FullNameEn = x.FullNameEn,
                        //                            ProfileImage = x.ProfileImage,
                        //                            UserName = x.Username
                        //                        }).FirstOrDefault(),
                        //    },
                        //    CommentId = y.CommentId,
                        //    CommentType = y.CommentType,
                        //    CreatedBy = y.CreatedBy,
                        //    Id = y.Id,
                        //    TopicId = y.TopicId,
                        //}).ToList(),
                        //TopicPauseDates = top.TopicPauseDates.Select(y => new TopicPauseDateDTO
                        //{
                        //    ContinueDateTime = y.ContinueDateTime,
                        //    CreatedBy = y.CreatedBy,
                        //    CreatedOn = y.CreatedOn,
                        //    Id = y.Id,
                        //    PauseDateTime = (DateTimeOffset)y.PauseDateTime,
                        //    TopicId = y.TopicId
                        //}).ToList(),
                        //TopicPoints = top.TopicPoints,
                        //TopicSurveies = top.TopicSurveies.Select(y => new SurveyDTO
                        //{
                        //    Attachments = y.Attachments.Select(z => new SurveyAttachmentDTO
                        //    {
                        //        // TODO Get Attachment
                        //        Attachment = new SavedAttachmentDTO
                        //        {
                        //            AttachmentName = z.Attachment.AttachmentName,
                        //            AttachmentTypeId = z.Attachment.AttachmentTypeId,
                        //            BinaryContent = z.Attachment.BinaryContent,
                        //            Height = z.Attachment.Height,
                        //            LFEntryId = z.Attachment.LFEntryId,
                        //            PagesCount = z.Attachment.PagesCount,
                        //            Width = z.Attachment.Width,
                        //            MimeType = z.Attachment.MimeType,
                        //            SavedAttachmentId = z.Attachment.SavedAttachmentId
                        //        },
                        //        // Attachment = _Mapper.Map(z.Attachment, typeof(SavedAttachment), typeof(SavedAttachmentDTO)) as SavedAttachmentDTO,
                        //        AttachmentId = z.AttachmentId,
                        //        SurveyAttachmentId = z.SurveyAttachmentId,
                        //        SurveyId = z.SurveyId,


                        //    }).ToList(),
                        //    IsShared = y.IsShared,
                        //    CreatedOn = y.CreatedOn,
                        //    CreatedBy = y.CreatedBy,
                        //    MeetingTopicId = (int)y.MeetingTopicId,
                        //    Multi = y.Multi,
                        //    Subject = y.Subject,
                        //    SurveyAnswers = y.SurveyAnswers.Select(x => new SurveyAnswerDTO
                        //    {
                        //        Answer = x.Answer,
                        //        SurveyAnswerId = x.SurveyAnswerId,
                        //        SurveyAnswerUsers = x.SurveyAnswerUsers.Select(y => new SurveyAnswerUserDTO
                        //        {
                        //            SurveyAnswerId = y.SurveyAnswerId,
                        //            SurveyAnswerUserId = y.SurveyAnswerUserId,
                        //            UserId = y.UserId,
                        //            User = new UserDetailsDTO
                        //            {
                        //                UserId = y.User.UserId,
                        //                FullNameAr = y.User.FullNameAr,
                        //                FullNameEn = y.User.FullNameEn,
                        //                UserName = y.User.FullNameEn,
                        //                ProfileImage = y.User.ProfileImage,
                        //            }
                        //        }).ToList()
                        //    }).ToList(),
                        //    SurveyId = y.SurveyId,
                        //    SurveyUsers = y.SurveyUsers.Select(x => new SurveyUserDTO { UserId = x.UserId }).ToList(),
                        //    CreatedByRoleId = y.CreatedByRoleId,
                        //    CreatedByUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Where(x => x.UserId == (int)y.CreatedBy)
                        //                        .Select(x => new UserDetailsDTO
                        //                        {
                        //                            UserId = x.UserId,
                        //                            FullNameAr = x.FullNameAr,
                        //                            FullNameEn = x.FullNameEn,
                        //                            ProfileImage = x.ProfileImage,
                        //                            UserName = x.Username
                        //                        }).FirstOrDefault(),

                        //}).ToList()
                    }).ToList(),
                    //MeetingCoordinators = x.MeetingCoordinators.Select(z => new MeetingCoordinatorDTO
                    //{
                    //    Id = z.Id,
                    //    MeetingId = (int)z.MeetingId,
                    //    Coordinator = new Models.UserDetailsDTO
                    //    {
                    //        UserId = z.CoordinatorId,
                    //        FullNameAr = z.Coordinator.FullNameAr,
                    //        FullNameEn = z.Coordinator.FullNameEn,
                    //        ProfileImage = z.Coordinator.ProfileImage,
                    //        // JobTitleName = _sessionServices.CultureIsArabic ? jobTitleName.JobTitleNameAr : jobTitleName.JobTitleNameEn,
                    //        ExternalUser = z.Coordinator.ExternalUser
                    //    },
                    //    CoordinatorId = z.CoordinatorId,
                    //    Available = z.Available,
                    //    ConfirmeAttendance = z.ConfirmeAttendance,
                    //    State = z.State
                    //}).ToList(),
                    MOMComment = x.MeetingComments.Where(c => c.CommentType == CommentType.Recommendation).Select(y => new MOMCommentDTO
                    {
                        Id = y.Id,
                        CommentId = y.CommentId,
                        Comment = new CommentDTO
                        {
                            CommentId = y.CommentId,
                            Text = y.Comment.Text
                        }
                    }).ToList(),
                    MOMSummaries = x.MinutesOfMeetings.Select(y => new MOMSummaryDTO
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Title = y.Title
                    }).ToList(),
                    MeetingHeaderAndFooters = x.MeetingHeaderAndFooters.Select(y => new Meeting_Meeting_HeaderAndFooterDTO
                    {
                        HeaderAndFooter = new MeetingHeaderAndFooterDTO
                        {
                            Html = y.HeaderAndFooter.Html,
                            HeaderAndFooterType = y.HeaderAndFooter.HeaderAndFooterType,
                            HeaderAndFooterTypeString = y.HeaderAndFooter.HeaderAndFooterTypeString,
                            Id = y.HeaderAndFooterId
                        }
                    }).ToList()

                }).FirstOrDefault(x => x.Id == meetingId);

            List<MeetingCommentDTO> MeetingComments = meetingCommentService.GetMeetingCommentsByMeetingId(meetingId);
            if (MeetingComments.Count() > 0)
            {
                //var T = MeetingComments[0].SurveyAnswers.Where(x => x.Answer == "موافق").SelectMany(x => x.SurveyAnswerUsers).Count();
                meeting.MeetingCommentsPercentage = MeetingComments.Select(x => new CommentsResultPercent
                {
                    CommentTitle = x.Comment.Text,
                    PercentAccept = (x.SurveyAnswers.Where(x => x.Answer == "موافق").SelectMany(x => x.SurveyAnswerUsers).Count() /
                              (float)(x.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers).Count() == 0 ? 1 : x.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers).Count())) * 100,

                    PercentReserved = (x.SurveyAnswers.Where(x => x.Answer == "متحفظ").SelectMany(x => x.SurveyAnswerUsers).Count() /
                              (float)(x.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers).Count() == 0 ? 1 : x.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers).Count())) * 100,

                    PercentReject = (x.SurveyAnswers.Where(x => x.Answer == "غير موافق").SelectMany(x => x.SurveyAnswerUsers).Count() /
                              (float)(x.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers).Count() == 0 ? 1 : x.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers).Count())) * 100
                }).ToList();
            }

            //var MeetingCommentsResult = from item in MeetingComments
            //                            select (new
            //                            {
            //                                Answer = item.Comment.Text,
            //                                SurvyAnswers = item.SurveyAnswers
            //                            });
            //if (MeetingCommentsResult.Count() > 0)
            //{

            //    foreach (var item in MeetingCommentsResult)
            //    {
            //        int TotalVotedUsers = 0;
            //        foreach (var SurvyAnswer in item.SurvyAnswers)
            //        {
            //            TotalVotedUsers += SurvyAnswer.SurveyAnswerUsers.Count();
            //        }
            //        var AgreeVoteUsers = item.SurvyAnswers.FirstOrDefault(x => x.Answer == "موافق");

            //        if (AgreeVoteUsers != null)
            //        {
            //            if (AgreeVoteUsers.SurveyAnswerUsers.Count() > 0)
            //            {
            //                float percent = ((float)AgreeVoteUsers.SurveyAnswerUsers.Count() / TotalVotedUsers) * 100;
            //                meeting.MeetingCommentsPercentage.Add(new CommentsResultPercent { CommentTitle = item.Answer, Percent = percent });
            //            }
            //            else meeting.MeetingCommentsPercentage.Add(new CommentsResultPercent { CommentTitle = item.Answer, Percent = 0 });
            //        }
            //        else meeting.MeetingCommentsPercentage.Add(new CommentsResultPercent { CommentTitle = item.Answer, Percent = 0 });
            //    }
            //}
            return meeting;
        }

        public void getMailMessage(CommiteeTaskDTO task, ref string mailMessage, ref string mailSubject, string mailTitle)
        {
            try
            {

                var TaskMailSubject = _commiteeLocalizationService.GetLocaliztionByCode("TaskMailSubject", _sessionServices.CultureIsArabic);
                var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", true);
                var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", false);
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);
                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("taskDetailsLink", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("taskDetailsLink", false);
                mailSubject = TaskMailSubject;
                string systemsettinglink = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;
                if (task.CommiteeTaskId == 0)
                {
                    var lastInsertedTaskId = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CommiteeTaskId).FirstOrDefault().CommiteeTaskId;
                    task.CommiteeTaskId = lastInsertedTaskId;
                }
                string commiteeTaskEncyption = Encription.EncriptStringAES(task.CommiteeTaskId.ToString());
                string unicodedCommiteeTaskId = HttpUtility.UrlEncode(commiteeTaskEncyption);
                string taskAfterReplace = null;
                var taskDetailsLink = "";
                //if (commiteeTaskEncyption.Contains('='))
                //{
                //    taskAfterReplace = commiteeTaskEncyption.Replace("=", "%3D");
                //}
                //if (commiteeTaskEncyption.Contains('/'))
                //{
                //    taskAfterReplace = taskAfterReplace.Replace("/", "%2F");
                //}
                //if (string.IsNullOrEmpty(taskAfterReplace))
                //{

                //    taskDetailsLink = $"{systemsettinglink}/tasks/{commiteeTaskEncyption}";
                //}
                taskDetailsLink = $"{systemsettinglink}/tasks/{unicodedCommiteeTaskId}";


                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                string Subject = task.Title;
                //string lblTransactionType = emailParams.lblTransactionType;
                //string lblTransactionDate = emailParams.lblTransactionDate;
                //string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                //string lblUrl = emailParams.lblTransURL;
                //string lblRequiredAction = emailParams.lblRequiredAction;
                //string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                //string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                //string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                //string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                //string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                //string lblUrlEn = emailParams.lblTransURLEn;
                //string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                //string lblMailHeader = emailParams.lblMailHeader;
                //string lblThanks = emailParams.ThanksLocalization;
                //string lblThanks2 = emailParams.ThanksLocalization2;
                //string incomingOrgNameAr = string.Empty;
                //string incomingOrgNameEn = string.Empty;
                //string IncomingOrganizationTR = string.Empty;
                //string ExternalDelegationOrgsTR = string.Empty;
                //string EmailValueColor = emailParams.EmailValueColor;
                //string EmailLblColor = emailParams.EmailLblColor;
                string Email_style = @"
                                            text-align: center;
                                            flex-direction: column;
                                            justify-content: center;
                                            align-items: center;";
                string Email_image_style = @" 
                                            text-align: center !important;
                                            margin: auto !important;
                                            justify-content: center;
                                            margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
                                            width: 100%;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: collapse;
                                            border-spacing: 2px;
                                            border-color: grey;";
                string tr_style = @" 
                                            //white-space: normal;
                                            //line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
                                    width: 3px;
                                    margin-bottom: -3px;
                                    font-weight: 600;
                                    margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
                                        width: 3px;
                                        margin-bottom: -3px;
                                        font-weight: 600;
                                        margin: -6px;
                                        direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";

                // Add url
                string HtmlString_new = $@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 100%;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                margin: 0;
                                                padding: 0;
                                                
                                            '>		
                                         <table style='width: 100%' border='1'>
                                            <tr style='{tr_style}'>
                                                <td colspan='5' style='{td_style}'>
                                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> {mailTitle} </h2>
                                                </td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {JobTitleEn} </td>   
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><span style='unicode-bidi: bidi-override;'>{task.Title}</span></td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {JobTitleAr} </td>
                                                

                                            </tr >  
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskCreatorEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{_sessionServices.EmployeeFullNameAr}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskCreatorAr} </td>   
                                            </tr > 
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskDetailsLinkEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{taskDetailsLink}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskDetailsLinkAr} </td>   
                                            </tr > 
                                            </table>
                                                </div>                                                     
                                                    </div>";
                //text-align:center

                //< td style = '{td_style_En + '; ' + w_20};background:{EmailLblColor}' >{ lblRequiredActionEn} </ td > 
                //< td style = '{td_style + '; ' + w_20 + rtl};background:{EmailLblColor}' >{ lblRequiredAction} </ td >



                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                
            }
        }


        // Get Mail Message For meeting 
        public void GetMailMessageForMeeting(MeetingUserDTO meetingUserDTO, ref string mailMessage, ref string mailSubject, string mailTitle)
        {
            try
            {


                string subject = _commiteeLocalizationService.GetLocaliztionByCode("AddMeetingRequestSubject", _sessionServices.CultureIsArabic);
                var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", true);
                var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", false);
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);
                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", false);
                var RejectRejectMeetingLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", true);
                var RejectRejectMeetingLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", false);
                mailSubject = subject;
                string systemsettinglink = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;
                if (meetingUserDTO.Id == 0)
                {
                    if (meetingUserDTO.UserType == UserType.Attendee)
                    {
                        var lastInsertedMeetingAttendeeId = _unitOfWork.GetRepository<MeetingAttendee>().GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        meetingUserDTO.Id = lastInsertedMeetingAttendeeId;
                    }
                    else
                    {
                        var lastInsertedMeetingCoordinatorId = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        meetingUserDTO.Id = lastInsertedMeetingCoordinatorId;
                    }
                }
                string meetingEncyption = Encription.EncriptStringAES(meetingUserDTO.MeetingId.ToString());
                string unicodedMeetingId = HttpUtility.UrlEncode(meetingEncyption);

                string userIdEncyption = Encription.EncriptStringAES(meetingUserDTO.UserId.ToString());
                string unicodedUserId = HttpUtility.UrlEncode(userIdEncyption);
                string taskAfterReplace = null;
                var MeetingDetailsLink = "";
                var RejectDetailsLink = "";
                //if (commiteeTaskEncyption.Contains('='))
                //{
                //    taskAfterReplace = commiteeTaskEncyption.Replace("=", "%3D");
                //}
                //if (commiteeTaskEncyption.Contains('/'))
                //{
                //    taskAfterReplace = taskAfterReplace.Replace("/", "%2F");
                //}
                //if (string.IsNullOrEmpty(taskAfterReplace))
                //{

                //    taskDetailsLink = $"{systemsettinglink}/tasks/{commiteeTaskEncyption}";
                //}
                // MeetingDetailsLink = $"{systemsettinglink}/api/Meetings/ConfirmMeetingAttendeesOrCoordinatorState?userId={meetingUserDTO.UserId}&meetingId={meetingUserDTO.MeetingId}&userType={meetingUserDTO.UserType}&state={meetingUserDTO.State}";

                //MeetingDetailsLink = $"{systemsettinglink}/meetings?userId={meetingUserDTO.UserId}&meetingId={meetingUserDTO.MeetingId}&userType={meetingUserDTO.UserType}&state={meetingUserDTO.State}";
                MeetingDetailsLink = $"{systemsettinglink}/assets/pages/confirmMail.html?userId={unicodedUserId}&meetingId={unicodedMeetingId}&userType={meetingUserDTO.UserType}&state={3}";

                RejectDetailsLink = $"{systemsettinglink}/assets/pages/confirmMail.html?userId={unicodedUserId}&meetingId={unicodedMeetingId}&userType={meetingUserDTO.UserType}&state={4}";



                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                //string Subject = ;
                //string lblTransactionType = emailParams.lblTransactionType;
                //string lblTransactionDate = emailParams.lblTransactionDate;
                //string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                //string lblUrl = emailParams.lblTransURL;
                //string lblRequiredAction = emailParams.lblRequiredAction;
                //string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                //string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                //string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                //string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                //string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                //string lblUrlEn = emailParams.lblTransURLEn;
                //string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                //string lblMailHeader = emailParams.lblMailHeader;
                //string lblThanks = emailParams.ThanksLocalization;
                //string lblThanks2 = emailParams.ThanksLocalization2;
                //string incomingOrgNameAr = string.Empty;
                //string incomingOrgNameEn = string.Empty;
                //string IncomingOrganizationTR = string.Empty;
                //string ExternalDelegationOrgsTR = string.Empty;
                //string EmailValueColor = emailParams.EmailValueColor;
                //string EmailLblColor = emailParams.EmailLblColor;
                string Email_style = @"
                                            text-align: center;
                                            flex-direction: column;
                                            justify-content: center;
                                            align-items: center;";
                string Email_image_style = @" 
                                            text-align: center !important;
                                            margin: auto !important;
                                            justify-content: center;
                                            margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
                                            width: 100%;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: collapse;
                                            border-spacing: 2px;
                                            border-color: grey;";
                string tr_style = @" 
                                            //white-space: normal;
                                            //line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
                                    width: 3px;
                                    margin-bottom: -3px;
                                    font-weight: 600;
                                    margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
                                        width: 3px;
                                        margin-bottom: -3px;
                                        font-weight: 600;
                                        margin: -6px;
                                        direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";

                // Add url
                string HtmlString_new = $@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 100%;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                margin: 0;
                                                padding: 0;
                                                
                                            '>		
                                         <table style='width: 100%' border='1'>
                                            <tr style='{tr_style}'>
                                                <td colspan='5' style='{td_style}'>
                                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> دعوه للإضافه على الإجتماع </h2>
                                                </td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {JobTitleEn} </td>   
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><span style='unicode-bidi: bidi-override;'>{mailTitle}</span></td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {JobTitleAr} </td>
                                                

                                            </tr >  
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskCreatorEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{_sessionServices.EmployeeFullNameAr}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskCreatorAr} </td>   
                                            </tr > 
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskDetailsLinkEn}   OR    {RejectRejectMeetingLinkEn}   </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><pre>     <a href ='{MeetingDetailsLink}'>Confirm<a/>       <a href ='{RejectDetailsLink}'>Reject<a/>     </pre></td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskDetailsLinkAr}    أو    {RejectRejectMeetingLinkAr} </td>   
                                            

                                            </tr > 
                                            </table>
                                                </div>                                                     
                                                    </div>";
                //text-align:center

                //< td style = '{td_style_En + '; ' + w_20};background:{EmailLblColor}' >{ lblRequiredActionEn} </ td > 
                //< td style = '{td_style + '; ' + w_20 + rtl};background:{EmailLblColor}' >{ lblRequiredAction} </ td >



                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
               
            }
        }
        public void GetMailMessageForMeetingGenReplacingCoordinate(MeetingUserDTO meetingUserDTO,
                                                ref string mailMessage,
                                                ref string mailSubject,
                                                string mailTitle,
                                                string _JobTitleAr,
                                                string _JobTitleEn)
        {
            try
            {


                string subject = _commiteeLocalizationService.GetLocaliztionByCode("exitMeeting", _sessionServices.CultureIsArabic);
                //var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", true);
                var JobTitleAr = _JobTitleAr;
                //var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", false);
                var JobTitleEn = _JobTitleEn;
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);
                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", false);
                var SubSubjectAr = _commiteeLocalizationService.GetLocaliztionByCode("Subject", true);
                var SubSubjectEn = _commiteeLocalizationService.GetLocaliztionByCode("Subject", false);
                var RejectRejectMeetingLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", true);
                var RejectRejectMeetingLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", false);
                mailSubject = subject;
                string systemsettinglink = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;
                if (meetingUserDTO.Id == 0)
                {
                    if (meetingUserDTO.UserType == UserType.Attendee)
                    {
                        var lastInsertedMeetingAttendeeId = _unitOfWork.GetRepository<MeetingAttendee>().GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        meetingUserDTO.Id = lastInsertedMeetingAttendeeId;
                    }
                    else
                    {
                        var lastInsertedMeetingCoordinatorId = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        meetingUserDTO.Id = lastInsertedMeetingCoordinatorId;
                    }
                }
                string meetingEncyption = Encription.EncriptStringAES(meetingUserDTO.MeetingId.ToString());
                string unicodedMeetingId = HttpUtility.UrlEncode(meetingEncyption);

                string userIdEncyption = Encription.EncriptStringAES(meetingUserDTO.UserId.ToString());
                string unicodedUserId = HttpUtility.UrlEncode(userIdEncyption);
                string taskAfterReplace = null;
                var MeetingDetailsLink = "";
                var RejectDetailsLink = "";
                //if (commiteeTaskEncyption.Contains('='))
                //{
                //    taskAfterReplace = commiteeTaskEncyption.Replace("=", "%3D");
                //}
                //if (commiteeTaskEncyption.Contains('/'))
                //{
                //    taskAfterReplace = taskAfterReplace.Replace("/", "%2F");
                //}
                //if (string.IsNullOrEmpty(taskAfterReplace))
                //{

                //    taskDetailsLink = $"{systemsettinglink}/tasks/{commiteeTaskEncyption}";
                //}
                // MeetingDetailsLink = $"{systemsettinglink}/api/Meetings/ConfirmMeetingAttendeesOrCoordinatorState?userId={meetingUserDTO.UserId}&meetingId={meetingUserDTO.MeetingId}&userType={meetingUserDTO.UserType}&state={meetingUserDTO.State}";

                //MeetingDetailsLink = $"{systemsettinglink}/meetings?userId={meetingUserDTO.UserId}&meetingId={meetingUserDTO.MeetingId}&userType={meetingUserDTO.UserType}&state={meetingUserDTO.State}";
                MeetingDetailsLink = $"{systemsettinglink}/assets/pages/confirmMail.html?userId={unicodedUserId}&meetingId={unicodedMeetingId}&userType={meetingUserDTO.UserType}&state={3}";

                RejectDetailsLink = $"{systemsettinglink}/assets/pages/confirmMail.html?userId={unicodedUserId}&meetingId={unicodedMeetingId}&userType={meetingUserDTO.UserType}&state={4}";



                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                //string Subject = ;
                //string lblTransactionType = emailParams.lblTransactionType;
                //string lblTransactionDate = emailParams.lblTransactionDate;
                //string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                //string lblUrl = emailParams.lblTransURL;
                //string lblRequiredAction = emailParams.lblRequiredAction;
                //string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                //string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                //string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                //string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                //string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                //string lblUrlEn = emailParams.lblTransURLEn;
                //string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                //string lblMailHeader = emailParams.lblMailHeader;
                //string lblThanks = emailParams.ThanksLocalization;
                //string lblThanks2 = emailParams.ThanksLocalization2;
                //string incomingOrgNameAr = string.Empty;
                //string incomingOrgNameEn = string.Empty;
                //string IncomingOrganizationTR = string.Empty;
                //string ExternalDelegationOrgsTR = string.Empty;
                //string EmailValueColor = emailParams.EmailValueColor;
                //string EmailLblColor = emailParams.EmailLblColor;
                string Email_style = @"
                                            text-align: center;
                                            flex-direction: column;
                                            justify-content: center;
                                            align-items: center;";
                string Email_image_style = @" 
                                            text-align: center !important;
                                            margin: auto !important;
                                            justify-content: center;
                                            margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
                                            width: 100%;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: collapse;
                                            border-spacing: 2px;
                                            border-color: grey;";
                string tr_style = @" 
                                            //white-space: normal;
                                            //line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
                                    width: 3px;
                                    margin-bottom: -3px;
                                    font-weight: 600;
                                    margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
                                        width: 3px;
                                        margin-bottom: -3px;
                                        font-weight: 600;
                                        margin: -6px;
                                        direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";


                // Add url
                string HtmlString_new = $@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 100%;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                margin: 0;
                                                padding: 0;
                                                
                                            '>		
                                         <table style='width: 100%' border='1'>
                                            <tr style='{tr_style}'>
                                                <td colspan='5' style='{td_style}'>
                                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> دعوه للإضافه على الإجتماع </h2>
                                                </td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {JobTitleEn} </td>   
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><span style='unicode-bidi: bidi-override;'>{mailTitle}</span></td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {JobTitleAr} </td>
                                                

                                            </tr >  
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskCreatorEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{_sessionServices.EmployeeFullNameAr}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskCreatorAr} </td>   
                                            </tr > 
                                                        <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskDetailsLinkEn}   OR    {RejectRejectMeetingLinkEn}   </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><pre>     <a href ='{MeetingDetailsLink}'>Confirm<a/>       <a href ='{RejectDetailsLink}'>Reject<a/>     </pre></td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskDetailsLinkAr}    أو    {RejectRejectMeetingLinkAr} </td>   
                                            

                                            </tr > 
                                            </table>
                                                </div>                                                     
                                                    </div>";
                //text-align:center

                //< td style = '{td_style_En + '; ' + w_20};background:{EmailLblColor}' >{ lblRequiredActionEn} </ td > 
                //< td style = '{td_style + '; ' + w_20 + rtl};background:{EmailLblColor}' >{ lblRequiredAction} </ td >



                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetMailMessageForVotingByEmail(MeetingUserDTO meetingUserDTO,
                                                ref string mailMessage,
                                                ref string mailSubject,
                                                string mailTitle,
                                                string _JobTitleAr,
                                                string _JobTitleEn,
                                                SurveyDTO SurveyDTO, string key, int _userid)
        {
            try
            {


                string subject = _commiteeLocalizationService.GetLocaliztionByCode(key, _sessionServices.CultureIsArabic);
                //var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", true);
                var JobTitleAr = _JobTitleAr;
                //var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", false);
                var JobTitleEn = _JobTitleEn;
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);
                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", false);
                var SubSubjectAr = _commiteeLocalizationService.GetLocaliztionByCode("Subject", true);
                var SubSubjectEn = _commiteeLocalizationService.GetLocaliztionByCode("Subject", false);
                var RejectRejectMeetingLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", true);
                var RejectRejectMeetingLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", false);
                mailSubject = subject;
                // design me card voting that contains voting creator,voting expiration ,voting title and voting answers that is more than element and submit vote user can vote only one answer using html and css only 

                var baseUrl = _systemSettingsService.GetByCode("MasarUrl").SystemSettingValue;


                //string values = JsonConvert.SerializeObject();
                // generate HTML for the survey options
                string  HtmlString_new = default;
                if (SurveyDTO != null)
                {

                     HtmlString_new = $@"<style>
                                        .voting-card {{
                                        background-color: #ffffff;
                                        border-radius: 10px;
                                        box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
                                        padding: 20px;
                                        width: 400px;
                                        }}

                                        .voting-card-header {{
                                        margin-bottom: 20px;
                                        }}

                                        .voting-card-title {{
                                        margin: 0;
                                        font-size: 24px;
                                        font-weight: bold;
                                        color: #333333;
                                        }}

                                        .voting-card-info {{
                                        font-size: 14px;
                                        margin: 5px 0;
                                        color: #666666;
                                        }}

                                        .voting-card-options {{
                                        display: flex;
                                        flex-direction: column;
                                        }}

                                        .voting-card-option {{
                                        display: flex;
                                        align-items: center;
                                        margin-bottom: 10px;
                                        cursor: pointer;
                                        padding: 10px;
                                        border-radius: 5px;
                                        transition: all 0.3s ease;
                                        border: 1px solid #dddddd;
                                        }}

                                        .voting-card-option input[type=""radio""] {{
                                        display: none;
                                        }}

                                        .voting-card-option label {{
                                        margin-left: 10px;
                                        font-size: 18px;
                                        color: #333333;
                                        }}

                                        .voting-card-option:hover {{
                                        background-color: #f1f1f1;
                                        }}

                                        .voting-card-option input[type=""radio""]:checked + label {{
                                        font-weight: bold;
                                        color: #333333;
                                        }}
                                    </style>
                                    <div class=""voting-card"" data-survey-id=""{_sessionServices.UserId}"">

                                        <div class=""voting-card-header"">
                                            <table style=""text-align: right;"">
                                                  <tr>
                                                    <td>
                                                      <h3 class=""voting-card-title"">{mailTitle}</h3>
                                                    </td>
                                                    <td>عنوان الاجتماع</td>
                                                  </tr>
                                                  <tr>
                                                    <td>
                                                      <h3 class=""""voting-card-title"""">{SurveyDTO.Subject} </h3>
                                                    </td>
                                                    <td>الموضوع (المراد التصويت عليه)</td>
                                                  </tr>
                                                  <tr>
                                                    <td>
                                                      {SurveyDTO.CreatedByUser.FullNameAr}
                                                    </td>
                                                    <td>أنشأ بواسطة </td>
                                                  </tr>
                                                  <tr>
                                                    <td>
                                                      {SurveyDTO.CreatedOn}
                                                    </td>
                                                    <td>أنشأ بتاريخ </td>
                                                  </tr>
                                                  <tr>
                                                    <td>
                                                      {SurveyDTO.SurveyEndDate}
                                                    </td>
                                                    <td> تاريخ أنتهاء التصويت </td>
                                                  </tr>
                                                </table>
                                        </div>
                                        <div class=""voting-card-options"">";

                    for (int i = 0; i < SurveyDTO.AnswerTitles.Count; i++)
                    {
                        HtmlString_new += $@"<div class=""voting-card-option"">
                                                        <a href=""{baseUrl}/assets/pages/VotingConfirmation.html?userId={_userid}&SurveyAnswerId={SurveyDTO.AnswerTitles[i].SurveyAnswerId}"" class=""btn btn-primary"">
                                                            <input type=""radio"" name=""voting-option"" id=""option-{i + 1}"" value=""{SurveyDTO.AnswerTitles[i].SurveyAnswerId}"" />
                                                            <label for=""option-{i + 1}"" >{SurveyDTO.AnswerTitles[i].answerTitle}</label><br/>
                                                        </a>
                                        </div>";
                    }
                    HtmlString_new += $@"
                                                </div>
                                            </div>
                                            ";
                }

                // add event listeners to the label elements
                //var document = new HtmlAgilityPack.HtmlDocument();
                //document.LoadHtml(HtmlString_new);

                //var labels = document.DocumentNode.Descendants("label");

                //foreach (var label in labels)
                //{
                //    var input = label.PreviousSibling;

                //    if (input.Name == "input" && input.GetAttributeValue("type", "") == "radio")
                //    {
                //        var answerId = input.GetAttributeValue("value", "");

                //        label.SetAttributeValue("data-answer-id", answerId);
                //        label.SetAttributeValue("style", "cursor: pointer;");
                //        label.Attributes.Add("onclick", $"SubmitAnswer('{_sessionServices.UserId}', '{answerId}');");
                //    }
                //}

                // render the HTML string with the event listeners added
                //HtmlString_new = document.DocumentNode.OuterHtml;

                // JavaScript function to submit the answer to the API


                //var jsFunction = @"
                //                    <script>
                //                        function SubmitAnswer(userId, answerId) {
                //                            var url = '@baseUrl/api/SurveyAnswerUsers/InsertCustome2';
                //                            var data = [{userId: userId, surveyAnswerId: answerId}];

                //                            fetch(url, {
                //                                method: 'POST',
                //                                headers: {
                //                                    'Content-Type': 'application/json',
                //                                },
                //                                body: JSON.stringify(data)
                //                            })
                //                            .then(response => response.json())
                //                            .then(data => console.log(true))
                //                            .catch(error => console.error(error));
                //                        }
                //                    </script>";

                // add the JavaScript function to the HTML string
                //HtmlString_new += jsFunction;
                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetMailMessageForMeetingForCancelMeeting(MeetingUserDTO meetingUserDTO,
                                                ref string mailMessage,
                                                ref string mailSubject,
                                                string mailTitle,
                                                string _JobTitleAr,
                                                string _JobTitleEn)
        {
            try
            {


                string subject = _commiteeLocalizationService.GetLocaliztionByCode("CloseMeetingRequestSubject", _sessionServices.CultureIsArabic);
                //var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", true);
                var JobTitleAr = _JobTitleAr;
                //var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", false);
                var JobTitleEn = _JobTitleEn;
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);
                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", false);
                var SubSubjectAr = _commiteeLocalizationService.GetLocaliztionByCode("Subject", true);
                var SubSubjectEn = _commiteeLocalizationService.GetLocaliztionByCode("Subject", false);
                var RejectRejectMeetingLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", true);
                var RejectRejectMeetingLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", false);
                mailSubject = subject;
                string systemsettinglink = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;
                if (meetingUserDTO.Id == 0)
                {
                    if (meetingUserDTO.UserType == UserType.Attendee)
                    {
                        var lastInsertedMeetingAttendeeId = _unitOfWork.GetRepository<MeetingAttendee>().GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        meetingUserDTO.Id = lastInsertedMeetingAttendeeId;
                    }
                    else
                    {
                        var lastInsertedMeetingCoordinatorId = _unitOfWork.GetRepository<MeetingCoordinator>().GetAll().OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        meetingUserDTO.Id = lastInsertedMeetingCoordinatorId;
                    }
                }
                string meetingEncyption = Encription.EncriptStringAES(meetingUserDTO.MeetingId.ToString());
                string unicodedMeetingId = HttpUtility.UrlEncode(meetingEncyption);

                string userIdEncyption = Encription.EncriptStringAES(meetingUserDTO.UserId.ToString());
                string unicodedUserId = HttpUtility.UrlEncode(userIdEncyption);
                string taskAfterReplace = null;
                var MeetingDetailsLink = "";
                var RejectDetailsLink = "";
                //if (commiteeTaskEncyption.Contains('='))
                //{
                //    taskAfterReplace = commiteeTaskEncyption.Replace("=", "%3D");
                //}
                //if (commiteeTaskEncyption.Contains('/'))
                //{
                //    taskAfterReplace = taskAfterReplace.Replace("/", "%2F");
                //}
                //if (string.IsNullOrEmpty(taskAfterReplace))
                //{

                //    taskDetailsLink = $"{systemsettinglink}/tasks/{commiteeTaskEncyption}";
                //}
                // MeetingDetailsLink = $"{systemsettinglink}/api/Meetings/ConfirmMeetingAttendeesOrCoordinatorState?userId={meetingUserDTO.UserId}&meetingId={meetingUserDTO.MeetingId}&userType={meetingUserDTO.UserType}&state={meetingUserDTO.State}";

                //MeetingDetailsLink = $"{systemsettinglink}/meetings?userId={meetingUserDTO.UserId}&meetingId={meetingUserDTO.MeetingId}&userType={meetingUserDTO.UserType}&state={meetingUserDTO.State}";
                MeetingDetailsLink = $"{systemsettinglink}/assets/pages/confirmMail.html?userId={unicodedUserId}&meetingId={unicodedMeetingId}&userType={meetingUserDTO.UserType}&state={3}";

                RejectDetailsLink = $"{systemsettinglink}/assets/pages/confirmMail.html?userId={unicodedUserId}&meetingId={unicodedMeetingId}&userType={meetingUserDTO.UserType}&state={4}";



                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                //string Subject = ;
                //string lblTransactionType = emailParams.lblTransactionType;
                //string lblTransactionDate = emailParams.lblTransactionDate;
                //string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                //string lblUrl = emailParams.lblTransURL;
                //string lblRequiredAction = emailParams.lblRequiredAction;
                //string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                //string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                //string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                //string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                //string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                //string lblUrlEn = emailParams.lblTransURLEn;
                //string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                //string lblMailHeader = emailParams.lblMailHeader;
                //string lblThanks = emailParams.ThanksLocalization;
                //string lblThanks2 = emailParams.ThanksLocalization2;
                //string incomingOrgNameAr = string.Empty;
                //string incomingOrgNameEn = string.Empty;
                //string IncomingOrganizationTR = string.Empty;
                //string ExternalDelegationOrgsTR = string.Empty;
                //string EmailValueColor = emailParams.EmailValueColor;
                //string EmailLblColor = emailParams.EmailLblColor;
                string Email_style = @"
                                            text-align: center;
                                            flex-direction: column;
                                            justify-content: center;
                                            align-items: center;";
                string Email_image_style = @" 
                                            text-align: center !important;
                                            margin: auto !important;
                                            justify-content: center;
                                            margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
                                            width: 100%;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: collapse;
                                            border-spacing: 2px;
                                            border-color: grey;";
                string tr_style = @" 
                                            //white-space: normal;
                                            //line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
                                    width: 3px;
                                    margin-bottom: -3px;
                                    font-weight: 600;
                                    margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
                                        width: 3px;
                                        margin-bottom: -3px;
                                        font-weight: 600;
                                        margin: -6px;
                                        direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";


                // Add url
                string HtmlString_new = $@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 100%;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                margin: 0;
                                                padding: 0;
                                                
                                            '>		
                                         <table style='width: 100%' border='1'>
                                            <tr style='{tr_style}'>
                                                <td colspan='5' style='{td_style}'>
                                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> تنبيه بإلغاء الإجتماع </h2>
                                                </td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {JobTitleEn} </td>   
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><span style='unicode-bidi: bidi-override;'>{mailTitle}</span></td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {JobTitleAr} </td>
                                                

                                            </tr >  
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskCreatorEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{_sessionServices.EmployeeFullNameAr}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskCreatorAr} </td>   
                                            </tr > 
                                            <tr style='{tr_style}'>
                                                <td colspan='5' style='{td_style + ';' + rtl}text-align: center; padding-top: 20px'><b><h3>تم إلغاء الإجتماع</h3></b></td>
                                            </tr > 
                                            </table>
                                                </div>                                                     
                                                    </div>";
                //text-align:center

                //< td style = '{td_style_En + '; ' + w_20};background:{EmailLblColor}' >{ lblRequiredActionEn} </ td > 
                //< td style = '{td_style + '; ' + w_20 + rtl};background:{EmailLblColor}' >{ lblRequiredAction} </ td >



                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetMailMessageForMeeting_2(UserDetailsDTO user, ref string mailMessage, ref string mailSubject, string mailTitle, DateOnly MeetingDate, TimeOnly MeetingFromTime, TimeOnly MeetingToTime)
        {
            try
            {


                string subject = _commiteeLocalizationService.GetLocaliztionByCode("meetingNotifyReminder", _sessionServices.CultureIsArabic);
                var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", true);
                var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("MeetingTitle", false);
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);

                var DateOfMeetingEn = _commiteeLocalizationService.GetLocaliztionByCode("DateOfMeeting", false);
                var DateOfMeetingAr = _commiteeLocalizationService.GetLocaliztionByCode("DateOfMeeting", true);

                var StartTimeOfMeetingEn = _commiteeLocalizationService.GetLocaliztionByCode("StartTimeOfMeeting", false);
                var StartTimeOfMeetingAr = _commiteeLocalizationService.GetLocaliztionByCode("StartTimeOfMeeting", true);

                var EndTimeOfMeetingEn = _commiteeLocalizationService.GetLocaliztionByCode("EndTimeOfMeeting", false);
                var EndTimeOfMeetingAr = _commiteeLocalizationService.GetLocaliztionByCode("EndTimeOfMeeting", true);

                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMeeting", false);
                var RejectRejectMeetingLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", true);
                var RejectRejectMeetingLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("RejectMeeting", false);
                mailSubject = subject;
                string systemsettinglink = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;



                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                string Email_style = @"
                                            text-align: center;
                                            flex-direction: column;
                                            justify-content: center;
                                            align-items: center;";
                string Email_image_style = @" 
                                            text-align: center !important;
                                            margin: auto !important;
                                            justify-content: center;
                                            margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
                                            width: 100%;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: collapse;
                                            border-spacing: 2px;
                                            border-color: grey;";
                string tr_style = @" 
                                            //white-space: normal;
                                            //line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
                                    width: 3px;
                                    margin-bottom: -3px;
                                    font-weight: 600;
                                    margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
                                        width: 3px;
                                        margin-bottom: -3px;
                                        font-weight: 600;
                                        margin: -6px;
                                        direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";

                // Add url
                string HtmlString_new = $@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 100%;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                margin: 0;
                                                padding: 0;
                                                
                                            '>		
                                         <table style='width: 100%' border='1'>
                                            <tr style='{tr_style}'>
                                                <td colspan='5' style='{td_style}'>
                                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'>  التذكير بإجتماع </h2>
                                                </td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {JobTitleEn} </td>   
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><span style='unicode-bidi: bidi-override;'>{mailTitle}</span></td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {JobTitleAr} </td>
                                                

                                            </tr >  
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {DateOfMeetingEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{MeetingDate}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {DateOfMeetingAr} </td>   
                                            </tr > 
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {StartTimeOfMeetingEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{MeetingFromTime}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {StartTimeOfMeetingAr} </td>   
                                            </tr > 
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {EndTimeOfMeetingEn} </td>
                                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{MeetingToTime}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {EndTimeOfMeetingAr} </td>   
                                            </tr > 
                                            </table>
                                                </div>                                                     
                                                    </div>";


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AlternateView CreateAlternateView(string message, object p, string v)
        {
            var msg = string.IsNullOrEmpty(message) ? " " : message;
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msg, null, "text/html");

            //string path_TopHeader = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//TopHeader.jpg"); //My TopHeader
            //                                                                                                             //------------------------------------TopHeader Image
            //LinkedResource imagelink_TopHeader = new LinkedResource(path_TopHeader, "image/png")
            //{
            //    ContentId = "TopHeader",

            //    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            //};
            //htmlView.LinkedResources.Add(imagelink_TopHeader);
            //--------------------------------------------------header Image
            //string pathheader = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//header.jpg"); //My Header


            //LinkedResource imagelink_header = new LinkedResource(pathheader, "image/png")
            //{
            //    ContentId = "header",

            //    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            //};
            //htmlView.LinkedResources.Add(imagelink_header);

            ////--------------------------------------------------Footer Image
            //string path_footer = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//footer.jpg"); //My footer


            //LinkedResource imagelink_Footer = new LinkedResource(path_footer, "image/png")
            //{
            //    ContentId = "footer",

            //    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            //};
            //htmlView.LinkedResources.Add(imagelink_Footer);
            return htmlView;
        }

        public List<UserDetailsDTO> UsersInMeeting(int MeetingId)
        {
            var usersRes = _unitOfWork.GetRepository<Meeting>().
                                    GetAll().
                                    Include(x => x.MeetingAttendees).
                                    Include(x => x.MeetingCoordinators).
                                    Where(x => x.Id == MeetingId).
                                    Select(x => new
                                    {
                                        Attendees = x.MeetingAttendees.Select(y => y.Attendee),
                                        Cordinators = x.MeetingCoordinators.Select(y => y.Coordinator)
                                    }).ToList();
            List<User> Users = usersRes[0].Attendees.Concat(usersRes[0].Cordinators).ToList();
            var res = mapper.Map<List<UserDetailsDTO>>(Users);
            return res;
        }

        public async Task GetMailMessageForMeetingAsync(MeetingDTO meeting)
        {
            List<UserDetailsDTO> Users = UsersInMeeting(meeting.Id);
            foreach (var item in Users)
            {
                string Message = "";
                string mailSubject = "";
                GetMailMessageForMeeting_2(item,
                                            ref Message,
                                            ref mailSubject,
                                            meeting.Title,
                                            new DateOnly(meeting.Date.Year, meeting.Date.Month, meeting.Date.Day),
                                            new TimeOnly(meeting.MeetingFromTime.Hour, meeting.MeetingFromTime.Minute, meeting.MeetingFromTime.Second),
                                            new TimeOnly(meeting.MeetingToTime.Hour, meeting.MeetingToTime.Minute, meeting.MeetingToTime.Second));
                AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                string body = $"نذكركم بموعد الاجتماع{meeting.Subject} سيبدأ بعد {meeting.ReminderBeforeMinutes}";
                Task.Run(() =>
                {
                    if (item.Email != null)
                    {
                        _MailServices.SendNotificationEmail(item.Email, mailSubject,
                            body, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                            );
                    }

                });

                smsServices.SendSMS(item.Mobile, new string[0] { },$" نذكركم بموعد الاجتماع{ meeting.Title}سيبدأ بعد { meeting.ReminderBeforeMinutes}", null);
            }
        }

        public void MeetingMembersNotifications(IEnumerable<MeetingDTO> entities)
        {
            var meeting = entities.FirstOrDefault();
            if (meeting.ReminderBeforeMinutes != 0)
            {
                var NewDateTime = meeting.MeetingFromTime.Subtract(TimeSpan.FromMinutes(meeting.ReminderBeforeMinutes));
                DateTime stime = new DateTime(meeting.Date.Year, meeting.Date.Month, meeting.Date.Day, NewDateTime.Hour, NewDateTime.Minute, NewDateTime.Second);
                BackgroundJob.Schedule(() => GetMailMessageForMeetingAsync(meeting), stime);
                //_unitOfWork.BackgroundJobClient.CreateBackgroundJob(JobQueues.SendEmail, () => GetMailMessageForMeetingAsync(meeting.Title, meeting.Subject));
            }
        }

        public List<UserDetailsDTO> CloseMeetingNew(int MeetingId)
        {
            return UsersNotVoting(MeetingId);
        }

        public List<UserDetailsDTO> UsersNotVoting(int MeetingId)
        {
            List<UserDetailsDTO> FinalOnlyUsersNotVoting = new List<UserDetailsDTO>();
            MeetingDTO MeetingDetails = GetDetails(MeetingId);
            IEnumerable<UserDetailsDTO> UserInMeeting = MeetingDetails.MeetingAttendees.Select(x => x.Attendee);
            IEnumerable<UserDetailsDTO> AllUserVotingCoordinators = MeetingDetails.MeetingCoordinators.Where(x => x.Attended == true).Where(x=> x.ConfirmeAttendance == true /*&&.IsCreator==true */).Select(x => x.Coordinator);
            IEnumerable<UserDetailsDTO> AllUserInMeeting = UserInMeeting.Concat(AllUserVotingCoordinators);
            //IEnumerable<UserDetailsDTO> OnlyUsersVoting = MeetingDetails.MeetingComments.SelectMany(x => x.SurveyAnswers.SelectMany(y => y.SurveyAnswerUsers)).Select(x => x.User);
            var _MOMExist = GetSurviesByMeetingId(MeetingId);
            var MOMUsers = GetSurviesByMeetingId(MeetingId).SelectMany(x => x.SurveyAnswers).SelectMany(x => x.SurveyAnswerUsers).Select(x => x.User).ToList();
            if (_MOMExist.Count() > 0)
            {
                var MOMUsersRest = AllUserInMeeting.Select(x => x.UserId).ToList().Except(MOMUsers.Select(x => x.UserId).ToList()).ToList();
                foreach (var item in MOMUsersRest)
                {
                    if (!FinalOnlyUsersNotVoting.Select(x => x.UserId).Contains(item)) FinalOnlyUsersNotVoting.Add(AllUserInMeeting.FirstOrDefault(x => x.UserId == item));
                }
            }
            //FinalOnlyUsersNotVoting = AllUserInMeeting.Where(item => !OnlyUsersVoting.Select(x => x.UserId).Contains(item.UserId) && !MOMUsers.Select(x => x.UserId).Contains(item.UserId)).ToList();
            IEnumerable<UserDetailsDTO> FinalOnlyUsersVoting;
            var MeetingComments = MeetingDetails.MeetingComments.ToList();
            foreach (var comment in MeetingComments)
            {
                if (comment.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers).Count() == 0)
                {
                    FinalOnlyUsersNotVoting = AllUserInMeeting.ToList();
                    break;
                }
                else
                {
                    var usersInComment = comment.SurveyAnswers.SelectMany(x => x.SurveyAnswerUsers.Select(x => x.User));
                    var UsersNotFoundInComment = AllUserInMeeting.Select(x => x.UserId).ToList().Except(usersInComment.Select(x => x.UserId));
                    foreach (var item in UsersNotFoundInComment)
                    {
                        if (!FinalOnlyUsersNotVoting.Select(x => x.UserId).Contains(item)) FinalOnlyUsersNotVoting.Add(AllUserInMeeting.FirstOrDefault(x => x.UserId == item));
                    }
                }
            }
            FinalOnlyUsersNotVoting = FinalOnlyUsersNotVoting.Distinct().ToList();
            return FinalOnlyUsersNotVoting;
        }

        public void VotingByEmail(int MeetingId, SurveyDTO surveyDTO, string filepath)
        {
            string Message = "";
            string mailSubject = "";
            MeetingDTO MeetingDetails = GetDetails(MeetingId);
            IEnumerable<UserDetailsDTO> UserInMeeting = MeetingDetails.MeetingAttendees.Select(x => x.Attendee);
            IEnumerable<UserDetailsDTO> AllUserVotingCoordinators = MeetingDetails.MeetingCoordinators.Where(x => x.Attended == true).Select(x => x.Coordinator);
            IEnumerable<UserDetailsDTO> AllUserInMeeting = UserInMeeting.Concat(AllUserVotingCoordinators).Append(MeetingDetails.CreatedByUser);
            foreach (var user in AllUserInMeeting)
            {
                MeetingUserDTO meetingUserDTO = new MeetingUserDTO()
                {
                    MeetingId = MeetingId,
                    UserId = user.UserId,
                    State = AttendeeState.Confirmed,

                };
                var codeAr = _commiteeLocalizationService.GetLocaliztionByCode("VotingByEmail", true);
                var codeEn = _commiteeLocalizationService.GetLocaliztionByCode("VotingByEmail", false);
                var meetingMoMEmailAr = _commiteeLocalizationService.GetLocaliztionByCode("MeetingMoM", true);
                var meetingMoMEmailEn = _commiteeLocalizationService.GetLocaliztionByCode("MeetingMoM", false);

                if (surveyDTO != null)
                {

                GetMailMessageForVotingByEmail(meetingUserDTO,
                                            ref Message,
                                            ref mailSubject,
                                            MeetingDetails.Title,
                                         codeAr,
                                         codeEn,
                                         surveyDTO, "VotingByEmail", user.UserId);
                }
                else
                {
                    GetMailMessageForVotingByEmail(meetingUserDTO,
                                           ref Message,
                                           ref mailSubject,
                                           MeetingDetails.Title,
                                        meetingMoMEmailAr,
                                        meetingMoMEmailEn,
                                        surveyDTO, "MeetingMoM", user.UserId);
                }

                AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                Task.Run(() =>
                {
                    //_MailServices.SendNotificationEmail(item.Attendee.Email, subject    , message.Replace("{Title}", meeting.Title), false              , null, ""                     , ""  , null, FilepathString); ;
                    _MailServices.SendNotificationEmail(user.Email, mailSubject, null, true, htmlViewForIncoming, null, null, null, filepath);
                });
            }
        }
    }
}   