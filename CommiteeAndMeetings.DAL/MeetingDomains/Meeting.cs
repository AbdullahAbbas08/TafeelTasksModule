using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("Meetings", Schema = "Meeting")]
    public class Meeting : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        public Meeting()
        {
            MeetingURls = new List<MeetingURl>();
            MeetingProjects = new List<MeetingProject>();
            MeetingCoordinators = new List<MeetingCoordinator>();
            MeetingAttendees = new List<MeetingAttendee>();
            MeetingTopics = new List<MeetingTopic>();
            MinutesOfMeetings = new List<MinuteOfMeeting>();
            MeetingHeaderAndFooters = new List<Meeting_Meeting_HeaderAndFooter>();
            MeetingComments = new List<MeetingComment>();
            Surveys = new List<Survey>();
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
        public virtual Commitee Commitee { get; set; }
        public virtual List<MeetingURl> MeetingURls { get; set; }
        public string PhysicalLocation { get; set; }
        public string ActualLocation { get; set; }
        public virtual List<MeetingProject> MeetingProjects { get; set; }
        public bool IsSecret { get; set; }
        public bool PermitedToEnterMeeting { get; set; }
        public bool MemberConfirmation { get; set; }
        public bool ApproveManual { get; set; } = false;
        public virtual List<MeetingCoordinator> MeetingCoordinators { get; set; }
        public virtual List<MeetingAttendee> MeetingAttendees { get; set; }
        public virtual List<MeetingTopic> MeetingTopics { get; set; }
        public virtual List<MinuteOfMeeting> MinutesOfMeetings { get; set; }
        public virtual List<Meeting_Meeting_HeaderAndFooter> MeetingHeaderAndFooters { get; set; }
        public virtual List<Survey> Surveys { get; set; }
        public virtual List<MeetingComment> MeetingComments { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool Colsed { get; set; } = false;
        public bool Canceled { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
    }
}
