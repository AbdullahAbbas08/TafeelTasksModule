namespace Models
{
    public class ClassificationSummaryDTO
    {
        public int ClassificationId { get; set; }
        public string ClassificationNameAr { get; set; }
        public string ClassificationNameEn { get; set; }
        public string ClassificationNameFn { get; set; }

        public bool IsShared { get; set; }
        public bool? IsDefault { get; set; }
        public int? ParentClassificationId { get; set; }

    }
}