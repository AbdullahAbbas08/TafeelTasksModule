using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class WorkFlowProcessDTO
    {
        public int WorkFlowProcessId { get; set; } = 0;
        public int? FromOrganizationId { get; set; }
        public int ToOrganizationId { get; set; }
        public bool? IsImplementHickal { get; set; } = false;
        public string DiagramXML { get; set; }
        public string FromOrganizationName { get; set; }
        public string ToOrganizationName { get; set; }
        public List<WorkFlowFilterDTO> WorkFlowFilters { get; set; }

    }
}
