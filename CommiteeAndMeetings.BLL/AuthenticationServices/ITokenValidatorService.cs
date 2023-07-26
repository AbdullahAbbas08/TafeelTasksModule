using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IBusinessServices.IAuthenticationServices
{
    public interface ITokenValidatorService : _IBusinessService
    {
        Task ValidateAsync(TokenValidatedContext context);
    }
}
