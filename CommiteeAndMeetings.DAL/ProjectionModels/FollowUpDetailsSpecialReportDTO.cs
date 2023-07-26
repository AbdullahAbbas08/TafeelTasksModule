using System;

namespace Models.ProjectionModels
{
    public class FollowUpDetailsSpecialReportDTO
    {
        public int Id { get; set; }
        public string incomingOrganizationNameAr { get; set; }
        public string incomingOrganizationNameEn { get; set; }
        public string incomingOrganizationNameFn { get; set; }

        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }

        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset TransactionCreatedOn { get; set; }
        public int NumberOfDays { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset FinishedDate { get; set; }
        public string statments { get; set; }
        public long totalCount { get; set; }
        public string CreatedOrganizationNameAr { get; set; }
        public string CreatedOrganizationNameEn { get; set; }
        public string CreatedOrganizationNameFn { get; set; }
        //public string ConfidentialityLevelNameAr { get; set; }
        //public string ConfidentialityLevelNameEn { get; set; }
        //public string TransactionTypeNameAr { get; set; }
        //public string TransactionTypeNameEn { get; set; }
        //public string ImportanceLevelNameAr { get; set; }
        //public string ImportanceLevelNameEn { get; set; }
        //public string ClassificationNameAr { get; set; }
        //public string ClassificationNameEn { get; set; }
        public string type { get; set; }
        public long? TransactionId { get; set; }
        public int? RelatedTransactionsCount { get; set; }
    }
}
