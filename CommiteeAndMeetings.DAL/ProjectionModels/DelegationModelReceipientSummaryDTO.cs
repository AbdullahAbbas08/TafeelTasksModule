namespace Models.ProjectionModels
{
    public class DelegationModelReceipientSummaryDTO
    {
        public int DelegationReceipientId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? RequiredActionId { get; set; }
        public bool IsCC { get; set; }
        public int? CorrespondentUserId { get; set; }
        public bool IsUrgent { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string Notes { get; set; }
        public bool SendNotification { get; set; }
        public bool IsHidden { get; set; } = false;
        public int DelegationId { get; set; }

    }
}
