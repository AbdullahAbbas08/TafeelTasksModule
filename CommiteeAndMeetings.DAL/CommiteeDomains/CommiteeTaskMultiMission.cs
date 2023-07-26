using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("CommiteeTaskMultiMission", Schema = "Committe")]
    public class CommiteeTaskMultiMission : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        public CommiteeTaskMultiMission()
        {
            CommiteeTaskMultiMissionUsers = new HashSet<CommiteeTaskMultiMissionUser>();
        }
        
        [Key]
        public int CommiteeTaskMultiMissionId { get; set; }
        public string Name { get; set; }
        public bool state { get; set; }
        //[ForeignKey("CommiteeTask")]
        public int CommiteeTaskId { get; set; }
        public virtual CommiteeTask CommiteeTask { get; set; }

        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? EndDateMultiMission { get; set; }
        // public virtual ICollection<UserTask> UserMultiMission { get; set; }
        public virtual ICollection<CommiteeTaskMultiMissionUser> CommiteeTaskMultiMissionUsers { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}
