using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("CommiteeTaskMultiMissionUsers", Schema = "Committe")]
    public class CommiteeTaskMultiMissionUser : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
           [Key]
        
            public int CommiteeTaskMultiMissionUserId { get; set; }
            [ForeignKey("CommiteeTaskMultiMission")]
            public int CommiteeTaskMultiMissionId { get; set; }
            public virtual CommiteeTaskMultiMission CommiteeTaskMultiMission { get; set; }
            [ForeignKey("User")]
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

