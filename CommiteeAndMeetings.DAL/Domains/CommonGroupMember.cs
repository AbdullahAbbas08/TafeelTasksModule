using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CommonGroupId), Name = "IX_CommonGroupMembers_CommonGroupId")]
    [Index(nameof(CreatedBy), Name = "IX_CommonGroupMembers_CreatedBy")]
    [Index(nameof(OrganizationId), Name = "IX_CommonGroupMembers_OrganizationId")]
    [Index(nameof(UserId), Name = "IX_CommonGroupMembers_UserId")]
    public partial class CommonGroupMember
    {
        [Key]
        public int CommonGroupMembersId { get; set; }
        public int CommonGroupId { get; set; }
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public bool IsUser { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [ForeignKey(nameof(CommonGroupId))]
        [InverseProperty("CommonGroupMembers")]
        public virtual CommonGroup CommonGroup { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("CommonGroupMemberCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("CommonGroupMembers")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("CommonGroupMemberUsers")]
        public virtual User User { get; set; }
    }
}
