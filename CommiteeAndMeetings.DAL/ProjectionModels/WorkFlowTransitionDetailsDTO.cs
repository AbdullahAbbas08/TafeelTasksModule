namespace Models.ProjectionModels
{
    public class WorkFlowTransitionDetailsDTO
    {
        public int WorkFlowTransitionId { get; set; }
        public int? WorkFlowProcessId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int WorkFlowProcessActionId { get; set; }
        public string WorkFlowProcessActionName { get; set; }
        public int WorkFlowProcessActionstep { get; set; }
        public int WorkFlowTransitionOrder { get; set; }



    }
}
