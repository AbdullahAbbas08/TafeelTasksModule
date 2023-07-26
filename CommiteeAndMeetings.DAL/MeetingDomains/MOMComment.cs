using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MOMComments", Schema = "Meeting")]
    public class MOMComment : _BaseEntity, IAuditableInsertNoRole
    {
        public int Id { get; set; }
        public int MinuteOfMeetingId { get; set; }
        public virtual MinuteOfMeeting MinuteOfMeeting { get; set; }
        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public CommentType CommentType { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
