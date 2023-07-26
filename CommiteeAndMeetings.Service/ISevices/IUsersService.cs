using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using LinqHelper;
using Models;
using Models.ProjectionModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IUsersService : IBusinessService<User, UserDetailsDTO>
    {
        Task<string> GetSerialNumberAsync(int userId);
        Task<User> FindUserPasswordAsync(string username, string password, bool isHashedPassword);
        Task<User> FindUserAsync(int userId);
        Task<User> GetCurrentUserAsync();
        
        UserDetailsDTO GetDetails2(string id);
        
        int CheckLoginWay();
        string AuthenticateADUserBySID(string SID);
        Task<bool> AuthenticateADUser(string userName, string password);
        
        bool? CheckFactorAuthValidation(string AuthCode, string UserName);
        bool? ResetUserFactorAuth(string UserName);
        bool? GenericResetUserFactorAuth(string UserName);
        string CheckFactorAuthSetting(string SystemSettingCode);
        bool? InsertUserFactorAuth(int? UserId, int applicationType);
        string GetCurrentUserNameBySid();
        EncryotAndDecryptLoginUserDTO Decrypet_And_Encrypt_loginUser(ref UserLoginModel loginUser);
        EncriptedAuthTicketDTO GetAuthDTO(string userName, int? organizationId = null, int? roleId = null, bool? personal = false);
        Task<User> FindUserByUserNameAsync(string username);
        new IEnumerable<object> Delete(IEnumerable<object> Ids);
        //  UserDetailsDTO GetIndividualUserDetails(string username, bool IsEncrypt);
        int GetCurrentUserId();
        
        string Decrypte(string name);
        EncriptedAuthTicketDTO GetUserAuthTicket(string username, int? organizationId, int? roleId, bool? personal);

        DataSourceResult<UserSummaryDTO> getUsersByOrganization(DataSourceRequest dataSourceRequest, int? organizationID, int? RoleId);

        UserRolePermissionsDTO getuserrolepermissionsbyusername_st(string username, bool isDelegated);
        IEnumerable<object> InsertNewUsers(IEnumerable<UserDetailsDTO> entities);
        IEnumerable<UserDetailsDTO> UpdateUsers(IEnumerable<UserDetailsDTO> Entities);
        UserRolePermissionsDTO GetUserRolePermissionsByValues(int userId, int userRoleId, int organizationId, int roleId, bool isEmployeeRole, bool ForDelegate);
        bool EditUserRolePermissionByValues(int userId, int permissionId, int roleId, int organizationId, int? permission_case);


        //Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        //DataSourceResult<UserSummaryDTO> GetAllDelegatedEmployees(DataSourceRequestDelegation dataSourceRequest);
        //IEnumerable<Lookup> GetAllDelegated(DataSourceRequestForDelegated dataSourceRequest);
        //UserDetailsDTO GetIndividualUserDetailsById(int userId);
        //IEnumerable<UserDetailsDTO> InsertIndividualUser(IEnumerable<UserDetailsDTO> entities);
        //Task<(bool Succeeded, string Error)> ChangePasswordSignatureAsync(User user, string currentPassword, string newPassword);
        //DataSourceResult<UserSummaryDTO> GetDelegatedEmployeeByOrgId(DataSourceRequestForDelegated dataSourceRequest);
        //string ModifyProfileImages();
        //Task<(bool Succeeded, string Error)> ChangePasswordForAdminAsync(int userId, string currentPassword, string newPassword);
        //IEnumerable<UserDetailsDTO> UpdateIndividualUser(IEnumerable<UserDetailsDTO> entities);
        //Task<(bool Succeeded, string Error)> ChangePasswordSignatureForAdminAsync(int userId, string newPassword);
        //bool CheckCountryAllow(string IP, string ContentRootPath);
        //JobTitle GetJobTitle(int? jobTitleId);
        //IEnumerable<Lookup> GetNinIndividualUsersLookup(string term);
        //void UpdateUsersState(IEnumerable<UserStatusDTO> userStatuses, int orgId);
        //EncriptedAuthTicketDTO GetAuthDTO(string userName, int? organizationId = null, int? roleId = null, bool? personal = false);
        //EncriptedAuthTicketDTO GetUserAuthTicket(string userName, int? organizationId = null, int? roleId = null, bool? personal = false);
        //EncryotAndDecryptLoginUserDTO Decrypet_And_Encrypt_loginUser(ref UserLoginModel loginUser);
        //string GetUserName(int? userId);
        //string GetDefaultCulture(int? userId);
        ////UserNotificationDataDTO GetUserNotificationData(int? userId);
        //UserDetailsDTO GetByUserName(string Username, bool decrpte);
        //bool UpdateUsersToIndividualUsers(List<int> userIds);
        //UserRolePermissionsDTO GetUserRolePermissionsByUserName(string username);
        //string EditUserRolePermissionsByUserName(UserRolePermissionsDTO AuthUserRolePermissionsDTO);
        //UserDetailsDTO AddUserImage(int userId, byte[] ProfileImage, string ProfileImageMimeType);
        //NafazDTo CheckNafaz();
        //bool ResetPassword(string email);
        //PasswordCheckDTO CheckResetPassword(string userName, int mehtod);
        //int CheckLoginWay();
        //Task<User> ADFindUserPasswordAsync();
        //UserRolePermissionsDTO getuserrolepermissionsbyusername_st(string username, bool isDelegated);
        //IEnumerable<TreeDetailsDTO> GeUserTreeDetails(bool isActive);
        //bool ResetPasswordByUserName(string userName);
        //DataSourceResult<UserSummaryDTO> GetAllUsersSummary(DataSourceRequest dataSourceRequest);
        //DataSourceResult<UserSummaryDTO> getUsersByOrganization(DataSourceRequest dataSourceRequest, int? organizationID, int? RoleId);
        //string GetMobileByuserName(string userName);
        //DataSourceResult<UserSummaryDTO> GetUsersReport(DataSourceRequest dataSourceRequest, int? organizationID, int? RoleId, bool? Active);
        //bool ActiveUserRole(long userRoleId, DateTimeOffset untillDate);
        //bool UnactiveUserRole(long userRoleId);
        //bool CheckNameIsExsist(string userName);

        //UserRolePermissionsDTO GetUserRolePermissionsByValues(int userId, int userRoleId, int organizationId, int roleId, bool isEmployeeRole, bool ForDelegate);
        //Task<User> FindUserByUserNameAsync(string username);

        ////IEnumerable<UserDetailsDTO> SearchIndividual(string SearchText);
        //string AuthenticateADUserBySID(string SID);
        //bool AuthenticateADUser(string userName, string password);
        //string GetCurrentUserNameBySid();
        //bool? getHijeriStatus();
        //bool? UpdateIsHiJeri();
        //bool Set_individualAttachmentId(int userId, int attachmentId);
        //bool CheckCurrentPassword(string currentPaasword, string userName);
        //DataSourceResult<UserDetailsDTO> GetAllIndividualusers(DataSourceRequest dataSourceRequest);
        //bool EditUserRolePermissionByValues(int userId, int permissionId, int roleId, int organizationId, int? permission_case);
        //bool EditUserRole_roleOverrideUserPermission(int userRoleId, bool? roleOverridesUserPermissions);
        //bool ChangeUserState(UserState state, string userName);
        //UserProfileDetailsDTO GetUserProfile();
        //bool UpdateUserProfile(UserProfileDetailsDTO user);
        //IEnumerable<Lookup> GetUsersForLookup(string searchText);
        //IEnumerable<UserDetailsDTO> GetCurrentLoggedInUsers(string searchText, int pageSize, int pageNumber, ITokenStoreService _tokenStoreService, object _signalRServices);
        //Task<bool> ChangeTempPassword(string userName, string oldPassword, string newPassword);
        //Task<bool> LogOutUser(List<LogOutUserParamsDTO> UserData, ITokenStoreService _tokenStoreService, object _signalRServices);


        //string CreateRandomFactorAuthValue();
        //string CheckFactorAuthSetting(string SystemSettingCode);
        //bool? InsertUserFactorAuth(int? UserId);
        //bool? CheckFactorAuthValidation(string AuthCode, string UserName);
        //bool? ResetUserFactorAuth(string UserName);
        //bool? GenericResetUserFactorAuth(string userName); // used for signature module to send auth code sms without settings
        //bool? CreatFactorAuth(User _user);

        //IEnumerable<Lookup> GetOrganizationChildren(int OrgID, string searchText);
        //Task<User> FindCorrespondentUserPasswordAsync(string username, string password);
        //Task<User> FindCorrespondentUserPasswordAsync(string username);
        //IEnumerable<EmployeesToReportDTO> GetEmployeesToReport(EmployeesToReportParam reportParam);
        //IEnumerable<UserReportDTO> GetEmployeesByOrganizationHierarchyToReport(int orgId);


        //IEnumerable<EmployeesToReportDTO> GetEmployeesToReportWithImage(EmployeesToReportParam reportParam);

        ////bool UpdateUserSysSetting(int userId, UserSysSettingModel userSysSettingModel);

        //bool ValidateUserName(string NewUsername);
        //bool InsertUserVaction(UserVacationDTO userVacation);
        //bool DeleteVacation(int userVacationId);
        //List<UserVacationDTO> GetVacationById(int userId);
        //UserVacationDelegationDTO Check_User_vacation(int userId);
        //CaptchaDTO GenerateCaptch(string userName, string passWord);
        //bool ValidateCaptcha(string userName, string code);
        //EncryotAndDecryptLoginUserDTO GetEncryptedUserData(string userName);
        //string Decrypte(string EncrypteString);
        //IEnumerable<object> InsertNewUsers(IEnumerable<UserDetailsDTO> entities);

        //IEnumerable<UserDetailsDTO> UpdateUsers(IEnumerable<UserDetailsDTO> Entities);
        //bool checkMasarSystemIntegratedValid(string moduleName, string password, string userName);
        //bool? InsertUserSignatureFactorAuth(int? userId);
        //bool CheckSignatureVerfCode(string factorAuthCode, int userId);
        //bool? ResetUserSignatureFactorAuth(int userId);
        //UserDetailsDTO GetByUserNameForSupplier(string Username, bool decrpte);
    }
}
