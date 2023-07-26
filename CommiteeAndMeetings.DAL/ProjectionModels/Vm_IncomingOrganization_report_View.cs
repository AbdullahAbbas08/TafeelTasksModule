using System;

namespace Models.ProjectionModels
{
    public class Vm_IncomingOrganization_report_View
    {
        public long Id { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public string IncomingLetterNumber { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public DateTimeOffset TCreatedOn { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public long TransactionId { get; set; }
        public int? RelatedTransactionsCount { get; set; }

    }
}
