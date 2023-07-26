using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_LetterTemplateOrganizations_CreatedBy")]
    [Index(nameof(LetterTemplateId), Name = "IX_LetterTemplateOrganizations_LetterTemplateId")]
    [Index(nameof(OrganizationId), Name = "IX_LetterTemplateOrganizations_OrganizationId")]
    [Index(nameof(UpdatedBy), Name = "IX_LetterTemplateOrganizations_UpdatedBy")]
    public partial class LetterTemplateOrganization
    {
        [Key]
        public int LetterTemplateOrganizationId { get; set; }
        public int OrganizationId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int LetterTemplateId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.LetterTemplateOrganizationCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(LetterTemplateId))]
        [InverseProperty("LetterTemplateOrganizations")]
        public virtual LetterTemplate LetterTemplate { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("LetterTemplateOrganizations")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.LetterTemplateOrganizationUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
