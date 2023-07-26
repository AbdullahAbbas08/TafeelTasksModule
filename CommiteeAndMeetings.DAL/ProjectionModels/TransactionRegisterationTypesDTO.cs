namespace Models.ProjectionModels
{
    public class TransactionRegisterationTypesDTO
    {
        public int TransactionRegisterationTypeId { get; set; }
        public string TransactionRegisterationTypeAr { get; set; }
        public string TransactionRegisterationTypeEn { get; set; }
        public string TransactionRegisterationTypeFn { get; set; }

        ////custom
        //public virtual ICollection<ActionReferralModeDTO> ActionReferralModes { get; set; }
    }
}
