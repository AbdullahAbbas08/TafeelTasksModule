namespace Models
{
    public class ClassificationDetailsDTO
    {
        public int ClassificationId { get; set; }
        public string ClassificationNameAr { get; set; }
        public string ClassificationNameEn { get; set; }
        public string ClassificationNameFn { get; set; }

        public string ReferenceNumberNameAr { get; set; }
        public string ReferenceNumberNameEn { get; set; }
        public string ReferenceNumberNameFn { get; set; }

        public bool IsShared { get; set; }

        //Custom 
        public string ClassificationName { get; set; }
        public bool IsExist { get; set; }
        public bool? IsDefault { get; set; }
        public bool IsReferenceRequired { get; set; }
        public string Color { get; set; }
        public int? ParentClassificationId { get; set; }
        public string ParentClassificationName { get; set; }


    }
}