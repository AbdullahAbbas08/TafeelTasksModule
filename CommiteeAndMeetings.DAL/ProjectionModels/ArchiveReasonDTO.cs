namespace Models.ProjectionModels
{
    public class ArchiveReasonDTO
    {
        public int ArchiveReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonNameAr { get; set; }
        public string ReasonNameEn { get; set; }
        public string ReasonNameFn { get; set; }

        public bool IsDefault { get; set; }
        public bool? IsForVIP { get; set; }
    }
}
