using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_IncomingTypes_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_IncomingTypes_UpdatedBy")]
    public partial class IncomingType
    {
        public IncomingType()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int IncomingTypeId { get; set; }
        [StringLength(400)]
        public string IncomingTypeNameAr { get; set; }
        [StringLength(400)]
        public string IncomingTypeNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsDefault { get; set; }
        public string IncomingTypeNameFn { get; set; }
        public bool HideFromRegistration { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.IncomingTypeCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.IncomingTypeUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Transaction.IncomingType))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
