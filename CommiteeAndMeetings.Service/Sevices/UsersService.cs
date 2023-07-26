using ActiveDirectoryHandling;
using AutoMapper;
using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.AuthenticationServices;
using CommiteeAndMeetings.BLL.BaseObjects.Repositories;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using DbContexts.MasarContext.ProjectionModels;
using HelperServices;
using HelperServices.LinqHelpers;
using IHelperServices;
using IHelperServices.Models;
using IntegrationSMS;
using iTextSharp.text;
using Laserfiche.RepositoryAccess;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Models;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BearerTokensOptions = CommiteeAndMeetings.BLL.AuthenticationServices.BearerTokensOptions;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class UsersService : BusinessService<User, UserDetailsDTO>, IUsersService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRoleService userRoleService;
        private readonly IRepository<User> _users;
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRepository<UserRole> _userRoles;
        private readonly IRepository<UserPermission> _rolePermissionRepository;
        private readonly IRepository<RolePermission> _rolePermissionsRepository;
        private readonly IMailServices mailServices;
        private readonly IDataProtectService _dataProtectService;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IHelperServices.ISessionServices _sessionServices;
        public readonly MasarContext _context;
        private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
        //private readonly AppSettings _AppSettings;
        private readonly IUserRepository _userRepository;
        private readonly ISystemSettingsService _SystemSettingsService;
        //private readonly IRepository<Smstemplate> SMSTemplateRepository;
        //private readonly ILocalizationRepository LocalizationRepository;
        private readonly ISMSTemplateRepository _SMSTemplateRepository;
        private readonly ILocalizationRepository _localizationRepository;
        private readonly SmsServices SmsServices;
        public AppSettings _AppSettings;

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer,  ISystemSettingsService systemSettingsService,IUserRoleService _userRoleService,IUserRepository userRepository,ISMSTemplateRepository sMSTemplateRepository,ILocalizationRepository localizationRepository,ISecurityService securityService, IHttpContextAccessor contextAccessor, IHelperServices.ISessionServices sessionServices, ISignalRServices signalRServices, IOptions<AppSettings> appSettings, IMailServices _mailServices, IDataProtectService dataProtectService, IOptionsSnapshot<BearerTokensOptions> configuration)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings )
        {
            _uow = base._UnitOfWork;
            _users = _uow.GetRepository<User>();
            _userRoles = _uow.GetRepository<UserRole>();
            _rolePermissionRepository = _uow.GetRepository<UserPermission>();
            _rolePermissionsRepository = _uow.GetRepository<RolePermission>();
            _securityService = securityService;
            _contextAccessor = contextAccessor;
            _sessionServices = sessionServices;
            _dataProtectService = dataProtectService;
            _stringLocalizer = stringLocalizer;
            _context = new MasarContext();
            _configuration = configuration;
            mailServices = _mailServices;
            userRoleService = _userRoleService;
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            _userRepository = userRepository;
            _localizationRepository = localizationRepository;
            _SMSTemplateRepository = sMSTemplateRepository;
            _SystemSettingsService = systemSettingsService;
            //SMSTemplateRepository = (ISMSTemplateRepository)_UnitOfWork.GetRepository<Smstemplate>();
            //LocalizationRepository = _uow.GetRepository<Localization>() as ILocalizationRepository;
            //SMSTemplateRepository = _uow.GetRepository<Smstemplate>();//_uow.GetRepository<Smstemplate>() as ISMSTemplateRepository;
            SmsServices = new SmsServices(base._appSettingsOption);
        }

        public async Task<User> FindUserAsync(int userId)
        {
            return await _users.FindAsync(userId);
        }

        public EncriptedAuthTicketDTO GetAuthDTO(string userName, int? organizationId = null, int? roleId = null, bool? personal = false)
        {
            bool IsArabic = _sessionServices.CultureIsArabic;
            User AuthUser = _userRepository.GetUserWithRolesAndPermissions(userName);
            if (AuthUser != null)
            {
                int? empRolId = _uow.GetRepository<Role>().GetAll().FirstOrDefault(r => r.IsEmployeeRole == true)?.RoleId;
               // int? lastSelectedId = AuthUser.UserRoles.LastOrDefault(r => r.LastSelected == true)?.RoleId;

                //AS EMPLOYEE
                //if (organizationId == null && roleId == null && (personal == true ||AuthUser.UserRoles.Count <= 1 || empRolId == lastSelectedId))
                //{
                //    UserRole userEmpRol = _uow.GetRepository<UserRole>().GetAll()
                //                       .FirstOrDefault(r => r.UserId == AuthUser.UserId && r.RoleId == empRolId);

                //    personal = true;
                //    roleId = empRolId;
                //    organizationId = userEmpRol.OrganizationId;
                //}

                // handle if User Is Active
                if (!AuthUser.Enabled)
                {
                    throw new BusinessException(_stringLocalizer.GetString("AccountIsDisabled"));
                }

                if (AuthUser.EnabledUntil.HasValue && AuthUser.EnabledUntil.Value < DateTimeOffset.Now)
                {
                    throw new BusinessException(_stringLocalizer.GetString("AccountIsDisabledSince", AuthUser.EnabledUntil));
                }

                //IEnumerable<UserRole> EnabledUserRoles = AuthUser.UserRoles
                //    .Where(x => !x.EnabledUntil.HasValue || x.EnabledUntil.Value > DateTimeOffset.Now)
                //    .Where(x => !x.EnabledSince.HasValue || x.EnabledSince.Value < DateTimeOffset.Now);

                //UserRole DefaultUserRole = (organizationId.HasValue && roleId.HasValue) ?
                //    EnabledUserRoles.SingleOrDefault(x => x.OrganizationId == organizationId && x.RoleId == roleId) ?? EnabledUserRoles.FirstOrDefault()
                //    : EnabledUserRoles.SingleOrDefault(x => x.LastSelected == true) ?? EnabledUserRoles.FirstOrDefault();

                // To Save Last Selected Role
                //if (roleId.HasValue && organizationId.HasValue)
                //{
                //    UserRole DefaultToSaved = _userRoles.GetAll()
                //        .Where(x => x.UserId == AuthUser.UserId && x.OrganizationId == (organizationId ?? DefaultUserRole.OrganizationId) && x.RoleId == (roleId ?? DefaultUserRole.RoleId))
                //        .FirstOrDefault();
                //    //if (DefaultToSaved != null)
                //    //{
                //    //    _users.UpdateUserRole(DefaultToSaved.UserRoleId, AuthUser.UserId);
                //    //}
                //}
                //if (DefaultUserRole == null)
                //{
                //    return null;
                //}
               // else
                //{

                    // First Of All Get Role Permissions
                   // List<Permission> DefaultRolePermissions = DefaultUserRole.Role.RolePermissions.Where(x => x.Enabled == true).Select(x => x.Permission).ToList();

                    // Second Get UserPermissions Where the same Oraganization and same RoleId 
                    //List<UserPermission> UserPermissions = DefaultUserRole == null ? null :
                    //                                                        DefaultUserRole.User.UserPermissions.Where(x => x.OrganizationId == DefaultUserRole.OrganizationId)
                    //                                                        .Where(x => x.RoleId == DefaultUserRole.RoleId)
                    //                                                        .ToList();



                    //UserPermissions.AsParallel().ForAll(x => x.Permission.Enabled = x.Enabled);

                    //List<Permission> ClonedDefaultRolePermissions = new List<Permission>(DefaultRolePermissions);
                    //IEnumerable<Permission> Intersection = DefaultRolePermissions.Intersect(UserPermissions.Select(x => x.Permission)).ToList();

                    // ClonedDefaultRolePermissions.RemoveAll(x => Intersection.Contains(x));
                    //foreach (Permission _userPrem in Intersection)
                    //{
                    //    ClonedDefaultRolePermissions.Remove(_userPrem);
                    //}
                    List<string> AvailablePermissions = null;
                    //if (DefaultUserRole.RoleOverridesUserPermissions == true)
                    //{
                    //    //AvailablePermissions = DefaultUserRole.Role.RolePermissions
                    //    //                                       .Where(x => x.Enabled && x.Permission.Enabled)
                    //    //                                       .Select(x => x.Permission.PermissionCode.ToUpper()).ToList();
                    //    AvailablePermissions = _rolePermissionRepository.GetAll()
                    //         .Where(x => x.RoleId == DefaultUserRole.RoleId && x.Enabled && x.Permission.Enabled)
                    //         .Select(x => x.Permission.PermissionCode.ToUpper()).ToList();
                    //}
                    //else
                    //{
                    //   // AvailablePermissions = _users.Sp_getPermissions(AuthUser.UserId, DefaultUserRole.OrganizationId, DefaultUserRole.RoleId).Select(s => s.PermissionCode).ToList();

                    //}
                    //var EnabledRoles = EnabledUserRoles.Where(x => x.Organization.IsActive && !x.Organization.DeletedBy.HasValue && !x.Organization.DeletedOn.HasValue).ToList();
                    //var ShowCount = (_uow.Repository<SystemSetting>() as ISystemSettingsRepository).GetAll().Where(w => w.SystemSettingCode == "ShowCountOfUnDelivered").FirstOrDefault().SystemSettingValue;
                    EncriptedAuthTicketDTO Result = new EncriptedAuthTicketDTO()
                    {
                        // added on 05-Jan-2022
                      //  SSN = Encription.EncriptStringAES(AuthUser.SSN),

                        Email = Encription.EncriptStringAES(AuthUser.Email),
                        //FullName = IsArabic ? AuthUser.FullNameAr : AuthUser.FullNameEn,
                        FullNameAr = Encription.EncriptStringAES(AuthUser.FullNameAr),
                        FullNameEn = Encription.EncriptStringAES(AuthUser.FullNameEn),
                        FullNameFn = Encription.EncriptStringAES(AuthUser.FullNameFn),
                        UserName = Encription.EncriptStringAES(AuthUser.Username),
                        UserId = Encription.EncriptStringAES(AuthUser.UserId.ToString()),
                        ProfileImageFileId = Encription.EncriptStringAES(AuthUser.ProfileImageFileId.ToString()),
                        DefaultCulture = Encription.EncriptStringAES(AuthUser.DefaultCulture),
                        UserImage = AuthUser.ProfileImage == null ? "" : Encription.EncriptStringAES(Convert.ToBase64String(AuthUser.ProfileImage)),
                        DefaultCalendar = Encription.EncriptStringAES(AuthUser.DefaultCalendar),
                        //OrganizationId = Encription.EncriptStringAES(DefaultUserRole.OrganizationId.ToString()),
                        //OrganizationNameAr = Encription.EncriptStringAES(DefaultUserRole.Organization.OrganizationNameAr),
                        //OrganizationNameEn = Encription.EncriptStringAES(DefaultUserRole.Organization.OrganizationNameEn),
                        //OrganizationNameFn = Encription.EncriptStringAES(DefaultUserRole.Organization.OrganizationNameFn),

                        //RoleId = Encription.EncriptStringAES(DefaultUserRole.RoleId.ToString()),
                        //UserRoleId = Encription.EncriptStringAES(DefaultUserRole.UserRoleId.ToString()),
                        //RoleNameAr = Encription.EncriptStringAES(DefaultUserRole.Role.RoleNameAr),
                        //RoleNameEn = Encription.EncriptStringAES(DefaultUserRole.Role.RoleNameEn),
                        //RoleNameFn = Encription.EncriptStringAES(DefaultUserRole.Role.RoleNameFn),
                        //UserRoles = MapUserRoles(EnabledRoles.Where(x => x.Role.IsEmployeeRole == false && !x.Role.IsCommitteRole).ToList(), ShowCount),
                        Permissions = AvailablePermissions,
                        IsEmployee = Encription.EncriptStringAES(personal.ToString()),
                        //isHijriDate = Encription.EncriptStringAES(AuthUser.isHijriDate.ToString()),
                        //RolesCount = Encription.EncriptStringAES(EnabledRoles.Count().ToString()),
                       // IsVip = Encription.EncriptStringAES(helperFunction.get_Permission(_SessionServices.UserRoleId, "LoginToVIPDirectly").ToString()),
                        //IsPreperationDirectly = Encription.EncriptStringAES(helperFunction.get_Permission(_SessionServices.UserRoleId, "LoginToPreperationDirectly").ToString()),
                        IsShowCalender = Encription.EncriptStringAES(AuthUser.IsShowCalender.ToString()),
                        IsShowPeriodStatistics = Encription.EncriptStringAES(AuthUser.IsShowPeriodStatistics.ToString()),
                        IsShowTask = Encription.EncriptStringAES(AuthUser.IsShowTask.ToString()),
                        IsShowTransactionOwner = Encription.EncriptStringAES(AuthUser.IsShowTransactionOwner.ToString()),
                        //IsAddCorrespondentInformation = Encription.EncriptStringAES(AuthUser.IsAddCorrespondentInformation.ToString()),
                        IsShowTransactionRelated = Encription.EncriptStringAES(AuthUser.IsShowTransactionRelated.ToString()),
                        DefaultInboxFilter = Encription.EncriptStringAES(AuthUser.DefaultInboxFilter.ToString()),
                       // DelegationDefaultID = Encription.EncriptStringAES(AuthUser.DelegationDefaultID.ToString()),
                        IsAdmin = Encription.EncriptStringAES(AuthUser.IsAdmin.ToString())


                    };
                    //Using Sessions Cache to Save AuthTicket
                    SessionServices.SetAuthTicket(Result.UserName, Result);
                    #region audit record action
                    if (IsAuditEnabled)
                    {
                        AuditService auditService = new AuditService(_appSettingsOption, _sessionServices, _contextAccessor);
                        try
                        {
                            auditService.SaveAuditTrail(null, Result, "Login", "Added", null, null);
                        }
                        catch (Exception ex)
                        {


                        }


                    }
                    #endregion
                return Result;
                }
            //}
            return null;
        }


        // implement SSO Login
        public int CheckLoginWay()
        {
            string checkAD_Settings = _AppSettings.SystemSettingOptions.SSO_LOGIN; //(_uow.Repository<SystemSetting>() as ISystemSettingsRepository).GetAll().Where(w => w.SystemSettingCode == "SSO_LOGIN").FirstOrDefault().SystemSettingValue;
            if (checkAD_Settings == "1")
            {
                return 1;
            }
            else if (checkAD_Settings == "2") { return 2; }// UQU login
            else if (checkAD_Settings == "3") { return 3; }// UJ login
            else if (checkAD_Settings == "4") { return 4; }// Nafaz
            else if (checkAD_Settings == "5") { return 5; }// Azure
            else
            {
                return 0;
            }

        }

        public string AuthenticateADUserBySID(string SID)
        {
            string ServerIp = _uow.GetRepository<SystemSetting>().GetAll().Where(w => w.SystemSettingCode == "ADServer").FirstOrDefault().SystemSettingValue;
            string UserName = _uow.GetRepository<SystemSetting>().GetAll().Where(w => w.SystemSettingCode == "ADUserName").FirstOrDefault().SystemSettingValue;
            string Password = _uow.GetRepository<SystemSetting>().GetAll().Where(w => w.SystemSettingCode == "ADPassword").FirstOrDefault().SystemSettingValue;
            string ObjectDn = _uow.GetRepository<SystemSetting>().GetAll().Where(w => w.SystemSettingCode == "ADObjectDn").FirstOrDefault().SystemSettingValue;


            var user = new ActiveDirectoryCls();
            user.ServerIp = ServerIp;
            user.UserName = UserName;
            user.Password = Password;
            user.ObjectDn = ObjectDn;
            try
            {
                clsActiveDirectoryUser userFromActive = user.GetADUserDataByUserSID(SID);
                if (userFromActive != null)
                return userFromActive.SamAccountName;
                return "";
            }
            catch
            {
                return "";
            }
        }


        public async Task<bool> AuthenticateADUser(string userName, string password)
        {
            //try
            //{
            //    int useAzureAD = _appSettings.azureADSettings.IsEnabled;
            //    if (useAzureAD == 1)
            //    {
            //        var Token = await AuthenticatAzureAD(userName, password);
            //        if (!string.IsNullOrEmpty(Token))
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    if (useAzureAD == 2)
            //    {
            //        var Token = await AuthenticatAzureADWithDomain(userName, password);
            //        if (!string.IsNullOrEmpty(Token))
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

            
            bool isADAuthenticated = false;
            string ADObjectDn = _uow.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(w => w.SystemSettingCode == "ADObjectDn").FirstOrDefault().SystemSettingValue;
            DirectoryEntry directoryEntry = null;
            string LDAP = _uow.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(w => w.SystemSettingCode == "LDAP").FirstOrDefault().SystemSettingValue;

            //string domainName = "tafeel";


            if (string.IsNullOrEmpty(LDAP) || LDAP.Length <= 1)
            {
                if (System.IO.File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start LDAB_2=" + LDAP);
                    }
                }
                directoryEntry = new DirectoryEntry(null, ADObjectDn + "\\" + userName, password);
                if (System.IO.File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start LDAB_2=" + directoryEntry);
                    }
                }
            }
            else
            {
                if (System.IO.File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start LDAB_3=" + LDAP + " --username=" + userName);
                    }
                }
                directoryEntry = new DirectoryEntry(LDAP, userName, password);
            }

            try
            {
                //DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry)
                //{
                // Filter = "samaccountname=" + userName
                //};
                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry)
                {
                    PageSize = int.MaxValue,
                    Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + userName + "))"
                };

                directorySearcher.PropertiesToLoad.Add("sn");

                SearchResult searchResult = directorySearcher.FindOne();
                //directorySearcher.PropertiesToLoad.Add("cn");
                //SearchResult searchResult = directorySearcher.FindOne();
                if (System.IO.File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start LDAB_4=" + LDAP + " searchResult=" + searchResult.Path);
                    }
                }
                if (searchResult != null)
                {
                    isADAuthenticated = true;
                }
            }
            catch (Exception ex)
            {
                isADAuthenticated = false;
            }

            return isADAuthenticated;
        }

        public EncryotAndDecryptLoginUserDTO Decrypet_And_Encrypt_loginUser(ref UserLoginModel loginUser)
        {
            if (loginUser.Continue)
            {
                loginUser.Username = Encription.DecryptStringAES(loginUser.Username);
                loginUser.Password = Encription.DecryptStringAES(loginUser.Password);
                var s = Encription.EncriptStringAES("new-password");
            }
            return new EncryotAndDecryptLoginUserDTO { UserTN = Encription.EncriptStringAES(loginUser.Username), UserTP = Encription.EncriptStringAES(loginUser.Password) };
        }

        public async Task<string> AuthenticatAzureAD(string userName, string password)
        {
            try
            {
                //  userName = "mfatest@mwan.gov.sa";
                // password = "Doto14081";
                string clientId = _AppSettings.azureADSettings.clientId;
                string tenant = _AppSettings.azureADSettings.tenant;

                // Open connection
                string authority = _AppSettings.azureADSettings.authority + tenant;

                string[] scopes = new string[] { "user.read" };
                IPublicClientApplication app;
                app = PublicClientApplicationBuilder.Create(clientId)
                      .WithAuthority(authority)
                      .Build();
                var securePassword = new SecureString();
                foreach (char c in password.ToCharArray())  // you should fetch the password
                    securePassword.AppendChar(c);

                var result = await app.AcquireTokenByUsernamePassword(scopes, userName, securePassword).ExecuteAsync();

                return result.IdToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> AuthenticatAzureADWithDomain(string userName, string password)
        {
            try
            {
                //  userName = "mfatest@mwan.gov.sa";
                // password = "Doto14081";
                string clientId = _AppSettings.azureADSettings.clientId;
                string tenant = _AppSettings.azureADSettings.tenant;

                // Open connection
                string authority = _AppSettings.azureADSettings.authority + tenant;

                string[] scopes = new string[] { "user.read" };
                IPublicClientApplication app;
                app = PublicClientApplicationBuilder.Create(clientId)
                      .WithAuthority(authority)
                      .Build();
                var securePassword = new SecureString();
                foreach (char c in password.ToCharArray())  // you should fetch the password
                    securePassword.AppendChar(c);

                var result = await app.AcquireTokenByUsernamePassword(scopes, userName + "@" + tenant, securePassword).ExecuteAsync();

                return result.IdToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> FindUserByUserNameAsync(string username)
        {
            User result = await _users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
            
            return result;
        }


        public bool? InsertUserFactorAuth(int? UserId, int applicationType)
        {
            int? userId = UserId;
            if (userId != null)
            {
                try
                {
                    /*------- to check thet the user have a code and valid period and not create or send new one -----*/
                    User _user = _users.GetById(userId);
                    if (!string.IsNullOrEmpty(_user.FactorAuthValue) && _user.FactorAuthDate != null)
                    {
                        double OtpPeriodValidation = 5;
                        double.TryParse(_SystemSettingsService.GetSystemSettingByCode("OtpPeriodValidation").SystemSettingValue, out OtpPeriodValidation);
                        DateTimeOffset date = (DateTimeOffset)_user.FactorAuthDate.Value.AddMinutes(OtpPeriodValidation);
                        if (DateTimeOffset.Now <= date)
                        {
                            return true;
                        }
                    }

                    return CreatFactorAuth(_user, applicationType);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    return false;
                }

            }
            else
            {
                return null;
            }
        }


        public bool? CheckFactorAuthValidation(string AuthCode, string UserName)
        {
            User _user = _users.GetAll().FirstOrDefault(x => x.Username == UserName);
            if (_user != null)
            {
                try
                {
                    //if (!string.IsNullOrEmpty(_user.RequestId))
                    //{
                    //    MCI_SMS_Class mCI_SMS = new MCI_SMS_Class();
                    //    Task<bool> Res = mCI_SMS.MCI_CheckOTP(_appSettings.SMSSettings.SenderAPIKey, Convert.ToInt32(_user.RequestId), AuthCode);
                    //    if (Res.Result)
                    //    {
                    //        return true;
                    //    }
                    //}

                    if (_user.FactorAuthValue == AuthCode)
                    {
                        double OtpPeriodValidation = 5;
                        double.TryParse(_SystemSettingsService.GetSystemSettingByCode("OtpPeriodValidation").SystemSettingValue, out OtpPeriodValidation);
                        DateTimeOffset date = (DateTimeOffset)_user.FactorAuthDate.Value.AddMinutes(OtpPeriodValidation);
                        if (DateTimeOffset.Now < date)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    return false;
                }

            }
            else
            {
                return null;
            }
        }



        public string CheckFactorAuthSetting(string SystemSettingCode)
        {
            if (SystemSettingCode.ToUpper() == "FactorAuth".ToUpper())
            {
                string app_setting_UsersystemValue = _AppSettings.SystemSettingOptions.FactorAuth;
                if ((long.Parse(app_setting_UsersystemValue) == 1))
                    return "TRUE";
                else
                    return "FALSE";
            }
            else if (SystemSettingCode.ToUpper() == "SignatureFactorAuth".ToUpper())
            {
                string app_setting_UsersystemValue = _AppSettings.SystemSettingOptions.SignatureFactorAuth;
                if ((long.Parse(app_setting_UsersystemValue) == 1))
                    return "TRUE";
                else
                    return "FALSE";
            }
            else
                return _uow.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(w => w.SystemSettingCode == SystemSettingCode).FirstOrDefault().SystemSettingValue;
        }

        public string CreateRandomFactorAuthValue()
        {
            int FactorAuthLength = 0;
            int.TryParse(CheckFactorAuthSetting("FactorAuthLength"), out FactorAuthLength);
            if (FactorAuthLength == 0)
            {
                return new Random().Next(0, 999999).ToString("D6");
            }
            else
            {
                int maxValue = int.Parse("9".PadLeft(FactorAuthLength, '9'));
                return new Random().Next(0, maxValue).ToString("D" + FactorAuthLength.ToString());
            }
        }


        //private ReturnSMSTypes SendSMSPost_MCI(string mobile, string[] body, string templateCode)
        //{
        //    try
        //    {
        //        MCI_SMS_Class mCI_SMS = new MCI_SMS_Class();
        //        Task<MCIGateService.ReturnResults> result = mCI_SMS.MCI_SendSMS(_appSettings.SMSSettings.SenderAPIKey, mobile, null, body);
        //        return new ReturnSMSTypes
        //        {
        //            success = true,
        //            message = $"result result.IsCompleted {result.IsCompleted}," +
        //                 $"result.IsCompletedSuccessfully {result.IsCompletedSuccessfully},result.Result ,{result.Result}" +
        //                 $"result.IsFaulted ,{result.IsFaulted}result.Id ,{result.Id}" +
        //                 $"result.Status ,{result.Status}result.IsCanceled ,{result.IsCanceled}"
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
        //    }

        //}

        public bool? CreatFactorAuth(User _user, int applicationType = 1)
        {
            try
            {
                _user.FactorAuthValue = CreateRandomFactorAuthValue();
                _user.FactorAuthDate = DateTimeOffset.Now;
                _users.Update(_user);
                _UnitOfWork.SaveChanges();

                #region Send Email & SMS
                if (CheckFactorAuthSetting("SendAuthSMS").ToUpper() == "TRUE")
                {
                    #region SMS Params Body and Template
                    // First Get Template Code & Check for send SMS from Web || Mobile
                    Smstemplate SMSTemplate_FactorAuth;
                    if (applicationType == (int)ApplicationTypeEnum.Web)
                        SMSTemplate_FactorAuth = _SMSTemplateRepository.getTemplateByCode(SMSTemplateCodes.FactorAuth) ?? new Smstemplate { };
                    else
                        SMSTemplate_FactorAuth = _SMSTemplateRepository.getTemplateByCode(SMSTemplateCodes.FactorAuthMobile) ?? new Smstemplate { };


                    SMS_Text_Params_DTO SMSTemplateDTO = new SMS_Text_Params_DTO
                    {
                        TextMessage = SMSTemplate_FactorAuth.TextMessage,
                        ParamtersString = SMSTemplate_FactorAuth.Parameters,
                        TemplateCode = SMSTemplate_FactorAuth.TemplateCode,
                        IsActive = SMSTemplate_FactorAuth.IsActive
                    };
                    SMSTemplate_FactorAuth = null;
                    // Second Create Fields
                    SMSDelegationFieldsDTO Fields = new SMSDelegationFieldsDTO()
                    {
                        TransactionNumberFormatted = _user.FactorAuthValue
                    };
                    SMS_Text_Params_DTO message_body = _SMSTemplateRepository.GetTemplateParams_Body(SMSTemplateDTO, Fields);
                    #endregion
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        if (SMSTemplateDTO.IsActive && message_body.TextMessage != "Error")
                        {

                            if (!string.IsNullOrEmpty(_user.Mobile))
                            {
                                SmsServices.SendSMS(_user.Mobile, message_body.Paramters.ToArray(), message_body.TextMessage, SMSTemplateDTO.TemplateCode);

                            }

                        }
                    }).Start();
                }
                if (CheckFactorAuthSetting("SendAuthEmail").ToUpper() == "TRUE")
                {
                    if (!string.IsNullOrEmpty(_user.Email))
                    {
                        // string mymail = "aeldsoky@tafeel.com";
                        var mailSubject = _sessionServices.Culture.ToLower() == "ar" ? _localizationRepository.GetByKey("MassarVerificationCode", "ar") :
                                          _sessionServices.Culture.ToLower() == "en" ? _localizationRepository.GetByKey("MassarVerificationCode", "en") :
                                           _localizationRepository.GetByKey("MassarVerificationCode", "fn");
                        var mailBody = _sessionServices.Culture.ToLower() == "ar" ? _localizationRepository.GetByKey("MassarVerificationCodeBody", "ar") :
                                          _sessionServices.Culture.ToLower() == "en" ? _localizationRepository.GetByKey("MassarVerificationCodeBody", "en") :
                                           _localizationRepository.GetByKey("MassarVerificationCodeBody", "fn");

                        mailServices.SendNotificationEmail(_user.Email, mailSubject, mailBody + " : " + _user.FactorAuthValue, false, null, "", Hosting.AngularRootPath, null);

                    }
                }
                //if (CheckFactorAuthSetting("SendAuthOTPRequest").ToUpper() == "TRUE")
                //{
                //    //if (!string.IsNullOrEmpty(_user.Mobile))
                //    {
                //        ReturnSMSTypes Result = SendSMSPost_MCI(_user.Mobile, new string[] { _user.FactorAuthValue, "Massar Verification Code Is : " }, null);
                //        if (Result.success)
                //        {
                //            MCI_SMS_Class mCI_SMS = new MCI_SMS_Class();
                //            long RequestID = mCI_SMS.MCI_GetRequestIDOTP(_appSettings.SMSSettings.SenderAPIKey, _user.Username, false).Result;
                //            _user.RequestId = RequestID.ToString();
                //            _users.Update(_user);
                //            _UnitOfWork.SaveChanges();
                //        }
                //    }

                //}
                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool? ResetUserFactorAuth(string UserName)
        {
            User _user = _users.GetAll().FirstOrDefault(x => x.Username == UserName);
            if (_user != null)
            {
                try
                {
                    /*------- to check thet the user have a code and valid period and not create or send new one -----*/
                    return CreatFactorAuth(_user);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    return false;
                }

            }
            else
            {
                return null;
            }
        }

        public bool? GenericCreatFactorAuth(User _user)
        {
            try
            {
                _user.FactorAuthValue = CreateRandomFactorAuthValue();
                _user.FactorAuthDate = DateTimeOffset.Now;
                _users.Update(_user);
                _UnitOfWork.SaveChanges();

                #region Send Email & SMS

                #region send auth sms

                #region SMS Params Body and Template
                // First Get Template Code
                Smstemplate SMSTemplate_FactorAuth = _SMSTemplateRepository.getTemplateByCode(SMSTemplateCodes.FactorAuth) ?? new Smstemplate { };
                SMS_Text_Params_DTO SMSTemplateDTO = new SMS_Text_Params_DTO
                {
                    TextMessage = SMSTemplate_FactorAuth.TextMessage,
                    ParamtersString = SMSTemplate_FactorAuth.Parameters,
                    TemplateCode = SMSTemplate_FactorAuth.TemplateCode,
                    IsActive = SMSTemplate_FactorAuth.IsActive
                };
                SMSTemplate_FactorAuth = null;
                // Second Create Fields
                SMSDelegationFieldsDTO Fields = new SMSDelegationFieldsDTO()
                {
                    TransactionNumberFormatted = _user.FactorAuthValue
                };
                SMS_Text_Params_DTO message_body = _SMSTemplateRepository.GetTemplateParams_Body(SMSTemplateDTO, Fields);
                #endregion
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    if (SMSTemplateDTO.IsActive && message_body.TextMessage != "Error")
                    {

                        if (!string.IsNullOrEmpty(_user.Mobile))
                        {
                            SmsServices.SendSMS(_user.Mobile, message_body.Paramters.ToArray(), message_body.TextMessage, SMSTemplateDTO.TemplateCode);

                        }

                    }
                }).Start();
                #endregion

                #region send auth email

                if (!string.IsNullOrEmpty(_user.Email))
                {
                    // string mymail = "aeldsoky@tafeel.com";
                    var mailSubject = _sessionServices.Culture.ToLower() == "ar" ? _localizationRepository.GetByKey("MassarVerificationCode", "ar") :
                                      _sessionServices.Culture.ToLower() == "en" ? _localizationRepository.GetByKey("MassarVerificationCode", "en") :
                                       _localizationRepository.GetByKey("MassarVerificationCode", "fn");
                    var mailBody = _sessionServices.Culture.ToLower() == "ar" ? _localizationRepository.GetByKey("MassarVerificationCodeBody", "ar") :
                                          _sessionServices.Culture.ToLower() == "en" ? _localizationRepository.GetByKey("MassarVerificationCodeBody", "en") :
                                           _localizationRepository.GetByKey("MassarVerificationCodeBody", "fn");

                    mailServices.SendNotificationEmail(_user.Email, mailSubject, mailBody + " : " + _user.FactorAuthValue, false, null, "", Hosting.AngularRootPath, null);

                }

                #endregion

                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool? GenericResetUserFactorAuth(string UserName)
        {
            User _user = _users.GetAll().FirstOrDefault(x => x.Username == UserName);
            if (_user != null)
            {
                try
                {
                    /*------- to check thet the user have a code and valid period and not create or send new one -----*/
                    return GenericCreatFactorAuth(_user);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    return false;
                }

            }
            else
            {
                return null;
            }
        }

        public string GetCurrentUserNameBySid()
        {
            string userName = string.Empty;
            string ADObjectDn = _uow.GetRepository<SystemSetting>().GetAll().Where(w => w.SystemSettingCode == "ADObjectDn").FirstOrDefault().SystemSettingValue;
            try
            {
                using (PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, ADObjectDn))
                {
                    using (UserPrincipal foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.Sid, UserPrincipal.Current.Sid.Value))
                    {
                        if (foundUser != null)
                        {
                            userName = Environment.UserName;
                        }
                    }
                }
            }
            catch
            {
                userName = string.Empty;
            }

            return userName;
        }

        public string CreatePassword(int length)
        {
            //const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            //StringBuilder res = new StringBuilder();
            //Random rnd = new Random();
            //while (0 < length--)
            //{
            //   res.Append(valid[rnd.Next(valid.Length)]);
            //}
            //return res.ToString();
            var upperCase = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            var lowerCase = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            var numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var rnd = new Random();

            var total = upperCase
                .Concat(lowerCase)
                .Concat(numbers)
                .ToArray();

            var chars = Enumerable
                .Repeat<int>(0, length)
                .Select(i => total[rnd.Next(total.Length)])
                .ToArray();

            var result = new string(chars);
            return result;
        }

        public bool ResetPasswordByUserName(string UserName)
        {
            var user = (_uow.GetRepository<User>() as IUserRepository).GetAll().FirstOrDefault(c => c.Username.ToUpper() == UserName.ToUpper());
            if (user != null)
            {

                var ApplyPasswordVaildation = _uow.GetRepository<SystemSetting>().GetAll().Where(w => w.SystemSettingCode == "ApplyPasswordVaildation").FirstOrDefault().SystemSettingValue;
                string newPassword = "";
                if (ApplyPasswordVaildation == "1")
                {
                    newPassword = RandomPassword.Generate(8, 16);
                }
                else
                {
                    newPassword = CreatePassword(8);
                }
                user.Password = _securityService.GetSha256Hash(newPassword);
                user.PasswordUpdatedOn = null;//DateTime.UtcNow;
                _uow.SaveChangesAsync();
                PasswordHistory passwordHistory = new PasswordHistory()
                {
                    IsFromAdmin = false,
                    Password = newPassword,
                    UserName = user.Username
                };
                _uow.GetRepository<PasswordHistory>().Insert(new List<PasswordHistory>() { passwordHistory });
                // First Get Template Code
                Smstemplate SMSTemplate_delegation = _SMSTemplateRepository.getTemplateByCode(SMSTemplateCodes.PasswordReset) ?? new Smstemplate { };
                SMS_Text_Params_DTO SMSTemplateDTO = new SMS_Text_Params_DTO
                {
                    TextMessage = SMSTemplate_delegation.TextMessage,
                    ParamtersString = SMSTemplate_delegation.Parameters,
                    TemplateCode = SMSTemplate_delegation.TemplateCode,
                    IsActive = SMSTemplate_delegation.IsActive
                };
                SMSTemplate_delegation = null;


                SmsServices.SendSMS(user.Mobile, null, _localizationRepository.GetByKey("SMSResetPasswordMessage", _sessionServices.CultureIsArabic ? "ar" : "en") + newPassword, SMSTemplate_delegation.TemplateCode);
                return true;
            }
            return false;
        }

        public async Task<User> FindUserPasswordAsync(string username, string password, bool isHashedPassword)
        {
            if (!isHashedPassword)
            {
                string passwordHash = _securityService.GetSha256Hash(password);
                User result = await _users.FirstOrDefaultAsync(x => x.Username == username && x.Password == passwordHash);
                return result;
            }
            else
            {
                User result = await _users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
                return result;
             }
        }

        public Task<User> GetCurrentUserAsync()
        {
            int userId = GetCurrentUserId();
            return FindUserAsync(userId);
        }

        public int GetCurrentUserId()
        {
            ClaimsIdentity claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            Claim userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
            string userId = userDataClaim?.Value;
            return string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId);
        }
        public IEnumerable<object> InsertNewUsers(IEnumerable<UserDetailsDTO> entities)
        {
            List<PasswordHistory> passwordHistories = new List<PasswordHistory>();
            string pass = "";
            foreach (UserDetailsDTO entity in entities)
            {
                //if (entity.FromActiveDirectory)
                //{
                //    entity.Password = GenratePassword(entity.IsAdmin);
                //}

                pass = entity.Password;
                string PasswordHash = _securityService.GetSha256Hash(entity.Password);
                if (entity.IsIndividual != true && string.IsNullOrEmpty(entity.Password))
                {
                    return null;
                }

                entity.Password = PasswordHash;
                entity.SerialNumber = Guid.NewGuid().ToString("N");
                string PasswordSignatureHash = _securityService.GetSha256Hash(entity.SignaturePassword);
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    entity.SignaturePassword = PasswordSignatureHash;
                }
                PasswordHistory passwordHistory = new PasswordHistory()
                {
                    IsFromAdmin = true,
                    Password = pass,
                    UserName = entity.UserName
                };
                passwordHistories.Add(passwordHistory);
                _uow.GetRepository<PasswordHistory>().Insert(passwordHistories);
            }
            List<User> users = new List<User>();

            var sendPassword = _uow.GetRepository<SystemSetting>().GetAll().Where(x => x.SystemSettingCode == "SendPasswordOnCreateNewUser").FirstOrDefault()?.SystemSettingValue;
            //_systemSettingsService.GetSystemSettingByCode("SendPasswordOnCreateNewUser").SystemSettingValue;

            var stopSendMailForNewUser = _uow.GetRepository<SystemSetting>().GetAll().Where(x => x.SystemSettingCode == "StopSendingMailWhenCreatingNewUser").FirstOrDefault()?.SystemSettingValue;

            string newUserMailSubject = _sessionServices.CultureIsArabic
                                            ? _UnitOfWork.GetRepository<Localization>().GetAll()
                                                    .FirstOrDefault(r => r.Key == "NewUserMailSubject").ValueAr
                                            : _UnitOfWork.GetRepository<Localization>().GetAll()
                                                    .FirstOrDefault(r => r.Key == "NewUserMailSubject").ValueEn;

            string firstHeader = _UnitOfWork.GetRepository<Localization>().GetAll()
                                       .FirstOrDefault(r => r.Key == "MailHeaderOneNewUser").ValueAr;
            string secondHeader = _UnitOfWork.GetRepository<Localization>().GetAll()
                                   .FirstOrDefault(r => r.Key == "MailHeaderTwoNewUser").ValueAr;
            string footer = _UnitOfWork.GetRepository<Localization>().GetAll()
                                   .FirstOrDefault(r => r.Key == "MailFooterNewUser").ValueAr;
            Localization usernamelbl = _UnitOfWork.GetRepository<Localization>().GetAll()
                                   .FirstOrDefault(r => r.Key == "MailUserNameNewUserlbl");
            Localization passwordlbl = _UnitOfWork.GetRepository<Localization>().GetAll()
                                   .FirstOrDefault(r => r.Key == "MailPasswordNewUserlbl");
            Localization urllbl = _UnitOfWork.GetRepository<Localization>().GetAll()
                                   .FirstOrDefault(r => r.Key == "MailUrlNewUserlbl");

            foreach (UserDetailsDTO user in entities)
            {
                User New_user = _Mapper.Map(user, typeof(UserDetailsDTO), typeof(User)) as User;
                //if (user.FromActiveDirectory)
                //    New_user.PasswordUpdatedOn = DateTime.Now;
                var e = new List<UserDetailsDTO>();
                e.Add(user);
                  _context.Users.Add(New_user);
                _context.SaveChanges();
                users.Add(New_user);
                var url = _contextAccessor.HttpContext.Request.Host;
                string URL = sendPassword == "1" ? url + "/login?IsTemp=true" : url + "/login";
                string UserName = user.UserName;
                string Password = pass;

                var html = CreateTable(sendPassword);

                if (stopSendMailForNewUser != "1")
                {
                    System.Net.Mail.AlternateView message = System.Net.Mail.AlternateView.CreateAlternateViewFromString(
                            html.Replace("{UserName}", UserName).Replace("{Password}", pass).Replace("{URL}", "<a href=http://" + URL + ">" + URL + "</a>")
                            .Replace("{FirstHeader}", firstHeader).Replace("{SecondHeader}", secondHeader).Replace("{Footer}", footer)
                            .Replace("{UserNameEn}", usernamelbl.ValueEn).Replace("{UserNameAr}", usernamelbl.ValueAr)
                            .Replace("{PasswordEn}", passwordlbl.ValueEn).Replace("{PasswordAr}", passwordlbl.ValueAr)
                            .Replace("{UrlEn}", urllbl.ValueEn).Replace("{UrlAr}", urllbl.ValueAr),
                        new System.Net.Mime.ContentType("text/html")
                         );
                    message.ContentType.CharSet = Encoding.UTF8.WebName;
                    mailServices.SendNotificationEmail(user.Email, newUserMailSubject, $"", true, message, "", Hosting.AngularRootPath, null);
                }
            }

            //var inserted_user = base.Insert(entities);

            //insert default user role
            if (users.Count != 0)
            {
                InsertEmpUserRol(users.FirstOrDefault().UserId, entities.FirstOrDefault().DefaultOrganizationId);
                InsertCommiteeUserRol(users.FirstOrDefault().UserId, entities.FirstOrDefault().DefaultOrganizationId);

            }

            return users.Select(u => new { UserId = _dataProtectService.Encrypt(u.UserId.ToString()), UserName = u.Username }).ToList();
        }
        public UserRole InsertEmpUserRol(int userId, int organizationId)
        {
            Role empRole = (_UnitOfWork.GetRepository<Role>()).GetAll().FirstOrDefault(r => r.IsEmployeeRole == true); //get empRole
            if (empRole == null) { InsertEmpRole(ref empRole); } //if not exist insert role and set it to empRole


            var userRole = new UserRole()
            {
                UserId = userId,
                RoleId = empRole.RoleId,
                OrganizationId = organizationId,
                CreatedBy = _sessionServices.UserId,
                CreatedOn = DateTimeOffset.Now,
                LastSelected = false
            };
            // _context.UserRoles.Add(userRole);
            //_context.SaveChanges();
            _uow.GetRepository<UserRole>().Insert(userRole);



            return userRole;
        }
        public UserRole InsertCommiteeUserRol(int userId, int organizationId)
        {
            Role empRole = (_UnitOfWork.GetRepository<Role>()).GetAll().FirstOrDefault(r => r.IsCommitteRole == true); //get empRole
           // if (empRole == null) { InsertEmpRole(ref empRole); } //if not exist insert role and set it to empRole


            var userRole = new UserRole()
            {
                UserId = userId,
                RoleId = empRole.RoleId,
                OrganizationId = organizationId,
                CreatedBy = _sessionServices.UserId,
                CreatedOn = DateTimeOffset.Now,
                LastSelected = false
            };
            // _context.UserRoles.Add(userRole);
            //_context.SaveChanges();
            _uow.GetRepository<UserRole>().Insert(userRole);



            return userRole;
        }

        public void InsertEmpRole(ref Role empRole)
        {
            empRole= (_UnitOfWork.GetRepository<Role>()).Insert(
                new Role
                {
                    RoleNameEn = "Employee Role",
                    RoleNameAr = "Employee Role",
                    RoleNameFn = "Employee Role",
                    IsEmployeeRole = true,
                });
        }
        private string CreateTable(string sendPassword)
        {

            //TODO Add URL
            var html = @"<div style='text-align: center;flex-direction: column;justify-content: center;align-items: center;'>
                             <div style='width: 810px;display: flex;flex-direction: column;justify-content: center;align-items: center;margin: 0;padding: 0;'>
                              <table style='width: 810px;justify-content: center;margin-bottom: -5px;direction: rtl;border: 1px solid #cccccc;display: table;border-collapse: separate;border-spacing: 2px;border-color: grey;'>
                                  <tr style='white-space: normal;line-height: normal;font-weight: normal;font-size: medium;font-style: normal;color: -internal-quirk-inherit;text-align: start;direction: rtl;font-variant: normal;'>
                                  <td colspan='3' style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;'>
                                  <h3 style='text-align: center;background: #13817E;padding: 9px 0;font-weight: 900;margin-bottom: 0px;color: #fff;'>{FirstHeader}</h3></td>
                              </tr>
                                <tr style='white-space: normal;line-height: normal;font-weight: normal;font-size: medium;font-style: normal;color: -internal-quirk-inherit;text-align: start;direction: rtl;font-variant: normal;'>
                                  <td colspan='3' style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;'>
                                  <h3 style='text-align: center;background: #13817E;padding: 9px 0;font-weight: 900;margin-bottom: 0px;color: #fff;'>{SecondHeader}</h3></td>
                              </tr>
                            </tr>
                           
                             <tr style='white-space: normal;line-height: normal;font-weight: normal;font-size: medium;font-style: normal;color: -internal-quirk-inherit;font-variant: normal;'>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;direction: rtl;text-align: start;'>{UserNameAr}</td>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;text-align: center;'>{UserName}</td>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;text-align: left;'>{UserNameEn}</td>
                              </tr>
                                ";

            if (sendPassword == "1")
            {
                html = html + @" <tr style='white-space: normal;line-height: normal;font-weight: normal;font-size: medium;font-style: normal;color: -internal-quirk-inherit;font-variant: normal;'>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;direction: rtl;text-align: start;'>{PasswordAr}</td>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;text-align: center;'>{Password}</td>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;text-align: left;'>{PasswordEn}</td>
                              </tr>
                                ";
            }
            html = html + @"<tr style='white-space: normal;line-height: normal;font-weight: normal;font-size: medium;font-style: normal;color: -internal-quirk-inherit;font-variant: normal;'>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;direction: rtl;text-align: start;'>{UrlAr}</td>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;text-align: center;'>{URL}</td>
                                  <td style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;text-align: left;'>{UrlEn}</td>
                              </tr>
                           ";
            if (sendPassword == "1")
            {
                html = html + @" <tr style='white-space: normal;line-height: normal;font-weight: normal;font-size: medium;font-style: normal;color: -internal-quirk-inherit;text-align: start;direction: rtl;font-variant: normal;'>
                                <td colspan='3' style='border: 1px solid #cccccc;border-bottom: 1px solid #EEE;padding: 10px;width: 3px;margin-bottom: -3px;font-weight: 600;margin: -6px;'>
                                <h3 style='text-align: center;background: #13817E;padding: 9px 0;font-weight: 900;margin-bottom: 0px;color: #fff;'>{Footer}</h3></td>
                              </tr>
                                ";
            }
            html = html + @" </table>
                            </div>   ";
            return html;
        }

        public UserRolePermissionsDTO GetUserRolePermissionsByValues(int userId, int UserRoleId, int OrganizationId, int roleId, bool IsEmployeeRole, bool ForDelegate)
        {
            try
            {
                List<ViewPermissionCategoryView> permissionsWith_Emp_Categories = null;
                List<ViewPermissionCategoryView> permissionsWith_Role_Categories = null;
                List<PermissionCategory> PermissionCategory_emp = null;
                List<PermissionCategory> PermissionCategory_Role = null;
                if (IsEmployeeRole == true)
                {
                    permissionsWith_Emp_Categories = _context.ViewPermissionCategoryView.FromSqlRaw($"exec sp_GetPermissionOrdered {true},{ForDelegate},{_sessionServices.CultureIsArabic}").ToList().Where(z=>z.IsCommittePermission).ToList();
                    var catlist = permissionsWith_Emp_Categories.Select(x => x.PermissionCategoryId).Distinct().ToList();
                    PermissionCategory_emp = _UnitOfWork.GetRepository<PermissionCategory>().GetAll(false).Where(w => w.IsEmployeeCategory == true && catlist.Contains(w.PermissionCategoryId)).ToList();

                }
                else
                {
                    // get All Permissions with IsEmployeeCategory == false and ForDelegate = ForDelegate Param
                    permissionsWith_Role_Categories = _context.ViewPermissionCategoryView.FromSqlRaw($"exec sp_GetPermissionOrdered {false},{ForDelegate},{_sessionServices.CultureIsArabic}").ToList().Where(z => z.IsCommittePermission).ToList();

                   // permissionsWith_Role_Categories = _IPermissionRepository.GetPermissionsGroupedByPermissionCategoryName_storedProcedure(false, ForDelegate).ToList();
                    if (ForDelegate && permissionsWith_Role_Categories != null)
                    {
                        PermissionCategory_Role = permissionsWith_Role_Categories.GroupBy(a => a.PermissionCategoryId).Select(b => b.First()).ToList().Select(s => new PermissionCategory
                        {
                            PermissionCategoryId = s.PermissionCategoryId,
                            PermissionCategoryNameAr = s.PermissionCategoryNameAr,
                            PermissionCategoryNameEn = s.PermissionCategoryNameEn,
                            PermissionCategoryNameFn = s.PermissionCategoryNameFn,
                            IsEmployeeCategory = s.IsEmployeeCategory
                        }).ToList();
                    }
                    else
                    {
                        var catlist = permissionsWith_Role_Categories.Select(x => x.PermissionCategoryId).Distinct().ToList();
                        PermissionCategory_Role = _UnitOfWork.GetRepository<PermissionCategory>().GetAll(false).Where(w => w.IsEmployeeCategory == false && catlist.Contains(w.PermissionCategoryId)).ToList();
                    }
                }


                List<UserRoles> _userRoles = this._userRoles.GetAll().Where(x => x.UserId == userId && x.UserRoleId == UserRoleId && x.OrganizationId == OrganizationId && x.RoleId == roleId)
                                            //.Include(x => x.Role).ThenInclude(x => x.RolePermissions)
                                            //.Include(x => x.User).ThenInclude(x => x.UserPermissions)
                                            .ToList()
                                                .Select(Ur => new UserRoles
                                                {
                                                    RoleOverridesUserPermissions = Ur.RoleOverridesUserPermissions,
                                                    PermissionCategories = (IsEmployeeRole ? PermissionCategory_emp : PermissionCategory_Role)
                                                                            .Select(PC => new PermissionCategories
                                                                            {
                                                                                PermissionCategoryName = _sessionServices.Culture.ToLower() == "ar" ? PC.PermissionCategoryNameAr
                                                                                                    : _sessionServices.Culture.ToLower() == "en" ? PC.PermissionCategoryNameEn
                                                                                                    : PC.PermissionCategoryNameFn,

                                                                                UserRolePermissions = (Ur.Role.IsEmployeeRole ? permissionsWith_Emp_Categories : permissionsWith_Role_Categories)
                                                                                                        .Where(w => w.PermissionCategoryId == PC.PermissionCategoryId)
                                                                                                        .Select(Urp => new UserRolePermission
                                                                                                        {
                                                                                                            PermissionId = Urp.PermissionId,
                                                                                                            PermissionName = _sessionServices.Culture.ToLower() == "ar" ? Urp.PermissionNameAr
                                                                                                            : _sessionServices.Culture.ToLower() == "en" ? Urp.PermissionNameEn
                                                                                                            : Urp.PermissionNameFn,
                                                                                                            cases = CheckValue(Ur.Role, Ur.User, Urp.PermissionId, Ur.OrganizationId, Ur.RoleId)
                                                                                                        }).ToList()
                                                                                //: permissionsWith_Role_Categories.Where(w => w.PermissionCategoryId == PC.PermissionCategoryId)
                                                                                //  .Select(Urp => new UserRolePermission
                                                                                //  {
                                                                                //      PermissionId = Urp.PermissionId,
                                                                                //      PermissionName = _SessionServices.CultureIsArabic ? Urp.PermissionNameAr : Urp.PermissionNameEn,
                                                                                //      //RolePermissionEnabled = Ur.Role.RolePermissions?.FirstOrDefault(w => w.PermissionId == Urp.PermissionId)?.Enabled,
                                                                                //      //UserPermissionEnabled = Ur.User.UserPermissions?.FirstOrDefault(w => w.PermissionId == Urp.PermissionId && w.OrganizationId == Ur.OrganizationId && w.RoleId == Ur.RoleId)?.Enabled
                                                                                //      cases = CheckValue(Ur.Role, Ur.User, Urp.PermissionId, Ur.OrganizationId, Ur.RoleId)

                                                                                //  }).ToList()
                                                                            }),
                                                }).ToList();



                UserRolePermissionsDTO Result = new UserRolePermissionsDTO()
                {
                    UserRoles = _userRoles
                };
                return Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private cases CheckValue(Role role, User user, int Urp_PermissionId, int OrganizationId, int RoleId)
        {
            cases cases = new cases();

            bool? RolePermissionEnabled = role.RolePermissions.FirstOrDefault(w => w.PermissionId == Urp_PermissionId)?.Enabled;
            if (RolePermissionEnabled == true)
            {
                cases.Default_case = 1;
            }
            if (RolePermissionEnabled == false)
            {
                cases.Default_case = 2;
            }
            bool? UserPermissionEnabled = user.UserPermissionUsers?.FirstOrDefault(w => w.PermissionId == Urp_PermissionId && w.OrganizationId == OrganizationId && w.RoleId == RoleId)?.Enabled;
            if (UserPermissionEnabled == null)
            {
                cases.new_case = null;
                return cases;
            }
            if (UserPermissionEnabled == true)
            {
                cases.new_case = 3;
                return cases;
            }
            else if (UserPermissionEnabled == false)
            {
                cases.new_case = 4;
                return cases;

            }

            return cases;
        }

        public bool EditUserRolePermissionByValues(int UserId, int PermissionId, int RoleId, int OrganizationId, int? permission_case)
        {
            //delete from UserPermissions where UserId = 12293 and PermissionId = 116 and RoleId = 27 and OrganizationId = 2643
            try
            {
                User User = _users.GetAll()
                    .Where(x => x.UserId == UserId)
                    .FirstOrDefault();

                UserRole userRole = _userRoles.GetAll().FirstOrDefault(x => x.UserId == UserId && x.OrganizationId == OrganizationId && x.RoleId == RoleId);
                //in case of permission_case = null means to delete UserPermission
                if (permission_case == null)
                {
                    IQueryable<UserPermission> objectToDelete = _UnitOfWork.GetRepository<UserPermission>().GetAll().Where(w => w.UserId == UserId && w.PermissionId == PermissionId && w.RoleId == RoleId && w.OrganizationId == OrganizationId);
                    _context.UserPermissions.Remove(objectToDelete.FirstOrDefault());
                    _context.SaveChanges();
                    //(_UnitOfWork.GetRepository<UserPermission>()).Delete(objectToDelete.Select(x=>x.u));

                   // _signalRServices.UserRoleUpdatedByValue(new List<string>() { User.Username }, userRole.UserRoleId);

                    return true;
                }
                // in case enabled
                else if (permission_case == 3)
                {
                    UserPermission objectToUpdate = _UnitOfWork.GetRepository<UserPermission>().GetAll().Where(w => w.UserId == UserId && w.PermissionId == PermissionId && w.RoleId == RoleId && w.OrganizationId == OrganizationId)?.FirstOrDefault();
                    if (objectToUpdate != null)
                    {
                        UserPermission previousObject = objectToUpdate.ShallowCopy();

                        objectToUpdate.Enabled = true;
                        objectToUpdate.UpdatedBy = _sessionServices.UserId;
                        objectToUpdate.UpdatedOn = DateTimeOffset.Now;

                        _UnitOfWork.GetRepository<UserPermission>().Update(objectToUpdate);
                        _UnitOfWork.SaveChanges();


                    }
                    else
                    {
                        // create new userPermission
                        UserPermission userPermission = _UnitOfWork.GetRepository<UserPermission>().Insert(new UserPermission
                        {
                            UserId = UserId,
                            PermissionId = PermissionId,
                            RoleId = RoleId,
                            OrganizationId = OrganizationId,
                            Enabled = true,
                            CreatedBy = _sessionServices.UserId,
                            CreatedOn = DateTimeOffset.Now
                        });


                    }

                    //_signalRServices.UserRoleUpdatedByValue(new List<string>() { User.Username }, userRole.UserRoleId);

                    return true;

                }
                // in case Disabled
                else if (permission_case == 4)
                {
                    UserPermission objectToUpdate = _UnitOfWork.GetRepository<UserPermission>().GetAll().Where(w => w.UserId == UserId && w.PermissionId == PermissionId && w.RoleId == RoleId && w.OrganizationId == OrganizationId)?.FirstOrDefault();

                    if (objectToUpdate != null)
                    {
                        UserPermission previousObject = objectToUpdate.ShallowCopy();

                        objectToUpdate.Enabled = false;
                        objectToUpdate.UpdatedBy = _sessionServices.UserId;
                        objectToUpdate.UpdatedOn = DateTimeOffset.Now;

                        _UnitOfWork.GetRepository<UserPermission>().Update(objectToUpdate);
                        _UnitOfWork.SaveChanges();

                    }
                    else
                    {
                        // create new userPermission
                        UserPermission userPermission = _UnitOfWork.GetRepository<UserPermission>().Insert(new UserPermission
                        {
                            UserId = UserId,
                            PermissionId = PermissionId,
                            RoleId = RoleId,
                            OrganizationId = OrganizationId,
                            Enabled = false,
                            CreatedBy = _sessionServices.UserId,
                            CreatedOn = DateTimeOffset.Now
                        });
                    }


                    //_signalRServices.UserRoleUpdatedByValue(new List<string>() { User.Username }, userRole.UserRoleId);

                    return true;

                }
                //// in case Disabled
                //else if (permission_case == 4)
                //{
                //    UserPermission objectToUpdate = _UnitOfWork.Repository<UserPermission>().GetAll().Where(w => w.UserId == UserId && w.PermissionId == PermissionId && w.RoleId == RoleId && w.OrganizationId == OrganizationId)?.FirstOrDefault();

                //    if (objectToUpdate != null)
                //    {
                //        UserPermission previousObject = objectToUpdate.ShallowCopy();

                //        objectToUpdate.Enabled = false;
                //        _UnitOfWork.Repository<UserPermission>().Update(objectToUpdate);
                //        _UnitOfWork.Save();

                //        _signalRServices.UserRoleUpdatedByValue(new List<string>() { User.Username }, userRole.UserRoleId);

                //        return true;

                //    }
                //    else
                //    {
                //        _signalRServices.UserRoleUpdatedByValue(new List<string>() { User.Username }, userRole.UserRoleId);

                //        return false;
                //    }

                //}
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        public UserDetailsDTO GetDetails2(string id)
        {
            // Username = Encription.DecryptStringAES(Username);
            int id1 = Convert.ToInt32(id);
            var User = _users.GetAll(false).FirstOrDefault(x => x.UserId == id1);

            
                if (User != null)
                {
                    int UserId = User.UserId;
                    if (UserId > 0)
                    {
                        UserDetailsDTO user = GetDetails(UserId);
                        //user roles
                        UserRole user_role = (_UnitOfWork.GetRepository<UserRole>()).GetAll()
                            .Include(ur => ur.Role)
                            .FirstOrDefault(u => u.UserId == UserId && u.Role.IsEmployeeRole == true && (u.EnabledUntil == null || u.EnabledUntil > DateTime.Now));
                        if (user != null && user_role != null)
                        {
                            user.DefaultOrganizationId = user_role.OrganizationId;
                            user.DefaultOrganizationNameAr = user_role.Organization.OrganizationNameAr;
                            user.DefaultOrganizationNameEn = user_role.Organization.OrganizationNameEn;
                            user.DefaultOrganizationNameFn = user_role.Organization.OrganizationNameFn;

                            //  user.DefaultOrganizationNameFn = user_role.Organization.OrganizationNameFn;
                        }
                        //user correspondent organizations
                        List<int> userCorrespondentOrganizationIds = (_UnitOfWork.GetRepository<UserCorrespondentOrganization>())
                            .GetAll()
                            .Where(x => x.UserId == user.UserId)
                            .Select(x => x.OrganizationId)
                            .ToList();
                        user.UserCorrespondentOrganizationIds = userCorrespondentOrganizationIds;
                        return user;
                    }
                }
            
            return null;
        }

        public async Task<string> GetSerialNumberAsync(int userId)
        {
            User user = await FindUserAsync(userId);
            return user.SerialNumber;
        }
        public string Decrypte(string EncrypteString)
        {
            return _dataProtectService.Decrypt(EncrypteString);
        }
        public string Encrypt(string EncrypteString)
        {
            return _dataProtectService.Encrypt(EncrypteString);
        }
        public EncriptedAuthTicketDTO GetUserAuthTicket(string userName, int? organizationId = null, int? roleId = null, bool? personal = false)
        {
            try
            {
                bool IsArabic = _sessionServices.CultureIsArabic;
                User AuthUser = _users.GetAll().Include(x => x.UserRoleUsers)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission).FirstOrDefault(x => x.Username == userName);
                if (AuthUser != null)
                {
                    // handle if User Is Active
                    if (!AuthUser.Enabled)
                    {
                        throw new BusinessException(_stringLocalizer.GetString("AccountIsDisabled"));
                    }

                    if (AuthUser.EnabledUntil.HasValue && AuthUser.EnabledUntil.Value < DateTimeOffset.Now)
                    {
                        throw new BusinessException(_stringLocalizer.GetString("AccountIsDisabledSince", AuthUser.EnabledUntil));
                    }

                    int? empRolId = _UnitOfWork.GetRepository<Role>().GetAll(false).FirstOrDefault(r => r.IsEmployeeRole == true)?.RoleId;
                    int? lastSelectedId = AuthUser.UserRoleUsers.LastOrDefault(r => r.LastSelected == true)?.RoleId;
                    // by gabr -- mahmoud issuee
                    if (lastSelectedId == null)
                        lastSelectedId = empRolId;
                    //AS EMPLOYEE
                    if (organizationId == null && roleId == null && (personal == true || AuthUser.UserRoleUsers.Count <= 1 || empRolId == lastSelectedId))
                    {
                        UserRole userEmpRol = _UnitOfWork.GetRepository<UserRole>().GetAll()
                                                .Where(r => r.UserId == AuthUser.UserId && r.RoleId == empRolId)
                                                .Where(x => !x.EnabledUntil.HasValue || x.EnabledUntil.Value > DateTimeOffset.Now)
                                                .Where(x => !x.EnabledSince.HasValue || x.EnabledSince.Value < DateTimeOffset.Now).FirstOrDefault();

                        personal = true;
                        roleId = empRolId;
                        //  organizationId = userEmpRol.OrganizationId;
                    }


                    // Get Enabled UserRoles of Auth User 
                    IEnumerable<UserRole> EnabledUserRoles = AuthUser.UserRoleUsers
                        .Where(x => x.Role.IsCommitteRole)
                        .Where(x => !x.EnabledUntil.HasValue || x.EnabledUntil.Value > DateTimeOffset.Now)
                        .Where(x => !x.EnabledSince.HasValue || x.EnabledSince.Value < DateTimeOffset.Now);
                    //.Where(roleValue => !roleValue.Organization.DeletedBy.HasValue && !roleValue.Organization.DeletedOn.HasValue);

                    // Then Get Default UserRole Depened On OrganizationId and RoleId and UserId if Nothing Select The First Enabled UserRole
                    UserRole DefaultUserRole = EnabledUserRoles.Where(x => x.Role.IsCommitteRole == true).FirstOrDefault() ?? EnabledUserRoles.FirstOrDefault();

                    // To Save Last Selected Role
                    if (roleId.HasValue && organizationId.HasValue)
                    {
                        UserRole DefaultToSaved = _userRoles.GetAll().Where(x => !x.EnabledUntil.HasValue || x.EnabledUntil.Value > DateTimeOffset.Now).FirstOrDefault(x => x.UserId == AuthUser.UserId && x.Role.IsCommitteRole);
                        if (DefaultToSaved != null)
                        {
                            AuthUser = _users.GetAll().Include(x => x.UserRoleUsers)
                                                      .ThenInclude(x => x.Role)
                                                      .ThenInclude(x => x.RolePermissions)
                                                      .ThenInclude(x => x.Permission).FirstOrDefault(x => x.Username == userName);
                            EnabledUserRoles = AuthUser.UserRoleUsers.Where(x => !x.EnabledUntil.HasValue || x.EnabledUntil.Value > DateTimeOffset.Now).Where(x => !x.EnabledSince.HasValue || x.EnabledSince.Value < DateTimeOffset.Now);
                        }
                    }
                    if (DefaultUserRole == null)
                    {
                        return null;
                    }
                    else
                    {
                        List<string> AvailablePermissions = null;

                        //AvailablePermissions = _rolePermissionsRepository.GetAll().Include(c => c.Role).Include(x => x.Permission)
                        //    .Where(c => c.Role.IsCommitteRole && c.RoleId == DefaultUserRole.RoleId && c.Enabled && c.Role.UserRoles.Any(x => x.UserId == AuthUser.UserId))
                        //     .Select(x => Encription.EncriptStringAES(x.Permission.PermissionCode.ToUpper())).ToList();
                        ////_users.Sp_getPermissions(AuthUser.UserId, DefaultUserRole.OrganizationId, DefaultUserRole.RoleId).Select(s => Encription.EncriptStringAES(s.PermissionCode)).ToList();


                        //AvailablePermissions = _rolePermissionRepository.GetAll().Include(C => C.Permission)
                        //       .Where(x => x.UserId == AuthUser.UserId && x.Enabled && x.Permission.Enabled && x.Permission.IsCommittePermission)
                        //       .Select(x => Encription.EncriptStringAES(x.Permission.PermissionCode.ToUpper())).ToList();

                        //var RolePermission = _userRoles.GetAll().Include(x => x.Role).ThenInclude(x => x.RolePermissions.Where(x => x.Enabled))
                        //       .Where(c => c.Role.IsCommitteRole && c.RoleId == DefaultUserRole.RoleId && c.Role.UserRoles.Any(x => x.UserId == AuthUser.UserId))
                        //       .Select(x => x.Role.RolePermissions).FirstOrDefault();
                        //AvailablePermissions = RolePermission.Select(x => x.Permission.PermissionCode).ToList();
                        if (DefaultUserRole.RoleOverridesUserPermissions == true)
                        {

                            AvailablePermissions = _rolePermissionsRepository.GetAll()
                            .Where(x => x.Role.IsCommitteRole && x.RoleId == DefaultUserRole.RoleId && x.Permission.IsCommittePermission && x.Enabled)
                            .Select(x => Encription.EncriptStringAES(x.Permission.PermissionCode.ToUpper())).ToList();
                        }
                        else
                        {
                            var permission = this._context.Vm_Permissions.FromSqlRaw($"exec sp_getPermissions {AuthUser.UserId},{DefaultUserRole.OrganizationId},{DefaultUserRole.RoleId}").ToList();
                            AvailablePermissions = permission.Select(s => Encription.EncriptStringAES(s.PermissionCode.ToUpper())).ToList();
                        }
                        //IEnumerable<UserRole> AfterUpdate = AuthUser.UserRoleUsers.Where(x => !x.EnabledUntil.HasValue || x.EnabledUntil.Value > DateTimeOffset.Now)
                        // .Where(x => !x.EnabledSince.HasValue || x.EnabledSince.Value < DateTimeOffset.Now);
                        var DefualtRole = (organizationId == null) ? null : _users.GetAll().Where(c => c.UserId == AuthUser.UserId).FirstOrDefault().UserRoleUsers.Where(x => x.Role.IsCommitteRole == true).FirstOrDefault();
                        EncriptedAuthTicketDTO Result = new EncriptedAuthTicketDTO()
                        {
                            Email = Encription.EncriptStringAES(AuthUser.Email),
                            //FullName = IsArabic ? AuthUser.FullNameAr : AuthUser.FullNameEn,
                            FullNameAr = Encription.EncriptStringAES(AuthUser.FullNameAr),
                            FullNameEn = Encription.EncriptStringAES(AuthUser.FullNameEn),
                            FullNameFn = Encription.EncriptStringAES(AuthUser.FullNameFn),
                            UserName = Encription.EncriptStringAES(AuthUser.Username),
                            UserId = Encription.EncriptStringAES(AuthUser.UserId.ToString()),
                            ProfileImageFileId = Encription.EncriptStringAES(AuthUser.ProfileImageFileId.ToString()),
                            DefaultCulture = Encription.EncriptStringAES(AuthUser.DefaultCulture),
                            UserImage = AuthUser.ProfileImage == null ? "" : Encription.EncriptStringAES(Convert.ToBase64String(AuthUser.ProfileImage)),
                            DefaultCalendar = Encription.EncriptStringAES(AuthUser.DefaultCalendar),
                            OrganizationId = Encription.EncriptStringAES(DefaultUserRole.OrganizationId.ToString()),
                            Mobile = Encription.EncriptStringAES(AuthUser.Mobile).ToString(),
                            //OrganizationNameAr = Encription.EncriptStringAES(DefaultUserRole.Organization.OrganizationNameAr),
                            //OrganizationNameEn = Encription.EncriptStringAES(DefaultUserRole.Organization.OrganizationNameEn),
                            //OrganizationNameFn = Encription.EncriptStringAES(DefaultUserRole.Organization.OrganizationNameFn),

                            RoleId = Encription.EncriptStringAES(DefaultUserRole.RoleId.ToString()),
                            UserRoleId = Encription.EncriptStringAES(DefaultUserRole.UserRoleId.ToString()),
                            RoleNameAr = Encription.EncriptStringAES(DefaultUserRole.Role.RoleNameAr),
                            RoleNameEn = Encription.EncriptStringAES(DefaultUserRole.Role.RoleNameEn),
                            RoleNameFn = Encription.EncriptStringAES(DefaultUserRole.Role.RoleNameFn),
                            UserRoles = MapUserRoles(EnabledUserRoles.Where(x => x.Role.IsEmployeeRole == false && x.Role.IsCommitteRole).ToList()),
                            //,
                            Permissions = AvailablePermissions,
                            IsEmployee = Encription.EncriptStringAES(personal.ToString()),
                            RolesCount = Encription.EncriptStringAES(EnabledUserRoles.Count().ToString()),
                            IsShowCalender = Encription.EncriptStringAES(AuthUser.IsShowCalender.ToString()),
                            IsShowPeriodStatistics = Encription.EncriptStringAES(AuthUser.IsShowPeriodStatistics.ToString()),
                            IsShowTask = Encription.EncriptStringAES(AuthUser.IsShowTask.ToString()),
                            IsShowTransactionOwner = Encription.EncriptStringAES(AuthUser.IsShowTransactionOwner.ToString()),
                            IsShowTransactionRelated = Encription.EncriptStringAES(AuthUser.IsShowTransactionRelated.ToString()),
                            DefaultInboxFilter = Encription.EncriptStringAES(AuthUser.DefaultInboxFilter.ToString()),
                            IsAdmin = Encription.EncriptStringAES(AuthUser.IsAdmin.ToString()),
                            AccessTokenExpirationMinutes = Encription.EncriptStringAES(_configuration.Value.AccessTokenExpirationMinutes.ToString())
                        };
                        var employeeOrganization = EnabledUserRoles.Where(x => x.Role.IsEmployeeRole == true).FirstOrDefault();
                        //foreach (var userRole in Result.UserRoles)
                        //{
                        //    userRole.IsDefaultOrganization = (userRole.OrganizationId == Encription.EncriptStringAES(employeeOrganization.OrganizationId.ToString())) ? Encription.EncriptStringAES(true.ToString()) : Encription.EncriptStringAES(false.ToString());
                        //}
                        //Using Sessions Cache to Save AuthTicket
                        //HelperServices.SessionServices.SetAuthTicket(Result.UserName, Result);

                        #region audit record action
                        if (IsAuditEnabled)
                        {
                            AuditService auditService = new AuditService(_appSettingsOption, _sessionServices, _contextAccessor);
                            try
                            {
                                auditService.SaveAuditTrail(null, Result, "Login", "Added", null, null);
                            }
                            catch (Exception ex)
                            {


                            }


                        }

                        #endregion

                        return Result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                //if (System.IO.File.Exists("C:\\testemail.txt"))
                //{
                //    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                //    {
                //        sw.WriteLine("function= GetUserAuthTicket");
                //        sw.WriteLine("userName= " + userName);
                //        sw.WriteLine("Mes=" + ex.Message);
                //        sw.WriteLine("Stack Trace=" + ex.StackTrace);
                //    }
                //}
                return null;
            }
        }

        private IEnumerable<EncriptedUserRoleDTO> MapUserRoles(List<UserRole> enabledRoles)
        {

            List<EncriptedUserRoleDTO> userRoles = enabledRoles.Select(x => new EncriptedUserRoleDTO
            {
                EnabledSince = (x.EnabledSince == null) ? "" : Encription.EncriptStringAES(x.EnabledSince.ToString()),
                EnabledUntil = (x.EnabledUntil == null) ? "" : Encription.EncriptStringAES(x.EnabledUntil.ToString()),
                Notes = (x.Notes == null) ? "" : Encription.EncriptStringAES(x.Notes.ToString()),
                Order = (x.Order == null) ? "" : Encription.EncriptStringAES(x.Order.ToString()),
                IsDefaultOrganization = Encription.EncriptStringAES(false.ToString()),
                OrganizationId = (x.OrganizationId == null) ? "" : Encription.EncriptStringAES(x.OrganizationId.ToString()),
                OrganizationNameAr = (x.Organization == null) ? "" : Encription.EncriptStringAES(x.Organization.OrganizationNameAr),
                OrganizationNameEn = (x.Organization == null) ? "" : Encription.EncriptStringAES(x.Organization.OrganizationNameEn),
                OrganizationNameFn = (x.Organization == null) ? "" : Encription.EncriptStringAES(x.Organization.OrganizationNameFn),
                RoleId = (x.RoleId == null) ? "" : Encription.EncriptStringAES(x.RoleId.ToString()),
                RoleNameAr = (x.Role == null) ? "" : Encription.EncriptStringAES(x.Role.RoleNameAr),
                RoleNameEn = (x.Role == null) ? "" : Encription.EncriptStringAES(x.Role.RoleNameEn),
                RoleNameFn = (x.Role == null) ? "" : Encription.EncriptStringAES(x.Role.RoleNameFn),
                RoleOverridesUserPermissions = (x.RoleOverridesUserPermissions == null) ? Encription.EncriptStringAES(false.ToString()) : Encription.EncriptStringAES(x.RoleOverridesUserPermissions.ToString()),
                UserId = (x.UserId == null) ? "" : Encription.EncriptStringAES(x.UserId.ToString()),
                UserRoleId = (x.UserRoleId == null) ? "" : Encription.EncriptStringAES(x.UserRoleId.ToString()),
            }).ToList();
            return userRoles;
        }

        public DataSourceResult<UserSummaryDTO> getUsersByOrganization(DataSourceRequest dataSourceRequest, int? organizationID, int? RoleId)
        {
            if (organizationID > 0)
            {
                var data = 
                        getUsersByOrganization(int.Parse(organizationID.ToString()), RoleId).Where(u => !u.DeletedOn.HasValue)
                        .Select(u => new UserSummaryDTO
                        {
                            DefaultOrganizationAr = (u.UserRoleUsers.OrderByDescending(u => u.UserRoleId).Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTimeOffset.Now)).FirstOrDefault() != null) ? (u.UserRoleUsers.OrderByDescending(u => u.UserRoleId).Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTimeOffset.Now)).FirstOrDefault().Organization.OrganizationNameAr) : "",
                            DefaultOrganizationEn = (u.UserRoleUsers.OrderByDescending(u => u.UserRoleId).Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTimeOffset.Now)).FirstOrDefault() != null) ? (u.UserRoleUsers.OrderByDescending(u => u.UserRoleId).Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTimeOffset.Now)).FirstOrDefault().Organization.OrganizationNameEn) : "",
                            DefaultOrganizationFn = (u.UserRoleUsers.OrderByDescending(u => u.UserRoleId).Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTimeOffset.Now)).FirstOrDefault() != null) ? (u.UserRoleUsers.OrderByDescending(u => u.UserRoleId).Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTimeOffset.Now)).FirstOrDefault().Organization.OrganizationNameFn) : "",
                            Email = u.Email,
                            Enabled = u.Enabled,
                            FullNameAr = u.FullNameAr,
                            FullNameEn = u.FullNameEn,
                            FullNameFn = u.FullNameFn,
                            IndividualAttachmentId = u.IndividualAttachmentId,
                            IqamaNumber = u.IqamaNumber,
                            IsEmployee = u.IsEmployee,
                            IsGeneral = u.IsGeneral,
                            IsIndividual = u.IsIndividual,
                            Mobile = u.Mobile,
                            PassportNumber = u.PassportNumber,
                            ProfileImage = u.ProfileImage,
                            ProfileImage_string = u.ProfileImage.ToString(),
                            UserId = u.UserId,
                            UserName = u.Username,
                            WorkPhoneNumber = u.WorkPhoneNumber,
                            SSN = u.Ssn,
                            JobTitleId = u.JobTitleId,
                            EmployeeNumber = u.EmployeeNumber
                            //JobTitleAr = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameAr:"",
                            //JobTitleEn = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameEn :"",
                            //JobTitleFn = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameFn :"",
                        })
                        //.ProjectTo<UserSummaryDTO>(_Mapper.ConfigurationProvider, _SessionServices)
                        .ToDataSourceResult(dataSourceRequest);
                return data;
            }
            else
            {
                if (RoleId == null)
                {
                    // var users = .ToList();
                    var Users = (_UnitOfWork.GetRepository<User>())
                     .GetAll().Where(a => a.EnabledUntil == null || a.EnabledUntil > DateTime.Now);
                    // .Include(u => u.UserRoles)
                    var data = Users.Where(u => u.IsEmployee == true && !u.DeletedOn.HasValue)
                       .Select(u => new UserSummaryDTO
                       {

                           //DefaultOrganizationAr = (u.UserRoles.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault() != null) ? (u.UserRoles.Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameAr) : "",
                           //DefaultOrganizationEn = (u.UserRoles.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault() != null) ? (u.UserRoles.Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameEn) : "",
                           //DefaultOrganizationFn = (u.UserRoles.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault() != null) ? (u.UserRoles.Where(o => o.Role.IsEmployeeRole && (u.EnabledUntil == null || u.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameFn) : "",
                           DefaultOrganizationAr = u.UserRoleUsers.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameAr,
                           DefaultOrganizationEn = u.UserRoleUsers.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameEn,
                           DefaultOrganizationFn = u.UserRoleUsers.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameFn,
                           Email = u.Email,
                           Enabled = u.Enabled,
                           FullNameAr = u.FullNameAr,
                           FullNameEn = u.FullNameEn,
                           FullNameFn = u.FullNameFn,
                           IndividualAttachmentId = u.IndividualAttachmentId,
                           IqamaNumber = u.IqamaNumber,
                           IsEmployee = u.IsEmployee,
                           IsGeneral = u.IsGeneral,
                           IsIndividual = u.IsIndividual,
                           Mobile = u.Mobile,
                           PassportNumber = u.PassportNumber,
                           ProfileImage = u.ProfileImage,
                           ProfileImage_string = u.ProfileImage.ToString(),
                           UserId = u.UserId,
                           UserName = u.Username,
                           WorkPhoneNumber = u.WorkPhoneNumber,
                           SSN = u.Ssn,
                           JobTitleId = u.JobTitleId,
                           EmployeeNumber = u.EmployeeNumber
                           //JobTitleAr = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameAr : "",
                           //JobTitleEn = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameEn : "",
                           //JobTitleFn = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameFn : "",
                       }).ToDataSourceResult(dataSourceRequest);
                    //  var dataSource = new DataSourceResult<UserSummaryDTO>
                    //{
                    //    Count =data.Count(),
                    //    Data = data.ToList()
                    //};
                    return data;
                }
                else
                {
                    var data = (_UnitOfWork.GetRepository<User>())
                     .GetAll()
                     .Where(u => u.IsEmployee == true && !u.DeletedOn.HasValue && u.UserRoleUsers.Any(c => c.RoleId == (int)RoleId && (c.EnabledUntil == null || c.EnabledUntil >= DateTime.Now)))
                      .Select(u => new UserSummaryDTO
                      {
                          DefaultOrganizationAr = u.UserRoleUsers.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameAr,
                          DefaultOrganizationEn = u.UserRoleUsers.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameEn,
                          DefaultOrganizationFn = u.UserRoleUsers.Where(a => a.Role.IsEmployeeRole == true && (a.EnabledUntil == null || a.EnabledUntil > DateTime.Now)).FirstOrDefault().Organization.OrganizationNameFn,
                          Email = u.Email,
                          Enabled = u.Enabled,
                          FullNameAr = u.FullNameAr,
                          FullNameEn = u.FullNameEn,
                          FullNameFn = u.FullNameFn,
                          IndividualAttachmentId = u.IndividualAttachmentId,
                          IqamaNumber = u.IqamaNumber,
                          IsEmployee = u.IsEmployee,
                          IsGeneral = u.IsGeneral,
                          IsIndividual = u.IsIndividual,
                          Mobile = u.Mobile,
                          PassportNumber = u.PassportNumber,
                          ProfileImage = u.ProfileImage,
                          ProfileImage_string = u.ProfileImage.ToString(),
                          UserId = u.UserId,
                          UserName = u.Username,
                          WorkPhoneNumber = u.WorkPhoneNumber,
                          SSN = u.Ssn,
                          JobTitleId = u.JobTitleId,
                          EmployeeNumber = u.EmployeeNumber
                          //JobTitleAr = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameAr : "",
                          //JobTitleEn = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameEn : "",
                          //JobTitleFn = (u.JobTitleId != null) ? u.JobTitle.JobTitleNameFn : "",
                      })
                     .ToDataSourceResult(dataSourceRequest);
                    return data;
                }

            }
        }

        public UserRolePermissionsDTO getuserrolepermissionsbyusername_st(string username, bool isDelegated)
        {
            try
            {
                string usernameOriginal = "";
                try
                {
                    usernameOriginal = Encription.DecryptStringAES(username);
                }
                catch (Exception)
                {
                    usernameOriginal = username;
                }
                IEnumerable<UserRoles> _userRoles = this._userRoles.GetAll(false).Where(x => x.User.Username == usernameOriginal && x.Role.IsCommitteRole && ((isDelegated && x.SyncTypeId == null) || (!isDelegated)))
                                                .Select(Ur => new UserRoles
                                                {
                                                    userId = Ur.UserId,
                                                    UserRoleId = Ur.UserRoleId,
                                                    OrganizationId = Ur.OrganizationId,
                                                    OrganizationName = _sessionServices.Culture.ToLower() == "ar" ? Ur.Organization.OrganizationNameAr
                                                                     : _sessionServices.Culture.ToLower() == "en" ? Ur.Organization.OrganizationNameEn
                                                                     : Ur.Organization.OrganizationNameFn,
                                                    RoleId = Ur.RoleId,
                                                    RoleName = _sessionServices.Culture.ToLower() == "ar" ? Ur.Role.RoleNameAr
                                                             : _sessionServices.Culture.ToLower() == "en" ? Ur.Role.RoleNameEn
                                                             : Ur.Role.RoleNameFn,
                                                    IsEmployeeRole = Ur.Role.IsEmployeeRole,
                                                    IsDelegatedEmployeeRole = Ur.Role.IsDelegatedEmployeeRole,
                                                    RoleOverridesUserPermissions = Ur.RoleOverridesUserPermissions,
                                                    IsActive = (!Ur.EnabledSince.HasValue || Ur.EnabledSince < DateTimeOffset.Now) && (!Ur.EnabledUntil.HasValue || Ur.EnabledUntil > DateTimeOffset.Now),
                                                    Order = Ur.Order,
                                                    IsOrganizationDeleted = Ur.Organization.DeletedBy == null ? false : true,
                                                    EnabledSince = Ur.EnabledSince,
                                                    EnabledUntil = Ur.EnabledUntil,
                                                    Notes = Ur.Notes,
                                                    CreatedByUser = _sessionServices.Culture.ToLower() == "ar" ? Ur.CreatedByNavigation.FullNameAr
                                                             : _sessionServices.Culture.ToLower() == "en" ? Ur.CreatedByNavigation.FullNameEn
                                                             : Ur.CreatedByNavigation.FullNameFn,
                                                }).OrderByDescending(x => x.RoleId);



                UserRolePermissionsDTO Result = new UserRolePermissionsDTO()
                {
                    UserId = _userRoles != null && _userRoles.Count()>0 ? _userRoles.FirstOrDefault().userId : 0,
                    UserRoles = _userRoles
                };
                return Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public IEnumerable<UserDetailsDTO> UpdateUsers(IEnumerable<UserDetailsDTO> Entities)
        {
           // Entities.ForEach(e => { e.UserId = Convert.ToInt32(_dataProtectService.Decrypt(e.UserIdEncrypt)); });
            UserDetailsDTO user = Entities.SingleOrDefault();


            //check if ther is no empRol >> add empRol
            UserRole userEmpRol = (_UnitOfWork.GetRepository<UserRole>())
                .GetAll()
                .Include(u => u.Role)
                .FirstOrDefault(ur => ur.UserId == user.UserId && ur.Role.IsEmployeeRole == true && (ur.EnabledUntil == null || ur.EnabledUntil > DateTime.Now));
            if (userEmpRol == null)
            {
                var userrole = (_UnitOfWork.GetRepository<UserRole>())
                .GetAll()
                .Include(u => u.Role)
                .FirstOrDefault(ur => ur.UserId == user.UserId && ur.Role.IsEmployeeRole == true && ur.OrganizationId == user.DefaultOrganizationId);
                if (userrole == null)
                {
                    userEmpRol = InsertEmpUserRol(user.UserId, user.DefaultOrganizationId);

                }
                else
                {
                    var w=_context.Database.ExecuteSqlInterpolated($"UPDATE UserRoles SET [EnabledUntil] = null WHERE UserRoleId = {userrole.UserRoleId}"
                      );
                    _context.SaveChanges();
                }
            }
            else
            {
                //check if change org >> change org in empRol
                if (userEmpRol.OrganizationId != user.DefaultOrganizationId)
                {
                    _context.Database.ExecuteSqlInterpolated($"UPDATE UserRoles SET [EnabledUntil] = {DateTimeOffset.Now} WHERE UserRoleId = {userEmpRol.UserRoleId}"
                    );
                    _context.SaveChanges();
                }
                var userrole = (_UnitOfWork.GetRepository<UserRole>())
                .GetAll()
                .Include(u => u.Role)
                .FirstOrDefault(ur => ur.UserId == user.UserId && ur.Role.IsEmployeeRole == true && ur.OrganizationId == user.DefaultOrganizationId);
                if (userrole == null)
                {
                    InsertEmpUserRol(user.UserId, user.DefaultOrganizationId);

                }
                else
                {
                   var e= _context.Database.ExecuteSqlInterpolated($"UPDATE UserRoles SET [EnabledUntil] = null WHERE UserRoleId = {userrole.UserRoleId}"
                      );
                    _context.SaveChanges();
                }

                //update current rol
                //_uow.RunSqlCommand("UPDATE UserRoles SET OrganizationId = @OrganizationId WHERE UserRoleId = @UserRoleId",
                //    new SqlParameter("UserRoleId", userEmpRol.UserRoleId),
                //    new SqlParameter("OrganizationId", user.DefaultOrganizationId)
                //    );
                //_UnitOfWork.Save();

                //}
            }


            //save user correspondent organizations
            SaveUserCorrespondentOrganizations(user);


            //IEnumerable<UserDetailsDTO> updateUserDTO = base.Update(Entities);
            foreach (var Entity in Entities)
            {
                var OldEntity = this._UnitOfWork.GetRepository<User>().GetById(Entity.UserId);
                byte[] profileImage = OldEntity.ProfileImage;
                User MappedEntity = _Mapper.Map(Entity, OldEntity, typeof(UserDetailsDTO), typeof(User)) as User;
                MappedEntity.ProfileImage = profileImage;
                this._UnitOfWork.GetRepository<User>().Update(MappedEntity as User);
                _UnitOfWork.SaveChanges();
            }
            return Entities.Select(e => new UserDetailsDTO { UserName = e.UserName ,IsMobileUser = e.IsMobileUser,AuditUser = e.AuditUser});
        }

        public void SaveUserCorrespondentOrganizations(UserDetailsDTO user)
        {
            if (user != null)
            {
                //delete old
                IQueryable<UserCorrespondentOrganization> UserCorrespondentOrganizations = _uow.GetRepository<UserCorrespondentOrganization>().GetAll().Where(x => x.UserId == user.UserId);
                _uow.GetRepository<UserCorrespondentOrganization>().Delete(UserCorrespondentOrganizations);



                //save new
                if(user.UserCorrespondentOrganizationIds !=null && user.UserCorrespondentOrganizationIds.Count() > 0)
                {
                user.UserCorrespondentOrganizationIds.ForEach(x =>
                {
                    _uow.GetRepository<UserCorrespondentOrganization>().Insert(new UserCorrespondentOrganization { UserId = user.UserId, OrganizationId = x, });


                });

                }


            }
        }


        public IQueryable<User> getUsersByOrganization(int organizationID, int? UserRoleId)
        {
            return (from u in _context.Users
                    join J in _context.JobTitles
                    on u.JobTitleId equals J.JobTitleId
                    join uR in _context.UserRoles
                      on u.UserId equals uR.UserId
                    where uR.OrganizationId == organizationID && (uR.EnabledUntil == null || uR.EnabledUntil > DateTime.Now) && u.IsEmployee == true && (UserRoleId == null || (uR.RoleId == (int)UserRoleId))
                    select u).Distinct();
        }
    }
}
