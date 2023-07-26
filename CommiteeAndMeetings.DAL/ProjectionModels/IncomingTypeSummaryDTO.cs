namespace Models
{
    public class IncomingTypeSummaryDTO
    {
        public int IncomingTypeId { get; set; }
        public string IncomingTypeNameAr { get; set; }
        public string IncomingTypeNameEn { get; set; }
        public string IncomingTypeNameFn { get; set; }

        public bool? IsDefault { get; set; }
        public bool HideFromRegistration { get; set; }

    }
}