namespace Models
{
    public class ImportanceLevelDetailsDTO
    {
        public int ImportanceLevelId { get; set; }
        public string ImportanceLevelNameAr { get; set; }
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelNameFn { get; set; }

        public int ImportanceAcheivementPeriod { get; set; }
        public int ImportanceFollowUpPeriod { get; set; }
        public string ImportanceLevelColor { get; set; }
        public bool? IsDefault { get; set; }
    }
}