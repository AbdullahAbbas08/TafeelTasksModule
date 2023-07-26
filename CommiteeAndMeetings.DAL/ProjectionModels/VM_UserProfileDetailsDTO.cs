namespace Models.ProjectionModels
{
    public class VM_UserProfileDetailsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }

        public byte[] ProfileImage { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string NationalityAr { get; set; }
        public string NationalityEn { get; set; }
        public string NationalityFn { get; set; }
        public string EmployeeNumber { get; set; }
        public string GenderAr { get; set; }
        public string GenderEn { get; set; }
        public string GenderFn { get; set; }
        public string JobTitleAr { get; set; }
        public string JobTitleEn { get; set; }
        public string JobTitleFn { get; set; }
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

    }
}
