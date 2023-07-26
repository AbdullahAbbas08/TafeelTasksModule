using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_TransactionRelationships_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionRelationships_UpdatedBy")]
    public partial class TransactionRelationship
    {
        public TransactionRelationship()
        {
            RelatedTransactions = new HashSet<RelatedTransaction>();
        }

        [Key]
        public int TransactionRelationshipId { get; set; }
        [StringLength(400)]
        public string TransactionRelationshipNameAr { get; set; }
        [StringLength(400)]
        public string TransactionRelationshipNameEn { get; set; }
        public int DisplayOrder { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool SavedRelatedTransaction { get; set; }
        public bool SentRelatedTransaction { get; set; }
        public string TransactionRelationshipNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionRelationshipCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionRelationshipUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(RelatedTransaction.TransactionRelationship))]
        public virtual ICollection<RelatedTransaction> RelatedTransactions { get; set; }
    }
}
