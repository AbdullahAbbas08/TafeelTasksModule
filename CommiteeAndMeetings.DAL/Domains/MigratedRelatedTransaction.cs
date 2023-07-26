using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(MigratedTransactionId), Name = "IX_MigratedRelatedTransactions_MigratedTransactionId")]
    [Index(nameof(RelatedMigratedTransactionId), Name = "IX_MigratedRelatedTransactions_RelatedMigratedTransactionId")]
    public partial class MigratedRelatedTransaction
    {
        [Key]
        public long MigratedRelatedTransactionId { get; set; }
        public long? MigratedTransactionId { get; set; }
        public long? RelatedMigratedTransactionId { get; set; }
        [StringLength(50)]
        public string RelationType { get; set; }

        [ForeignKey(nameof(MigratedTransactionId))]
        [InverseProperty("MigratedRelatedTransactionMigratedTransactions")]
        public virtual MigratedTransaction MigratedTransaction { get; set; }
        [ForeignKey(nameof(RelatedMigratedTransactionId))]
        [InverseProperty("MigratedRelatedTransactionRelatedMigratedTransactions")]
        public virtual MigratedTransaction RelatedMigratedTransaction { get; set; }
    }
}
