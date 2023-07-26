using System;
using System.Collections.Generic;

namespace Models
{
    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string SerialNumber { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string UserName { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }

        public string FullNameAr { get; set; }
        public bool Enabled { get; set; }
        public DateTimeOffset? EnabledUntil { get; set; }
        public byte[] ProfileImage { get; set; }
        public int? SignatureFileId { get; set; }
        public int? TrkeenFileId { get; set; }
        public int? StampFileId { get; set; }
        public string EmployeeNumber { get; set; }
        public string SSN { get; set; }
        public string PassportNumber { get; set; }
        public string Password { get; set; }
        public string IqamaNumber { get; set; }
        public int? GenderId { get; set; }
        public string GenderName { get; set; }
        public int? NationalityId { get; set; }
        public string NationalityName { get; set; }
        public string FaxUserId { get; set; }
        public int? JobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public int? WorkPlaceId { get; set; }
        public string WorkPlaceName { get; set; }
        public string Address { get; set; }
        public string DefaultCulture { get; set; }
        public string DefaultCalendar { get; set; }
        public bool NotificationByMail { get; set; }
        public bool NotificationBySMS { get; set; }
        public bool IsIndividual { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsCorrespondent { get; set; }
        public bool IsCorrespondentForAllOrganizations { get; set; }
        public string SignaturePassword { get; set; }
        public string ProfileImageMimeType { get; set; }
        public int DefaultOrganizationId { get; set; }
        public int userTokenId { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        //custom
        public List<int> UserCorrespondentOrganizationIds { get; set; }
        public int? IndividualAttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public AttachmentDetailsDTO AttachmentDetails { get; set; }
        public int? isMobile { get; set; }

        public bool IsShowCalender { get; set; } = true;
        public bool IsShowTask { get; set; } = true;
        public bool IsShowPeriodStatistics { get; set; } = true;
        public bool IsShowTransactionOwner { get; set; }
        public bool IsShowTransactionRelated { get; set; }

        public int DelegationDefaultID { get; set; }
        public bool? IsGeneral { get; set; }
        public bool HasFactorAuth { get; set; } = false;
        public bool HasSignatureFactorAuth { get; set; } = false;
        public string DefaultOrganizationNameAr { get; set; }
        public string DefaultOrganizationNameEn { get; set; }
        public string DefaultOrganizationNameFn { get; set; }

        public string UserIdEncrypt { get; set; }
        public bool ExternalUser { get; set; } = false;
        public bool AuditUser { get; set; } = false;
        public bool IsMobileUser { get; set; }
        //[NotMapped]
        // public string UserNameEncrypted { get { return EncryptHelper.Encrypt(this.UserName.ToString()); } }
        //[NotMapped]
        //public string UserIdEncrypt { get { return EncryptHelper.Encrypt(this.UserId.ToString()); } }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        #region ShallowCopy

        public UserDetailsDTO ShallowCopy()
        {
            return (UserDetailsDTO)this.MemberwiseClone();
        }

        #endregion

    }
}