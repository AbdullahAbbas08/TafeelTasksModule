namespace Models.Enums
{
    public enum InBoxTransactionWorkFlowEnum
    {
        All = 1,
        //Delete Filter 
        //DelegatedFromMyOrganization=2,
        Unreceived = 3,
        Received = 4,
        ConfidentialTransactions = 5,
        CC = 6,
        DelayedTransactions = 7,
        DescisionsAndCirculars = 8,
        // # 9 is for mobile mixing 3,4,5
        // # 10 is for Count UnSeen  غير مطلع عليها
        Preparation = 11,
        Confirmation = 12
    }
}
