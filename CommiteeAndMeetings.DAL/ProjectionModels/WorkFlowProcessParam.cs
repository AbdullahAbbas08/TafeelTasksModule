using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class WorkFlowProcessParam
    {
        public WorkFlowProcessDTO WorkFlowProcess { get; set; }
        public List<WorkFlowFilterDTO> WorkFlowFilters { get; set; }
        public List<WorkFlowTransitionDTO> WorkFlowTransitions { get; set; }

    }
}
