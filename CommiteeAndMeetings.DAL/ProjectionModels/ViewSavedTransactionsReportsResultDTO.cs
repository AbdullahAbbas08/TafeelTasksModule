using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ProjectionModels
{
    public class ViewSavedTransactionsReportsResultDTO
    {

        public long TransactionId { get; set; }
        public int TransactionActionId { get; set; }
        [Key]
        public int TransactionActionRecipientId { get; set; }

        public string TransactionNumberFormatted { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset? SavedDate { get; set; }
        public string ReasonSave { get; set; }

        public int? IncomingOrganizationId { get; set; }
        public string IncomingOrganizationNameAr { get; set; }
        public string IncomingOrganizationNameEn { get; set; }
        public string IncomingOrganizationNameFn { get; set; }


        public int? RecipientStatusChangedBy { get; set; }
        public string UserChangedByNameAr { get; set; }
        public string UserChangedByNameEn { get; set; }
        public string UserChangedByNameFn { get; set; }


        public int? OrganizationSavedId { get; set; }
        public string OrganizationSavedNameAr { get; set; }
        public string OrganizationSavedNameEn { get; set; }
        public string OrganizationSavedNameFn { get; set; }


        public int? RelatedTransactionsCount { get; set; }
        public string TransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionId.ToString()); } }
        public string TransactionActionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionActionId.ToString()); } }
        public string TransactionActionRecipientIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionActionRecipientId.ToString()); } }


    }

}