using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("ValidityPeriods", Schema = "Committe")]
    public class ValidityPeriod : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        [Key]
        public int ValidityPeriodId { get; set; }
        public DateTime ValidityPeriodFrom { get; set; }
        public DateTime ValidityPeriodTo { get; set; }
        public PeriodState PeriodState { get; set; }
        public int CommiteeId { get; set; }
        public virtual Commitee Commitee { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
    public enum PeriodState
    {
        Active = 1,
        Archive = 2,
        Extend = 3
    }
}