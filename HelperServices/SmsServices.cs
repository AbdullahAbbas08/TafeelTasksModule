using CommiteeAndMeetings.BLL;
using EMailIntegration;
using IHelperServices;
using IHelperServices.Models;
using IntegrationSMS;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace HelperServices
{
    public class SmsServices : _HelperService, ISmsServices
    {
        //IOptions<AppSettings> appSettings;
        private readonly DbContextFactory _dbContextFactory;
        private IntegrationSMSClass SmsIntegration { get; set; }
       
        public SmsServices(IOptions<AppSettings> _appSettings)
        {
            _dbContextFactory = new DbContextFactory();
           // appSettings = _appSettings;
            // this.SmsIntegration = new SmsIntegrations(appSettings);
            this.SmsIntegration = new IntegrationSMSClass(_appSettings, _dbContextFactory);
           
        }

        //public void Send(string recievermobilenumber, string[] bodyParameters, string message, string templateCode)
        //{
        //    //try
        //    //{
        //    //    if (File.Exists("C:\\logsms.txt"))
        //    //    {
        //    //        using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
        //    //        {
        //    //            sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
        //    //        }
        //    //    }
        //    //    string url = appSettings.Value.SMSSettings.ServiceUrl;

        //    //    string receiver = recievermobilenumber.Length > 9
        //    //                          ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
        //    //                          : recievermobilenumber;
        //    //    receiver = "966" + receiver;
        //    //    string username = HttpUtility.UrlEncode(appSettings.Value.SMSSettings.Account);
        //    //    string password = HttpUtility.UrlEncode(appSettings.Value.SMSSettings.Password);
        //    //    string sender = HttpUtility.UrlEncode(appSettings.Value.SMSSettings.SMS_SENDER); //"U of jeddah"
        //    //    string RECIEVER = HttpUtility.UrlEncode(receiver);
        //    //    string MESSAGE = ConvertToUnicode(message);
        //    //    HttpWebRequest req = (HttpWebRequest)
        //    //    WebRequest.Create(url);
        //    //    req.Method = "POST";
        //    //    req.ContentType = "application/x-www-form-urlencoded";
        //    //    string postData = "mobile=" + username + "&password=" + password + "&numbers=" + RECIEVER + "&sender=" + sender + "&msg=" + MESSAGE + "&applicationType=61&msgId=0";
        //    //    req.ContentLength = postData.Length;

        //    //    StreamWriter stOut = new
        //    //    StreamWriter(req.GetRequestStream(),
        //    //    System.Text.Encoding.ASCII);
        //    //    stOut.Write(postData);
        //    //    stOut.Close();
        //    //    // Do the request to get the response
        //    //    string strResponse;
        //    //    StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
        //    //    strResponse = stIn.ReadToEnd();
        //    //    stIn.Close();
        //    //    //return strResponse;
        //    //    if (File.Exists("C:\\logsms.txt"))
        //    //    {
        //    //        using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
        //    //        {
        //    //            sw.WriteLine("End recievermobilenumber=" + recievermobilenumber + "  message" + message + "  strResponse" + strResponse);
        //    //        }
        //    //    }
        //    //    // notification log 
        //    //    //NotificationLog log = new NotificationLog()
        //    //    //{ Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //    //    //_DbContext.NotificationLog.Add(log);
        //    //    //_DbContext.SaveChanges();
        //    //    // notification log 
        //    //    //using (var _DbContext = new MainDbContext())
        //    //    //{
        //    //    //    NotificationLog log = new NotificationLog()
        //    //    //    { Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //    //    //    _DbContext.NotificationLog.Add(log);
        //    //    //    _DbContext.SaveChanges();
        //    //    //}
        //    //    // notification log 

        //    //    //return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
        //    //}
        //    try
        //    {
        //        string response = "";
        //        string serviceUrl = appSettings.Value.SMSSettings.ServiceUrl;

        //        if (File.Exists("C:\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
        //            {
        //                sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
        //            }
        //        }

        //        string receiver = recievermobilenumber.Length > 9
        //                              ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
        //                              : recievermobilenumber;
        //        receiver = "966" + receiver;
        //        string userName = HttpUtility.UrlEncode(appSettings.Value.SMSSettings.Account);
        //        string password = HttpUtility.UrlEncode(appSettings.Value.SMSSettings.Password);
        //        string sender = HttpUtility.UrlEncode(appSettings.Value.SMSSettings.SMS_SENDER); //"Hajj sender"
        //        string mobileNumber = HttpUtility.UrlEncode(receiver);
        //        string text = HttpUtility.UrlEncode(message);


        //        string requestUrl = serviceUrl + "?user=" + userName + "&pwd=" + password + "&senderid=" + sender + "&CountryCode=966&mobileno=" + mobileNumber + "&msgtext=" + text;

        //        using (WebClient client = new WebClient())
        //        {
        //            response = client.DownloadString(requestUrl);
        //        }
        //        // notification log 
        //        //_DbContext.Dispose();
        //        //_DbContext = new MainDbContext();
        //        //NotificationLog log = new NotificationLog()
        //        //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0]?.ToString() };
        //        //_DbContext.NotificationLog.Add(log);
        //        //_DbContext.SaveChanges();
        //        // notification log 
        //        //using (var _DbContext = new MainDbContext())
        //        //{
        //        //    NotificationLog log = new NotificationLog()
        //        //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //        //    _DbContext.NotificationLog.Add(log);
        //        //    _DbContext.SaveChanges();
        //        //}
        //        // notification log 
        //        //using (var _DbContext = _dbContextFactory.Create())
        //        //{
        //        //    NotificationLog log = new NotificationLog()
        //        //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //        //    _DbContext.NotificationLog.Add(log);
        //        //    _DbContext.SaveChanges();
        //        //}
        //        //  return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
        //        if (File.Exists("C:\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
        //            {
        //                sw.WriteLine("End recievermobilenumber=" + recievermobilenumber + "  message" + message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (File.Exists("C:\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
        //            {
        //                sw.WriteLine("End recievermobilenumber with error " +  ex.Message);
        //            }
        //        }
        //        // return new ReturnSMSTypes { success = false, message = "Sending Fail because " + ex.InnerException.ToString() };
        //    }
        //}

        public void SendSMS(string to, string[] bodyParameters, string TextMessage, string templateCode)
        {
            this.SmsIntegration.SendSMS(to, bodyParameters, TextMessage, templateCode);
        }

        private string ConvertToUnicode(string val)
        {
            string msg2 = string.Empty;

            for (int i = 0; i < val.Length; i++)
            {
                msg2 += this.convertToUnicode_(System.Convert.ToChar(val.Substring(i, 1)));
            }

            return msg2;
        }

        private string fourDigits(string val)
        {
            string result = string.Empty;

            switch (val.Length)
            {
                case 1: result = "000" + val; break;
                case 2: result = "00" + val; break;
                case 3: result = "0" + val; break;
                case 4: result = val; break;
            }

            return result;
        }
        private string convertToUnicode_(char ch)
        {
            System.Text.UnicodeEncoding class1 = new System.Text.UnicodeEncoding();
            byte[] msg = class1.GetBytes(System.Convert.ToString(ch));

            return this.fourDigits(msg[1] + msg[0].ToString("X"));
        }
    }
}