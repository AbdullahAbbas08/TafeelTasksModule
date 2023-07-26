namespace Models.ProjectionModels
{
    public class HeaderAndFooterDTO
    {
        public int HeaderAndFooterId { get; set; }
        public int TypeId { get; set; }
        public string ReportName { get; set; }
        public string ReportNameAr { get; set; }
        public string ReportNameEn { get; set; }
        public string ReportNameFn { get; set; }

        public string HTML { get; set; }
    }
}
