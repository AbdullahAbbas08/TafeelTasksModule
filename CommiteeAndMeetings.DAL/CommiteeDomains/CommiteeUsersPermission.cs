using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommiteeUsersPermission", Schema = "Committe")]
    public class CommiteeUsersPermission : _BaseEntity, IAuditableInsert, IAuditableUpdate
    {
        [Key]
        public int Commitee_CommiteePermissionId { get; set; }

        public bool IsDelegated { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public virtual CommitePermission Permission { get; set; }
        public int CommiteeId { get; set; }
        public virtual Commitee Commitee { get; set; }
        public bool Enabled { get; set; }
        public string Notes { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}
