using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(MigratedTransactionActionId), Name = "IX_MigratedTransactionActionRecipients_MigratedTransactionActionId")]
    public partial class MigratedTransactionActionRecipient
    {
        [Key]
        public long MigratedTransactionActionRecipientId { get; set; }
        public long? MigratedTransactionActionId { get; set; }
        [StringLength(100)]
        public string DelegationType { get; set; }
        [StringLength(300)]
        public string ParticipantUserName { get; set; }
        [StringLength(300)]
        public string ParticipantOrganizationName { get; set; }
        [StringLength(100)]
        public string ParticipantType { get; set; }
        [StringLength(4000)]
        public string Instructions { get; set; }
        public bool? IsSaved { get; set; }
        [Column("IsCC")]
        public bool? IsCc { get; set; }
        [StringLength(20)]
        public string ToDeptCode { get; set; }
        public bool? IsPending { get; set; }
        [StringLength(20)]
        public string ToUserName { get; set; }

        [ForeignKey(nameof(MigratedTransactionActionId))]
        [InverseProperty("MigratedTransactionActionRecipients")]
        public virtual MigratedTransactionAction MigratedTransactionAction { get; set; }
    }
}
