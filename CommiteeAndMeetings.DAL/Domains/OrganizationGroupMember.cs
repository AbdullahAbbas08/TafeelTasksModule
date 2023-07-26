using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(OrganizationGroupId), Name = "IX_OrganizationGroupMembers_OrganizationGroupId")]
    [Index(nameof(OrganizationId), Name = "IX_OrganizationGroupMembers_OrganizationId")]
    public partial class OrganizationGroupMember
    {
        [Key]
        public int OrganizationGroupMemberId { get; set; }
        public int OrganizationGroupId { get; set; }
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("OrganizationGroupMembers")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(OrganizationGroupId))]
        [InverseProperty("OrganizationGroupMembers")]
        public virtual OrganizationGroup OrganizationGroup { get; set; }
    }
}
