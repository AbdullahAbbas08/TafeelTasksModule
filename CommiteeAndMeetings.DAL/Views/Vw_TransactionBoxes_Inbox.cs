using System;

namespace CommiteeAndMeetings.DAL.Views
{
    public class Vw_TransactionBoxes_Inbox
    {
        public int Id { get; set; }
        public long TransactionId { get; set; }
        public int ActionId { get; set; }
        public int TransactionActionId { get; set; }
        public int? CorrespondentUserId { get; set; }
        public string FromUserNameAr { get; set; }
        public string FromUserNameEn { get; set; }
        public string FromUserNameFn { get; set; }

        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? RequiredActionId { get; set; }
        public string RequiredActionNameAr { get; set; }
        public string RequiredActionNameEn { get; set; }
        public string RequiredActionNameFn { get; set; }

        public int? RecipientStatusId { get; set; }
        public string RecipientStatusNameAr { get; set; }
        public string RecipientStatusNameEn { get; set; }
        public string RecipientStatusNameFn { get; set; }

        public string transactionStatus { get; set; }
        public int? FromUserId { get; set; }
        public int? FromOrganizationId { get; set; }
        public bool IsCC { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public string ConfidentialityLevelNameAr { get; set; }
        public string ConfidentialityLevelNameEn { get; set; }
        public string ConfidentialityLevelNameFn { get; set; }

        public int? transactionTypeId { get; set; }
        public string transactionTypeNameAr { get; set; }
        public string transactionTypeNameEn { get; set; }
        public string transactionTypeNameFn { get; set; }

        public bool IsUrgent { get; set; }
        public bool IsConfidential { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public string TransactionNumber { get; set; }
        public string Subject { get; set; }
        public string SubjectEn { get; set; }
        public string SubjectFn { get; set; }

        public DateTimeOffset? createdOn { get; set; }
        public DateTimeOffset? TransactionActionCreatedOn { get; set; }

        public byte[] ProfileImage { get; set; }
        public int LetterCount { get; set; }
        public int DocumentCount { get; set; }
        public int PhysicalCount { get; set; }
        public int ActionLetterCount { get; set; }
        public int ActionDocumentCount { get; set; }
        public int ActionPhysicalCount { get; set; }
        public int RelatedTransactionsCount { get; set; }
        public int? ImportanceLevelId { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public string FromOrganizationNameFn { get; set; }

        public string ImportanceLevelNameAr { get; set; }
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelNameFn { get; set; }

        public string NormalizedSubject { get; set; }
        public int IsLate { get; set; }
        public int IndivnidualsCounts { get; set; }
        public string ImportanceLevelColor { get; set; }
        public int? archieveDate { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public string CorrespondingUserNameAR { get; set; }
        public string CorrespondingUserNameEn { get; set; }
        public string CorrespondingUserNameFn { get; set; }

        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }

        public string ToUserFullNameAr { get; set; }
        public string ToUserFullNameEn { get; set; }
        public string ToUserFullNameFn { get; set; }

        public bool? ToOrganizatioIsOuterOrganization { get; set; }
        public int? hasCorrespondence { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public string secretSubject { get; set; }
        public string incomingOrganizationNameAr { get; set; }
        public string incomingOrganizationNameEn { get; set; }
        public string incomingOrganizationNameFn { get; set; }

        public string Notes { get; set; }
        public int? ExecutionPeriod { get; set; }
        public bool IsFinished { get; set; }
        public DateTimeOffset? FollowUPFinishedDate { get; set; }
        public bool IsPrinted { get; set; }
        public int CreatedByUserRoleId { get; set; }
        public string CreatedUserRoleOrganizationNameAr { get; set; }
        public string CreatedUserRoleOrganizationNameEn { get; set; }
        public string CreatedUserRoleOrganizationNameFn { get; set; }
        public bool IsShowAllTime { get; set; }
        public int IsRejected { get; set; }
        public string CreatedUserName { get; set; }
    }
}
