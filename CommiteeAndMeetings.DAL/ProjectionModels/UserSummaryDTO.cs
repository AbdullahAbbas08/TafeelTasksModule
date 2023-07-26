using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class UserSummaryDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameFn { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public string WorkPhoneNumber { get; set; }
        public bool Enabled { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ProfileImage_string { get; set; }
        public string PassportNumber { get; set; }
        public string IqamaNumber { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsIndividual { get; set; }
        public int? IndividualAttachmentId { get; set; }
        public bool? IsGeneral { get; set; }
        [NotMapped]
        public string UserNameEncrypted { get { return EncryptHelper.Encrypt(this.UserName.ToString()); } }
        public string DefaultOrganizationAr { get; set; }
        public string DefaultOrganizationEn { get; set; }
        public string DefaultOrganizationFn { get; set; }
        public string SSN { get; set; }
        public string JobTitleAr { get; set; }
        public string JobTitleEn { get; set; }
        public string JobTitleFn { get; set; }
        public int? JobTitleId { get; set; }
        public string EmployeeNumber { get; set; }

    }
}