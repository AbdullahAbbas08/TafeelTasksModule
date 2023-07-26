using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_CommonGroups_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_CommonGroups_DeletedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_CommonGroups_UpdatedBy")]
    public partial class CommonGroup
    {
        public CommonGroup()
        {
            CommonGroupMembers = new HashSet<CommonGroupMember>();
        }

        [Key]
        public int CommonGroupId { get; set; }
        [Column("CommonGroupNameAR")]
        [StringLength(400)]
        public string CommonGroupNameAr { get; set; }
        [Column("CommonGroupNameEN")]
        [StringLength(400)]
        public string CommonGroupNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        [Column("CommonGroupNameFN")]
        public string CommonGroupNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.CommonGroupCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty(nameof(User.CommonGroupDeletedByNavigations))]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.CommonGroupUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(CommonGroupMember.CommonGroup))]
        public virtual ICollection<CommonGroupMember> CommonGroupMembers { get; set; }
    }
}
