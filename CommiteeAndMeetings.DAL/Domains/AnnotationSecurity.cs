using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AnnotationId), Name = "IX_AnnotationSecurities_AnnotationId")]
    [Index(nameof(UserId), Name = "IX_AnnotationSecurities_UserId")]
    public partial class AnnotationSecurity
    {
        [Key]
        public int Id { get; set; }
        public int AnnotationId { get; set; }
        public int UserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(AnnotationId))]
        [InverseProperty("AnnotationSecurities")]
        public virtual Annotation Annotation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("AnnotationSecurities")]
        public virtual User User { get; set; }
    }
}
