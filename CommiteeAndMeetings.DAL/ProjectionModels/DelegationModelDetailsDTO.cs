using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class DelegationModelDetailsDTO
    {
        public int DelegationId { get; set; }
        public string Title { get; set; }
        public int ActionId { get; set; }
        public int? followUpOrganizationId { get; set; }
        public string followUpOrganizationName { get; set; }
        public List<DelegationModelReceipientDetailsDTO> DelegationRecepients { get; set; }
    }
}
