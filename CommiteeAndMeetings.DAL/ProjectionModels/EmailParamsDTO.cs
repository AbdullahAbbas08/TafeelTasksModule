namespace Models.ProjectionModels
{
    public class EmailParamsDTO
    {
        public string transactionActionRecipientToBeReplaceEn;

        public string lblTransactionNumber { get; set; }
        //موضوع المعاملة
        public string lblTransactionSubject { get; set; }
        //نوع المعاملة
        public string lblTransactionType { get; set; }
        //تاريخ المعاملة 
        public string lblTransactionDate { get; set; }
        //الاداره المحيلة
        public string lblDelegationFromOrg { get; set; }
        public string lblImportance_level { get; set; }
        // مستوي السرية
        public string lblConfidentiality_level { get; set; }
        // رابط المعاملة
        public string lblTransURL { get; set; }
        public string lblNotes { get; set; }
        public string lblNotesEn { get; set; }
        public string NotesToBeReplace { get; set; }
        public string NotesToBeReplaceEn { get; set; }
        public string lblIncomingOutgoingOrganization { get; set; }
        public string lblIncomingOutgoingOrganizationEn { get; set; }


        public string lblTransactionNumberEn { get; set; }
        //موضوع المعاملة بالانجليزية
        public string lblTransactionSubjectEn { get; set; }
        //نوع المعاملة بالانجليزية
        public string lblTransactionTypeEn { get; set; }
        // بالانجليزيةتاريخ المعاملة 
        public string lblTransactionDateEn { get; set; }
        //الاداره المحيلة بالانجليزية
        public string lblDelegationFromOrgEn { get; set; }
        public string lblImportance_levelEn { get; set; }
        // مستوي السرية بالانجليزية
        public string lblConfidentiality_levelEn { get; set; }
        // رابط المعاملة بالانجليزية
        public string lblTransURLEn { get; set; }
        public string ThanksLocalization { get; set; }
        // Set the Value To Be replaced with TransactionActionRecipeintId
        public string transactionActionRecipientToBeReplace { get; set; }

        // Set the Value To Be replaced with RequredActionId
        public string RequiredActionToBeReplace { get; set; }

        // الاجراء المقترح 
        public string lblRequiredAction { get; set; }
        //الاجراء المقترح بالانجليزية
        public string lblRequiredActionEn { get; set; }
        public string RequiredActionToBeReplaceEn { get; set; }
        public string lblMailHeader { get; set; }
        public bool IsExternalDelegation { get; set; }
        public string OrgNamesAr { get; set; }
        public string OrgNamesEn { get; set; }
        public string lblExternalOrgsDelegate { get; set; }
        public string lblExternalOrgsDelegateEn { get; set; }
        public string ThanksLocalization2 { get; set; }
    }
}
