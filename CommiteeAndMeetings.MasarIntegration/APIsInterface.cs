using CommiteeAndMeetings.MasarIntegration.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.MasarIntegration
{
    public static class APIsInterface
    {
      //  static string absoluteUrl;
        static APIsInterface()
        {
           // string startupPath = System.IO.Directory.GetCurrentDirectory()+ "appsettings.json";
          //  absoluteUrl = System.Configuration.ConfigurationManager.AppSettings["AbsoluteUrl"];
        }
        public static string Get(string uri, Dictionary<string, string> parameters,string accessToken,string absoluteUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                if (parameters != null)
                    return client.GetAsync($"{absoluteUrl}{uri}".AttachParameters(parameters)).Result.Content.ReadAsStringAsync().Result;
                else
                    return client.GetAsync($"{absoluteUrl}{uri}").Result.Content.ReadAsStringAsync().Result;
            }
        }

        public static string Post(string uri, Dictionary<string, string> parameters, HttpContent data, string accessToken,string absoluteUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                if (parameters != null)
                    return client.PostAsync($"{absoluteUrl}{uri}".AttachParameters(parameters),data).Result.Content.ReadAsStringAsync().Result;
                else
                    return client.PostAsync($"{absoluteUrl}{uri}", data).Result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
