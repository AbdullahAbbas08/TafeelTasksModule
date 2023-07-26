using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using Models;
using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingAttendeeDTO
    {
        public int Id { get; set; }
        public int AttendeeId { get; set; }
        public UserDetailsDTO Attendee { get; set; }
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
        public bool? Attended { get; set; }
        public DateTimeOffset? SendingDate { get; set; }
        public int? AlternativeAttendeeId { get; set; }
        public UserDetailsDTO AlternativeAttendee { get; set; } = new UserDetailsDTO();
        public string ReasonForReplacing { get; set; }
        public virtual UserDetailsDTO UserDelegate { get; set; }
    }
}