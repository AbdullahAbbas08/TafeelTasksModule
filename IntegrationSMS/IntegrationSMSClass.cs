using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.DAL.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace IntegrationSMS
{
    public class IntegrationSMSClass
    {
        private readonly DbContextFactory _dbContextFactory;
        public string Account { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }
        public string SmsPostActivate { get; set; }
        public string SenderAPIKey { get; set; }
        public string x_uqu_auth { get; set; }
        public string SMS_SENDER { get; set; }
        //Dawan
        public string senderName { get; set; }
        public string LoginURl { get; set; }
        public string apiKey { get; set; }
        public string UserName { get; set; }
        public string sendMessageURL { get; set; }
        public AppSettings _AppSettings;
        public IntegrationSMSClass(IOptions<AppSettings> appSettings, DbContextFactory dbContextFactory)
        {
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            _dbContextFactory = dbContextFactory;
            this.Account = _AppSettings.SMSSettings.Account;
            this.Password = _AppSettings.SMSSettings.Password;
            this.ServiceUrl = _AppSettings.SMSSettings.ServiceUrl;
            this.SmsPostActivate = _AppSettings.SMSSettings.SmsPostActivate;
            this.SenderAPIKey = _AppSettings.SMSSettings.SenderAPIKey;
            this.x_uqu_auth = _AppSettings.SMSSettings.x_uqu_auth;
            this.SMS_SENDER = _AppSettings.SMSSettings.SMS_SENDER;
            //Dawan
            this.senderName = _AppSettings.SMSSettings.senderName;
            this.LoginURl = _AppSettings.SMSSettings.LoginURl;
            this.apiKey = _AppSettings.SMSSettings.apiKey;
            this.UserName = _AppSettings.SMSSettings.UserName;
            this.sendMessageURL = _AppSettings.SMSSettings.sendMessageURL;
        }

        #region Helpers
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
        private string ConvertToUnicode(string val)
        {
            string msg2 = string.Empty;

            for (int i = 0; i < val.Length; i++)
            {
                msg2 += this.convertToUnicode_(System.Convert.ToChar(val.Substring(i, 1)));
            }

            return msg2;
        }
        #endregion

        public ReturnSMSTypes SendSMS(string to, string[] body, string TextMessage, string templateCode = null)
        {

            switch (this.SmsPostActivate)
            {
                case "1":
                    return this.SendSMSPostJeddah(to, body, TextMessage, templateCode);
                //case "2":
                  //  return this.SendSMSPost_MCI(to, body, templateCode);
                case "3":
                    return this.SendSmsPostUQU(to, body, TextMessage);
                case "4":
                    return this.SendSMSPostAffairs(to, body, TextMessage);
                case "5":
                    return this.SendHajjSMS(to, body, TextMessage, templateCode);
                case "6":
                    return this.SendRoomsSMS(to, body, TextMessage, templateCode);
                case "7":
                    return this.SendDeewanSMS(to, TextMessage);
                case "8":
                    return this.SendSMSPostYammahUrl(to, body, TextMessage, templateCode);
                //case "9":
                 // return this.SendSMSPostEximSOAP(to, body, TextMessage, templateCode);
                case "10":
                    return this.SendSMSPostJeddahNew(to, body, TextMessage, templateCode);
                case "11":
                    return this.SendSMSPostAffairsNew(to, body, TextMessage);
                case "12":
                    return this.SendSMSPostMakkah(to, body, TextMessage);

            }
            return new ReturnSMSTypes { success = false, message = " SmsPostActivate Must be from 1 to 8" };

        }

        private ReturnSMSTypes SendSMSPostJeddah(string recievermobilenumber, string[] body, string message, string templateCode = null)
        {
            try
            {
                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }
                string url = this.ServiceUrl;

                string receiver = recievermobilenumber.Length > 9
                                      ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                      : recievermobilenumber;
                receiver = "966" + receiver;
                string username = HttpUtility.UrlEncode(this.Account);
                string password = HttpUtility.UrlEncode(this.Password);
                string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"U of jeddah"
                string RECIEVER = HttpUtility.UrlEncode(receiver);
                string MESSAGE = ConvertToUnicode(message);
                HttpWebRequest req = (HttpWebRequest)
                WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                string postData = "mobile=" + username + "&password=" + password + "&numbers=" + RECIEVER + "&sender=" + sender + "&msg=" + MESSAGE + "&applicationType=61&msgId=0";
                req.ContentLength = postData.Length;

                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("Post Data : " + postData);
                    }
                }

                StreamWriter stOut = new
                StreamWriter(req.GetRequestStream(),
                System.Text.Encoding.ASCII);
                stOut.Write(postData);
                stOut.Close();
                // Do the request to get the response
                string strResponse;
                StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());

                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("stIn : " + stIn);
                    }
                }


                strResponse = stIn.ReadToEnd();
                stIn.Close();
                //return strResponse;
                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("End recievermobilenumber=" + recievermobilenumber + "  message" + message + "  strResponse" + strResponse);
                    }
                }
                // notification log 
                //NotificationLogs log = new NotificationLogs()
                //{ Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //_DbContext.NotificationLogs.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                using (var _DbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

            }
            catch (Exception ex)
            {
                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("Exception : " + ex.Message);
                    }
                }

                return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
            }
        }
        private ReturnSMSTypes SendSMSPostJeddahNew(string recievermobilenumber, string[] body, string message, string templateCode = null)
        {
            try
            {
                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }
                string url = this.ServiceUrl;

                string receiver = recievermobilenumber.Length > 9
                                      ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                      : recievermobilenumber;
                receiver = "966" + receiver;
                string username = this.Account;
                string password = this.Password;
                string sender = this.SMS_SENDER; //"U of jeddah"
                string RECIEVER = receiver;
                string MESSAGE = HttpUtility.UrlEncode(message);
                string SenderAPIKey = this.SenderAPIKey;
                string x_uqu_auth = this.x_uqu_auth;
                HttpWebRequest req = (HttpWebRequest)
                WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                // https://sms.wfcc.com.sa/fccsms_SA.aspx?IID=2081&UID=UJFAM&P=Uj@987654321&S=UofJeddah&G=966541583057&M=2%D8%AA%D8%AC%D8%B1%D8%A8%D8%A9&L=A
                string postData = "IID=" + SenderAPIKey + "&UID=" + username + "&P=" + password + "&S=" + sender + "&G=" + RECIEVER + "&M=" + MESSAGE + "&L=" + x_uqu_auth;
                req.ContentLength = postData.Length;

                StreamWriter stOut = new
                StreamWriter(req.GetRequestStream(),
                System.Text.Encoding.ASCII);
                stOut.Write(postData);
                stOut.Close();
                // Do the request to get the response
                string strResponse;
                StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                strResponse = stIn.ReadToEnd();
                stIn.Close();
                //return strResponse;
                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("End recievermobilenumber=" + recievermobilenumber + "  message" + message + "  strResponse" + strResponse);
                    }
                }
                // notification log 
                //NotificationLogs log = new NotificationLogs()
                //{ Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //_DbContext.NotificationLogs.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                using (var _DbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
            }
        }
        //private ReturnSMSTypes SendSMSPost_MCI(string recepient, string[] body, string templateCode)
        //{
        //    try
        //    {
        //        MCI_SMS mCI_SMS = new MCI_SMS();
        //        var result = mCI_SMS.MCI_SendSMS(this.SenderAPIKey, recepient, templateCode, body);
        //        //return new ReturnSMSTypes { success = false, message = "" };
        //        // notification log 
        //        //NotificationLogs log = new NotificationLogs()
        //        //{ Subject = body.ToString(), Reciever = recepient, SendingDate = DateTimeOffset.Now,TransactionNumber=body[0].ToString() };
        //        //_DbContext.NotificationLogs.Add(log);
        //        //_DbContext.SaveChanges();
        //        // notification log 
        //        //using (var _DbContext = new MainDbContext())
        //        //{
        //        //    NotificationLogs log = new NotificationLogs()
        //        //    { Subject = body.ToString(), Reciever = recepient, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //        //    _DbContext.NotificationLogs.Add(log);
        //        //    _DbContext.SaveChanges();
        //        //}
        //        // notification log 
        //        using (var _DbContext = _dbContextFactory.Create())
        //        {
        //            NotificationLogs log = new NotificationLogs()
        //            { Subject = body.ToString(), Reciever = recepient, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //            _DbContext.NotificationLogs.Add(log);
        //            _DbContext.SaveChanges();
        //        }
        //        return new ReturnSMSTypes
        //        {
        //            success = true,
        //            message = $"result result.IsCompleted {result.IsCompleted}," +
        //                 $"result.IsCompletedSuccessfully {result.IsCompletedSuccessfully},result.Result ,{result.Result}" +
        //                 $"result.IsFaulted ,{result.IsFaulted}result.Id ,{result.Id}" +
        //                 $"result.Status ,{result.Status}result.IsCanceled ,{result.IsCanceled}"
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
        //    }

        //}

        //private ReturnSMSTypes SendSmsPostUQU(string recievermobilenumber, string[] body, string message)
        //{
        //    try
        //    {
        //        string url = ServiceUrl;
        //        string receiver = recievermobilenumber.Length > 9
        //                              ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
        //                              : recievermobilenumber;
        //        receiver = "966" + receiver;
        //        string RECIEVER = receiver;
        //        string MESSAGE = message;
        //        string SenderAPIKey = this.SenderAPIKey;
        //        string x_uqu_auth = this.x_uqu_auth;
        //        string MessageType = "sms";


        //        url = url + "?SenderAPIKey=" + SenderAPIKey + "&MessageType=" + MessageType + "&MessageContent=" + MESSAGE + "&ReceiversDirect=" + RECIEVER + "&IsDept=1";
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

        //        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //        httpWebRequest.Method = "POST";
        //        httpWebRequest.Headers["x-uqu-auth"] = x_uqu_auth;

                

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        string result = "";
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            result = streamReader.ReadLine();
        //        }
        //        // notification log 
        //        //NotificationLogs log = new NotificationLogs()
        //        //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //        //_DbContext.NotificationLogs.Add(log);
        //        //_DbContext.SaveChanges();
        //        // notification log 
        //        //using (var _DbContext = new MainDbContext())
        //        //{
        //        //    NotificationLogs log = new NotificationLogs()
        //        //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //        //    _DbContext.NotificationLogs.Add(log);
        //        //    _DbContext.SaveChanges();
        //        //}
        //        // notification log 
        //        using (var _DbContext = _dbContextFactory.Create())
        //        {
        //            NotificationLog log = new NotificationLog()
        //            { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //            _DbContext.NotificationLogs.Add(log);
        //            _DbContext.SaveChanges();
        //        }
        //        return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

        //    }
        //    catch (Exception ex)
        //    {
        //        //if (System.IO.File.Exists("C:\\testemail.txt"))
        //        //{
        //        //    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
        //        //    {
        //        //        sw.WriteLine("Function is = Send SMS UQU");
        //        //        sw.WriteLine("Mes=" + ex.Message);
        //        //        sw.WriteLine("Stack Trace=" + ex.StackTrace);
        //        //    }
        //        //}
        //        return new ReturnSMSTypes { success = false, message = "Sending faild" };
        //    }
        //}

        private ReturnSMSTypes SendSmsPostUQU(string receiverMobileNumber, string[] body, string message)
        {
            try
            {
                string url = ServiceUrl;
                string receiver = "966" + receiverMobileNumber.Substring(receiverMobileNumber.Length > 9 ? 1 : 0);
                string messageType = "sms";
                string senderAPIKey = this.SenderAPIKey;
                string xUquAuth = this.x_uqu_auth;

                url = $"{url}?SenderAPIKey={senderAPIKey}&MessageType={messageType}&MessageContent={message}&ReceiversDirect={receiver}&IsDept=1";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["x-uqu-auth"] = xUquAuth;

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadLine();
                }

                using (var dbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    {
                        Subject = message,
                        Reciever = receiver,
                        SendingDate = DateTimeOffset.Now,
                        TransactionNumber = body[0].ToString()
                    };
                    dbContext.NotificationLogs.Add(log);
                    dbContext.SaveChanges();
                }

                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = "Sending failed" };
            }
        }


        private ReturnSMSTypes SendDeewanSMS(string recievermobilenumber, string message)
        {
            try
            {
                string url = LoginURl;// "https://apis.deewan.sa/auth/v1/signin";
                var key = new LoginKey
                {
                    apiKey = apiKey,//"11db3ada-5c41-41aa-bd11-f30594e0a4d0",
                    userName = UserName //"Tafeel01"
                };
                var request = (HttpWebRequest)WebRequest.Create(url);

                string json = JsonConvert.SerializeObject(key, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                var data = Encoding.ASCII.GetBytes(json);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var res = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                var auth = res.data.access_token;

                url = sendMessageURL;// "https://apis.deewan.sa/sms/v1/messages";
                var request2 = (HttpWebRequest)WebRequest.Create(url);

                var messagesObject = new Message
                {
                    senderName = senderName,
                    messageText = message,
                    recipients = recievermobilenumber,
                    messageType = "text"
                };
                string json2 = JsonConvert.SerializeObject(messagesObject, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All/*, Culture = new System.Globalization.CultureInfo("ar-sa") */});

                var data2 = Encoding.UTF8.GetBytes(json2);

                request2.Method = "POST";
                request2.ContentType = "application/json";
                request2.ContentLength = data2.Length;
                request2.PreAuthenticate = true;
                request2.Headers.Add("Authorization", "Bearer " + auth);
                using (var stream2 = request2.GetRequestStream())
                {
                    stream2.Write(data2, 0, data2.Length);
                }

                var response2 = request2.GetResponse();

                var responseString2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                // notification log 
                //NotificationLogs log = new NotificationLogs()
                //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //_DbContext.NotificationLogs.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = message, Reciever = recievermobilenumber, SendingDate = DateTimeOffset.Now, TransactionNumber = message };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                using (var _DbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = message, Reciever = recievermobilenumber, SendingDate = DateTimeOffset.Now, TransactionNumber = message };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

            }
            catch (Exception ex)
            {
                //if (System.IO.File.Exists("C:\\testemail.txt"))
                //{
                //    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                //    {
                //        sw.WriteLine("Function is = Send SMS UQU");
                //        sw.WriteLine("Mes=" + ex.Message);
                //        sw.WriteLine("Stack Trace=" + ex.StackTrace);
                //    }
                //}
                return new ReturnSMSTypes { success = false, message = "Sending faild" };
            }
        }
        //private ReturnSMSTypes SendSMSPostAffairs(string recievermobilenumber, string[] body, string message)
        //{

        //    try
        //    {
        //        string url = ServiceUrl;
        //        string receiver = recievermobilenumber.Length > 9
        //                              ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
        //                              : recievermobilenumber;
        //        receiver = "966" + receiver;
        //        string username = this.Account;
        //        string password = this.Password;
        //        string sender = this.SMS_SENDER; 
        //        //string RECIEVER = HttpUtility.UrlEncode(receiver);
        //        //string MESSAGE = ConvertToUnicode(message);
        //        string RECIEVER = receiver;
        //        string MESSAGE = message;
        //        string MessageType = "text";


        //        url = url + "?UserName=" + username + "&Password=" + password + "&MessageType=" + MessageType + "&Recipients=" + RECIEVER + "&SenderName="+ sender+ "&MessageText="+ MESSAGE;
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

        //        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //        httpWebRequest.Method = "POST";


        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        string result = "";
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            result = streamReader.ReadLine();
        //        }
        //        // notification log 
        //        //NotificationLogs log = new NotificationLogs()
        //        //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //        //_DbContext.NotificationLogs.Add(log);
        //        //_DbContext.SaveChanges();
        //        // notification log 
        //        using (var _DbContext = new MainDbContext())
        //        {
        //            NotificationLogs log = new NotificationLogs()
        //            { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //            _DbContext.NotificationLogs.Add(log);
        //            _DbContext.SaveChanges();
        //        }
        //        return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

        //    }
        //    catch (Exception ex)
        //    {
        //        if (System.IO.File.Exists("C:\\testemail.txt"))
        //        {
        //            using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
        //            {
        //                sw.WriteLine("Function is = Send SMS UQU");
        //                sw.WriteLine("Mes=" + ex.Message);
        //                sw.WriteLine("Stack Trace=" + ex.StackTrace);
        //            }
        //        }
        //        return new ReturnSMSTypes { success = false, message = "Sending faild" };
        //    }
        //}

        //private ReturnSMSTypes SendSMSPostHajj(string recievermobilenumber, string[] body, string message, string templateCode = null)
        //{
        //    try
        //    {
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
        //        string username = HttpUtility.UrlEncode(this.Account);
        //        string password = HttpUtility.UrlEncode(this.Password);
        //        string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"Hajj sender"
        //        string RECIEVER = HttpUtility.UrlEncode(receiver);
        //        string MESSAGE = HttpUtility.UrlEncode(message);

        //        string url = this.ServiceUrl + "username=" + username + "&password=" + password +  "&Sender=" + sender + "&Text=" + MESSAGE + "&number=" + RECIEVER  ;
        //        if (File.Exists("C:\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
        //            {
        //                sw.WriteLine("start url=" + url );
        //            }
        //        }
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

        //        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //        httpWebRequest.Method = "POST";

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        string result = "";
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            result = streamReader.ReadLine();
        //        }
        //        // notification log 
        //        NotificationLogs log = new NotificationLogs()
        //        { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //        _DbContext.NotificationLogs.Add(log);
        //        _DbContext.SaveChanges();
        //        return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

        //    }
        //    catch (Exception ex)
        //    {
        //        if (System.IO.File.Exists("C:\\testemail.txt"))
        //        {
        //            using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
        //            {
        //                sw.WriteLine("Function is = Send SMS UQU");
        //                sw.WriteLine("Mes=" + ex.Message);
        //                sw.WriteLine("Stack Trace=" + ex.StackTrace);
        //            }
        //        }
        //        return new ReturnSMSTypes { success = false, message = "Sending faild" };
        //    }
        //}
        private ReturnSMSTypes SendSMSPostAffairs(string recievermobilenumber, string[] body, string message)
        {
            try
            {
                string response = "";
                string serviceUrl = this.ServiceUrl;

                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }

                string receiver = recievermobilenumber.Length > 9
                                      ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                      : recievermobilenumber;
                receiver = "966" + receiver;
                string userName = HttpUtility.UrlEncode(this.Account);
                string password = HttpUtility.UrlEncode(this.Password);
                string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"Hajj sender"
                string mobileNumber = HttpUtility.UrlEncode(receiver);
                string text = HttpUtility.UrlEncode(message);


                string requestUrl = serviceUrl + "?UserName=" + userName + "&Password=" + password + "&MessageType=text&Recipients=" + mobileNumber + "&SenderName=" + sender + "&MessageText=" + text;

                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString(requestUrl);
                }
                // notification log 
                //_DbContext.Dispose();
                //_DbContext = new MainDbContext();
                //NotificationLogs log = new NotificationLogs()
                //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0]?.ToString() };
                //_DbContext.NotificationLogs.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                using (var _DbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = "Sending Fail because " + ex.InnerException.ToString() };
            }
        }
        private ReturnSMSTypes SendSMSPostAffairsNew(string recievermobilenumber, string[] body, string message)
        {
            try
            {
                string response = "";
                string serviceUrl = this.ServiceUrl;

                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }

                string receiver = recievermobilenumber.Length > 9
                                      ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                      : recievermobilenumber;
                receiver = "966" + receiver;
                string userName = HttpUtility.UrlEncode(this.Account);
                string password = HttpUtility.UrlEncode(this.Password);
                string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"Hajj sender"
                string mobileNumber = HttpUtility.UrlEncode(receiver);
                string text = HttpUtility.UrlEncode(message);


                string requestUrl = serviceUrl + "?user=" + userName + "&pwd=" + password + "&senderid=" + sender + "&CountryCode=966&mobileno=" + mobileNumber + "&msgtext=" + text;

                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString(requestUrl);
                }
                // notification log 
                //_DbContext.Dispose();
                //_DbContext = new MainDbContext();
                //NotificationLogs log = new NotificationLogs()
                //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0]?.ToString() };
                //_DbContext.NotificationLogs.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                using (var _DbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body.Length > 0? body[0].ToString():"" };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = "Sending Fail because " + ex?.InnerException??"".ToString() };
            }
        }
        private ReturnSMSTypes SendSMSPostMakkah(string recievermobilenumber, string[] body, string message)
        {
            try
            {
                string response = "";
                string serviceUrl = this.ServiceUrl;

                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }

                string receiver = recievermobilenumber.Length > 9
                                      ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                      : recievermobilenumber;
                receiver = "966" + receiver;
                string userName = HttpUtility.UrlEncode(this.Account);
                string password = HttpUtility.UrlEncode(this.Password);
                string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"Hajj sender"
                string mobileNumber = HttpUtility.UrlEncode(receiver);
                string text = HttpUtility.UrlEncode(message);


                string requestUrl = serviceUrl + "?strUserName=" + userName + "&strPassword=" + password + "&strTagName=" + sender + "&strRecepientNumbers=" + mobileNumber + "&strMessage=" + text + "&SendDateTime=0";

                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString(requestUrl);
                }
                // notification log 
                //_DbContext.Dispose();
                //_DbContext = new MainDbContext();
                //NotificationLogs log = new NotificationLogs()
                //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0]?.ToString() };
                //_DbContext.NotificationLogs.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                using (var _DbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = "Sending Fail because " + ex.InnerException.ToString() };
            }
        }
        private ReturnSMSTypes SendHajjSMS(string recievermobilenumber, string[] body, string message, string templateCode = null)
        {
            try
            {
                string response = "";
                string serviceUrl = this.ServiceUrl;

                if (File.Exists("C:\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }

                string receiver = recievermobilenumber.Length > 9
                                      ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                      : recievermobilenumber;
                receiver = "966" + receiver;
                string userName = HttpUtility.UrlEncode(this.Account);
                string password = HttpUtility.UrlEncode(this.Password);
                string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"Hajj sender"
                string mobileNumber = HttpUtility.UrlEncode(receiver);
                string text = HttpUtility.UrlEncode(message);


                string requestUrl = serviceUrl + "username=" + userName + "&password=" + password + "&Sender=" + sender + "&Text=" + text + "&number=" + mobileNumber;

                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString(requestUrl);
                }
                // notification log 
                //_DbContext.Dispose();
                //_DbContext = new MainDbContext();
                //NotificationLogs log = new NotificationLogs()
                //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0]?.ToString() };
                //_DbContext.NotificationLogs.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                //using (var _DbContext = _dbContextFactory.Create())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
            }
            catch (Exception ex)
            {
                //if (File.Exists("C:\\logsms.txt"))
                //{
                //    using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                //    {
                //        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "Sending Fail because " + ex.InnerException.ToString());
                //    }
                //}
                return new ReturnSMSTypes { success = false, message = "Sending Fail because " + ex.InnerException.ToString() };
            }

        }

        private ReturnSMSTypes SendRoomsSMS(string recievermobilenumber, string[] body, string message, string templateCode = null)
        {
            string response = "";
            string serviceUrl = this.ServiceUrl;

            if (File.Exists("C:\\logsms.txt"))
            {
                using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                {
                    sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                }
            }

            string receiver = recievermobilenumber.Length > 9
                                  ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                  : recievermobilenumber;
            receiver = "966" + receiver;
            string userName = HttpUtility.UrlEncode(this.Account);
            string password = HttpUtility.UrlEncode(this.Password);
            string sender = HttpUtility.UrlEncode(this.SMS_SENDER); //"Hajj sender"
            string mobileNumber = HttpUtility.UrlEncode(receiver);
            string text = HttpUtility.UrlEncode(message);


            string requestUrl = serviceUrl + "userid=" + userName + "&password=" + password + "&msg=" + text + "&sender=" + sender + "&to=" + mobileNumber + "&encoding=UTF8";
            if (File.Exists("C:\\logsms.txt"))
            {
                using (StreamWriter sw = File.AppendText("C:\\logsms.txt"))
                {
                    sw.WriteLine("start requestUrl=" + requestUrl);
                }
            }

            using (WebClient client = new WebClient())
            {
                response = client.DownloadString(requestUrl);
            }
            // notification log 
            //NotificationLogs log = new NotificationLogs()
            //{ Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
            //_DbContext.NotificationLogs.Add(log);
            //_DbContext.SaveChanges();
            // notification log 
            //using (var _DbContext = new MainDbContext())
            //{
            //    NotificationLogs log = new NotificationLogs()
            //    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
            //    _DbContext.NotificationLogs.Add(log);
            //    _DbContext.SaveChanges();
            //}
            // notification log 
            using (var _DbContext = _dbContextFactory.Create())
            {
                NotificationLog log = new NotificationLog()
                { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                _DbContext.NotificationLogs.Add(log);
                _DbContext.SaveChanges();
            }
            return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

        }

        private ReturnSMSTypes SendSMSPostYammahUrl(string recievermobilenumber, string[] body, string message, string templateCode = null)
        {
            try
            {
                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("Start json recievermobilenumber=" + recievermobilenumber + "  message" + message);
                    }
                }
                string receiver = recievermobilenumber;

                if (!recievermobilenumber.StartsWith("966"))
                {
                    receiver = "";
                    receiver = recievermobilenumber.Length > 9
                                           ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                           : recievermobilenumber;
                    receiver = "966" + receiver;
                }
                string RECIEVER = HttpUtility.UrlEncode(receiver);
                string MESSAGE = message;
                string url = this.ServiceUrl;
                SendSMSRequest request = new SendSMSRequest
                {
                    Username = this.Account,
                    Password = this.Password,
                    Message = MESSAGE,
                    RecepientNumber = RECIEVER,
                    Tagname = this.SMS_SENDER,
                    ReplacementList = string.Empty,
                    VariableList = string.Empty,
                    SendDateTime = 0
                };
                //its recommended for access API

                //1- Serialize it to string using Newtonsoft.Json its faster JSON converter (you can use your lib)
                // var requestJSON = JsonConvert.SerializeObject(request);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync(url, request).Result;
                //2- Call API by HttpPost.
                //var responseJSON = HttpPost("http://api.yamamah.com/SendSMS", requestJSON);

                //3- Deserialize response JSON to object
                //SendSMSResponse response = JsonConvert.DeserializeObject<SendSMSResponse>(responseJSON);
                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("End recievermobilenumber=" + recievermobilenumber + "  message" + message + "  strResponse" + response.StatusCode.ToString());
                    }
                }

                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                // notification log 
                using (var _DbContext = _dbContextFactory.Create())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

            }
            catch (Exception ex)
            {
                return new ReturnSMSTypes { success = false, message = "Sending Fail" };
            }
        }
        //private ReturnSMSTypes SendSMSPostEximSOAP(string recievermobilenumber, string[] body, string message, string templateCode = null)
        //{
        //    string url = this.ServiceUrl;
        //    string username = this.Account;
        //    string password = this.Password;
        //    string sender = this.SMS_SENDER;
        //    string code = this.SenderAPIKey;
        //    string receiver = recievermobilenumber;

        //    try
        //    {
        //        if (!recievermobilenumber.StartsWith("966") && !recievermobilenumber.StartsWith("+"))
        //        {
        //            receiver = "";
        //            receiver = recievermobilenumber.Length > 9
        //                                   ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
        //                                   : recievermobilenumber;
        //            receiver = "966" + receiver;
        //        }
        //        if (File.Exists("C:\\ucm_log\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
        //            {
        //                sw.WriteLine("request 0 :" + code);
        //            }
        //        }
        //        EXIM_SMS.EXIM_SMS exims_SMS = new EXIM_SMS.EXIM_SMS();
        //        if (File.Exists("C:\\ucm_log\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
        //            {
        //                sw.WriteLine("request 1 :" + receiver);
        //            }
        //        }

        //        var result = exims_SMS.EXIM_SendSMS(receiver, message, code, username, password);
        //        if (File.Exists("C:\\ucm_log\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
        //            {
        //                sw.WriteLine("request 2 :" + message);
        //            }
        //        }

        //        // notification log 
        //        using (var _DbContext = _dbContextFactory.Create())
        //        {
        //            NotificationLog log = new NotificationLog()
        //            { Subject = message, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
        //            _DbContext.NotificationLogs.Add(log);
        //            _DbContext.SaveChanges();
        //        }
        //        return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
        //    }

        //    catch (Exception ex)
        //    {
        //        if (File.Exists("C:\\ucm_log\\logsms.txt"))
        //        {
        //            using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
        //            {
        //                sw.WriteLine("sms exception 3:" + ex.Message);
        //            }
        //        }

        //        return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
        //    }

        //}
        private ReturnSMSTypes SendSMSPostYammahUrlOld(string recievermobilenumber, string[] body, string message, string templateCode = null)
        {
            try
            {
                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("start recievermobilenumber=" + recievermobilenumber + " " + "message" + message);
                    }
                }

                // http://api.yamamah.com/SendSMSV2?Username=CODE-Subaccontusername&Password=subaccountpassword&Tagname=SenderID&RecepientNumber=MobileNo&Message=MessageBody&SendDateTime=0&EnableDR=true&SentMessageID=true

                string url = this.ServiceUrl;
                string username = HttpUtility.UrlEncode(this.Account);
                string password = HttpUtility.UrlEncode(this.Password);
                string sender = HttpUtility.UrlEncode(this.SMS_SENDER);

                string receiver = recievermobilenumber;

                if (!recievermobilenumber.StartsWith("966"))
                {
                    receiver = "";
                    receiver = recievermobilenumber.Length > 9
                                           ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                           : recievermobilenumber;
                    receiver = "966" + receiver;
                }
                string RECIEVER = HttpUtility.UrlEncode(receiver);
                string MESSAGE = HttpUtility.UrlEncode(message);
                string postData = "Username=" + username + "&Password=" + password + "&Tagname=" + sender + "&RecepientNumber=" + RECIEVER + "&Message=" + MESSAGE + "&SendDateTime=0&EnableDR=true&SentMessageID=true";
                url = url + postData;

                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("sms service url =" + url);
                    }
                }

                var request = (HttpWebRequest)WebRequest.Create(url);

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();



                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("End recievermobilenumber=" + recievermobilenumber + "  message" + message + "  strResponse" + responseString);
                    }
                }

                //using (var _DbContext = new MainDbContext())
                //{
                //    NotificationLogs log = new NotificationLogs()
                //    { Subject = MESSAGE, Reciever = receiver, SendingDate = DateTimeOffset.Now, TransactionNumber = body[0].ToString() };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };

            }
            catch (Exception ex)
            {
                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("SMS SEND Exception = " + ex.Message.ToString());
                    }
                }

                return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
            }
        }

        private ReturnSMSTypes SendSMSPostYammahSOAP(string recievermobilenumber, string[] body, string message, string templateCode = null)
        {
            string url = this.ServiceUrl;
            string username = this.Account;
            string password = this.Password;
            string sender = this.SMS_SENDER;
            string code = this.SenderAPIKey;
            string sendURL = this.sendMessageURL;//http://www.gaca.gov.sa/wsdl/sms

            string receiver = recievermobilenumber;
            string requestBody = "";

            try
            {
                if (!recievermobilenumber.StartsWith("966") && !recievermobilenumber.StartsWith("+"))
                {
                    receiver = "";
                    receiver = recievermobilenumber.Length > 9
                                           ? recievermobilenumber.Substring(1, recievermobilenumber.Length - 1)
                                           : recievermobilenumber;
                    receiver = "966" + receiver;
                }

                requestBody = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sms=" + sendURL + @">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <sms:SendSingleSMSRq>
                                         <sms:MsgRqHdr>
                                            <sms:RqUID>ET" + Guid.NewGuid() + @"</sms:RqUID>
                                            <sms:SCId>ETS</sms:SCId>
                                         </sms:MsgRqHdr>
                                         <sms:Body>
                                            <sms:Username>" + username + @"</sms:Username>
                                            <sms:Password>" + password + @"</sms:Password>
                                            <sms:Tagname>" + sender + @"</sms:Tagname>
                                            <sms:RecepientNumber>" + receiver + @"</sms:RecepientNumber>
                                            <sms:Message>" + message + @"</sms:Message>
                                         </sms:Body>
                                      </sms:SendSingleSMSRq>
                                   </soapenv:Body>
                                </soapenv:Envelope>";
                //<sms:code>" + code + @"</sms:code>
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add(@"SOAP:Action");
                // webRequest.Headers.Add("Authorization", "Basic " + this.BasicAuthorizationToken);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                string responseXML = "";

                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("request url :" + url);
                        sw.WriteLine("requestBody :" + requestBody);
                    }
                }

                XDocument soapEnvelopeXml = XDocument.Parse(requestBody);

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }


                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseXML = rd.ReadToEnd();
                    }

                    if (File.Exists("C:\\ucm_log\\logsms.txt"))
                    {
                        using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                        {
                            sw.WriteLine("sms responseXML :" + responseXML);
                        }
                    }
                }

                return new ReturnSMSTypes { success = true, message = "Sending Successfully" };
            }

            catch (Exception ex)
            {
                if (File.Exists("C:\\ucm_log\\logsms.txt"))
                {
                    using (StreamWriter sw = File.AppendText("C:\\ucm_log\\logsms.txt"))
                    {
                        sw.WriteLine("sms exception :" + ex.Message);
                        sw.WriteLine("requestBody error :" + requestBody);
                    }
                }

                return new ReturnSMSTypes { success = false, message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
            }

        }

        public class SendSMSRequest
        {
            public string Username { get; set; }

            public string Password { get; set; }

            public string Tagname { get; set; }

            public string RecepientNumber { get; set; }

            public string VariableList { get; set; }

            public string ReplacementList { get; set; }

            public string Message { get; set; }

            public long SendDateTime { get; set; }
        }
        public class Message
        {
            public string senderName { get; set; }
            public string messageType { get; set; } = "text";
            public string recipients { get; set; }
            public string messageText { get; set; }

        }
        public class LoginKey
        {
            public string apiKey { get; set; }
            public string userName { get; set; }
        }
        public class LoginResponse
        {
            public int replyCode { get; set; }
            public string replyMessage { get; set; }
            public string requestId { get; set; }
            public string clientRequestId { get; set; }
            public string requestTime { get; set; }
            public Data data { get; set; }
        }
        public class Data
        {
            public string access_token { get; set; }
        }
    }
}
