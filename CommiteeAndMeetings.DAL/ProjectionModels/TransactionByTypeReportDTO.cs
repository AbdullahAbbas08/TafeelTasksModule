namespace Models.ProjectionModels
{
    public class TransactionByTypeReportDTO
    {
        public int Id { get; set; }
        public string TransactionTypeAr { get; set; }
        public string TransactionTypeEn { get; set; }
        public string TransactionTypeFn { get; set; }
        public int Count { get; set; }
    }
}
