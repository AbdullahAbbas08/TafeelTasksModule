using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_Localizations_CreatedBy")]
    [Index(nameof(Key), Name = "IX_Localizations_Key", IsUnique = true)]
    [Index(nameof(UpdatedBy), Name = "IX_Localizations_UpdatedBy")]
    public partial class Localization
    {
        [Key]
        public int LocalizationId { get; set; }
        [Required]
        [StringLength(100)]
        public string Key { get; set; }
        [Required]
        public string ValueAr { get; set; }
        [Required]
        public string ValueEn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string ValueFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.LocalizationCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.LocalizationUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
