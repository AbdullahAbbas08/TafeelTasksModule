using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CommiteeAndMeetings.BLL.Hosting
{
    public static class Hosting
    {
        public static IHostingEnvironment HostingEnvironment { get; set; }

        public static string RootPath
        {
            get { return HostingEnvironment.ContentRootPath; }
        }

        public static string WebRootPath
        {
            get { return HostingEnvironment.WebRootPath; }
        }

        public static string ApplicationName
        {
            get { return HostingEnvironment.ApplicationName; }
        }

        public static string EnvironmentName
        {
            get { return HostingEnvironment.EnvironmentName; }
        }

        public static bool IsDebugMode
        {
            get
            {
#if DEBUG
                return true;
#else
                    return false;
#endif
            }
        }

        public static string AngularRootPath
        {
            get
            {
                //return Path.Combine(RootPath, (IsDebugMode ? "ClientApp\\src" : "wwwroot"));
                return Path.Combine(RootPath, "wwwroot");
            }
        }


    }
}
