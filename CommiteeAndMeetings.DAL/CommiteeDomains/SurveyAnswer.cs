using CommiteeAndMeetings.DAL.CommiteeAndMeetingsEnums;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("SurveyAnswers", Schema = "Committe")]
    public class SurveyAnswer : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        [Key]
        public int SurveyAnswerId { get; set; }
        public string Answer { get; set; }
        //public EnumSurveyAnswer VotingFlag { get; set; } 
        public int SurveyId { get; set; }
        //[ForeignKey("MeetingComment")]
       // public int MeetingCommentId { get; set; }
        public virtual MeetingComment MeetingComment { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual List<SurveyAnswerUser> SurveyAnswerUsers { get; set; }
        public int? CreatedBy { get; set; }
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