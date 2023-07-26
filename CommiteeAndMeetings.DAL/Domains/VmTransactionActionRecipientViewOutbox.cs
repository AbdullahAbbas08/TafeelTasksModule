using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionActionRecipientViewOutbox
    {
        public int? Id { get; set; }
        public long TransactionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public int? TransactionActionId { get; set; }
        public int? CorrespondentUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public DateTimeOffset? TransactionActionCreatedOn { get; set; }
        public int ActionId { get; set; }
        public int? RequiredActionId { get; set; }
        [StringLength(50)]
        public string RequiredActionNameAr { get; set; }
        [StringLength(50)]
        public string RequiredActionNameEn { get; set; }
        public string RequiredActionNameFn { get; set; }
        public int? RecipientStatusId { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameAr { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameEn { get; set; }
        public string RecipientStatusNameFn { get; set; }
        [Required]
        [Column("transactionStatus")]
        [StringLength(1)]
        public string TransactionStatus { get; set; }
        public int? FromUserId { get; set; }
        public int? FromOrganizationId { get; set; }
        [Column("IsCC")]
        public bool? IsCc { get; set; }
        [Column("archieveDate")]
        public int ArchieveDate { get; set; }
        [StringLength(100)]
        public string FromUserNameAr { get; set; }
        [StringLength(100)]
        public string FromUserNameEn { get; set; }
        public string FromUserNameFn { get; set; }
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
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        [StringLength(400)]
        public string TransactionTypeNameAr { get; set; }
        [StringLength(400)]
        public string TransactionTypeNameEn { get; set; }
        public string TransactionTypeNameFn { get; set; }
        public bool? IsUrgent { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public string SubjectEn { get; set; }
        public string SubjectFn { get; set; }
        public string Notes { get; set; }
        public int ExecutionPeriod { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? ImportanceLevelId { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameAr { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelNameFn { get; set; }
        public string ImportanceLevelColor { get; set; }
        [Column("CorrespondingUserNameAR")]
        [StringLength(100)]
        public string CorrespondingUserNameAr { get; set; }
        [StringLength(100)]
        public string CorrespondingUserNameEn { get; set; }
        public string CorrespondingUserNameFn { get; set; }
        [StringLength(100)]
        public string ToUserFullNameAr { get; set; }
        [StringLength(100)]
        public string ToUserFullNameEn { get; set; }
        public string ToUserFullNameFn { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }
        public bool? ToOrganizatioIsOuterOrganization { get; set; }
        [Column("incomingOrganizationNameAr")]
        public string IncomingOrganizationNameAr { get; set; }
        [Column("incomingOrganizationNameEn")]
        public string IncomingOrganizationNameEn { get; set; }
        [Column("incomingOrganizationNameFn")]
        public string IncomingOrganizationNameFn { get; set; }
        public int ReferrerTransactionActionId { get; set; }
        public bool IsConfidential { get; set; }
        public byte[] ProfileImage { get; set; }
        public string NormalizedSubject { get; set; }
        public int? DocumentCount { get; set; }
        public int? ActionDocumentCount { get; set; }
        public int? LetterCount { get; set; }
        public int? ActionLetterCount { get; set; }
        public int? PhysicalCount { get; set; }
        public int? ActionPhysicalCount { get; set; }
        public int? RelatedTransactionsCount { get; set; }
        public int IsLate { get; set; }
        public int? IndivnidualsCounts { get; set; }
        [Column("hasCorrespondence")]
        public int HasCorrespondence { get; set; }
        [Column("isCancelled")]
        public bool? IsCancelled { get; set; }
        public int? MaxTransactionActionRecipientId { get; set; }
        public bool IsShowAllTime { get; set; }
    }
}
