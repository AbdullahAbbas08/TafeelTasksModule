using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class DashBoardReportDTO
    {
        public DashboardStatisticsReportDTO dashboardStatisticsReport { get; set; }
        public List<InboxClassificationCountDTO> inboxClassificationCountList { get; set; }
        public IEnumerable<GetTopEmployeesReportDTO> topEmployees { get; set; }


    }
}
