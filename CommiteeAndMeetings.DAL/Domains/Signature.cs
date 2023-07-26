using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AnnotationTypeId), Name = "IX_Signatures_AnnotationTypeId")]
    [Index(nameof(CreatedBy), Name = "IX_Signatures_CreatedBy")]
    [Index(nameof(OrganizationId), Name = "IX_Signatures_OrganizationId")]
    [Index(nameof(UpdatedBy), Name = "IX_Signatures_UpdatedBy")]
    [Index(nameof(UserId), Name = "IX_Signatures_UserId")]
    public partial class Signature
    {
        public Signature()
        {
            Annotations = new HashSet<Annotation>();
        }

        [Key]
        public int SignatureId { get; set; }
        public int? UserId { get; set; }
        public byte[] SignatureFile { get; set; }
        public int? AnnotationTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [StringLength(500)]
        public string MimeType { get; set; }
        public int? OrganizationId { get; set; }
        [Required]
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column("TemplateHTML")]
        public string TemplateHtml { get; set; }

        [ForeignKey(nameof(AnnotationTypeId))]
        [InverseProperty("Signatures")]
        public virtual AnnotationType AnnotationType { get; set; }
        //[ForeignKey(nameof(CreatedBy))]
        //[InverseProperty("SignatureCreatedByNavigations")]
        [NotMapped]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("Signatures")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("SignatureUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("SignatureUsers")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(Annotation.Signature))]
        public virtual ICollection<Annotation> Annotations { get; set; }
    }
}
