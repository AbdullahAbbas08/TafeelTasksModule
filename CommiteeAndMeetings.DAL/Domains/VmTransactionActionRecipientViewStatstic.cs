using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionActionRecipientViewStatstic
    {
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public DateTimeOffset? TransactionCreatedOn { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameAr { get; set; }
        [StringLength(100)]
        public string ConfidentialityLevelNameEn { get; set; }
        public string ConfidentialityLevelNameFn { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameAr { get; set; }
        [StringLength(400)]
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelNameFn { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? ActionId { get; set; }
        [Column("archieveDate")]
        public int? ArchieveDate { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int? DirectedFromUserId { get; set; }
        public long TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public bool? IsConfidential { get; set; }
        [Column("IsCC")]
        public bool? IsCc { get; set; }
        public int? RecipientStatusId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        [Column("TARCreationOn")]
        public DateTimeOffset? TarcreationOn { get; set; }
        public DateTimeOffset? RecipientStatusChangedOn { get; set; }
    }
}
