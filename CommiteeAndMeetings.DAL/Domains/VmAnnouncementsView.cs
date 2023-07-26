using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Vm_Announcements_View")]
    public partial class VmAnnouncementsView
    {
        [Key]
        public int Id { get; set; }
        public long TransactionId { get; set; }
        public int ActionId { get; set; }
        public int TransactionActionId { get; set; }
        public int? CorrespondentUserId { get; set; }
        public string FromUserNameAr { get; set; }
        public string FromUserNameEn { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? RequiredActionId { get; set; }
        public string RequiredActionNameAr { get; set; }
        public string RequiredActionNameEn { get; set; }
        public int? RecipientStatusId { get; set; }
        public string RecipientStatusNameAr { get; set; }
        public string RecipientStatusNameEn { get; set; }
        [Column("transactionStatus")]
        public string TransactionStatus { get; set; }
        public int? FromUserId { get; set; }
        public int? FromOrganizationId { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public string ConfidentialityLevelNameAr { get; set; }
        public string ConfidentialityLevelNameEn { get; set; }
        [Column("transactionTypeId")]
        public int? TransactionTypeId { get; set; }
        [Column("transactionTypeNameAr")]
        public string TransactionTypeNameAr { get; set; }
        [Column("transactionTypeNameEn")]
        public string TransactionTypeNameEn { get; set; }
        public bool IsUrgent { get; set; }
        public bool IsConfidential { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        [Column("createdOn")]
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? TransactionActionCreatedOn { get; set; }
        public byte[] ProfileImage { get; set; }
        public int LetterCount { get; set; }
        public int DocumentCount { get; set; }
        public int PhysicalCount { get; set; }
        public int ActionLetterCount { get; set; }
        public int ActionDocumentCount { get; set; }
        public int ActionPhysicalCount { get; set; }
        public int RelatedTransactionsCount { get; set; }
        public int? ImportanceLevelId { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public string ImportanceLevelNameAr { get; set; }
        public string ImportanceLevelNameEn { get; set; }
        public string NormalizedSubject { get; set; }
        public int IsLate { get; set; }
        public int IndivnidualsCounts { get; set; }
        public string ImportanceLevelColor { get; set; }
        [Column("archieveDate")]
        public int? ArchieveDate { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        [Column("CorrespondingUserNameAR")]
        public string CorrespondingUserNameAr { get; set; }
        public string CorrespondingUserNameEn { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToUserFullNameAr { get; set; }
        public string ToUserFullNameEn { get; set; }
        public bool? ToOrganizatioIsOuterOrganization { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public string Notes { get; set; }
        public int? ExecutionPeriod { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public string ConfidentialityLevelNameFn { get; set; }
        public string CorrespondingUserNameFn { get; set; }
        public string FromOrganizationNameFn { get; set; }
        public string FromUserNameFn { get; set; }
        public string ImportanceLevelNameFn { get; set; }
        public string RecipientStatusNameFn { get; set; }
        public string RequiredActionNameFn { get; set; }
        public string ToOrganizationNameFn { get; set; }
        public string ToUserFullNameFn { get; set; }
        [Column("transactionTypeNameFn")]
        public string TransactionTypeNameFn { get; set; }
    }
}
