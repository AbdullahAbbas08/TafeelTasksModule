namespace Models.Enums
{
    public enum ProductivityReportEnum
    {
        ReceivedTransactionsReport = 1,
        RefusedTransactionsReport = 2,
        ReplyTransactionsReport = 7,
        SavedTransactionsReport = 3,
        WaitingTransactionsReport = 8,
        DelegatedAsToTransactionsReport = 18,
        DelegatedAsCCTransactionsReport = 19,
        DelegatedAsDecisionTransactionsReport = 9,
        DelegatedAsExternalDecisionTransactionsReport = 10,
        SeenDecisionTransactionsReport = 6,
        SeenExternalDecisionTransactionsReport = 4,
        SeenCCTransactionsReport = 5,
        DraftsReport = 21,
        RelatedTransactionsReport = 12,
        IndividualsReport = 13,
        AttachmentsCountReport = 11,
        AttachedPagesCountReport = 14,
        TransactionsCountInDraftsReport = 20,
        ExternallyDelegatedTransactionsCountReport = 15,
        ExternallyDelegatedAttachmentsCountReport = 16,
        ExternallyDelegatedAttachedPagesCountReport = 17,
    }
}
