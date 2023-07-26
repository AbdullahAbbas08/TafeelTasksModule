using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class AnnotationType
    {
        public AnnotationType()
        {
            Annotations = new HashSet<Annotation>();
            Signatures = new HashSet<Signature>();
        }

        [Key]
        public int AnnotationTypeId { get; set; }
        [StringLength(400)]
        public string AnnotationTypeCode { get; set; }
        [StringLength(400)]
        public string AnnotationTypeNameAr { get; set; }
        [StringLength(400)]
        public string AnnotationTypeNameEn { get; set; }
        public string TemplateHtml { get; set; }
        public string AnnotationTypeNameFn { get; set; }

        [InverseProperty(nameof(Annotation.AnnotationType))]
        public virtual ICollection<Annotation> Annotations { get; set; }
        [InverseProperty(nameof(Signature.AnnotationType))]
        public virtual ICollection<Signature> Signatures { get; set; }
    }
}
