using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_TransactionTags_CreatedBy")]
    [Index(nameof(TagId), Name = "IX_TransactionTags_TagId")]
    [Index(nameof(TransactionId), Name = "IX_TransactionTags_TransactionId")]
    public partial class TransactionTag
    {
        [Key]
        public int TransactionTagId { get; set; }
        public long TransactionId { get; set; }
        public int? TagId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionTags))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("TransactionTags")]
        public virtual Tag Tag { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransactionTags")]
        public virtual Transaction Transaction { get; set; }
    }
}
