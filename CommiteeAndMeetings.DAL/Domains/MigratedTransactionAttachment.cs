using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(MigratedTransactionId), Name = "IX_MigratedTransactionAttachments_MigratedTransactionId")]
    public partial class MigratedTransactionAttachment
    {
        [Key]
        public long MigratedTransactionAttachmentId { get; set; }
        public long? MigratedTransactionId { get; set; }
        public long? AttachmentId { get; set; }
        public string Permissions { get; set; }

        [ForeignKey(nameof(MigratedTransactionId))]
        [InverseProperty("MigratedTransactionAttachments")]
        public virtual MigratedTransaction MigratedTransaction { get; set; }
    }
}
