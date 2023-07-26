using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MeetingAttendee", Schema = "Meeting")]
    public class MeetingAttendee : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        public int Id { get; set; }
        public int AttendeeId { get; set; }
        public virtual User Attendee { get; set; }
        public int? MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
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
        public virtual User AlternativeAttendee { get; set; }
        public string ReasonForReplacing { get; set; }
        public virtual User? UserDelegate { get; set; }

    }

}