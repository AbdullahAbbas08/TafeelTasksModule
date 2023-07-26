using System;

namespace Models.ProjectionModels
{
    public class EmployeAchievementReportParameters
    {
        public bool isEmployee { get; set; }
        public string type { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public int[] ids { get; set; }
        public int organizationId { get; set; }


    }
}
