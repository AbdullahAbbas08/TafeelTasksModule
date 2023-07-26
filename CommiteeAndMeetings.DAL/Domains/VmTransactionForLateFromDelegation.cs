﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionForLateFromDelegation
    {
        public long TransactionId { get; set; }
        [Required]
        [StringLength(1)]
        public string OldTransactionDate { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public string NormalizedSubject { get; set; }
        public int TransactionTypeId { get; set; }
        public int? ImportanceLevelId { get; set; }
        public int? ClassificationId { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public int? RequiredActionId { get; set; }
        public int? RegisteredBy { get; set; }
        public int? CreatedByEmployee { get; set; }
        public DateTimeOffset? RegisterationDate { get; set; }
        public int? FromOrganizationId { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public string FromOrganizationNameFn { get; set; }
        public int? ToOrganizationId { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }
        public int? OwnerId { get; set; }
        [StringLength(50)]
        public string UserIdentificationNumber { get; set; }
        [StringLength(50)]
        public string UserMobile { get; set; }
        [StringLength(50)]
        public string ReferenceNumber { get; set; }
        [StringLength(100)]
        public string FromUserNameAr { get; set; }
        [StringLength(100)]
        public string FromUserNameEn { get; set; }
        public string FromUserNameFn { get; set; }
        public byte[] FromUserProfileImage { get; set; }
        public int? ToUserId { get; set; }
        [StringLength(100)]
        public string ToUserNameAr { get; set; }
        [StringLength(100)]
        public string ToUserNameEn { get; set; }
        public string ToUserNameFn { get; set; }
        public byte[] ToUserProfileImage { get; set; }
        [Column("LbLReferenceNumber")]
        public string LbLreferenceNumber { get; set; }
        public string AttachmentNotification { get; set; }
        [StringLength(50)]
        public string IncomingLetterNumber { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public string ImportanceLevelColor { get; set; }
        public int RecipientStatusId { get; set; }
        public int? ActionId { get; set; }
        public bool? IsCancelled { get; set; }
        [Column("IsCC")]
        public bool? IsCc { get; set; }
        public string Notes { get; set; }
        public bool? IsConfidential { get; set; }
        public int? CreatedByUserRoleId { get; set; }
        [StringLength(50)]
        public string RequiredActionNameAr { get; set; }
        [StringLength(50)]
        public string RequiredActionNameEn { get; set; }
        public string RequiredActionNameFn { get; set; }
        public int IsLate { get; set; }
        public int HijriYear { get; set; }
        [Column("individual_userId")]
        public int? IndividualUserId { get; set; }
        [Column("individual_Mobile")]
        [StringLength(50)]
        public string IndividualMobile { get; set; }
        [Column("individual_IdentificationNumber")]
        [StringLength(50)]
        public string IndividualIdentificationNumber { get; set; }
        public int? TagId { get; set; }
        public int RelatedTransactionsCount { get; set; }
        public string AttchmentText { get; set; }
        public int CorrespondentUserId { get; set; }
        public int CompanyId { get; set; }
        [Column("Transaction_Notes")]
        public string TransactionNotes { get; set; }
        [Required]
        public string IncomingOrganizationNameAr { get; set; }
        [Required]
        public string IncomingOrganizationNameEn { get; set; }
        [Required]
        public string IncomingOrganizationNameFr { get; set; }
        public DateTimeOffset? DelegationDate { get; set; }
        public int LatePeriod { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameAr { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameEn { get; set; }
        public string RecipientStatusNameFn { get; set; }
    }
}
