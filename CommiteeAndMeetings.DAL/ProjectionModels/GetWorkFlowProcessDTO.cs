using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class GetWorkFlowProcessDTO
    {
        public WorkFlowProcessDetailsDTO WorkFlowProcess { get; set; }
        public List<WorkFlowTransitionDetailsDTO> WorkFlowTransitions { get; set; }
        public List<WorkFlowFilterDTO> WorkFlowFilters { get; set; }
    }
}
