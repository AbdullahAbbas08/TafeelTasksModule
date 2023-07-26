using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string ProjectNameAr { get; set; }
        public string ProjectNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}