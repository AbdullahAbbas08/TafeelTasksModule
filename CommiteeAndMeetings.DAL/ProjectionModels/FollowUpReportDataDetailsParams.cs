using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class FollowUpReportDataDetailsParams
    {
        public int? fromOuterOrganizationId { get; set; }
        public List<int> selectedOrganizations { get; set; }
        public bool selectAll { get; set; }
        public int followUpOrganizationId { get; set; }
        public DateTimeOffset fromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }
}
