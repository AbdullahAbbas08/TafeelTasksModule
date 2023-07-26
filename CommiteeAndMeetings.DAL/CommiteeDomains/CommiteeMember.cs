using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("CommiteeMembers", Schema = "Committe")]
    public class CommiteeMember : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int CommiteeMemberId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CommiteeId { get; set; }
        public virtual Commitee Commitee { get; set; }

        // stateMember
        public MemberState MemberState { get; set; }

        public virtual List<CommiteeUsersRole> CommiteeRoles { get; set; }
        //public int CommiteeUsersRoleId { get; set; }
        //public virtual CommiteeUsersRole CommiteeUsersRole { get; set; }
        public bool Active { get; set; }
        //public bool IsLogin { get; set; }
        public bool IsReserveMember { get; set; } = false; // default for basicMember = false
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

    }
}
