using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommiteeUsersRoles", Schema = "Committe")]
    public class CommiteeUsersRole : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate
    {
        [Key]
        public int CommiteeUsersRoleId { get; set; }
        public int CommiteeId { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual CommiteeRole Role { get; set; }

        public virtual Commitee Commitee { get; set; }
        public bool Delegated { get; set; } = false;
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int? CommiteeMemberId { get; set; }
        //public virtual CommiteeMember CommiteeMember { get; set; }
        public bool Enabled { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset? EnableUntil { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        //[ForeignKey("CreatedByRole")]
        //public int? CreatedByRoleId { get; set; }
        //public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}
