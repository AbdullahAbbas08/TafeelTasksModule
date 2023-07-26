using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class AttachmentType
    {
        public AttachmentType()
        {
            Attachments = new HashSet<Attachment>();
        }

        [Key]
        public int AttachmentTypeId { get; set; }
        [StringLength(400)]
        public string AttachmentTypeCode { get; set; }
        [StringLength(400)]
        public string AttachmentTypeNameAr { get; set; }
        [StringLength(400)]
        public string AttachmentTypeNameEn { get; set; }
        public string AttachmentTypeNameFn { get; set; }

        [InverseProperty(nameof(Attachment.AttachmentType))]
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
