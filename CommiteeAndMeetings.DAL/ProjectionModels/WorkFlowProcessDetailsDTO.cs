namespace Models.ProjectionModels
{
    public class WorkFlowProcessDetailsDTO
    {
        public int WorkFlowProcessId { get; set; } = 0;
        public int? FromOrganizationId { get; set; }
        public string FromOrganizationName { get; set; }
        public int WorkFlowFilterId { get; set; }
        public string WorkFlowFilterName { get; set; }
        public int ToOrganizationId { get; set; }
        public string ToOrganizationName { get; set; }
        public string DiagramXML { get; set; }


    }
}
