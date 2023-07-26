using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("CommiteeRoles", Schema = "Committe")]
    public class CommiteeRole : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        public CommiteeRole()
        {
            RolePermissions = new List<CommiteeRolePermission>();
        }
        public int CommiteeRoleId { get; set; }
        public string CommiteeRolesNameAr { get; set; }
        public string CommiteeRolesNameEn { get; set; }
        public bool IsMangerRole { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public virtual List<CommiteeRolePermission> RolePermissions { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}
