namespace Models.Enums
{
    public enum OutboxTransactionEnum
    {
        All = 1,
        OutToInternalOrganization = 2,
        DelegationToEmployees = 3,
        ExportingDelegation = 4,
        exported = 5,
        DescisionsAndCirculars = 7

    }
}
