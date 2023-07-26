using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionForSavedForReport
    {
        public long TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset? SavedDate { get; set; }
        public string ReasonSave { get; set; }
        public int? IncomingOrganizationId { get; set; }
        [Required]
        public string IncomingOrganizationNameAr { get; set; }
        [Required]
        public string IncomingOrganizationNameEn { get; set; }
        [Required]
        public string IncomingOrganizationNameFn { get; set; }
        public int? RecipientStatusChangedBy { get; set; }
        [StringLength(100)]
        public string UserChangedByNameAr { get; set; }
        [StringLength(100)]
        public string UserChangedByNameEn { get; set; }
        public string UserChangedByNameFn { get; set; }
        public int? OrganizationSavedId { get; set; }
        public string OrganizationSavedNameAr { get; set; }
        public string OrganizationSavedNameEn { get; set; }
        public string OrganizationSavedNameFn { get; set; }
        public DateTimeOffset? FilterDate { get; set; }
        public int RelatedTransactionsCount { get; set; }
    }
}
