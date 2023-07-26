using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Service.Sevices;
using DbContexts.MasarContext.ProjectionModels;
using IHelperServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly ISignalRServices _signalRServices;
        ICommitteeNotificationService _committeeNotificationService;
        private readonly IHelperServices.ISessionServices _SessionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public AccountController(ICommitteeNotificationService committeeNotificationService, IUsersService usersService, ITokenStoreService tokenStoreService, IHelperServices.ISessionServices sessionServices, IHttpContextAccessor httpContextAccessor, ISignalRServices signalRServices)
        {
            _usersService = usersService;
            _committeeNotificationService = committeeNotificationService;
            _tokenStoreService = tokenStoreService;
            _SessionServices = sessionServices;
            _signalRServices = signalRServices;
            _httpContextAccessor = httpContextAccessor;
            
        }
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = "Windows")]
        //[IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(AccessToken))]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginUser, string culture)
        {
            // 
            EncryotAndDecryptLoginUserDTO Encr_Data = new EncryotAndDecryptLoginUserDTO();
            //int LOGIN_Way = _usersService.CheckLoginWay();
            //if (LOGIN_Way != 2 && loginUser.DisableSSO && ((LOGIN_Way == 0 || LOGIN_Way == 3) && loginUser.DisableSSO))
            //{

            //    Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
            //}
            int LOGIN_Way = _usersService.CheckLoginWay(); // One Value Now Equal 0
            User _User = null;
            bool lsLdapAuth = true;
            bool isFactorAuth = false;

            //SET CULTURE SESSION
            _SessionServices.Culture = culture;

            //SET ApplicationType SESSION
            _SessionServices.ApplicationType = loginUser.applicationType;

            loginUser.Username = Encription.DecryptStringAES(loginUser.Username);
            loginUser.Password = Encription.DecryptStringAES(loginUser.Password);



            //Window Authentication
            if (LOGIN_Way == 1)
            {
                if (string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
                  {
                    //_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value

                    var s = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    //var SID2 = UserPrincipal.Current.Sid.Value;
                    if (System.IO.File.Exists("C:\\comlog.txt"))
                    {
                        using (StreamWriter sw = System.IO.File.AppendText("C:\\comlog.txt"))
                        {
                            sw.WriteLine("s =" + s);
                            //sw.WriteLine("SID2 =" + SID2);
                        }
                    }
                    var xuserName = User.Identity?.Name;
                    string userName1 = _httpContextAccessor.HttpContext.User.Identity.Name;
                    if (System.IO.File.Exists("C:\\comlog.txt"))
                    {
                        using (StreamWriter sw = System.IO.File.AppendText("C:\\comlog.txt"))
                        {
                            sw.WriteLine("userName =" + userName1);
                        }
                    }

                    var SID = _httpContextAccessor.HttpContext.User?.FindFirst(c => c.Type == ClaimTypes.PrimarySid)?.Value;//.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (System.IO.File.Exists("C:\\comlog.txt"))
                    {
                        using (StreamWriter sw = System.IO.File.AppendText("C:\\comlog.txt"))
                        {
                            sw.WriteLine("xuserName =" + xuserName);
                            sw.WriteLine("SID =" + SID);
                        }
                    }
                    string userName = _usersService.AuthenticateADUserBySID(SID);


                    if (!string.IsNullOrEmpty(userName))
                    {
                        _User = await _usersService.FindUserByUserNameAsync(userName);
                    }
                    if (_User != null)
                    {
                        isFactorAuth = true;
                        //loginUser.Username = _User.Username;
                        //Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
                    }
                }
                
                else if (await _usersService.AuthenticateADUser(loginUser.Username, loginUser.Password))
                {
                    //Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
                    _User = await _usersService.FindUserByUserNameAsync(loginUser.Username);
                }
                else
                {
                    //Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
                    _User = await _usersService.FindUserPasswordAsync(loginUser.Username, loginUser.Password,/* (LOGIN_Way == 2 && !loginUser.DisableSSO) ? true : */false);
                    lsLdapAuth = false;
                }

                if (_User == null || !_User.Enabled || _User.ExternalUser)
                {
                    return Unauthorized();
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false });
                }
                if (_User.IsLocked || (!_User.IsMobileuser && loginUser.applicationType == "2"))
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false, IsLocked = true });
                }
                if (_User.PasswordUpdatedOn == null)
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
                }

            }
            if (string.IsNullOrEmpty(loginUser.Username.Trim()) && LOGIN_Way != 1)
            {
                return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth });
            }

            //implement SSO Login
            if (LOGIN_Way == 0)
            {
                bool isADAuthenticated = await _usersService.AuthenticateADUser(loginUser.Username, loginUser.Password);

                if (isADAuthenticated)
                {
                    _User = await _usersService.FindUserByUserNameAsync(loginUser.Username);
                }
                else
                {
                    _User = await _usersService.FindUserPasswordAsync(loginUser.Username, loginUser.Password,((LOGIN_Way == 0 && !loginUser.DisableSSO)) ? true :false);
                     lsLdapAuth = false;
                }

                if (_User == null || !_User.Enabled /*|| _User.ExternalUser*/)
                {
                    //return Unauthorized();
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false });
                }
                if (_User.IsLocked || (!_User.IsMobileuser && loginUser.applicationType == "2"))
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false, IsLocked = true });
                }
                if (_User.PasswordUpdatedOn == null)
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
                }
            }

            if (LOGIN_Way == 4 || LOGIN_Way == 5)
            {
                _User = await _usersService.FindUserByUserNameAsync(loginUser.Username);
            }

            if (_User.HasFactorAuth && ((loginUser.applicationType == "1" && _usersService.CheckFactorAuthSetting("FactorAuth").ToUpper() == "TRUE") ||
              (loginUser.applicationType == "2" && _usersService.CheckFactorAuthSetting("FactorAuthMobile").ToUpper() == "TRUE")) && !isFactorAuth)
            {
                _usersService.InsertUserFactorAuth(_User.UserId, int.Parse(loginUser.applicationType));
                if (_User.IsLocked || (!_User.IsMobileuser && loginUser.applicationType == "2"))
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_factor_auth = true, IsLocked = true, IsAdmin = _User.IsAdmin });
                }
                if (_User.PasswordUpdatedOn == null)
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
                }
                return Ok(new AccessToken { access_token = null, refresh_token = null, is_factor_auth = true });
            }
            if (_User.HasSignatureFactorAuth && ((loginUser.applicationType == "1" && _usersService.CheckFactorAuthSetting("SignatureFactorAuth").ToUpper() == "TRUE") ||
           (loginUser.applicationType == "2" && _usersService.CheckFactorAuthSetting("SignatureFactorAutMobile").ToUpper() == "TRUE")) && !isFactorAuth)
            {
                int application_Type = int.Parse(loginUser.applicationType);
                //  _usersService.InsertUserSignatureFactorAuth(_User.UserId);
                if (_User.IsLocked || _User.ExternalUser || (!_User.IsMobileuser && loginUser.applicationType == "2"))
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, signature_factor_auth = true, IsLocked = true, IsAdmin = _User.IsAdmin });
                }
                if (_User.PasswordUpdatedOn == null)
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
                }
                var (access_Token, refresh_Token, claimsFromTwoFactorAuth) = await _tokenStoreService.CreateJwtTokens(_User, application_Type, refreshTokenSource: null);
                bool IsHasRole = _User.UserId > 0 && access_Token == null ? false : true;
                return Ok(new AccessToken { access_token = access_Token, refresh_token = refresh_Token, signature_factor_auth = true, HasRole = IsHasRole });
            }
            // normal username & Password 
            //else
            //{

            //   _User = await _usersService.FindUserPasswordAsync(loginUser.Username, loginUser.Password, false);
            //}
            
            if (_User == null || !_User.Enabled)
            {
                    //return Unauthorized();
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = false, IsTemp = false });
                
            }
            

            IEnumerable<UserRole> EnabledUserRoles = _User.UserRoleUsers
                      .Where(x => x.Role.IsCommitteRole)
                      .Where(x => x.EnabledUntil == null || x.EnabledUntil > DateTimeOffset.Now);
                if (EnabledUserRoles.Count() == 0)
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = false, IsTemp = false });
                }
                int applicationType = int.Parse(loginUser.applicationType);

                if (_User.IsLocked || (!_User.IsMobileuser && loginUser.applicationType == "2"))
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_factor_auth = false, IsLocked = true, IsAdmin = _User.IsAdmin });
                }
                if (_User.PasswordUpdatedOn == null)
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
                }
                
                var (accessToken, refreshToken,claims) = await _tokenStoreService.CreateJwtTokens(_User, applicationType ,refreshTokenSource: null);
                return Ok(new AccessToken { access_token = accessToken, refresh_token = refreshToken, is_mobile = applicationType == (int)ApplicationTypeEnum.mobile ? true : false, is_ldap_auth = lsLdapAuth, is_factor_auth = false, UserTN = Encr_Data.UserTN, UserTP = Encr_Data.UserTP, IsAdmin = _User.IsAdmin });
            //}
            //_User = await _usersService.FindUserPasswordAsync(loginUser.Username, loginUser.Password, false);
            //if (_User == null)
            //{
            //    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = false, IsTemp = false });
            //}
            #region Commented Code from Masar
            // if (LOGIN_Way == 1)
            // {
            //     //string currentUserName = _usersService.GetCurrentUserNameBySid();
            //     //if (currentUserName != string.Empty)
            //     //{
            //     //    _User = await _usersService.FindUserByUserNameAsync(currentUserName);
            //     //}
            //     //if (_User == null || !_User.Enabled)
            //     //{
            //     //    //return Unauthorized();
            //     //    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth });
            //     //}
            //     //if (_User.IsLocked)
            //     //{
            //     //    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsLocked = true });
            //     //}
            //     //if (_User.PasswordUpdatedOn == null)
            //     //{
            //     //    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsTemp = true });
            //     //}
            //     if (string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
            //     {
            //         //var s = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //         //var SID = UserPrincipal.Current.Sid.Value;
            //         var xuserName = User.Identity?.Name;
            //         //string userName = _httpContextAccessor.HttpContext.User.Identity.Name;

            //         var SID = _httpContextAccessor.HttpContext.User?.FindFirst(c => c.Type == ClaimTypes.PrimarySid)?.Value;//.FindFirst(ClaimTypes.NameIdentifier).Value;
            //         string userName = _usersService.AuthenticateADUserBySID(SID);
            //         if (System.IO.File.Exists("C:\\logsms.txt"))
            //         {
            //             //using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
            //             //{
            //             //    sw.WriteLine("start Name" + wp.Identity.Name);
            //             //}
            //             //using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
            //             //{
            //             //    sw.WriteLine("start Owner" + System.Security.Principal.WindowsIdentity.GetCurrent().Owner);
            //             //}
            //             using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
            //             {
            //                 sw.WriteLine("start xusername" + xuserName);
            //             }
            //             using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
            //             {
            //                 sw.WriteLine("start SID" + SID);
            //             }
            //             using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
            //             {
            //                 sw.WriteLine("start Username" + userName);
            //             }
            //             using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
            //             {
            //                 foreach (var item in System.Security.Principal.WindowsIdentity.GetCurrent().Claims)
            //                 {
            //                     sw.WriteLine("ClaimTypes SID" + item.Value + " type=" + item.Type);

            //                 }
            //             }
            //         }
            //         if (System.IO.File.Exists("C:\\logsms.txt"))
            //         {
            //             using (StreamWriter sw = System.IO.File.AppendText("C:\\logsms.txt"))
            //             {
            //                 sw.WriteLine("start userName_1=" + userName);
            //             }
            //         }
            //         if (!string.IsNullOrEmpty(userName))
            //         {
            //             _User = await _usersService.FindUserByUserNameAsync(userName);
            //         }
            //         if (_User != null)
            //         {
            //             isFactorAuth = true;
            //             //loginUser.Username = _User.Username;
            //             //Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
            //         }
            //     }
            //     else if (_usersService.AuthenticateADUser(loginUser.Username, loginUser.Password))
            //     {
            //         Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
            //         _User = await _usersService.FindUserByUserNameAsync(loginUser.Username);
            //     }
            //     else
            //     {
            //         Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
            //         _User = await _usersService.FindUserPasswordAsync(loginUser.Username, loginUser.Password, (LOGIN_Way == 2 && !loginUser.DisableSSO) ? true : false);
            //         lsLdapAuth = false;
            //     }

            //     if (_User == null || !_User.Enabled)
            //     {
            //         //return Unauthorized();
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false });
            //     }
            //     if (_User.IsLocked)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false, IsLocked = true });
            //     }
            //     if (_User.PasswordUpdatedOn == null)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
            //     }


            // }
            // if (string.IsNullOrEmpty(loginUser.Username.Trim()) && LOGIN_Way != 1)
            // {
            //     return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth });
            // }

            // if (LOGIN_Way == 0 || LOGIN_Way == 2 || (LOGIN_Way == 3 && loginUser.DisableSSO))
            // {
            //     if (LOGIN_Way == 2 && loginUser.Continue)
            //     {
            //         Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
            //     }
            //     bool isADAuthenticated = _usersService.AuthenticateADUser(loginUser.Username, loginUser.Password);

            //     if (isADAuthenticated)
            //     {
            //         _User = await _usersService.FindUserByUserNameAsync(loginUser.Username);
            //     }
            //     else
            //     {
            //         _User = await _usersService.FindUserPasswordAsync(loginUser.Username, loginUser.Password, (LOGIN_Way == 2 && !loginUser.DisableSSO) ? true : false);
            //         lsLdapAuth = false;
            //     }

            //     if (_User == null || !_User.Enabled)
            //     {
            //         //return Unauthorized();
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false });
            //     }
            //     if (_User.IsLocked)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false, IsLocked = true });
            //     }
            //     if (_User.PasswordUpdatedOn == null)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
            //     }
            // }
            // if (LOGIN_Way == 4)
            // {
            //     _User = await _usersService.FindUserByUserNameAsync(loginUser.Username);
            // }
            // if (LOGIN_Way == 3 && !loginUser.DisableSSO)
            // {
            //     Dictionary<string, string> postParams = new Dictionary<string, string>();
            //     using (var httpClient = new HttpClient())
            //     {
            //         var request = new HttpRequestMessage
            //         {
            //             Method = HttpMethod.Get,
            //             RequestUri = new Uri(_systemSettingsService.GetSystemSettingByCode("API_URL_UJ").SystemSettingValue),
            //         };
            //         var api_UserName = _systemSettingsService.GetSystemSettingByCode("API_UserName_UJ").SystemSettingValue;
            //         var api_Password = _systemSettingsService.GetSystemSettingByCode("API_Password_UJ").SystemSettingValue;
            //         Encr_Data = _usersService.Decrypet_And_Encrypt_loginUser(ref loginUser);
            //         postParams.Add("API_UserName", api_UserName);
            //         postParams.Add("API_Password", api_Password);
            //         postParams.Add("UserName", loginUser.Username);
            //         postParams.Add("Password", loginUser.Password);
            //         postParams.Add("IP", "");
            //         postParams.Add("MAC", "");
            //         request.Content = new FormUrlEncodedContent(postParams);
            //         var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            //         var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //         var deserialized = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            //         if (response.StatusCode == HttpStatusCode.Accepted)
            //         {
            //             _User = await _usersService.FindUserByUserNameAsync(deserialized.ERP_EmployeeNumber);
            //             if (_User == null || !_User.Enabled)
            //             {
            //                 return Ok(new AccessToken { access_token = null, refresh_token = null, ERP_EmployeeNumber = deserialized.ERP_EmployeeNumber, is_ldap_auth = false });
            //             }
            //         }
            //         else if (response.StatusCode == HttpStatusCode.BadRequest)
            //         {
            //             return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false, ResponseMessage = deserialized.Message });

            //         }
            //         else if (response.StatusCode == HttpStatusCode.Unauthorized)
            //         {
            //             return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false, ResponseMessage = deserialized.Message });

            //         }
            //         else if (response.StatusCode == HttpStatusCode.NotAcceptable)
            //         {
            //             return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false });
            //         }
            //     }
            // }

            // if (_User.HasFactorAuth && ((loginUser.applicationType == "1" && _usersService.CheckFactorAuthSetting("FactorAuth").ToUpper() == "TRUE") ||
            //     (loginUser.applicationType == "2" && _usersService.CheckFactorAuthSetting("FactorAuthMobile").ToUpper() == "TRUE")) && !isFactorAuth)
            // {
            //     _usersService.InsertUserFactorAuth(_User.UserId);
            //     if (_User.IsLocked)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_factor_auth = true, IsLocked = true, IsAdmin = _User.IsAdmin });
            //     }
            //     if (_User.PasswordUpdatedOn == null)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
            //     }
            //     return Ok(new AccessToken { access_token = null, refresh_token = null, is_factor_auth = true });
            // }
            // if (_User.HasSignatureFactorAuth && ((loginUser.applicationType == "1" && _usersService.CheckFactorAuthSetting("SignatureFactorAuth").ToUpper() == "TRUE") ||
            //(loginUser.applicationType == "2" && _usersService.CheckFactorAuthSetting("SignatureFactorAutMobile").ToUpper() == "TRUE")) && !isFactorAuth)
            // {
            //     int applicationType = int.Parse(loginUser.applicationType);
            //     //  _usersService.InsertUserSignatureFactorAuth(_User.UserId);
            //     if (_User.IsLocked)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, signature_factor_auth = true, IsLocked = true, IsAdmin = _User.IsAdmin });
            //     }
            //     if (_User.PasswordUpdatedOn == null)
            //     {
            //         return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
            //     }
            //     var (accessToken, refreshToken, claims) = await _tokenStoreService.CreateJwtTokens(_User, applicationType, refreshTokenSource: null);
            //     return Ok(new AccessToken { access_token = accessToken, refresh_token = refreshToken, signature_factor_auth = true });
            // }

            // else
            // {
            #endregion
            
            //if (_User.IsLocked)
            //{
            //    return Ok(new AccessToken { access_token = null, refresh_token = null, is_factor_auth = false, IsLocked = true, IsAdmin = _User.IsAdmin });
            //}
            //if (_User.PasswordUpdatedOn == null)
            //{
            //    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth, IsAdmin = _User.IsAdmin, IsTemp = true });
            //}
            //_antiforgery.RegenerateAntiForgeryCookies(claims);
            
            //}
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(AccessToken))]
        public async Task<IActionResult> CheckVerfCode(string FactorAuthCode, string UserName, string applicationType, string culture)
        {
            UserName = Encription.DecryptStringAES(UserName);

            if ((bool)_usersService.CheckFactorAuthValidation(FactorAuthCode, UserName))
            {
                //_usersService.TempErrorLogger("*************************************************");

                //_usersService.TempErrorLogger("-User " + UserName + " is verified with auth code " + FactorAuthCode);

                User _User = null;
                bool lsLdapAuth = true;

                //SET CULTURE SESSION
                _SessionServices.Culture = culture;
                //_usersService.TempErrorLogger("-Culture: " + _SessionServices.Culture);

                //SET ApplicationType SESSION
                _SessionServices.ApplicationType = applicationType;
                int LOGIN_Way = applicationType == "2" ? 0 : _usersService.CheckLoginWay();

                if (LOGIN_Way == 1)
                {
                    string currentUserName = _usersService.GetCurrentUserNameBySid();
                    if (currentUserName != string.Empty)
                    {
                        _User = await _usersService.FindUserByUserNameAsync(currentUserName);
                    }
                    if (_User == null || !_User.Enabled)
                    {
                        //return Unauthorized();
                        return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth });
                    }
                }

                if (string.IsNullOrEmpty(UserName.Trim()))
                {
                    return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = lsLdapAuth });
                }

                if (LOGIN_Way == 0)
                {
                    _User = await _usersService.FindUserByUserNameAsync(UserName);
                    lsLdapAuth = false;
                    if (_User == null || !_User.Enabled)
                    {
                        //return Unauthorized();
                        return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false });
                    }
                }
                if (LOGIN_Way == 3)
                {
                    _User = await _usersService.FindUserByUserNameAsync(UserName);
                    if (_User == null || !_User.Enabled)
                    {
                        //return Unauthorized();
                        return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false });
                    }
                }
                int _applicationType = int.Parse(_SessionServices.ApplicationType);

                //_usersService.TempErrorLogger("-ApplicationType: " + _SessionServices.ApplicationType);
                //_usersService.TempErrorLogger("-User Object : " + _User);

                var (accessToken, refreshToken, claims) = await _tokenStoreService.CreateJwtTokens(_User, _applicationType, refreshTokenSource: null);
                return Ok(new AccessToken { access_token = accessToken, refresh_token = refreshToken, is_mobile = _applicationType == (int)ApplicationTypeEnum.mobile ? true : false, is_ldap_auth = lsLdapAuth, is_factor_auth = true });
            }
            return Ok(new AccessToken { access_token = null, refresh_token = null, is_ldap_auth = false, is_factor_auth = true });
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult ResendVerfCode(string UserName)
        {
            UserName = Encription.DecryptStringAES(UserName);

            if ((bool)_usersService.ResetUserFactorAuth(UserName))
            {
                return Ok("True");
            }
            return BadRequest("False");
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult GenericResendVerfCode(string UserName)
        {
            UserName = Encription.DecryptStringAES(UserName);

            if ((bool)_usersService.GenericResetUserFactorAuth(UserName))
            {
                return Ok("True");
            }
            return BadRequest("False");
        }


        [HttpGet("[action]")]
        [ProducesResponseType(200, Type = typeof(EncriptedAuthTicketDTO))]
        [Authorize]
        public IActionResult GetUserAuthTicket(int? CommitteeId, int? roleId, bool? personal = false)
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            string Username = _usersService.Decrypte(claimsIdentity.Name);
            EncriptedAuthTicketDTO AuthTicket = this._usersService.GetUserAuthTicket(Username, CommitteeId, roleId, personal);
            return Ok(AuthTicket != null ? AuthTicket : null);
        }
        // Fake  method to get AuthTicketDTO in swagger
        [HttpGet("[action]")]
        [ProducesResponseType(200, Type = typeof(AuthTicketDTO))]
        [Authorize]
        public IActionResult GetAuthTicketFake(int? CommitteeId, int? roleId, bool? personal = false)
        {

            AuthTicketDTO AuthTicket = new AuthTicketDTO();
            return Ok(AuthTicket != null ? AuthTicket : null);
        }
        [Authorize]
        [HttpGet("[action]")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> Logout(string refreshToken)
        {
            try
            {
                ClaimsIdentity claimsIdentity = this.User.Identity as ClaimsIdentity;
                string userIdValue = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;

                // The Jwt implementation does not support "revoke OAuth token" (logout) by design.
                // Delete the user's tokens from the database (revoke its bearer token)
                await _tokenStoreService.RevokeUserBearerTokensAsync(userIdValue, refreshToken);
                string[] ExecptParm = new string[] { };
                _SessionServices.ClearSessionsExcept(ExecptParm);
                // _antiforgery.DeleteAntiForgeryCookies();
                _signalRServices.SignOut(User.Identity.Name);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        
    }
}
