using System;

namespace Models.ProjectionModels
{
    public class FollowUpViewDTO
    {
        #region followUp properties
        public int Id { get; set; }
        public int FollowUpId { get; set; }
        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public int FollowUpStatusTypeId { get; set; }
        public DateTimeOffset FollowUpStatusCreatedOn { get; set; }
        public DateTimeOffset? ModifiedDateTo { get; set; }
        public int? ChangeStatusByUserId { get; set; }
        public DateTimeOffset? FinishedDate { get; set; }

        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }

        public string FromUserNameAr { get; set; }
        public string FromUserNameEn { get; set; }

        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationName { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public string fromAny { get; set; }
        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationName { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToUserFullNameAr { get; set; }
        public string ToUserFullNameEn { get; set; }
        public string toAny { get; set; }
        //public string Notes { get; set; }
        public DateTimeOffset? TCreatedOn { get; set; }
        public int? ImportanceLevelId { get; set; }

        public string ImportanceLevelName { get; set; }
        public string ImportanceLevelNameAr { get; set; }
        public string ImportanceLevelNameEn { get; set; }
        public string ImportanceLevelColor { get; set; }
        public string TouserProfileImage { get; set; }
        public string FromUserProfileImage { get; set; }
        public string incomingOrganizationName { get; set; }
        public DateTimeOffset? incomingLetterDate { get; set; }
        public bool isLate { get; set; }
        public int? RecipientStatusId { get; set; }
        public string RecipientStatusName { get; set; }
        public string notes { get; set; }
        public int? DateModifiedCount { get; set; }
        public int? FollowUpStatementCount { get; set; }
        public bool isFinished { get; set; }
        #endregion
        public int daysOfLate { get; set; }
        public string FollowUpStatementsString { get; set; }
        public int? RelatedTransactionsCount { get; set; }
        public string TransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionId.ToString()); } }
        public string TransactionActionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionActionId.ToString()); } }
        public string TransactionActionRecipientIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionActionRecipientId.ToString()); } }
    }
}
