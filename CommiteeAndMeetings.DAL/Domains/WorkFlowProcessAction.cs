using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("WorkFlowProcessAction")]
    public partial class WorkFlowProcessAction
    {
        public WorkFlowProcessAction()
        {
            WorkFlowTransitions = new HashSet<WorkFlowTransition>();
        }

        [Key]
        public int WorkFlowProcessActionId { get; set; }
        public string WorkFlowProcessActionNameAr { get; set; }
        public string WorkFlowProcessActionNameEn { get; set; }
        public int WorkFlowProcessActionStep { get; set; }
        public string Description { get; set; }
        public string WorkFlowProcessActionNameFn { get; set; }

        [InverseProperty(nameof(WorkFlowTransition.WorkFlowProcessAction))]
        public virtual ICollection<WorkFlowTransition> WorkFlowTransitions { get; set; }
    }
}
