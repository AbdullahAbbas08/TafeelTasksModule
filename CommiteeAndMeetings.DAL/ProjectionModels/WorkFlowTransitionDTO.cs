namespace Models.ProjectionModels
{
    public class WorkFlowTransitionDTO
    {
        public int OrganizationId { get; set; }
        public int WorkFlowProcessActionId { get; set; }
        public int WorkFlowTransitionOrder { get; set; } = 1;

    }
}
