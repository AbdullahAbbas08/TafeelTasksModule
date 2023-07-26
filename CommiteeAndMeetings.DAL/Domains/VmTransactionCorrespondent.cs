using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmTransactionCorrespondent
    {
        public int Id { get; set; }
        public int? CorrespondentUser { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }
        public long TransactionId { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public string DirectedToOrganizationNameAr { get; set; }
        public string DirectedToOrganizationNameEn { get; set; }
        public string DirectedFromOrganizationNameAr { get; set; }
        public string DirectedFromOrganizationNameEn { get; set; }
        public bool IsOuterOrganization { get; set; }
        [Column("DeliveryCorrespondentTransactionID")]
        public int? DeliveryCorrespondentTransactionId { get; set; }
        public int? CorrespondentUserId { get; set; }
        public string NormalizedSubject { get; set; }
        [Column("hasDeliveryCorresponden")]
        public int HasDeliveryCorresponden { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
    }
}
