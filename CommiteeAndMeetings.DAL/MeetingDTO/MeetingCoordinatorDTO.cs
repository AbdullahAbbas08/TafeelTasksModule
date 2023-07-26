using CommiteeAndMeetings.DAL.Enums;
using Models;
using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingCoordinatorDTO
    {
        public int Id { get; set; }
        public int CoordinatorId { get; set; }
        public UserDetailsDTO Coordinator { get; set; }
        public int MeetingId { get; set; }
        public MeetingDTO Meeting { get; set; }
        public bool Available { get; set; }
        public AttendeeState State { get; set; }
        public bool ConfirmeAttendance { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool IsCreator { get; set; }
        public DateTimeOffset? SendingDate { get; set; }
        public bool? Attended { get; set; }

        public int? AlternativeCoordinatorId { get; set; }
        public  UserDetailsDTO AlternativeCoordinator { get; set; }
        public string ReasonForReplacing { get; set; }
        public virtual UserDetailsDTO UserDelegate { get; set; }

    }
}