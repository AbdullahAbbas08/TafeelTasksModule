using System;

namespace Models.ProjectionModels
{
    public class ViewTransactionsReportsResultDTO
    {
        public long Id { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public int? TransactionTypeId { get; set; }

        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }


        public string ToUserNameAr { get; set; }
        public string ToUserNameEn { get; set; }
        public string ToUserNameFn { get; set; }

        public byte[] ToUserProfileImage { get; set; }

        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public string FromOrganizationNameFn { get; set; }


        public string FromUserNameAr { get; set; }
        public string FromUserNameEn { get; set; }
        public string FromUserNameFn { get; set; }
        public byte[] FromUserProfileImage { get; set; }

        public string RequiredActionNameAr { get; set; }
        public string RequiredActionNameEn { get; set; }
        public string RequiredActionNameFn { get; set; }
        public string RecipientStatusNameAr { get; set; }
        public string RecipientStatusNameEn { get; set; }
        public string RecipientStatusNameFn { get; set; }
        public bool? IsCC { get; set; }
        public string ImportanceLevelColor { get; set; }
        public string fromAny { get; set; }
        public string toAny { get; set; }
        public long TransactionId { get; set; }
        public string TransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionId.ToString()); } }
        public int? RelatedTransactionsCount { get; set; }

    }

}