using Serilog;

namespace CommiteeAndMeetings.MasarIntegration.Utility
{
    public static class Logger
    {
        private static Serilog.Core.Logger _logger = null;
        private static object lockT = new object();
        public static Serilog.Core.Logger Log
        {

            get
            {
                lock (lockT)
                {
                    if (_logger == null)
                    {
                        _logger = new LoggerConfiguration()
                        // .WriteTo.File($"{System.Configuration.ConfigurationManager.AppSettings["LogsFolder"]}/Logs.txt", rollOnFileSizeLimit: true)
                          .CreateLogger();

                    }
                    return _logger;
                }
            }
        }
    }
}
