using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentId), Name = "IX_AttachmentTags_AttachmentId")]
    [Index(nameof(CreatedBy), Name = "IX_AttachmentTags_CreatedBy")]
    [Index(nameof(TagId), Name = "IX_AttachmentTags_TagId")]
    public partial class AttachmentTag
    {
        [Key]
        public int AttachmentTagId { get; set; }
        public int AttachmentId { get; set; }
        public int? TagId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("AttachmentTags")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.AttachmentTags))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("AttachmentTags")]
        public virtual Tag Tag { get; set; }
    }
}
