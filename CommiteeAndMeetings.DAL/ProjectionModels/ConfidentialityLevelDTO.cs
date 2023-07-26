namespace DbContexts.MasarContext.ProjectionModels
{
    public class ConfidentialityLevelDTO
    {
        public int ConfidentialityLevelId { get; set; }
        public string ConfidentialityLevelNameAr { get; set; }
        public string ConfidentialityLevelNameEn { get; set; }
        public string ConfidentialityLevelNameFn { get; set; }

        public bool IsConfidential { get; set; }
    }
}
