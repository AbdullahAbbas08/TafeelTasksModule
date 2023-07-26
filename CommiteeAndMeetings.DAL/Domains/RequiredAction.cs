using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_RequiredActions_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_RequiredActions_UpdatedBy")]
    public partial class RequiredAction
    {
        public RequiredAction()
        {
            ActionDefaultCcrequiredActions = new HashSet<Action>();
            ActionDefaultToRequiredActions = new HashSet<Action>();
            ActionRequiredActions = new HashSet<ActionRequiredAction>();
            DelegationReceipients = new HashSet<DelegationReceipient>();
            TransactionActionRecipients = new HashSet<TransactionActionRecipient>();
        }

        [Key]
        public int RequiredActionId { get; set; }
        [StringLength(50)]
        public string RequiredActionNameAr { get; set; }
        [StringLength(50)]
        public string RequiredActionNameEn { get; set; }
        [Column("AllowedInCC")]
        public bool AllowedInCc { get; set; }
        public bool AllowedInTo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool AllowedInVip { get; set; }
        public string Code { get; set; }
        public bool? IsForPreparation { get; set; }
        public string RequiredActionNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.RequiredActionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.RequiredActionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Action.DefaultCcrequiredAction))]
        public virtual ICollection<Action> ActionDefaultCcrequiredActions { get; set; }
        [InverseProperty(nameof(Action.DefaultToRequiredAction))]
        public virtual ICollection<Action> ActionDefaultToRequiredActions { get; set; }
        [InverseProperty(nameof(ActionRequiredAction.RequiredAction))]
        public virtual ICollection<ActionRequiredAction> ActionRequiredActions { get; set; }
        [InverseProperty(nameof(DelegationReceipient.RequiredAction))]
        public virtual ICollection<DelegationReceipient> DelegationReceipients { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.RequiredAction))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipients { get; set; }
    }
}
