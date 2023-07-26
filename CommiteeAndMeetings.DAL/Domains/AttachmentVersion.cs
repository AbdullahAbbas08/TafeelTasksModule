using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentId), Name = "IX_AttachmentVersions_AttachmentId")]
    [Index(nameof(CreatedBy), Name = "IX_AttachmentVersions_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_AttachmentVersions_DeletedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_AttachmentVersions_UpdatedBy")]
    public partial class AttachmentVersion
    {
        [Key]
        public int AttachmentVersionId { get; set; }
        public int AttachmentId { get; set; }
        public string Text { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("AttachmentVersions")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.AttachmentVersionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty(nameof(User.AttachmentVersionDeletedByNavigations))]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.AttachmentVersionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
