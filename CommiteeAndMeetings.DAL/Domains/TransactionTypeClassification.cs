using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(TransactionTypeClassificationId), Name = "IX_TransactionTypeClassifications_TransactionTypeId")]
    public partial class TransactionTypeClassification
    {
        [Key]
        public int TransactionTypeClassificationId { get; set; }
        public int TransactionTypeId { get; set; }
        public int ClassificationId { get; set; }
        public bool? IsFavorite { get; set; }

        [ForeignKey(nameof(ClassificationId))]
        [InverseProperty("TransactionTypeClassifications")]
        public virtual Classification Classification { get; set; }
        [ForeignKey(nameof(TransactionTypeId))]
        [InverseProperty("TransactionTypeClassifications")]
        public virtual TransactionType TransactionType { get; set; }
    }
}
