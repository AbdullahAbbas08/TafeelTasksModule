using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Services.ISevices;
using Hangfire.Storage.Monitoring;
using LinqHelper;
using Models;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IMeetingService : IBusinessService<Meeting, MeetingDTO>
    {
        MeetingUserAvailabilityDTO InsertMeetingAttendeesOrCoordinators(MeetingUserDTO attendeeDTO);
        List<MeetingUserAttendationDTO> UsersForSpectificMeetings(int meetingId);
        List<MeetingUserAvailabilityDTO> InsertMeetingMultiAttendeesOrCoordinators(ListOfMeetingUserDTO attendeeDTOs);
        MeetingAvailabilityDTO CheckAvailability(int attendeeId, int meetingId, DateTimeOffset From, DateTimeOffset To, UserType userType, bool withConfict = false);
        MeetingUserAvailabilityDTO ChangeMeetingAttendeesOrCoordinatorState(int userId, int meetingId, UserType userType, AttendeeState state);
        //MeetingUserAvailabilityDTO ConfirmMeetingAttendeesOrCoordinatorState(int userId, int meetingId, UserType userType, AttendeeState state);
        MeetingUserAvailabilityDTO ConfirmMeetingAttendeesOrCoordinatorState(string userIdEncrpted, string meetingIdEncrpted, UserType userType, AttendeeState state);
        bool ChangeApproveManual(int mettingId, bool ApproveManual);
        List<MeetingDetailsDTO> DisplayMeetings(DateTimeOffset fromDate, DateTimeOffset? toDate, DisplayMeetingCallType displayMeetingCallType);
        bool TakeAttendees(int meetingId, AttendeesList AttendedUsers);
        DataSourceResult<MeetingDetailsDTO> DisplayClosedMeeting(DataSourceRequest dataSourceRequest);
        
        DataSourceResult<MeetingDetailsDTO> DisplayFinishedMeeting(DataSourceRequest dataSourceRequest);
        List<MeetingActivityLookup> GetAllActivities(DataSourceRequest dataSourceRequest);
        bool ColseMeeting(int meetingId);
        bool CanceledMeeting(int meetingId);
      //  bool GettingReplacingCoordinateOrAttendee(int meetingId, string reason, int userId);
        bool GettingReplacingCoordinateOrAttendee(int meetingId, string reason, int? userId);
        //AllSurveyDTOINMeeting GetSurviesByMeetingId(int meetingId);
        List<SurveyDTO> GetSurviesByMeetingId(int meetingId);
        MeetingUserAvailabilityDTO ToogleCoordinatorConfirmMeetingAttendance(int userId, int meetingId, UserType type);
        MeetingSummaryDTO GetMeetingSummary(int meetingId);
        ReletedMeetingListDTO GetRelatedMeetingsByReferenceNumber(int referenceNumber);
        bool DeleteMeetingAttendeesOrCoordinator(int userId, int meetingId, UserType userType);
        public void MeetingMembersNotifications(IEnumerable<MeetingDTO> entities);
        public List<UserDetailsDTO> UsersInMeeting(int MeetingId);
        public List<UserDetailsDTO> CloseMeetingNew(int MeetingId);
        List<UserDetailsDTO> UsersNotVoting(int MeetingId);
        public void VotingByEmail(int MeetingId, SurveyDTO surveyDTO,string  FilepathString);
        public void GetMailMessageForVotingByEmail(MeetingUserDTO meetingUserDTO,
                                              ref string mailMessage,
                                              ref string mailSubject,
                                              string mailTitle,
                                              string _JobTitleAr,
                                              string _JobTitleEn,
                                              SurveyDTO answerTitles,string key,int userid);
    }
}