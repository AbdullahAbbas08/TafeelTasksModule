using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_PhysicalAttachmentTypes_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_PhysicalAttachmentTypes_UpdatedBy")]
    public partial class PhysicalAttachmentType
    {
        public PhysicalAttachmentType()
        {
            Attachments = new HashSet<Attachment>();
        }

        [Key]
        public int PhysicalAttachmentTypeId { get; set; }
        [StringLength(400)]
        public string PhysicalAttachmentTypeNameAr { get; set; }
        [StringLength(400)]
        public string PhysicalAttachmentTypeNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string PhysicalAttachmentTypeNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.PhysicalAttachmentTypeCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.PhysicalAttachmentTypeUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Attachment.PhysicalAttachmentType))]
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
