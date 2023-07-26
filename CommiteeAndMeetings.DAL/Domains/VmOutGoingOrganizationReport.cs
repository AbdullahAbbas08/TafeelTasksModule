using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmOutGoingOrganizationReport
    {
        public int Id { get; set; }
        public long TransactionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public DateTimeOffset? TransactionCreatedOn { get; set; }
        public int TransactionActionId { get; set; }
        public int? CorrespondentUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public DateTimeOffset? TransactionActionCreatedOn { get; set; }
        public int ActionId { get; set; }
        public int? RequiredActionId { get; set; }
        public int? RecipientStatusId { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameAr { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameEn { get; set; }
        [Required]
        [Column("transactionStatus")]
        [StringLength(1)]
        public string TransactionStatus { get; set; }
        public int? FromUserId { get; set; }
        public int? FromOrganizationId { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
        [Column("archieveDate")]
        public int ArchieveDate { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public int TransactionTypeId { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        public bool IsUrgent { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public string Notes { get; set; }
        public int ExecutionPeriod { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? ImportanceLevelId { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public bool? ToOrganizatioIsOuterOrganization { get; set; }
        [StringLength(100)]
        public string FromUserNameAr { get; set; }
        [StringLength(100)]
        public string FromUserNameEn { get; set; }
        public int RelatedTransactionsCount { get; set; }
    }
}
