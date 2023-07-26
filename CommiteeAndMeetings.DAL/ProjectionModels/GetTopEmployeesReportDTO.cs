namespace Models.ProjectionModels
{
    public class GetTopEmployeesReportDTO
    {
        public int Id { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }

        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public string Image { get; set; }

        public int cnt { get; set; }
    }
}
