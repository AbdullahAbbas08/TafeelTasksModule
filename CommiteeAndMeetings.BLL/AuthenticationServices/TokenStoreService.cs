using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DbContexts.MasarContext.Models;
using HelperServices;
using HelperServices.Hubs;
using IBusinessServices.IAuthenticationServices;
using IHelperServices;
using IRepositories;
using IRepositories.IRepositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusinessServices.AuthenticationServices
{
    public class TokenStoreService : ITokenStoreService
    {
        private readonly ISecurityService _securityService;
        private readonly IUnitOfWork _uow;
        private readonly IUserTokenRepository _tokens;
        private readonly IUserRepository _users;
        private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
        private readonly IRolesService _rolesService;
        private readonly ISessionServices _SessionServices;
        private readonly IDataProtectService _dataProtectService;
        public TokenStoreService(
            IUnitOfWork uow,
            ISecurityService securityService,
            IRolesService rolesService,
            IOptionsSnapshot<BearerTokensOptions> configuration,
            ISessionServices sessionServices, IDataProtectService dataProtectService)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));

            _rolesService = rolesService;
            _rolesService.CheckArgumentIsNull(nameof(rolesService));

            _tokens = _uow.Repository<UserToken>() as IUserTokenRepository;
            _users = _uow.Repository<User>() as IUserRepository;

            _configuration = configuration;
            _configuration.CheckArgumentIsNull(nameof(configuration));

            _SessionServices = sessionServices;
            _dataProtectService = dataProtectService;
        }

        public async Task AddUserTokenAsync(UserToken userToken)
        {
            if (!_configuration.Value.AllowMultipleLoginsFromTheSameUser)
            {
                await InvalidateUserTokensAsync(userToken.UserId);
            }
            await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.RefreshTokenIdHashSource);
            var _userToken = _tokens.Insert(new List<UserToken> { userToken });
            _SessionServices.UserTokenId = _userToken.ToList().FirstOrDefault().Id.ToString();
            _uow.Save();
        }

        public async Task AddUserTokenAsync(User user, string refreshToken, string accessToken, string refreshTokenSource, int ApplicationType)
        {
            //  var context = SignalRHubConnectionHandler.Connections.
            var now = DateTimeOffset.UtcNow;
            var token = new UserToken
            {
                UserId = user.UserId,
                // Refresh token handles should be treated as secrets and should be stored hashed
                RefreshTokenIdHash = _securityService.GetSha256Hash(refreshToken),
                RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSource) ?
                                           null : _securityService.GetSha256Hash(refreshTokenSource),
                AccessTokenHash = _securityService.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTime = now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
                AccessTokenExpiresDateTime = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
                ApplicationType = ApplicationType,
            };
            await AddUserTokenAsync(token);
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var now = DateTimeOffset.UtcNow;
            var ExpiredTokens = _tokens.GetAll().Where(x => x.RefreshTokenExpiresDateTime < now);
            if (ExpiredTokens != null && ExpiredTokens.Count() > 0)
                _tokens.Delete(ExpiredTokens);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteTokenAsync(string refreshToken)
        {
            var token = await FindTokenAsync(refreshToken);
            if (token != null)
            {
                _tokens.Delete(new List<UserToken> { token });
            }
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
            {
                return;
            }
            var ToBeDeletedTokens = _tokens.GetAll().Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource).ToList();
            // check if Its null
            if (ToBeDeletedTokens.Count == 0)
            {
                //check if It _SessionServices.UserTokenId not = 0 if =0 will Delete All _sessions of  this User
                if (int.Parse(_SessionServices.UserTokenId) == 0)
                {
                    await InvalidateUserTokensAsync(_SessionServices.UserId.Value);
                }
                else
                {
                    await InvalidateUserTokensAsync(_SessionServices.UserId.Value, int.Parse(_SessionServices.UserTokenId));

                }
            }
            else
            {
                //(ToBeDeletedTokens != null && ToBeDeletedTokens.Count() > 0)
                _tokens.Delete(ToBeDeletedTokens);
            }
            await _uow.SaveChangesAsync();
        }

        public async Task RevokeUserBearerTokensAsync(string userIdValue, string refreshToken)
        {
            if (!string.IsNullOrWhiteSpace(userIdValue) && int.TryParse(userIdValue, out int userId))
            {
                if (_configuration.Value.AllowSignoutAllUserActiveClients)
                {
                    await InvalidateUserTokensAsync(userId);
                }
            }

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var refreshTokenIdHashSource = _securityService.GetSha256Hash(refreshToken);
                await DeleteTokensWithSameRefreshTokenSourceAsync(refreshTokenIdHashSource);
            }
            await DeleteExpiredTokensAsync();
            await _uow.SaveChangesAsync();
        }

        public async Task<UserToken> FindTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return null;
            }
            var refreshTokenIdHash = _securityService.GetSha256Hash(refreshToken);
            return await _tokens.GetAll().Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash);
        }

        public async Task InvalidateUserTokensAsync(int userId)
        {
            var UserTokens = _tokens.GetAll().Where(x => x.UserId == userId);
            if (UserTokens != null && UserTokens.Count() > 0)
                _tokens.Delete(UserTokens);
            await _uow.SaveChangesAsync();
        }
        public async Task InvalidateUserTokensAsync(int userId, int UserokenId)
        {
            var UserTokens = _tokens.GetAll().Where(x => x.UserId == userId && x.Id == UserokenId);
            if (UserTokens != null && UserTokens.Count() > 0)
                _tokens.Delete(UserTokens);
            await _uow.SaveChangesAsync();
        }

        public async Task<bool> IsValidTokenAsync(string accessToken, int userId)
        {
            var accessTokenHash = _securityService.GetSha256Hash(accessToken);
            var userToken = await _tokens.GetAll().FirstOrDefaultAsync(
                x => x.AccessTokenHash == accessTokenHash && x.UserId == userId);
            return userToken?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
        }

        public async Task<(string accessToken, string refreshToken, IEnumerable<Claim> Claims)> CreateJwtTokens(User user, int applicationType, string refreshTokenSource)
        {
            var result = await createAccessTokenAsync(user);
            if (result.Claims == null)
                return (null, null, null);
            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");
            await AddUserTokenAsync(user, refreshToken, result.AccessToken, refreshTokenSource, applicationType);
            await _uow.SaveChangesAsync();



            return (result.AccessToken, refreshToken, result.Claims);
        }

        private async Task<(string AccessToken, IEnumerable<Claim> Claims)> createAccessTokenAsync(User user)
        {

            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(ClaimTypes.Name,_dataProtectService.Encrypt(user.Username), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(_dataProtectService.Encrypt("DisplayName"),_dataProtectService.Encrypt( _SessionServices.CultureIsArabic ? user.FullNameAr : user.FullNameEn), ClaimValueTypes.String, _configuration.Value.Issuer),
                // to invalidate the cookie
                new Claim(ClaimTypes.SerialNumber, user.SerialNumber??"", ClaimValueTypes.String, _configuration.Value.Issuer),
                // custom data
                new Claim(ClaimTypes.UserData, user.UserId.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer)
            };

            // add current LoggedIn OrganizationId
            // add current LoggedIn RoleId

            var LastAndCurrentLogedInUserRole = await _users.FindLastSelectedRoleAndOrganization(user.UserId) ?? null;
            if (LastAndCurrentLogedInUserRole == null)
            {
                return (null, null);
            }
            claims.Add(new Claim(_dataProtectService.Encrypt("CurrentRoleId"), _dataProtectService.Encrypt(LastAndCurrentLogedInUserRole.RoleId.ToString()), ClaimValueTypes.String, _configuration.Value.Issuer));
            claims.Add(new Claim(_dataProtectService.Encrypt("CurrentOrganizationId"), _dataProtectService.Encrypt(LastAndCurrentLogedInUserRole.OrganizationId.ToString()), ClaimValueTypes.String, _configuration.Value.Issuer));
            claims.Add(new Claim(_dataProtectService.Encrypt("CurrentUserRoleId"), _dataProtectService.Encrypt(LastAndCurrentLogedInUserRole.UserRoleId.ToString()), ClaimValueTypes.String, _configuration.Value.Issuer));
            claims.Add(new Claim(_dataProtectService.Encrypt("UserFullNameAr"), _dataProtectService.Encrypt(user.FullNameAr), ClaimValueTypes.String, _configuration.Value.Issuer));
            claims.Add(new Claim(_dataProtectService.Encrypt("UserFullNameEn"), _dataProtectService.Encrypt(user.FullNameEn), ClaimValueTypes.String, _configuration.Value.Issuer));
            claims.Add(new Claim(_dataProtectService.Encrypt("CurrentOrganizationNameAr"), _dataProtectService.Encrypt(LastAndCurrentLogedInUserRole.Organization.OrganizationNameAr), ClaimValueTypes.String, _configuration.Value.Issuer));

            if (LastAndCurrentLogedInUserRole.Organization.OrganizationNameEn != null)
                claims.Add(new Claim(_dataProtectService.Encrypt("CurrentOrganizationNameEn"), (_dataProtectService.Encrypt(LastAndCurrentLogedInUserRole.Organization.OrganizationNameEn)) ?? "", ClaimValueTypes.String, _configuration.Value.Issuer));

            claims.Add(new Claim(_dataProtectService.Encrypt("CurrentRoleNameAr"), _dataProtectService.Encrypt(LastAndCurrentLogedInUserRole.Role.RoleNameAr), ClaimValueTypes.String, _configuration.Value.Issuer));
            claims.Add(new Claim(_dataProtectService.Encrypt("CurrentRoleNameEn"), _dataProtectService.Encrypt(LastAndCurrentLogedInUserRole.Role.RoleNameEn), ClaimValueTypes.String, _configuration.Value.Issuer));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _configuration.Value.Issuer,
                audience: _configuration.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
                signingCredentials: creds);


            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }
    }
}