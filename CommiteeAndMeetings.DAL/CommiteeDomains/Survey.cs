using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("Surveys", Schema = "Committe")]
    public class Survey : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        public Survey()
        {
            SurveyAnswers = new List<SurveyAnswer>();
            Attachments = new List<SurveyAttachment>();
            Comments = new List<SurveyComment>();
        }
        [Key]
        public int SurveyId { get; set; }
        public string Subject { get; set; }
        public bool Multi { get; set; }
        public virtual List<SurveyAnswer> SurveyAnswers { get; set; }
        public virtual List<SurveyAttachment> Attachments { get; set; }
        public virtual List<SurveyComment> Comments { get; set; }
        public int? CommiteeId { get; set; }
        public virtual Commitee Commitee { get; set; }
        public int? MeetingTopicId { get; set; }
        public virtual MeetingTopic MeetingTopic { get; set; }
        public int? MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public bool IsShared { get; set; } = true;
        public string SurveyEndDate { get; set; }
        public virtual List<SurveyUser> SurveyUsers { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}