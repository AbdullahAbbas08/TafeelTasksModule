using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("ActionReferralMode")]
    [Index(nameof(ActionId), Name = "IX_ActionReferralMode_ActionId")]
    [Index(nameof(TransactionTypeId), Name = "IX_ActionReferralMode_TransactionTypeId")]
    public partial class ActionReferralMode
    {
        [Key]
        public int ActionReferralModeId { get; set; }
        public int ActionId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(TransactionTypeId))]
        [InverseProperty("ActionReferralModes")]
        public virtual TransactionType TransactionType { get; set; }
    }
}
