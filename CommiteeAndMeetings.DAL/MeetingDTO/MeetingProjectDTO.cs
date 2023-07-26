using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingProjectDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public virtual ProjectDTO Project { get; set; }
        public int MeetingId { get; set; }
        public virtual MeetingDTO Meeting { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}