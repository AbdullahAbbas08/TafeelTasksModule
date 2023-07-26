using System;

namespace Models.ProjectionModels
{
    public class FollowUpReportDataParams
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int? fromOrganizationId { get; set; }
        public int? toOrganizationId { get; set; }
        public DateTimeOffset fromCreatedOn { get; set; }
        public DateTimeOffset toCreatedOn { get; set; }
        public int followUpStatus { get; set; }
        public int? selectedOrganizationId { get; set; }
        public int? selectedEmpoloyeeId { get; set; }
    }
}
