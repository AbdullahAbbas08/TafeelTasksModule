using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MeetingComment", Schema = "Meeting")]
    public class MeetingComment : _BaseEntity, IAuditableInsertNoRole
    {
        public MeetingComment()
        {
            SurveyAnswers = new List<SurveyAnswer>();
        }
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public CommentType CommentType { get; set; }

        public virtual List<SurveyAnswer> SurveyAnswers { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}