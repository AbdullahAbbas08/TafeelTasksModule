using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class DelegationModelDTO
    {
        public int DelegationId { get; set; }
        public string Title { get; set; }
        public int ActionId { get; set; }
        public int? followUpOrganizationId { get; set; }
        public List<DelegationModelReceipientSummaryDTO> DelegationRecepients { get; set; }
    }
}
