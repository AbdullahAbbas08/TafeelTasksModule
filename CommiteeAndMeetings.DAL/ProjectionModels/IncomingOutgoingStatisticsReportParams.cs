using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class IncomingOutgoingStatisticsReportParams
    {
        public List<int> FromOrgIdList { get; set; }
        public List<int> ToOrgIdList { get; set; }
        public bool IsExternal { get; set; }
        public bool IsIncoming { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }
}
