using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.ProjectionModels
{
    public class ViewSystemSearchResultDTO
    {
        public long Id { get; set; }
        public string OldTransactionDate { get; set; }
        public long TransactionId { get; set; }
        public int TransactionActionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public string TransactionNumber { get; set; }

        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        //public string NormalizedSubject { get; set; }
        public int? TransactionTypeId { get; set; }
        //public int? ImportanceLevelId { get; set; }
        //public int? ClassificationId { get; set; }
        //public int? ConfidentialityLevelId { get; set; }
        //public int? RequiredActionId { get; set; }
        //public int? RegisteredBy { get; set; }
        //public int? CreatedByEmployee { get; set; }
        //public DateTimeOffset? RegisterationDate { get; set; }

        //public int? ToOrganizationId { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public string FromOrganizationNameFn { get; set; }


        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }


        //public int? ToUserId { get; set; }
        public string FromUserNameAr { get; set; }
        public string FromUserNameEn { get; set; }
        public string FromUserNameFn { get; set; }
        public byte[] FromUserProfileImage { get; set; }
        public string ToUserNameAr { get; set; }
        public string ToUserNameEn { get; set; }
        public string ToUserNameFn { get; set; }
        public byte[] ToUserProfileImage { get; set; }


        //public string LbLReferenceNumber { get; set; }
        //public string AttachmentNotification { get; set; }

        //public string IncomingLetterNumber { get; set; }
        //public int? IncomingOrganizationId { get; set; }

        //public int? OwnerId { get; set; }
        //public string UserIdentificationNumber { get; set; }
        //public string UserMobile { get; set; }
        //public string ReferenceNumber { get; set; }


        //public int? FromOrganizationId { get; set; }
        //public bool IsConfidential { get; set; }
        //public int? CreatedByUserRoleId { get; set; }


        //FOR TABLE / TABLE ACTIONS
        public string ImportanceLevelColor { get; set; }
        public int? RecipientStatusId { get; set; }
        public int? ActionId { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsCC { get; set; }
        public string fromAny { get; set; }
        public string toAny { get; set; }
        public bool IsOwner { get; set; }
        public string Type { get; set; }
        public bool ShowReferralBtn { get; set; }
        public bool isOld { get; set; }
        [NotMapped]
        public string TransactionIdEncrypt { get; set; }
        [NotMapped]
        public string TransactionActionRecipientIdEncrypt { get; set; }
        [NotMapped]
        public string TransactionActionIdEncrypt { get; set; }
    }
}
