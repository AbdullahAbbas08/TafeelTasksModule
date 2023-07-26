using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("TransactionTypeSerialAdjustment")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionTypeSerialAdjustment_CreatedBy")]
    [Index(nameof(TransactionTypeId), Name = "IX_TransactionTypeSerialAdjustment_TransactionTypeId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionTypeSerialAdjustment_UpdatedBy")]
    public partial class TransactionTypeSerialAdjustment
    {
        [Key]
        public int TransactionTypeSerialAdjustmentId { get; set; }
        public int TransactionTypeId { get; set; }
        public int AdjustPeriod { get; set; }
        public int Year { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionTypeSerialAdjustmentCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TransactionTypeId))]
        [InverseProperty("TransactionTypeSerialAdjustments")]
        public virtual TransactionType TransactionType { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionTypeSerialAdjustmentUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
