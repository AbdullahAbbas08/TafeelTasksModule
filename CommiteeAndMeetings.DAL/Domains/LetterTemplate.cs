using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_LetterTemplates_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_LetterTemplates_UpdatedBy")]
    public partial class LetterTemplate
    {
        public LetterTemplate()
        {
            LetterTemplateOrganizations = new HashSet<LetterTemplateOrganization>();
        }

        [Key]
        public int LetterTemplateId { get; set; }
        [StringLength(100)]
        public string LetterTemplateNameAr { get; set; }
        [StringLength(100)]
        public string LetterTemplateNameEn { get; set; }
        public string Description { get; set; }
        public bool? IsShared { get; set; }
        public string Text { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool? IsDefault { get; set; }
        public string LetterTemplateNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.LetterTemplateCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.LetterTemplateUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(LetterTemplateOrganization.LetterTemplate))]
        public virtual ICollection<LetterTemplateOrganization> LetterTemplateOrganizations { get; set; }
    }
}
