using Models;
using System.Collections.Generic;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class AuthTicketDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }

        public int UserId { get; set; }
        public int? ProfileImageFileId { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public string UserImage { get; set; }
        public int? RoleId { get; set; }
        public int UserRoleId { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameFn { get; set; }

        public string DefaultCulture { get; set; }
        public string DefaultCalendar { get; set; }
        public virtual IEnumerable<string> Permissions { get; set; }
        public virtual IEnumerable<UserRoleDTO> UserRoles { get; set; }

        public bool? IsEmployee { get; set; }
        public bool? isHijriDate { get; set; }
        public int RolesCount { get; set; }

        public bool IsVip { get; set; }
        public bool IsPreperationDirectly { get; set; }


        public bool IsShowCalender { get; set; }
        public bool IsShowTask { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsShowPeriodStatistics { get; set; }

        public bool IsShowTransactionOwner { get; set; }
        public bool IsShowTransactionRelated { get; set; }

        public int DelegationDefaultID { get; set; }
        public int? DefaultInboxFilter { get; set; }
        public bool ShowUserPopup { get; set; }
        public string UserIdEncrypt { get { return EncryptHelper.Encrypt(UserId.ToString()); } }

    }
    public class EncriptedAuthTicketDTO
    {
       // public string SSN { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }

        public string UserId { get; set; }
        public string ProfileImageFileId { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public string UserImage { get; set; }
        public string RoleId { get; set; }
        public string UserRoleId { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameFn { get; set; }

        public string DefaultCulture { get; set; }
        public string DefaultCalendar { get; set; }
        public virtual IEnumerable<string> Permissions { get; set; }
        public virtual IEnumerable<EncriptedUserRoleDTO> UserRoles { get; set; }

        public string IsEmployee { get; set; }
        public string isHijriDate { get; set; }
        public string RolesCount { get; set; }

        public string IsVip { get; set; }
        public string IsPreperationDirectly { get; set; }


        public string IsShowCalender { get; set; }
        public string IsShowTask { get; set; }
        public string IsAdmin { get; set; }
        public string IsShowPeriodStatistics { get; set; }

        public string IsShowTransactionOwner { get; set; }
        public string IsShowTransactionRelated { get; set; }

        public string DelegationDefaultID { get; set; }
        public string DefaultInboxFilter { get; set; }
        public string ShowUserPopup { get; set; }
        public string UserIdEncrypt { get { return UserId.ToString(); } }

        public string AccessTokenExpirationMinutes { get; set; }
        public string Mobile { get; set; }
    }
}
