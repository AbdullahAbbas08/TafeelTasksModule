using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MeetingCoordinators", Schema = "Meeting")]
    public class MeetingCoordinator : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        public int Id { get; set; }
        public int CoordinatorId { get; set; }
        public virtual User Coordinator { get; set; }
        public int? MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public bool Available { get; set; }
        public bool IsCreator { get; set; }
        public AttendeeState State { get; set; }
        public bool ConfirmeAttendance { get; set; }
        public bool? Attended { get; set; }

        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? SendingDate { get; set; }
        public int? AlternativeCoordinatorId { get; set; }
        public virtual User AlternativeCoordinator { get; set; }
        public string ReasonForReplacing { get; set; }
        public virtual User? UserDelegate { get; set; } 

    }
}