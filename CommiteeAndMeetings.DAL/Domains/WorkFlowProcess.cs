using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("WorkFlowProcess")]
    [Index(nameof(CreatedBy), Name = "IX_WorkFlowProcess_CreatedBy")]
    [Index(nameof(FromOrganizationId), Name = "IX_WorkFlowProcess_FromOrganizationId")]
    [Index(nameof(ToOrganizationId), Name = "IX_WorkFlowProcess_ToOrganizationId")]
    [Index(nameof(UpdatedBy), Name = "IX_WorkFlowProcess_UpdatedBy")]
    public partial class WorkFlowProcess
    {
        public WorkFlowProcess()
        {
            TransactionWorkFlowProcesses = new HashSet<TransactionWorkFlowProcess>();
            WorkFlowFilters = new HashSet<WorkFlowFilter>();
            WorkFlowTransitions = new HashSet<WorkFlowTransition>();
        }

        [Key]
        public int WorkFlowProcessId { get; set; }
        public int? FromOrganizationId { get; set; }
        public int ToOrganizationId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [Column("DiagramXML")]
        public string DiagramXml { get; set; }
        public bool? IsImplementHickal { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.WorkFlowProcessCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(FromOrganizationId))]
        [InverseProperty(nameof(Organization.WorkFlowProcessFromOrganizations))]
        public virtual Organization FromOrganization { get; set; }
        [ForeignKey(nameof(ToOrganizationId))]
        [InverseProperty(nameof(Organization.WorkFlowProcessToOrganizations))]
        public virtual Organization ToOrganization { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.WorkFlowProcessUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(TransactionWorkFlowProcess.WorkFlowProcess))]
        public virtual ICollection<TransactionWorkFlowProcess> TransactionWorkFlowProcesses { get; set; }
        [InverseProperty(nameof(WorkFlowFilter.WorkFlowProcess))]
        public virtual ICollection<WorkFlowFilter> WorkFlowFilters { get; set; }
        [InverseProperty(nameof(WorkFlowTransition.WorkFlowProcess))]
        public virtual ICollection<WorkFlowTransition> WorkFlowTransitions { get; set; }
    }
}
