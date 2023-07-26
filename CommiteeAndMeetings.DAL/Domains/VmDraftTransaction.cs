using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmDraftTransaction
    {
        public long Id { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public string SubjectEn { get; set; }
        public string SubjectFn { get; set; }
        public string NormalizedSubject { get; set; }
        public string Notes { get; set; }
        public int TransactionTypeId { get; set; }
        public int? TransactionBasisTypeId { get; set; }
        public int? ClassificationId { get; set; }
        public int? ImportanceLevelId { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public int? IncomingTypeId { get; set; }
        [StringLength(50)]
        public string IncomingLetterNumber { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public int? IncomingOrganizationId { get; set; }
        [StringLength(50)]
        public string IncomingCorrespondentName { get; set; }
        [StringLength(50)]
        public string IncomingCorrespondentMobileNumber { get; set; }
        [StringLength(50)]
        public string IncomingCorrespondentEmail { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public int ExecutionPeriod { get; set; }
        public bool IsForAll { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? IncomingOutgoingLetterDate { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        [StringLength(50)]
        public string TransactionTypeCode { get; set; }
        [StringLength(50)]
        public string TransactionTypeNameAr { get; set; }
        [StringLength(50)]
        public string TransactionTypeNameEn { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsExternal { get; set; }
        public bool IsInternal { get; set; }
        public bool IsDecision { get; set; }
        public string TransactionTypeCodeForSerial { get; set; }
        public int? DocumentCount { get; set; }
        public int? LetterCount { get; set; }
        public int? PhysicalCount { get; set; }
        public long TransactionId { get; set; }
        [Column("isCancelled")]
        public bool? IsCancelled { get; set; }
        public int? CreatedByUserRoleId { get; set; }
    }
}
