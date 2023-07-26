using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmFollowUpView
    {
        public int Id { get; set; }
        public int FollowUpId { get; set; }
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CreatedBy { get; set; }
        public int FollowUpStatusTypeId { get; set; }
        public DateTimeOffset FollowUpStatusCreatedOn { get; set; }
        public DateTimeOffset? ModifiedDateTo { get; set; }
        public long? TransactionId { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public string Notes { get; set; }
        public int? ImportanceLevelId { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int? DirectedFromUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
    }
}
