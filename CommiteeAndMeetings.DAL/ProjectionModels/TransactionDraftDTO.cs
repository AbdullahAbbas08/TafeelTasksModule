using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionDraftDTO
    {
        public long Id { get; set; }

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
        public int ExecutionPeriod { get; set; }
        public DateTimeOffset? IncomingOutgoingLetterDate { get; set; }
        public bool IsForAll { get; set; }
        public int? createdBy { get; set; }
        public int? updatedBy { get; set; }
        public int? deletedBy { get; set; }
        public DateTimeOffset? createdOn { get; set; }
        public DateTimeOffset? updatedOn { get; set; }
        public DateTimeOffset? deletedOn { get; set; } //32


        //TransactionType
        public string TransactionTypeCode { get; set; }

        public string TransactionTypeName { get; set; }
        public string TransactionTypeNameAr { get; set; }

        public string TransactionTypeNameEn { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsInternal { get; set; }

        public bool IsExternal { get; set; }

        public bool IsDecision { get; set; }

        public string TransactionTypeCodeForSerial { get; set; } //40
        public int DocumentCount { get; set; }
        public int LetterCount { get; set; }
        public int PhysicalCount { get; set; }
        public string TransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionId.ToString()); } }

        public List<AttachmentViewDTO> transactionAttachments { get; set; }

    }
}
