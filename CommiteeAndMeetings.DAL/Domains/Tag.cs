using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_Tags_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_Tags_UpdatedBy")]
    public partial class Tag
    {
        public Tag()
        {
            AttachmentTags = new HashSet<AttachmentTag>();
            TransactionTags = new HashSet<TransactionTag>();
        }

        [Key]
        public int TagId { get; set; }
        public string Text { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string TextEn { get; set; }
        public string TextFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TagCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TagUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(AttachmentTag.Tag))]
        public virtual ICollection<AttachmentTag> AttachmentTags { get; set; }
        [InverseProperty(nameof(TransactionTag.Tag))]
        public virtual ICollection<TransactionTag> TransactionTags { get; set; }
    }
}
