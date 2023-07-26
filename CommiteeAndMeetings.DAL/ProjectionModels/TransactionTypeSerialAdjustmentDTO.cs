namespace Models.ProjectionModels
{
    public class TransactionTypeSerialAdjustmentDTO
    {
        public int TransactionTypeSerialAdjustmentId { get; set; }
        public int TransactionTypeId { get; set; }
        public int AdjustPeriod { get; set; }
        public int Year { get; set; }
    }
}
