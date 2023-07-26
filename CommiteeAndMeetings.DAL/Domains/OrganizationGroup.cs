using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("OrganizationGroup")]
    [Index(nameof(UserId), Name = "IX_OrganizationGroup_UserId")]
    public partial class OrganizationGroup
    {
        public OrganizationGroup()
        {
            OrganizationGroupMembers = new HashSet<OrganizationGroupMember>();
        }

        [Key]
        public int OrganizationGroupId { get; set; }
        public string OrganizationGroupNameAr { get; set; }
        public string OrganizationGroupNameEn { get; set; }
        public int OrganizationGroupPriority { get; set; }
        public int UserId { get; set; }
        public string OrganizationGroupNameFn { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("OrganizationGroups")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(OrganizationGroupMember.OrganizationGroup))]
        public virtual ICollection<OrganizationGroupMember> OrganizationGroupMembers { get; set; }
    }
}
