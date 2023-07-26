using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionBoxDTO
    {
        public int count { get; set; }
        public bool IsConfidential { get; set; }
        public long TransactionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }
        public string SubjectEn { get; set; }
        public string SubjectFn { get; set; }

        public bool secretSubject { get; set; }
        public int? DirectedFromId { get; set; }
        public string DirectedFromName { get; set; }
        public int? archieveDate { get; set; }
        //        public string DirectedFromNameEn { get; set; }
        public int? DirectedToId { get; set; }
        public string DirectedToName { get; set; }
        public int? RequiredActionId { get; set; }
        public string RequiredActionName { get; set; }
        public bool IsCC { get; set; }
        public int? CorrespondentUserId { get; set; }
        public string CorrespondentUserName { get; set; }
        public string CorrespondentUserNameEng { get; set; }
        public int? RecipientStatusId { get; set; }
        public string RecipientStatusName { get; set; }
        public int? RecipientStatusChangedBy { get; set; }
        public DateTimeOffset? RecipientStatusChangedOn { get; set; }
        public bool IsUrgent { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string Notes { get; set; }
        public bool SendNotification { get; set; }
        public DateTimeOffset? createdOn { get; set; }

        public string fromUserProfileImage { get; set; }
        public int LetterCount { get; set; }
        public int DocumentCount { get; set; }
        public int PhysicalCount { get; set; }
        public int relatedTransactions { get; set; }
        public int? ImportanceLevelId { get; set; }
        public string ImportanceLevelName { get; set; }
        public int Period { get; set; }
        public int? transactionTypeId { get; set; }
        public string transactionTypeName { get; set; }
        public string transactionStatus { get; set; }
        public int? ConfidentialId { get; set; }
        public string ConfidentialName { get; set; }
        public bool IsLate { get; set; }
        public bool iswared { get; set; }
        public int IndivnidualsCounts { get; set; }
        public string ImportanceLevelColor { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public string attachmentsNumber_string { get; set; }
        public bool? isCancelled { get; set; }
        public bool? IsMine { get; set; }
        public string incomingOrganizationName { get; set; }
        public IEnumerable<AttachmentViewDTO> transactionActionRecipientAttachment { get; set; }
        public List<AttachmentViewDTO> transactionActionAttachments { get; set; }
        //public IEnumerable<TransactionsView> TaskCCAndTo { get; set; }
        public IEnumerable<TransactionActionRecipientsDTO> taskRecipientsAndTO { get; set; }
        //public IEnumerable<TransactionsView> taskSenders { get; set; }
        // public List<RecipientsNames> RecipientsNames { get; set; }
        public bool isWithDrawelRequest { get; set; }
        public int? actionId { get; set; }
        public string physicalAttachment { get; set; }
        public bool IsFinished { get; set; }
        public DateTimeOffset? FollowUPFinishedDate { get; set; }
        public int? hasCorrespondence { get; set; }
        public bool IsDirectedToUser { get; set; }

        public bool IsPrinted { get; set; }
        public int? DeliverySheetId { get; set; }
        public int? ArchiveReasonId { get; set; }
        public string ArchiveReasonNameAr { get; set; }
        public string ArchiveReasonNameEn { get; set; }

        public string TransactionIdEncrypt { get; set; }
        public string TransactionActionRecipientIdEncrypt { get; set; }
        public string TransactionActionIdEncrypt { get; set; }
        public bool IsShowAllTime { get; set; }

        public PreprationDetailsDTO PreprationDetails { get; set; }
        public int TotalCount { get; set; } = 0;
        public string CreatedUserName { get; set; }
    }
}
