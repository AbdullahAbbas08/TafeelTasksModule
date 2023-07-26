using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.Domains
{
    public class TaskGroups:_BaseEntity, IAuditableInsert, IAuditableUpdate
    {
        [Key]
        public int TaskGroupId { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [ForeignKey("CommiteeTask")]
        public int TaskId { get; set; }

        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        public virtual Group Group { get; set; }
        public virtual CommiteeTask CommiteeTask { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
        
        
       
    }
}