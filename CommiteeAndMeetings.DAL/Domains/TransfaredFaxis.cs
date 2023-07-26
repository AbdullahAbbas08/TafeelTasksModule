using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(TransactionId), Name = "IX_TransfaredFaxes_TransactionId")]
    public partial class TransfaredFaxis
    {
        [Key]
        public int Id { get; set; }
        public string FaxId { get; set; }
        public long TransactionId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransfaredFaxes")]
        public virtual Transaction Transaction { get; set; }
    }
}
