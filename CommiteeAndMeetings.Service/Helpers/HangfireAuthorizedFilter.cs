//using CommiteeAndMeetings.Service.ISevices;
using Hangfire.Dashboard;
using IHelperServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.Helpers
{
    public class HangFireAuthorizedFilter : IDashboardAuthorizationFilter
    {

        private readonly ISessionServices _SessionServices;

        public HangFireAuthorizedFilter(ISessionServices SessionServices)
        {
            _SessionServices = SessionServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;

            if (_SessionServices.UserId == null)
                return false;

            return true;
        }
    }
}
