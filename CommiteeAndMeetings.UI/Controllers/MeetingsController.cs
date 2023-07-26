using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using CommiteeAndMeetings.Service.Sevices;
using LinqHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CommiteeAndMeetings.UI.Helpers;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : _BaseController<Meeting, MeetingDTO>
    {
        private readonly IMeetingService _meetingService;
        private readonly IMeetingCommentService meetingCommentService;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        private readonly IUnitOfWork uow;

        public MeetingsController(IMeetingService businessService,
                                  IMeetingCommentService _meetingCommentService,
                                  IHelperServices.ISessionServices sessionSevices,
                                  IUnitOfWork _uow) : base(businessService, sessionSevices)
        {
            this._meetingService = businessService;
            _SessionServices = sessionSevices;
            uow = _uow;
            meetingCommentService = _meetingCommentService;
        }

        //[HttpGet("GetByIdCustome")]
        //public virtual MeetingDTO GetByIdCustome(string id)
        //{
        //    return _BusinessService.GetDetails(id, true);
        //}


        [HttpPost("InsertCustom")]
        public virtual IEnumerable<MeetingDTO> PostCustom([FromBody] IEnumerable<MeetingDTO> entities)
        {
            var res = _BusinessService.Insert(entities);
            _meetingService.MeetingMembersNotifications(res);
            return res;
        }

        [HttpPost("InsertMeetingAttendeesOrCoordinators")]
        public MeetingUserAvailabilityDTO InsertMeetingAttendeesOrCoordinators(MeetingUserDTO attendeeDTO)
        {
            return _meetingService.InsertMeetingAttendeesOrCoordinators(attendeeDTO);
        }

        [HttpGet("UsersForMeeting")]
        public ActionResult<List<MeetingUserAttendationDTO>> UserShouldAttendMeeting(int meetingId)
        {
            var meeting = _meetingService.GetDetails(meetingId);
            if (meeting.Id > 0)
            {
                var users = _meetingService.UsersForSpectificMeetings(meetingId);
                return Ok(users);
            }
            else
            {
                return BadRequest(new List<MeetingUserAttendationDTO>());
            }

        }

        [HttpPost("InsertMeetingMultiAttendeesOrCoordinators")]
        public List<MeetingUserAvailabilityDTO> InsertMeetingMultiAttendeesOrCoordinators(ListOfMeetingUserDTO attendeeDTO)
        {
            return _meetingService.InsertMeetingMultiAttendeesOrCoordinators(attendeeDTO);
        }

        [HttpPut("ToogleCoordinatorOrAttendeeConfirmMeetingAttendance")]
        public MeetingUserAvailabilityDTO ToogleCoordinatorOrAttendeeConfirmMeetingAttendance(int userId, int meetingId, UserType type)
        {
            return _meetingService.ToogleCoordinatorConfirmMeetingAttendance(userId, meetingId, type);
        }

        [HttpPost("ChangeMeetingAttendeesOrCoordinatorState")]
        public MeetingUserAvailabilityDTO ChangeMeetingAttendeesOrCoordinatorState(int userId, int meetingId, UserType userType, AttendeeState state)
        {
            return _meetingService.ChangeMeetingAttendeesOrCoordinatorState(userId, meetingId, userType, state);
        }
        [AllowAnonymous]
        [HttpPost("ConfirmMeetingAttendeesOrCoordinatorState")]
        public MeetingUserAvailabilityDTO ConfirmMeetingAttendeesOrCoordinatorState(string userId, string meetingId, UserType userType, AttendeeState state)
        {
            return _meetingService.ConfirmMeetingAttendeesOrCoordinatorState(userId, meetingId, userType, state);
        }
        [HttpPost("DeleteMeetingAttendeesOrCoordinator")]
        public bool DeleteMeetingAttendeesOrCoordinator(int userId, int meetingId, UserType userType)
        {
            return _meetingService.DeleteMeetingAttendeesOrCoordinator(userId, meetingId, userType);
        }
        [HttpGet("GetMeetingUserAvailability")]
        public MeetingAvailabilityDTO GetMeetingUserAvailability(int userId, int meetingId, UserType userType)
        {
            var meeting = _meetingService.GetDetails(meetingId);
            return _meetingService.CheckAvailability(userId, meetingId, meeting.MeetingFromTime, meeting.MeetingToTime, userType);
        }
        [HttpGet("ChangeApproveManual")]
        public bool ChangeApproveManual(int mettingId, bool ApproveManual)
        {
            return _meetingService.ChangeApproveManual(mettingId, ApproveManual);
        }
        [HttpGet("DisplayMeetings")]
        public List<MeetingDetailsDTO> DisplayMeetings(DateTimeOffset FromDate, DateTimeOffset? ToDate, DisplayMeetingCallType displayMeetingCallType = DisplayMeetingCallType.Other)
        {
            return _meetingService.DisplayMeetings(FromDate, ToDate, displayMeetingCallType);
        }
        [HttpPost("DisplayClosedMeeting")]
        public DataSourceResult<MeetingDetailsDTO> DisplayClosedMeeting([FromBody] DataSourceRequest dataSourceRequest)
        {
            return _meetingService.DisplayClosedMeeting(dataSourceRequest);
        }
        [HttpPost("DisplayFinishedMeeting")]
        public DataSourceResult<MeetingDetailsDTO> DisplayFinishedMeeting([FromBody] DataSourceRequest dataSourceRequest)
        {
            return _meetingService.DisplayFinishedMeeting(dataSourceRequest);
        }
        [HttpPost("GetAllActivities")]
        public List<MeetingActivityLookup> GetAllActivities([FromBody] DataSourceRequest dataSourceRequest)
        {
            return _meetingService.GetAllActivities(dataSourceRequest);
        }
        [HttpPost("PostMeetingAttendees")]
        public bool PostMeetingAttendees(int meetingId, AttendeesList AttendedUsers)
        {
            return _meetingService.TakeAttendees(meetingId, AttendedUsers);

        }
        [HttpPost("ColseMeeting")]
        public bool ColseMeeting(int meetingId)
        {
            return _meetingService.ColseMeeting(meetingId);
        }
        [HttpPost("GetSurviesByMeetingId")]
        public List<SurveyDTO> GetSurviesByMeetingId(int meetingId)
        {
            return _meetingService.GetSurviesByMeetingId(meetingId);
        }
        [HttpGet("GetMeetingSummary")]
        public MeetingSummaryDTO GetMeetingSummary(int meetingId)
        {
            MeetingSummaryDTO result = _meetingService.GetMeetingSummary(meetingId);
            return result;
        }
        [HttpPost("GetRelatedMeetingsByReferenceNumber")]
        public ReletedMeetingListDTO GetRelatedMeetingsByReferenceNumber(int referenceNumber)
        {

            return _meetingService.GetRelatedMeetingsByReferenceNumber(referenceNumber);
        }

        [HttpPost("CanceledMeeting")]
        public bool CanceledMeeting(int meetingId)
        {
            return _meetingService.CanceledMeeting(meetingId);
        }

        [HttpPost("UsersInMeeting")]
        public List<UserDetailsDTO> UsersInMeeting(int meetingId)
        {
            return _meetingService.UsersInMeeting(meetingId);
        }

        [HttpGet("CloseMeetingNew")]
        public List<UserDetailsDTO> CloseMeetingNew(int meetingId)
        {
           return  _meetingService.CloseMeetingNew(meetingId);
        }


        // api for get replacing coordinate & attendee 
        [HttpPost("GettingReplacingCoordinateOrAttendee")]
        public bool GettingReplacingCoordinateOrAttendee(string meetingId, string reason, string userIdReplacing)
        {
            UserIdAndRoleIdAfterDecryptDTO meetingIdAfterDecrypt = _SessionServices.UserIdAndRoleIdAfterDecrypt(meetingId, false);
            if (string.IsNullOrEmpty(userIdReplacing))
            {
                return _meetingService.GettingReplacingCoordinateOrAttendee(meetingIdAfterDecrypt.Id, reason, null);

            }
            UserIdAndRoleIdAfterDecryptDTO userIdReplacingAfterDecrypt = _SessionServices.UserIdAndRoleIdAfterDecrypt(userIdReplacing, false);
            return _meetingService.GettingReplacingCoordinateOrAttendee(meetingIdAfterDecrypt.Id, reason, userIdReplacingAfterDecrypt.Id);
        }
    }
}
