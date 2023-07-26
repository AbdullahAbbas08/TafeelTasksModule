namespace Models.ProjectionModels
{
    public class ActionReferralModeDTO
    {
        public int ActionReferralModeId { get; set; }
        public int ActionId { get; set; }
        public int TransactionTypeId { get; set; }

        //custom
        public string ActionName { get; set; }
        public string TransactionTypeName { get; set; }
    }
}
