using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ActionId), Name = "IX_Delegations_ActionId")]
    [Index(nameof(CreatedBy), Name = "IX_Delegations_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_Delegations_UpdatedBy")]
    public partial class Delegation
    {
        public Delegation()
        {
            DelegationReceipients = new HashSet<DelegationReceipient>();
        }

        [Key]
        public int DelegationId { get; set; }
        public string Title { get; set; }
        public int ActionId { get; set; }
        [Column("followUpOrganizationId")]
        public int? FollowUpOrganizationId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(ActionId))]
        [InverseProperty("Delegations")]
        public virtual Action Action { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.DelegationCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.DelegationUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(DelegationReceipient.Delegation))]
        public virtual ICollection<DelegationReceipient> DelegationReceipients { get; set; }
    }
}
