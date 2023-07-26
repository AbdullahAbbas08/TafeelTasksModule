using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MeetingProjects", Schema = "Meeting")]
    public class MeetingProject : _BaseEntity, IAuditableInsertNoRole, IAuditableDelete
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public int MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}