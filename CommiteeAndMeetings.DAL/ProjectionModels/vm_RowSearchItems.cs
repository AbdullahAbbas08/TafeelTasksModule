namespace Models.ProjectionModels
{
    public class vm_RowSearchItems
    {
        public int Id { get; set; }
        public int TransactionActionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public string FromOrganizationNameAr { get; set; }
        public string FromOrganizationNameEn { get; set; }
        public string FromOrganizationNameFn { get; set; }

        public string ToOrganizationNameAr { get; set; }
        public string ToOrganizationNameEn { get; set; }
        public string ToOrganizationNameFn { get; set; }

        public string FromUserNameAr { get; set; }
        public string FromUserNameEn { get; set; }
        public string FromUserNameFn { get; set; }

        public byte[] FromUserProfileImage { get; set; }
        public string ToUserNameAr { get; set; }
        public string ToUserNameEn { get; set; }
        public string ToUserNameFn { get; set; }

        public byte[] ToUserProfileImage { get; set; }
        public int? RecipientStatusId { get; set; }
        public int? ActionId { get; set; }
        public bool? IsCC { get; set; }
        public bool IsOwner { get; set; }
        public string Type { get; set; }
        public bool ShowReferralBtn { get; set; }


    }
}
