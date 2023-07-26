﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionMapView
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
        public int? RecipientStatusChangedBy { get; set; }
        public DateTimeOffset? RecipientStatusChangedOn { get; set; }
        public DateTimeOffset? TransactionActionCreatedOn { get; set; }
        public int ActionId { get; set; }
        public int? RequiredActionId { get; set; }
        [StringLength(50)]
        public string RequiredActionNameAr { get; set; }
        [StringLength(50)]
        public string RequiredActionNameEn { get; set; }
        public string RequiredActionCode { get; set; }
        public int? RecipientStatusId { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameAr { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameEn { get; set; }
        [Required]
        [StringLength(50)]
        public string RecipientStatusLocalizeNameAr { get; set; }
        [Required]
        [StringLength(50)]
        public string RecipientStatusLocalizeNameEn { get; set; }
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
        [StringLength(100)]
        public string FromUserNameAr { get; set; }
        [StringLength(100)]
        public string FromUserNameEn { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameAr { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameEn { get; set; }
        public int TransactionTypeId { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        [StringLength(400)]
        public string TransactionTypeNameAr { get; set; }
        [StringLength(400)]
        public string TransactionTypeNameEn { get; set; }
        public bool IsUrgent { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public string Notes { get; set; }
        public int ExecutionPeriod { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsHidden { get; set; }
        public int? ImportanceLevelId { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameAr { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelColor { get; set; }
        [Column("CorrespondingUserNameAR")]
        [StringLength(100)]
        public string CorrespondingUserNameAr { get; set; }
        [StringLength(100)]
        public string CorrespondingUserNameEn { get; set; }
        [StringLength(100)]
        public string ToUserFullNameAr { get; set; }
        [StringLength(100)]
        public string ToUserFullNameEn { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public bool? ToOrganizatioIsOuterOrganization { get; set; }
        [Column("incomingOrganizationNameAr")]
        public string IncomingOrganizationNameAr { get; set; }
        [Column("incomingOrganizationNameEn")]
        public string IncomingOrganizationNameEn { get; set; }
        public int ReferrerTransactionActionId { get; set; }
        public int ReferrerTransactionActionRecipientId { get; set; }
        public bool IsConfidential { get; set; }
        public byte[] ProfileImage { get; set; }
        public string NormalizedSubject { get; set; }
        public int IsLate { get; set; }
        public int WorkDays { get; set; }
        public int HasCorrespnd { get; set; }
        [StringLength(100)]
        public string CreatedByNameAr { get; set; }
        [StringLength(100)]
        public string CreatedByNameEn { get; set; }
        public int FromOrganizationBlackBoxOrganizationId { get; set; }
        public string FromOrganizationBlackBoxOrganizationNameAr { get; set; }
        public string FromOrganizationBlackBoxOrganizationNameEn { get; set; }
        public int FromUserOrganizationBlackBoxOrganizationId { get; set; }
        public string FromUserOrganizationBlackBoxOrganizationNameAr { get; set; }
        public string FromUserOrganizationBlackBoxOrganizationNameEn { get; set; }
        public int ToOrganizationBlackBoxOrganizationId { get; set; }
        public string ToOrganizationBlackBoxOrganizationNameAr { get; set; }
        public string ToOrganizationBlackBoxOrganizationNameEn { get; set; }
        public int ToUserOrganizationBlackBoxOrganizationId { get; set; }
        public string ToUserOrganizationBlackBoxOrganizationNameAr { get; set; }
        public string ToUserOrganizationBlackBoxOrganizationNameEn { get; set; }
        public int BlackBoxEnabled { get; set; }
        public int SameHickalDirectedToOrganizationId { get; set; }
        public int SameHickalFromOrganizationId { get; set; }
        public bool IsNoteHidden { get; set; }
    }
}
