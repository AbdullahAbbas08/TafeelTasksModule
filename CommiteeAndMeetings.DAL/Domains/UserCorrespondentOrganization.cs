using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_UserCorrespondentOrganizations_CreatedBy")]
    [Index(nameof(OrganizationId), Name = "IX_UserCorrespondentOrganizations_OrganizationId")]
    [Index(nameof(UpdatedBy), Name = "IX_UserCorrespondentOrganizations_UpdatedBy")]
    [Index(nameof(UserId), Name = "IX_UserCorrespondentOrganizations_UserId")]
    public partial class UserCorrespondentOrganization
    {
        [Key]
        public int UserCorrespondentOrganizationId { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("UserCorrespondentOrganizationCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("UserCorrespondentOrganizations")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("UserCorrespondentOrganizationUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserCorrespondentOrganizationUsers")]
        public virtual User User { get; set; }
    }
}
