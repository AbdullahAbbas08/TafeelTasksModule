using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class AttachmentStatus
    {
        public AttachmentStatus()
        {
            TransactionActionRecipientAttachments = new HashSet<TransactionActionRecipientAttachment>();
        }

        [Key]
        public int AttachmentStatusId { get; set; }
        [StringLength(400)]
        public string AttachmentStatusCode { get; set; }
        [StringLength(400)]
        public string AttachmentStatusNameAr { get; set; }
        [StringLength(400)]
        public string AttachmentStatusNameEn { get; set; }
        public string AttachmentStatusNameFn { get; set; }

        [InverseProperty(nameof(TransactionActionRecipientAttachment.AttachmentStatus))]
        public virtual ICollection<TransactionActionRecipientAttachment> TransactionActionRecipientAttachments { get; set; }
    }
}
