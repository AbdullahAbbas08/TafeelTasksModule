using System;

namespace Models.ProjectionModels
{
    public class TransactionDTO
    {
        public long TransactionId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }

        public string Subject { get; set; }
        public string NormalizedSubject { get; set; }

        public string Notes { get; set; }

        public int TransactionTypeId { get; set; }

        public int? TransactionBasisTypeId { get; set; }
        public int? ClassificationId { get; set; }
        public int? ImportanceLevelId { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public int? IncomingTypeId { get; set; }
        public string IncomingLetterNumber { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public string IncomingCorrespondentName { get; set; }
        public string IncomingCorrespondentMobileNumber { get; set; }
        public string IncomingCorrespondentEmail { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public DateTimeOffset? IncomingOutgoingLetterDate { get; set; }
        public int ExecutionPeriod { get; set; }
        public bool IsForAll { get; set; }
        public string TransactionTypeName { get; set; }
        public string TransactionBasisTypeName { get; set; }
        public string ClassificationName { get; set; }
        public string ImportanceLevelName { get; set; }
        public string ConfidentialityLevelName { get; set; }
        public string IncomingTypeName { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
