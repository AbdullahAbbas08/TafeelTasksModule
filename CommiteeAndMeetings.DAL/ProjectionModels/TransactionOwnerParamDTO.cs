namespace Models.ProjectionModels
{
    public class TransactionOwnerParamDTO
    {
        public string TransactionNumberFormated { get; set; }
        public string TransactionSubject { get; set; }
        public int TransactionTypeId { get; set; }
        public int Year { get; set; }
        public int? IncommingOragnazationId { get; set; }

    }
}