namespace Models
{
    public class TransactionBasisTypeSummaryDTO
    {
        public int TransactionBasisTypeId { get; set; }
        public string TransactionBasisTypeNameAr { get; set; }
        public string TransactionBasisTypeNameEn { get; set; }
        public string TransactionBasisTypeNameFn { get; set; }

        public int DisplayOrder { get; set; }
        public bool? IsDefault { get; set; }
    }
}