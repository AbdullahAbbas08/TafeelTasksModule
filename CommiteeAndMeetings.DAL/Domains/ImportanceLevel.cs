using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_ImportanceLevels_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_ImportanceLevels_UpdatedBy")]
    public partial class ImportanceLevel
    {
        public ImportanceLevel()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int ImportanceLevelId { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameAr { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int ImportanceAcheivementPeriod { get; set; }
        public string ImportanceLevelColor { get; set; }
        public int? ImportanceFollowUpPeriod { get; set; }
        public bool IsDefault { get; set; }
        public string ImportanceLevelNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.ImportanceLevelCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.ImportanceLevelUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Transaction.ImportanceLevel))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
