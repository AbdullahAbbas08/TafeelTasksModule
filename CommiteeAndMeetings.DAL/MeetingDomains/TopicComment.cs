using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("TopicComments", Schema = "Meeting")]
    public class TopicComment : _BaseEntity, IAuditableInsertNoRole
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public virtual MeetingTopic Topic { get; set; }
        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public CommentType CommentType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
    public enum CommentType
    {
        NormalComment = 1,
        Recommendation = 2
    }
}