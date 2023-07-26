using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class IncomingOrganization_reportParams
    {
        public List<int> FromOrgIds { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public int Page { get; set; }
        public int FromOrg { get; set; }
        public int PageSize { get; set; }
    }
}
