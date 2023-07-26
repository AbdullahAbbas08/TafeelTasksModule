using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("WorkFlowTransition")]
    [Index(nameof(CreatedBy), Name = "IX_WorkFlowTransition_CreatedBy")]
    [Index(nameof(OrganizationId), Name = "IX_WorkFlowTransition_OrganizationId")]
    [Index(nameof(UpdatedBy), Name = "IX_WorkFlowTransition_UpdatedBy")]
    [Index(nameof(WorkFlowProcessActionId), Name = "IX_WorkFlowTransition_WorkFlowProcessActionId")]
    [Index(nameof(WorkFlowProcessId), Name = "IX_WorkFlowTransition_WorkFlowProcessId")]
    public partial class WorkFlowTransition
    {
        [Key]
        public int WorkFlowTransitionId { get; set; }
        public int? WorkFlowProcessId { get; set; }
        public int OrganizationId { get; set; }
        public int WorkFlowProcessActionId { get; set; }
        public int WorkFlowTransitionOrder { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.WorkFlowTransitionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("WorkFlowTransitions")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.WorkFlowTransitionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(WorkFlowProcessId))]
        [InverseProperty("WorkFlowTransitions")]
        public virtual WorkFlowProcess WorkFlowProcess { get; set; }
        [ForeignKey(nameof(WorkFlowProcessActionId))]
        [InverseProperty("WorkFlowTransitions")]
        public virtual WorkFlowProcessAction WorkFlowProcessAction { get; set; }
    }
}
