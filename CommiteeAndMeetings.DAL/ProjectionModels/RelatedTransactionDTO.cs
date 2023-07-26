using Models;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class RelatedTransactionDTO
    {
        public int RelatedTransactionId { get; set; }
        public long ParentTransactionId { get; set; }
        public long? ChildTransactionId { get; set; }
        public long? ChildOldTransactionId { get; set; }
        public int? TransactionRelationshipId { get; set; }
        public int? TransactionSourceId { get; set; }
        //public IEnumerable<Transaction> Parenttransactions { get; set; }

        public long TransactionId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string TransactionDate { get; set; }
        public string Subject { get; set; }
        public string NormalizedSubject { get; set; }
        public string Notes { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeNameAr { get; set; }
        public string TransactionTypeNameEn { get; set; }
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
        public bool IsForAll { get; set; }

        public string ClassificationName { get; set; }
        public string ImportanceLevelName { get; set; }
        public string IncomingTypeName { get; set; }
        public string TransactionBasisTypeName { get; set; }
        public string ConfidentialityLevelName { get; set; }
        public string IncomingOrganizationName { get; set; }
        public string TransactionRelationshipName { get; set; }
        public string TransactionSourceName { get; set; }
        public string TransactionTypeName { get; set; }
        public bool? IsShowAttachment { get; set; }
        public bool isParent { get; set; }
        public string encryptId
        {
            get
            {
                var id = TransactionSourceId == 2 ? ChildOldTransactionId : isParent ? ParentTransactionId : ChildTransactionId;
                return EncryptHelper.Encrypt(id.ToString());
            }
        }

        public List<AttachmentViewDTO> transactionActionRecipientAttachment { get; set; }
    }
}
