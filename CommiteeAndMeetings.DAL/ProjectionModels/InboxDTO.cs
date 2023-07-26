using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class InboxDTO
    {
        public int count { get; set; }
        public long TransactionId { get; set; }
        public bool IsConfidential { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public int? DirectedFromId { get; set; }
        public string DirectedFromName { get; set; }
        //        public string DirectedFromNameEn { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public string DirectedToOrganizationNameAr { get; set; }
        public string DirectedToOrganizationNameEn { get; set; }
        public int? RequiredActionId { get; set; }
        public string RequiredActionName { get; set; }
        public bool IsCC { get; set; }
        public int? CorrespondentUserId { get; set; }
        public string CorrespondentUserName { get; set; }
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
        public int IsLate { get; set; }
        public int IndivnidualsCounts { get; set; }
        public string ImportanceLevelColor { get; set; }
        //public int archieveDate { get; set; }
        //Unassigned Properties 

        public List<AttachmentViewDTO> transactionActionRecipientAttachment { get; set; }
    }
}
