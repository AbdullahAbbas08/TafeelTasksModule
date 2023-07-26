using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("ECMTemplate")]
    [Index(nameof(OrganizationId), Name = "IX_ECMTemplate_OrganizationId")]
    [Index(nameof(UserId), Name = "IX_ECMTemplate_UserId")]
    public partial class Ecmtemplate
    {
        [Key]
        [Column("ECMTemplateId")]
        public int EcmtemplateId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserId { get; set; }
        public string TemplateName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("Ecmtemplates")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Ecmtemplates")]
        public virtual User User { get; set; }
    }
}
