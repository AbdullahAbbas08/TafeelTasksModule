using System;

namespace Models.ProjectionModels
{
    public class EmployeeAchievementToReportParam
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public bool count { get; set; } = true;
        public int? OrganizationId { get; set; }
        public int? UserId { get; set; }
        public int? ClassificationId { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public bool? IsCC { get; set; }
        public bool? IsDesicionsAndCirculars { get; set; }
    }
}
