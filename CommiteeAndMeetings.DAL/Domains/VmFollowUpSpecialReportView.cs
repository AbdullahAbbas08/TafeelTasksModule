using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmFollowUpSpecialReportView
    {
        public int Id { get; set; }
        public long TransactionId { get; set; }
        public DateTimeOffset? TransactionCreatedOn { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public int TransactionActionId { get; set; }
        public int? CorrespondentUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public DateTimeOffset? TransactionActionCreatedOn { get; set; }
        public int ActionId { get; set; }
        public int? RequiredActionId { get; set; }
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
        public string FromOrganizationNameFn { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameAr { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameEn { get; set; }
        public string ConfidentialityLevelNameFn { get; set; }
        public int TransactionTypeId { get; set; }
        [StringLength(50)]
        public string TransactionTypeNameAr { get; set; }
        [StringLength(50)]
        public string TransactionTypeNameEn { get; set; }
        public string TransactionTypeNameFn { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        public bool IsUrgent { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public string Notes { get; set; }
        public int ExecutionPeriod { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }
        public bool? ToOrganizatioIsOuterOrganization { get; set; }
        public int? ImportanceLevelId { get; set; }
        [StringLength(50)]
        public string ImportanceLevelNameAr { get; set; }
        [StringLength(50)]
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelNameFn { get; set; }
        public string ImportanceLevelColor { get; set; }
        [Column("incomingOrganizationNameAr")]
        public string IncomingOrganizationNameAr { get; set; }
        [Column("incomingOrganizationNameEn")]
        public string IncomingOrganizationNameEn { get; set; }
        [Column("incomingOrganizationNameFn")]
        public string IncomingOrganizationNameFn { get; set; }
        public int ReferrerTransactionActionId { get; set; }
        [Column("ParentDirectedToORganizationAr")]
        public string ParentDirectedToOrganizationAr { get; set; }
        [Column("ParentDirectedToORganizationEn")]
        public string ParentDirectedToOrganizationEn { get; set; }
        [Column("ParentDirectedToORganizationFn")]
        public string ParentDirectedToOrganizationFn { get; set; }
        [StringLength(50)]
        public string ClassificationNameAr { get; set; }
        [StringLength(50)]
        public string ClassificationNameEn { get; set; }
        public string ClassificationNameFn { get; set; }
    }
}
