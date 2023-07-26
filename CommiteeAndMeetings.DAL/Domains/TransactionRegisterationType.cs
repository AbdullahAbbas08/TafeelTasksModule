using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("TransactionRegisterationType")]
    public partial class TransactionRegisterationType
    {
        [Key]
        public int TransactionRegisterationTypeId { get; set; }
        [StringLength(400)]
        public string TransactionRegisterationTypeAr { get; set; }
        [StringLength(400)]
        public string TransactionRegisterationTypeEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string TransactionRegisterationTypeFn { get; set; }
    }
}
