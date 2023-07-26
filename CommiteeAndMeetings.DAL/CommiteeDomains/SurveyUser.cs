using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("SurveyUsers", Schema = "Committe")]
    public class SurveyUser : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        public int SurveyUserId { get; set; }
        public int SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
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