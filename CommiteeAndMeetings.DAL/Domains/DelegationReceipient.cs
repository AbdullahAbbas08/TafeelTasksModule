using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CorrespondentUserId), Name = "IX_DelegationReceipients_CorrespondentUserId")]
    [Index(nameof(DelegationId), Name = "IX_DelegationReceipients_DelegationId")]
    [Index(nameof(DirectedToOrganizationId), Name = "IX_DelegationReceipients_DirectedToOrganizationId")]
    [Index(nameof(DirectedToUserId), Name = "IX_DelegationReceipients_DirectedToUserId")]
    [Index(nameof(RequiredActionId), Name = "IX_DelegationReceipients_RequiredActionId")]
    public partial class DelegationReceipient
    {
        [Key]
        public int DelegationReceipientId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? RequiredActionId { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
        public int? CorrespondentUserId { get; set; }
        public bool IsUrgent { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string Notes { get; set; }
        public bool SendNotification { get; set; }
        public bool IsHidden { get; set; }
        public int DelegationId { get; set; }

        [ForeignKey(nameof(CorrespondentUserId))]
        [InverseProperty(nameof(User.DelegationReceipientCorrespondentUsers))]
        public virtual User CorrespondentUser { get; set; }
        [ForeignKey(nameof(DelegationId))]
        [InverseProperty("DelegationReceipients")]
        public virtual Delegation Delegation { get; set; }
        [ForeignKey(nameof(DirectedToOrganizationId))]
        [InverseProperty(nameof(Organization.DelegationReceipients))]
        public virtual Organization DirectedToOrganization { get; set; }
        [ForeignKey(nameof(DirectedToUserId))]
        [InverseProperty(nameof(User.DelegationReceipientDirectedToUsers))]
        public virtual User DirectedToUser { get; set; }
        [ForeignKey(nameof(RequiredActionId))]
        [InverseProperty("DelegationReceipients")]
        public virtual RequiredAction RequiredAction { get; set; }
    }
}
