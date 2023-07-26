using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class EmployeeStatisticsReportParams
    {
        public int userId { get; set; }
        public List<int> orgIdList { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public bool IsCreator { get; set; }
    }
}
