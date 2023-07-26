using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionActionRecipientViewKpi
    {
        public long TransactionId { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionActionCreatedOn { get; set; }
        public string Subject { get; set; }
        public int? FromUserId { get; set; }
        public int? FromOrganizationId { get; set; }
        [StringLength(100)]
        public string FromUserNameAr { get; set; }
        [StringLength(100)]
        public string FromUserNameEn { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        [StringLength(100)]
        public string ToUserFullNameAr { get; set; }
        [StringLength(100)]
        public string ToUserFullNameEn { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string Notes { get; set; }
        public int? RequiredActionId { get; set; }
        [StringLength(50)]
        public string RequiredActionNameAr { get; set; }
        [StringLength(50)]
        public string RequiredActionNameEn { get; set; }
        [Column("transactionStatus")]
        public int? TransactionStatus { get; set; }
        public bool IsConfidential { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameAr { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameEn { get; set; }
        public bool? ToOrganizatioIsOuterOrganization { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
    }
}
