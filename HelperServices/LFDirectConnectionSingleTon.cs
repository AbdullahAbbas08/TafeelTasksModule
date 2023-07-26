using LFSO102Lib;

namespace HelperServices
{
    public class LFDirectConnectionSingleTon
    {
        private static LFDatabase lfDB = null;
        private static object padlock = new object();

        public static string LFServerStr { get; set; }
        public static string LFRepostryStr { get; set; }
        public static string LFUserName { get; set; }
        public static string LFUserPassword { get; set; }


        public static void setLFConnectionCredentials(string _LFServerStr,
                                                     string _LFRepostryStr,
                                                     string _LFUserName,
                                                     string _LFUserPassword)
        {

            if (string.IsNullOrEmpty(LFServerStr) ||
               string.IsNullOrEmpty(LFRepostryStr) ||
               string.IsNullOrEmpty(LFUserName) ||
               string.IsNullOrEmpty(LFUserPassword)
               )

            {
                if (lfDB != null)
                {
                    ConnectionTerminate();
                }

                LFServerStr = _LFServerStr;
                LFRepostryStr = _LFRepostryStr;
                LFUserName = _LFUserName;
                LFUserPassword = _LFUserPassword;
            }


            if (LFServerStr.ToLower() != _LFServerStr &&
               LFRepostryStr.ToLower() != _LFRepostryStr.ToLower() &&
               LFUserName.ToLower() != _LFUserName.ToLower() &&
               LFUserPassword.ToLower() != _LFUserPassword.ToLower())
            {
                if (lfDB != null)
                {
                    ConnectionTerminate();
                }

                LFServerStr = _LFServerStr;
                LFRepostryStr = _LFRepostryStr;
                LFUserName = _LFUserName;
                LFUserPassword = _LFUserPassword;
            }

            try
            {
                if (lfDB == null || lfDB.CurrentConnection.IsTerminated)
                {
                    lfDB = null;

                    LFServerStr = _LFServerStr;
                    LFRepostryStr = _LFRepostryStr;
                    LFUserName = _LFUserName;
                    LFUserPassword = _LFUserPassword;
                }
            }
            catch
            {
                lfDB = null;

                LFServerStr = _LFServerStr;
                LFRepostryStr = _LFRepostryStr;
                LFUserName = _LFUserName;
                LFUserPassword = _LFUserPassword;
            }
        }

        public static LFDatabase LFDatabaseInstance()
        {
            if (lfDB == null || lfDB.CurrentConnection.IsTerminated)
            {
                lock (padlock)
                {
                    if (lfDB == null || lfDB.CurrentConnection.IsTerminated)
                    {

                        LFApplication App = new LFApplication();
                        LFServer Serv = App.GetServerByName(LFServerStr);
                        lfDB = Serv.GetDatabaseByName(LFRepostryStr);
                        LFConnection Conn = new LFConnection();
                        Conn.UserName = LFUserName;
                        Conn.Password = LFUserPassword;
                        Conn.Create(lfDB);
                    }
                }
            }
            return lfDB;
        }


        public static void ConnectionTerminate()
        {
            //lfDB.CurrentConnection.Terminate();
            lfDB = null;
        }
    }
}
