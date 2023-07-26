using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(MigratedTransactionId), Name = "IX_MigratedTransactionActions_MigratedTransactionId")]
    public partial class MigratedTransactionAction
    {
        public MigratedTransactionAction()
        {
            MigratedTransactionActionRecipients = new HashSet<MigratedTransactionActionRecipient>();
        }

        [Key]
        public long MigratedTransactionActionId { get; set; }
        public long? MigratedTransactionId { get; set; }
        [StringLength(300)]
        public string FromOrganizationName { get; set; }
        [StringLength(100)]
        public string UrgencyLevel { get; set; }
        [StringLength(100)]
        public string ImportanceLevel { get; set; }
        [StringLength(100)]
        public string DelegationDate { get; set; }
        public string Instructions { get; set; }
        [StringLength(100)]
        public string ApplicationType { get; set; }
        [StringLength(100)]
        public string FromOrganizationCode { get; set; }
        [StringLength(100)]
        public string FromUserName { get; set; }
        [StringLength(100)]
        public string TransactionActionDate { get; set; }
        [StringLength(100)]
        public string TransactionActionDateHijri { get; set; }

        [ForeignKey(nameof(MigratedTransactionId))]
        [InverseProperty("MigratedTransactionActions")]
        public virtual MigratedTransaction MigratedTransaction { get; set; }
        [InverseProperty(nameof(MigratedTransactionActionRecipient.MigratedTransactionAction))]
        public virtual ICollection<MigratedTransactionActionRecipient> MigratedTransactionActionRecipients { get; set; }
    }
}
