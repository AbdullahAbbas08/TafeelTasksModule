using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommiteeRolePermissions", Schema = "Committe")]
    public class CommiteeRolePermission : _BaseEntity, IAuditableInsert, IAuditableUpdate
    {
        [Key]
        public int CommiteeRolePermissionId { get; set; }
        public int RoleId { get; set; }
        public virtual CommiteeRole Role { get; set; }
        public int PermissionId { get; set; }
        public virtual CommitePermission Permission { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}