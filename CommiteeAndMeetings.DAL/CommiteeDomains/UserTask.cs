using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("UserTasks", Schema = "Committe")]
    public class UserTask : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        [Key]
        public int UserTaskId { get; set; }
        public int CommiteeTaskId { get; set; }
        //public int CommiteeTaskMultiMissionId { get; set; }
        public virtual CommiteeTask CommiteeTask { get; set; }
        //public virtual CommiteeTaskMultiMission CommiteeTaskMultiMission { get; set; }
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
