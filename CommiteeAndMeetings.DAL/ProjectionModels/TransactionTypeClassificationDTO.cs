namespace Models.ProjectionModels
{
    public class TransactionTypeClassificationDTO
    {
        public int TransactionTypeId { get; set; }
        public int ClassificationId { get; set; }
        public bool? IsFavorite { get; set; }
        public int TransactionTypeClassificationId { get; set; }
    }
}
