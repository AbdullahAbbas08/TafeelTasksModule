using System.Collections.Generic;
using System.Text;

namespace CommiteeAndMeetings.MasarIntegration.Utility
{
    public static class UriExtensions
    {
        public static string AttachParameters(this string uri, Dictionary<string, string> parameters)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string str = "?";

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                stringBuilder.Append(str + parameter.Key + "=" + parameter.Value);
                str = "&";
            }
            return uri + stringBuilder;
        }
    }
}
