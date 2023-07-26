namespace Models
{
    public class TransactionTypeSummaryDTO
    {
        public int TransactionTypeId { get; set; }
        public string TransactionTypeCode { get; set; }
        public string TransactionTypeNameAr { get; set; }
        public string TransactionTypeNameEn { get; set; }
        public string TransactionTypeNameFn { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsInternal { get; set; }
        public bool IsExternal { get; set; }
        public bool IsDecision { get; set; }
        public bool AllowDelegateToMulti { get; set; }
        public bool AddOrgCodeToSerial { get; set; }
    }
}