using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ChildTransactionId), Name = "IX_RelatedTransactions_ChildTransactionId")]
    [Index(nameof(CreatedBy), Name = "IX_RelatedTransactions_CreatedBy")]
    [Index(nameof(ParentTransactionId), Name = "IX_RelatedTransactions_ParentTransactionId")]
    [Index(nameof(TransactionRelationshipId), Name = "IX_RelatedTransactions_TransactionRelationshipId")]
    [Index(nameof(TransactionSourceId), Name = "IX_RelatedTransactions_TransactionSourceId")]
    [Index(nameof(UpdatedBy), Name = "IX_RelatedTransactions_UpdatedBy")]
    public partial class RelatedTransaction
    {
        [Key]
        public int RelatedTransactionId { get; set; }
        public long ParentTransactionId { get; set; }
        public long? ChildTransactionId { get; set; }
        public long? ChildOldTransactionId { get; set; }
        public int? TransactionRelationshipId { get; set; }
        public int? TransactionSourceId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool? IsShowAttachment { get; set; }

        [ForeignKey(nameof(ChildOldTransactionId))]
        [InverseProperty(nameof(MigratedTransaction.RelatedTransactions))]
        public virtual MigratedTransaction ChildOldTransaction { get; set; }
        [ForeignKey(nameof(ChildTransactionId))]
        [InverseProperty(nameof(Transaction.RelatedTransactionChildTransactions))]
        public virtual Transaction ChildTransaction { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.RelatedTransactionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(ParentTransactionId))]
        [InverseProperty(nameof(Transaction.RelatedTransactionParentTransactions))]
        public virtual Transaction ParentTransaction { get; set; }
        [ForeignKey(nameof(TransactionRelationshipId))]
        [InverseProperty("RelatedTransactions")]
        public virtual TransactionRelationship TransactionRelationship { get; set; }
        [ForeignKey(nameof(TransactionSourceId))]
        [InverseProperty("RelatedTransactions")]
        public virtual TransactionSource TransactionSource { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.RelatedTransactionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
