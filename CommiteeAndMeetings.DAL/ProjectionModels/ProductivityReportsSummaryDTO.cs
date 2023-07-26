namespace Models.ProjectionModels
{
    public class ProductivityReportsSummaryDTO
    {
        public long Id { get; set; }
        public int UserId { get; set; }

        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }
        public int cnt { get; set; }
    }
}
