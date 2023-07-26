using CommiteeAndMeetings.DAL.Domains;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class WorkFlowCustom_ProcessDTO
    {
        public int WorkFlowProcessId { get; set; }
        public int? FromOrganizationId { get; set; }
        public int ToOrganizationId { get; set; }
        public bool? IsImplementHickal { get; set; }

        /// <summary>
        /// عدد العناصر الغير موجودة 
        /// </summary>
        public int NotMatchsNumber { get; set; } = 0;
        public Organization FromOrganization { get; set; }


        public Organization ToOrganization { get; set; }


        public List<WorkFlowTransition> WorkFlowTransitions { get; set; }
        public List<WorkFlowFilter> WorkFlowFilters { get; set; }
        public List<FilterPairs> WorkFlowFilters_FP { get; set; }
        public int MatchsNumber { get; set; }
    }
}
