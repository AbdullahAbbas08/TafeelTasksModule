using System;

namespace Models.ProjectionModels
{
    public class ProductivityReportParams
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public int OrgnizationId { get; set; }
        public bool isOuter { get; set; }
        public string UserIdList { get; set; }
        public int filterId { get; set; }
    }
}
