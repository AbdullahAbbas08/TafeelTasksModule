namespace Models.ProjectionModels
{
    public class ActionReferralModeDetails
    {
        public int ActionId { get; set; }
        public int TransactionTypeId { get; set; }
        public string ActionNameEn { get; set; }
        public string ActionNameAr { get; set; }
        public string ActionName { get; set; }
        public int? DefaultToRequiredActionId { get; set; }
        public int? DefaultCCRequiredActionId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
