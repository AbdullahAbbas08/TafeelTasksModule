namespace Models
{
    public class ActionSummaryDTO
    {
        public int ActionId { get; set; }
        public string ActionNameAr { get; set; }
        public string ActionNameEn { get; set; }
        public string ActionName { get; set; }
        public string ActionCode { get; set; }
        public int DisplayOrder { get; set; }
        public string ActionNameFn { get; set; }

        public int? DefaultToRequiredActionId { get; set; }
        public int? DefaultCCRequiredActionId { get; set; }
        public bool? AllowMulti { get; set; }
        public bool? AllowToInternalOrganization { get; set; }
        public bool? AllowToExternalOrganization { get; set; }
        public bool? AllowToEmployees { get; set; }
        public bool? AllowCCInternalOrganization { get; set; }
        public bool? AllowCCExternalOrganization { get; set; }
        public bool? AllowCCEmployees { get; set; }
        public bool? AllowInCaseCCTransactionInPreparation { get; set; }

    }
}