using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GhostFactory
{
    internal static class HelperMethods
    {
        public static void PreperateHTML(string angular_root, string html_path, params string[] stylesheets)
        {
            string text = ReadFile(html_path), res = string.Empty;

            MatchCollection mathes = Regex.Matches(text, "<img.*?>");
            foreach (Match item in mathes)
            {
                string imgSrc = Regex.Match(item.Value, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                var imgName = imgSrc.Split('/')[imgSrc.Split('/').Length - 1];
                text = text.Replace(imgSrc, Path.Combine(angular_root, "assets", "images", imgName));
            }

            //text = Regex.Replace(text, "<img.*?>", "<img src='" + logo + "' />");

            if (!text.ToLower().Contains("<!DOCTYPE HTML>".ToLower()))
            {
                res = "<!DOCTYPE HTML><html lang='ar'><head><meta charset='UTF-8'>";
                if(stylesheets != null)
                {
                    foreach (var item in stylesheets)
                    {
                        res += "<link href='" + item + "' rel='stylesheet'/>";
                    }
                }
                res += "</head><body>" + text + "</body></html>";

                //write file
                WriteFile(html_path, res);
            }
        }


        public static string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static byte[] ReadeBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void WriteFile(string path, string contents)
        {
            File.WriteAllText(path, contents, Encoding.UTF8);
        }

        public static Stream GenerateStreamFromText(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string GetTextFromstream(Stream stream)
        {
            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
            stream.Position = 0L;
            return reader.ReadToEnd();
        }
    }
}
