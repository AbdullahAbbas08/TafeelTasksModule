using System;

namespace Models.ProjectionModels
{
    public class FollowUpDetailsSpecialReportStatisticsParam
    {
        public int followUpOrganizationId { get; set; }
        public DateTimeOffset fromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public string fromIncomingOrganizationIds { get; set; }
    }
}
