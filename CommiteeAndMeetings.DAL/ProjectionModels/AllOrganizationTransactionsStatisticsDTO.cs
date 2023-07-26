using System;

namespace Models.ProjectionModels
{
    public class AllOrganizationTransactionsStatisticsDTO
    {
        public long Id { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public int AllTransactionCount { get; set; }
        public int FinishedTransactionCount { get; set; }
        public int UnfinishedTransactionCount { get; set; }
        public int TransactionInProgressCount { get; set; }
        public int TransactionsCompletedLateCount { get; set; }
        public int TransactionsCompletedOnTimeCount { get; set; }
        public int LateAndUnfinishedTransactionsCount { get; set; }
    }
    public class OrganizationChildTransactionsStatisticsDTO
    {
        public long Id { get; set; }
        public int OrgId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public int AllTransactionCount { get; set; }
        public int FinishedTransactionCount { get; set; }
        public int TransactionInProgressCount { get; set; }
        public int TransactionsCompletedLateCount { get; set; }
        public int FalteringTransactionsCount { get; set; }
    }
    public class OrganizationChildTransactionsDTO
    {
        public long Id { get; set; }
        public int? FromOrganizationId { get; set; }
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset ExecutionDate { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ConfidentialityLevelNameAr { get; set; }
        public string ConfidentialityLevelNameEn { get; set; }
        public string ConfidentialityLevelNameFn { get; set; }
        public string ImportanceLevelNameAr { get; set; }
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelNameFn { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }


    }
}
