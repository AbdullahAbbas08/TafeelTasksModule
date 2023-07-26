using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ActionId), Name = "IX_ActionRequiredActions_ActionId")]
    [Index(nameof(CreatedBy), Name = "IX_ActionRequiredActions_CreatedBy")]
    [Index(nameof(RequiredActionId), Name = "IX_ActionRequiredActions_RequiredActionId")]
    [Index(nameof(UpdatedBy), Name = "IX_ActionRequiredActions_UpdatedBy")]
    public partial class ActionRequiredAction
    {
        [Key]
        public int ActionRequiredActionId { get; set; }
        public int ActionId { get; set; }
        public int RequiredActionId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.ActionRequiredActionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(RequiredActionId))]
        [InverseProperty("ActionRequiredActions")]
        public virtual RequiredAction RequiredAction { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.ActionRequiredActionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
