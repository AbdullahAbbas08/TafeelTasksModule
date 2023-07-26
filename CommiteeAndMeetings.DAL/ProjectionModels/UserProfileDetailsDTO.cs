namespace Models.ProjectionModels
{
    public class UserProfileDetailsDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }

        public byte[] ProfileImage { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string EmployeeNumber { get; set; }
        public string Gender { get; set; }
        public string JobTitle { get; set; }
        public string SSN { get; set; }
        public string IqamaNumber { get; set; }
        public string Address { get; set; }
        public bool IsCorrespondent { get; set; }
        public bool IsCorrespondentForAllOrganizations { get; set; }
        public bool NotificationBySMS { get; set; }
        public bool NotificationByMail { get; set; }
        public bool IsIndividual { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool HasFactorAuth { get; set; } = false;
        public bool HasSignatureFactorAuth { get; set; } = false;
        public string defaultOrganization { get; set; }
        public string UserIdEncrypt { get; set; }
        //[NotMapped]
        //public string UserIdEncrypt { get { return EncryptHelper.Encrypt(this.UserId.ToString()); } }
    }
}
