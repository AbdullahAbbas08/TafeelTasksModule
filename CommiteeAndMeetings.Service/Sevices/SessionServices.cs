using DbContexts.MasarContext.ProjectionModels;
using HelperServices;
using IHelperServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class SessionServices : _HelperService, ISevices.ISessionServices
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEncryptionServices _EncryptionServices;
        private readonly string Key = "23A5A8E6-9000-4D61-9E1C-6C498D14EDF5"; //Key For Encryption and Decryption

        public string Encrypt(string clearText)
        {
            try
            {
                if (string.IsNullOrEmpty(clearText))
                    return null;
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (System.Security.Cryptography.Aes encryptor = System.Security.Cryptography.Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray()).Replace("/", "CfDJ8OBfQIsnvBREihT6eG7K").Replace("+", "CfDfQIsnvBREihT6eG7K").Replace("=", "CfDJ8OBfQIsnvT6eG7K");
                    }
                }
                return clearText;
            }
            catch (Exception) { return null; }

        }

        public string Decrypt(string cipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return "";
                cipherText = cipherText.Replace("CfDJ8OBfQIsnvBREihT6eG7K", "/").Replace("CfDfQIsnvBREihT6eG7K", "+").Replace("CfDJ8OBfQIsnvT6eG7K", "=");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (System.Security.Cryptography.Aes encryptor = System.Security.Cryptography.Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception) { return null; }

        }
        public SessionServices(IHttpContextAccessor httpContextAccessor, IEncryptionServices encryptionServices)
        {
            _HttpContextAccessor = httpContextAccessor;
            _EncryptionServices = encryptionServices;
        }

        public HttpContext HttpContext
        {
            get
            {
                return _HttpContextAccessor.HttpContext;
            }
            set
            {
                _HttpContextAccessor.HttpContext = value;
            }
        }

        public int? UserId
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null)
                    return null;
                return int.Parse(claim.Value);
            }
        }

        public string UserName
        {
            get
            {
                if (HttpContext.User == null || HttpContext.User.Identity == null)
                    return null;
                return Decrypt(HttpContext.User.Identity.Name);
            }
        }

        public int? OrganizationId
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("CurrentOrganizationId"));
                if (claim == null)
                    return null;
                return int.Parse(Decrypt(claim.Value));
            }
        }

        public int UserRoleId
        {
            get
            {
                if (HttpContext.User == null)
                    return 0;
                var claim = HttpContext.User.FindFirst(Encrypt("UserRoleId"));
                if (claim == null)
                    return 0;
                return int.Parse(Decrypt(claim.Value));
            }
            set => HttpContext.Session.SetString("UserRoleId", ToString());
        }

        public int? RoleId
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("CurrentRoleId"));
                if (claim == null)
                    return null;
                return int.Parse(Decrypt(claim.Value));
            }
        }
        public string UserTokenId
        {
            get => string.IsNullOrEmpty(HttpContext.Session.GetString("userTokenId")) ? "0" : HttpContext.Session.GetString("userTokenId");
            set => HttpContext.Session.SetString("userTokenId", value.ToString());
        }

        //public string MachineName
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //}

        //public string MachineIP
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //}

        //public string Browser
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //}

        //public string Url
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //}

        public string Culture
        {
            get => string.IsNullOrEmpty(HttpContext.Session.GetString("culture")) ? "ar" : HttpContext.Session.GetString("culture");
            set => HttpContext.Session.SetString("culture", value.ToString());
        }

        public bool CultureIsArabic
        {
            get
            {
                return Culture == "ar";
            }
        }


        public string ApplicationType
        {
            get => string.IsNullOrEmpty(HttpContext.Session.GetString("applicationType")) ? "1" : HttpContext.Session.GetString("applicationType");
            set => HttpContext.Session.SetString("applicationType", value.ToString());
        }

        public string EmployeeFullNameAr
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("UserFullNameAr"));
                if (claim == null)
                    return null;
                return Decrypt(claim.Value).ToString();
            }
        }

        public string EmployeeFullNameEn
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("UserFullNameEn"));
                if (claim == null)
                    return null;
                return Decrypt(claim.Value).ToString();
            }
        }

        public string OrganizationNameAr
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("CurrentOrganizationNameAr"));
                if (claim == null)
                    return null;
                return Decrypt(claim.Value).ToString();
            }
        }

        public string OrganizationNameEn
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("CurrentOrganizationNameEn"));
                if (claim == null)
                    return null;
                return Decrypt(claim.Value).ToString();
            }
        }

        public string RoleNameAr
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("CurrentRoleNameAr"));
                if (claim == null)
                    return null;
                return Decrypt(claim.Value).ToString();
            }
        }

        public string RoleNameEn
        {
            get
            {
                if (HttpContext.User == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("CurrentRoleNameEn"));
                if (claim == null)
                    return null;
                return Decrypt(claim.Value).ToString();
            }
        }

        public string ClientIP
        {
            get
            {
                if (HttpContext.User == null || HttpContext.User.Identity == null)
                    return null;
                return HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }
        public string FaxUserId
        {
            get
            {
                if (HttpContext.User == null || HttpContext.User.Identity == null)
                    return null;
                var claim = HttpContext.User.FindFirst(Encrypt("FaxUserId"));
                if (claim == null)
                    return null;
                return Decrypt(claim.Value).ToString();
            }
        }

        //public string ClaimTest { get { return GetClaim<string>("S"); } }

        public void SetAuthTicket(string username, EncriptedAuthTicketDTO authTicket)
        {
            HttpContext.Session.SetString(username.ToUpper(), JsonConvert.SerializeObject(authTicket));
        }

        public EncriptedAuthTicketDTO GetAuthTicket(string username)
        {
            var Auth = HttpContext.Session.GetString(username.ToUpper());
            if (Auth != null)
                return JsonConvert.DeserializeObject<EncriptedAuthTicketDTO>(Auth);
            else
                return null;
        }


        #region Private Methods

        private T GetClaim<T>(string key, T defaultValue = default(T))
        {
            T result = defaultValue;
            var value = HttpContext.User.HasClaim(x => x.Type == key) ? HttpContext.User.FindFirst(key).Value : null;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    Type t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                    result = (T)Convert.ChangeType(value, t);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        private string SharedEncryptionKey { get; set; }

        private string EncryptionKey
        {
            get
            {
                var result = SharedEncryptionKey ?? GetCookie<string>("_k", null, false);
                if (result == null)
                {
                    result = Guid.NewGuid().ToString();
                    SharedEncryptionKey = result;
                    SetCookie("_k", result, false);
                }
                return result;
            }
        }

        private void SetCookie<T>(string key, T value, bool encrypt = true)
        {
            var str = Convert.ToString(value);
            if (encrypt)
            {
                str = _EncryptionServices.EncryptString(str, EncryptionKey, key);
            }
            _HttpContextAccessor.HttpContext.Response.Cookies.Append(key, str);
        }

        private T GetCookie<T>(string key, T defaultValue = default(T), bool decrypt = true)
        {
            T result = defaultValue;
            var value = _HttpContextAccessor.HttpContext.Request.Cookies[key];
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (decrypt)
                    {
                        value = _EncryptionServices.DecryptString(value, EncryptionKey, key);
                    }
                    Type t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                    result = (T)Convert.ChangeType(value, t);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        #endregion Private Methods


        public void ClearSessionsExcept(params string[] keys)
        {
            //store
            string[] values = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                PropertyInfo propInfo = typeof(SessionServices).GetProperty(keys[i]);
                values[i] = propInfo.GetValue(this, null).ToString();
            }
            //clear
            this.HttpContext.Session.Clear();
            //reset
            for (int i = 0; i < keys.Length; i++)
            {
                PropertyInfo propInfo = typeof(SessionServices).GetProperty(keys[i]);
                propInfo.SetValue(this, values[i]);
            }
        }

    }
}
