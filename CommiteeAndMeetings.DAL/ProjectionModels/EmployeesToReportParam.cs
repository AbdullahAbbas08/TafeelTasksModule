namespace Models.ProjectionModels
{
    public class EmployeesToReportParam
    {
        public string OrganizationIds { get; set; }
        public string Search { get; set; }
        public int pageSize { get; set; } = 20;

    }
}
