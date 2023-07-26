using System;

namespace Models.ProjectionModels
{
    public class FollowUpDetailsSpecialReportParam
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string fromIncomingOrganizationIds { get; set; }
        public string toOrganizationIds { get; set; }
        public DateTime fromCreatedOn { get; set; }
        public DateTime toCreatedOn { get; set; }
        public int filterId { get; set; }
        public int? selectedOrganizationId { get; set; }
        public bool isInternal { get; set; }
    }
}
