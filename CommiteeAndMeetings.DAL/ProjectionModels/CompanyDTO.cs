namespace Models.ProjectionModels
{
    public class CompanyDTO
    {
        public int CompanyId { get; set; }
        public string CompanyNameEn { get; set; }
        public string CompanyNameAr { get; set; }
        public string LogoPath { get; set; }
        public bool IsDefault { get; set; }
    }
}
