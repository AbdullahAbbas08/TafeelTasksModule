namespace Models.ProjectionModels
{
    public class TransactionRelationshipDTO
    {
        public int TransactionRelationshipId { get; set; }
        public string TransactionRelationshipNameAr { get; set; }
        public string TransactionRelationshipNameEn { get; set; }
        public string TransactionRelationshipNameFn { get; set; }

        public int DisplayOrder { get; set; }
        public bool SavedRelatedTransaction { get; set; }
        public bool SentRelatedTransaction { get; set; }
    }
}
