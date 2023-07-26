using CommiteeAndMeetings.DAL.CommiteeDTO;
using Models;
using System;
using System.Collections.Generic;
namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingDTO
    {
        public MeetingDTO()
        {
            MeetingURls = new List<MeetingURlDTO>();
            MeetingProjects = new List<MeetingProjectDTO>();
            MeetingCoordinators = new List<MeetingCoordinatorDTO>();
            MeetingAttendees = new List<MeetingAttendeeDTO>();
            MeetingTopics = new List<MeetingTopicDTO>();
            //MinutesOfMeetings = new List<MinuteOfMeetingDTO>();
            MeetingComments = new List<MeetingCommentDTO>();
            Surveys = new List<SurveyDTO>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public bool Repated { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset MeetingFromTime { get; set; }
        public DateTimeOffset MeetingToTime { get; set; }
        public int ReferenceNumber { get; set; }
        public int ReminderBeforeMinutes { get; set; }
        public int? CommitteId { get; set; }
        public virtual CommiteeDTO.CommiteeDTO Commitee { get; set; }
        public List<MeetingURlDTO> MeetingURls { get; set; }
        public string PhysicalLocation { get; set; }
        public string ActualLocation { get; set; }
        public List<MeetingProjectDTO> MeetingProjects { get; set; }
        public bool IsSecret { get; set; }
        public bool PermitedToEnterMeeting { get; set; }
        public bool MemberConfirmation { get; set; }
        public bool ApproveManual { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public virtual List<MeetingCoordinatorDTO> MeetingCoordinators { get; set; }
        public List<MeetingAttendeeDTO> MeetingAttendees { get; set; }
        public List<MeetingTopicDTO> MeetingTopics { get; set; }
        //15-07-2021 (Don't want to load MinutesOfMeetings)
        //  public List<MinuteOfMeetingDTO> MinutesOfMeetings { get; set; }
        public virtual List<SurveyDTO> Surveys { get; set; }
        public virtual List<MeetingCommentDTO> MeetingComments { get; set; }
        public int? CreatedBy { get; set; }
        public UserDetailsDTO CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool Colsed { get; set; } = false;
        public bool IsCoordinator { get; set; }
        public bool IsCreator { get; set; }
        public bool IsFinished { get; set; }
        public int RepatedTimes { get; set; }
        public Period PeriodByDays { get; set; }
    }
    public enum Period
    {
        Daily = 1,
        Weekly = 7,
        Monthly = 30,
        Yearly = 365
    }
}
