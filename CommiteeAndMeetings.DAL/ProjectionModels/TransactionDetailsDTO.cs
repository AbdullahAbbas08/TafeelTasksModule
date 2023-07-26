using Models;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class TransactionDetailsDTO
    {
        private DateTimeOffset? executionDate;
        public long TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public string SubjectEn { get; set; }
        public string SubjectFn { get; set; }

        public string NormalizedSubject { get; set; }
        public string Notes { get; set; }
        public string NotesEn { get; set; }
        public string NotesFn { get; set; }

        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public int? RecipientStatusId { get; set; }
        public int? TransactionBasisTypeId { get; set; }
        public int? ClassificationId { get; set; }
        public int? ImportanceLevelId { get; set; }
        public bool? IsConfidential { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public int? IncomingTypeId { get; set; }
        public string IncomingLetterNumber { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public DateTimeOffset? IncomingOutgoingLetterDate { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public string IncomingCorrespondentName { get; set; }
        public string IncomingCorrespondentMobileNumber { get; set; }
        public string IncomingCorrespondentEmail { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTimeOffset? ExecutionDate
        {
            get
            {
                return executionDate.HasValue ? executionDate.Value.LocalDateTime : DateTimeOffset.Now.LocalDateTime;
            }
            set
            {
                executionDate = value.HasValue ? value.Value.LocalDateTime : DateTimeOffset.Now.LocalDateTime;
            }
        }

        public int ExecutionPeriod { get; set; }
        public bool IsForAll { get; set; }

        public string ClassificationName { get; set; }
        public string ImportanceLevelName { get; set; }
        public string IncomingTypeName { get; set; }
        public string TransactionBasisTypeName { get; set; }
        public string ConfidentialityLevelName { get; set; }
        public string IncomingOrganizationName { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsCC { get; set; }
        public bool? IsCancelled { get; set; }

        public bool IsUrgent { get; set; }
        public int? RequiredActionId { get; set; }
        public string RequiredActionName { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string TransactionCurrentStatus { get; set; }
        public string Explanation { get; set; }
        public DateTimeOffset? LastDelegationDate { get; set; }
        public string AchievementInDays { get; set; }
        public string Action { get; set; }
        public int ActionId { get; set; }
        public string Responsibilities { get; set; }
        public string ResponsibilitiesCC { get; set; }

        public int NumberOfDaysForExecution { get; set; }
        public string RecievingStatus { get; set; }
        public string Correspondents { get; set; }
        public DateTimeOffset? IncomingOutgoingDate { get; set; }
        //public DateTimeOffset? IncomingLetterDate { get; set; }
        public string Sender { get; set; }
        public string RegisterByOrganization { get; set; }
        public string CreatedByEmployee { get; set; }
        public string TransactionBasisStatus { get; set; }
        public string mainPhoto { get; set; }
        public int? followUpId { get; set; }
        public int? directedToOrganizationId { get; set; }
        public int? directedTouserId { get; set; }
        public bool? IsTransaction { get; set; } = true;
        public bool isOldTransaction { get; set; }
        public bool isFromFax { get; set; }
        public int? migrationTransactionId { get; set; }
        public int? transactionRelatedCount { get; set; }
        public int? transactionIndividualsCount { get; set; }
        public int? transactionAssignmentsCount { get; set; }

        //public int IncomingLetterNumber { get; set; }
        //public int ReferenceNumber { get; set; }
        public string transactionNotes { get; set; }
        public int[] tagListIds { get; set; }
        public string TransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionId.ToString()); } }
        public string TransactionActionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionActionId.ToString()); } }
        public string TransactionActionRecipientIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionActionRecipientId.ToString()); } }
        public List<TransactionIndividualDTO> TransactionIndividuals { get; set; }
        public List<AttachmentViewDTO> transactionActionRecipientAttachment { get; set; }
        public List<RecipientsNamesDTO> NamesCC { get; set; }
        public List<RecipientsNamesDTO> NamesTO { get; set; }
        public List<SenderNamesDTO> NameFrom { get; set; }
        public string archiveReason { get; set; }
        public List<TagDTO> tagDTOList { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsShowAllTime { get; set; }
        public bool IsNoteHidden { get; set; }
        public bool HasCorrespondent { get; set; } = false;
    }
}
