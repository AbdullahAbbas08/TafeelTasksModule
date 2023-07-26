namespace Models.Enums
{
    public enum OutboxTransactionWorkFlowEnum
    {
        All = 1,
        OutToInternalOrganization = 2,
        DelegationToEmployees = 3,
        ExportingDelegation = 4,
        exported = 5,
        Confirmation = 6,
        DescisionsAndCirculars = 7

    }
}
