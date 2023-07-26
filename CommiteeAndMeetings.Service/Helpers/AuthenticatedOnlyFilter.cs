using IHelperServices;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.Helpers
{
    public class AuthenticatedOnlyFilter : AuthorizationHandler<AuthenticatedOnlyRequirment>
    {
        private readonly ISessionServices sessionServices;

        public AuthenticatedOnlyFilter(ISessionServices _sessionServices)
        {
            sessionServices = _sessionServices;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthenticatedOnlyRequirment requirement)
        {
            if (sessionServices.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }

    public class AuthenticatedOnlyRequirment : IAuthorizationRequirement
    {

    }
}
