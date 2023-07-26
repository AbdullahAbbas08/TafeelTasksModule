using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_Classifications_CreatedBy")]
    [Index(nameof(ParentClassificationId), Name = "IX_Classifications_ParentClassificationId")]
    [Index(nameof(UpdatedBy), Name = "IX_Classifications_UpdatedBy")]
    public partial class Classification
    {
        public Classification()
        {
            InverseParentClassification = new HashSet<Classification>();
            TransactionTypeClassifications = new HashSet<TransactionTypeClassification>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int ClassificationId { get; set; }
        [StringLength(400)]
        public string ClassificationNameAr { get; set; }
        [StringLength(400)]
        public string ClassificationNameEn { get; set; }
        public bool IsShared { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsDefault { get; set; }
        [StringLength(400)]
        public string ReferenceNumberNameAr { get; set; }
        [StringLength(400)]
        public string ReferenceNumberNameEn { get; set; }
        public string Color { get; set; }
        public bool IsReferenceRequired { get; set; }
        public string ClassificationNameFn { get; set; }
        public string ReferenceNumberNameFn { get; set; }
        public int? ParentClassificationId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.ClassificationCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(ParentClassificationId))]
        [InverseProperty(nameof(Classification.InverseParentClassification))]
        public virtual Classification ParentClassification { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.ClassificationUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Classification.ParentClassification))]
        public virtual ICollection<Classification> InverseParentClassification { get; set; }
        [InverseProperty(nameof(TransactionTypeClassification.Classification))]
        public virtual ICollection<TransactionTypeClassification> TransactionTypeClassifications { get; set; }
        [InverseProperty(nameof(Transaction.Classification))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
