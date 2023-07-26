using Aspose.Email;
using Aspose.Email.Mapi;
using Ghostscript.NET;
using Ghostscript.NET.Processor;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using iTextSharp.text.pdf;

namespace GhostFactory
{
    public class GhostScriptFactory
    {
        private readonly string hosting_root = "";
        private readonly string angular_root = "";
        private readonly string documents_root = "";
        private readonly string Fonts_root = "";
        private readonly string Images_root = "";
        private static readonly string HtmlToPdfExeName = "wkhtmltopdf.exe";
        private static readonly object ConvertPDFToTiffAndSaveLocker = new object();
        private readonly string pdfResolution;
        private readonly Boolean pdfToColor;
        public string ChromeExecutablePath;
        public string masarUrl;


        public GhostScriptFactory(string hosting_root, string angular_root, string _pdfResolution, Boolean _pdfToColor)
        {
            this.hosting_root = hosting_root;
            this.angular_root = angular_root;
            this.pdfResolution = _pdfResolution;
            this.pdfToColor = _pdfToColor;

            /* file path on local */
            //this.Fonts_root = Path.Combine(hosting_root, "ClientApp\\src\\assets\\fonts\\");
            /* file path on server */
            this.Fonts_root = Path.Combine(angular_root, "assets\\fonts\\");
            this.Images_root = Path.Combine(angular_root, "assets\\images\\");
            this.documents_root = Path.Combine(hosting_root, "Documents\\");
            if (!Directory.Exists(documents_root))
            {
                Directory.CreateDirectory(documents_root);
            }
        }

        public GhostScriptFactory(StringBuilder outlook_root)
        {

        }

        public async System.Threading.Tasks.Task<string> UrlToPdfHeadless(string tokenValue, int committeeId, string userName)
        {
            if (!string.IsNullOrEmpty(ChromeExecutablePath))
            {
                string userFileName = "";

                string attachDirectory = Path.Combine(documents_root, Guid.NewGuid().ToString());
                if (!Directory.Exists(attachDirectory))
                    Directory.CreateDirectory(attachDirectory);

                string pdfFileName = $"CommitteMintues_{committeeId}_{userName}";

                string pdf_path = Path.Combine(attachDirectory, $"{pdfFileName}.pdf");
                string committeMinutes_downloadjs_path = Path.Combine(angular_root, @"assets\static-pages\committeMinutes.js");
                string committeMinutes_downloadhtml_path = Path.Combine(angular_root, @"assets\static-pages\committeMinutes.html");

                #region Copy Static-File
                userFileName = userName;

                // 1- create new Paths
                string newHtmlFile = Path.Combine(angular_root, $@"assets\static-pages\{userFileName}.html");
                string newJsFile = Path.Combine(angular_root, $@"assets\static-pages\{userFileName}.js");

                // 2- copy static-files 
                File.Copy(committeMinutes_downloadjs_path, newJsFile, true);
                File.Copy(committeMinutes_downloadhtml_path, newHtmlFile, true);

                // 3- replace values in html
                string[] htmlFileLines = File.ReadAllLines(committeMinutes_downloadhtml_path);
                string newJsFileName = $"./{userFileName}.js";
                string newHtmlFileName = $"{userFileName}.html";
                for (int i = 0; i < htmlFileLines.Length; i++)
                {
                    if (htmlFileLines[i].Contains(@"./committeMinutes.js"))
                    {
                        //< script src = "./static-download.js" ></ script >
                        htmlFileLines[i] = $@"<script src=""{newJsFileName}""></script>";
                        break;
                    }
                }
                File.WriteAllLines(newHtmlFile, htmlFileLines);
                #endregion

                string staticdownloadpathcontent = File.ReadAllText(newJsFile);
                if (staticdownloadpathcontent.Contains("{accesstokenvalue}"))
                {
                    staticdownloadpathcontent = staticdownloadpathcontent.Replace("{accesstokenvalue}", tokenValue);
                    File.WriteAllText(newJsFile, staticdownloadpathcontent);
                }
                else
                {
                    string[] staticdownloadpathcontentlines = File.ReadAllLines(newJsFile);
                    for (int i = 0; i < staticdownloadpathcontentlines.Length; i++)
                    {
                        if (staticdownloadpathcontentlines[i].Contains("var token ="))
                        {
                            staticdownloadpathcontentlines[i] = "var token =\"" + tokenValue + "\";";
                            break;
                        }
                    }
                    File.WriteAllLines(newJsFile, staticdownloadpathcontentlines);
                }
                if (staticdownloadpathcontent.Contains("{hosturl}"))
                {
                    staticdownloadpathcontent = staticdownloadpathcontent.Replace("{hosturl}", this.masarUrl);
                    File.WriteAllText(newJsFile, staticdownloadpathcontent);
                }
                //await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

                using (PuppeteerSharp.Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, IgnoreHTTPSErrors = true, ExecutablePath = ChromeExecutablePath }))
                {
                    var navigation = new NavigationOptions
                    {
                        Timeout = 0,
                        WaitUntil = new[] { WaitUntilNavigation.Networkidle0 },
                    };
                    var page = await browser.NewPageAsync();
                    await page.SetJavaScriptEnabledAsync(true);
                    //Dictionary<string, string> parameters = new Dictionary<string, string>();
                    //parameters.Add("Authorization", "Bearer ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAianRpIjogIjk5NjQ4NWQ5LTk1OGQtNGE4Zi04NzNjLTkxY2YxZDg0ZDc5NSIsDQogICJpc3MiOiAiaHR0cDovL2xvY2FsaG9zdDo1MDAwLyIsDQogICJpYXQiOiAxNjQ0NzYzODg0LA0KICAiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiAiMSIsDQogICJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjogIm9hajl4V29DdjZlTE9Nb21DZkRmUUlzbnZCUkVpaFQ2ZUc3S0w1alpBQ2ZESjhPQmZRSXNudlQ2ZUc3S0NmREo4T0JmUUlzbnZUNmVHN0siLA0KICAiV3IweDRyS1pRRkNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S2xmMVJzTmI3VDV2ZkZDSnl5eDkzcWJYa1ZEa1hnT3VrQ2ZESjhPQmZRSXNudlQ2ZUc3SyI6ICJLdUNmRGZRSXNudkJSRWloVDZlRzdLMVFVZ1ZxajJoWmxacXh3T0ZKQ1hkZThxS0l1bGtJRFVVQjBDZkRKOE9CZlFJc252QlJFaWhUNmVHN0t3V1VMbDVYSXplWHpsMmx6MURGU3Q0WnBkIiwNCiAgImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvc2VyaWFsbnVtYmVyIjogImQ3ZmMyNTA0OWMzYjRiMDk4MTI3ODM5ZTdiZjdjMTk5IiwNCiAgImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvdXNlcmRhdGEiOiAiMSIsDQogICJlWDhDZkRKOE9CZlFJc252QlJFaWhUNmVHN0s5N2gwWDJHUlRhVFNWekFVNkdBWVljMzBxZVI2cTdoZEswcWtLbEFDZkRKOE9CZlFJc252VDZlRzdLIjogIklZaXhZUloxM0huNnVaamVwYm1MY2dDZkRKOE9CZlFJc252VDZlRzdLQ2ZESjhPQmZRSXNudlQ2ZUc3SyIsDQogICJzdE5UVXlNS3RTeFVmNENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S2Rpbm0xdkdEaVNKcUozZVRrb2xHcGk3cDQwcFI2blowQVBJdkQ1MjRKQ2ZEZlFJc252QlJFaWhUNmVHN0tLbGJONUhEIjogIlJ5YXdTblVTZnhUMFRSWlJrc1FFb2dDZkRKOE9CZlFJc252VDZlRzdLQ2ZESjhPQmZRSXNudlQ2ZUc3SyIsDQogICJ3ck9sZWxkcU1ZWHE4SmxNejZNY2V0c0M2M21oQ25tZ1dKTUdrblFvcjhKTTJvY2NWUWpIcG5uaG5HVk5pZ2FHIjogIlBKWWZSMjhkYTdwQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLY1FKTlk2eUdPQUNmREo4T0JmUUlzbnZUNmVHN0tDZkRKOE9CZlFJc252VDZlRzdLIiwNCiAgImVhakNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3SzY1MGJ5REJVOU04Z0Nqa3V1NExjVWxhM3hFTW1teHZrR09Femgxc0NmREo4T0JmUUlzbnZUNmVHN0siOiAiS3VDZkRmUUlzbnZCUkVpaFQ2ZUc3SzFRVWdWcWoyaFpsWnF4d09GSkNYZGU4cUtJdWxrSURVVUIwQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLd1dVTGw1WEl6ZVh6bDJsejFERlN0NFpwZCIsDQogICJlYWpDZkRKOE9CZlFJc252QlJFaWhUNmVHN0s2NTBieURCVTlNOGdDamt1dTNzOVI0b0wza2FWRlF1YUZtRWhseFlDZkRKOE9CZlFJc252VDZlRzdLIjogImJYRkYzOUlYSUpzWmtDZkRmUUlzbnZCUkVpaFQ2ZUc3S0N1YW9TNXFYZGxLMDlHbzlxeU5jTzlieGFvYUNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S25kZXFkQWhGT2xqeWNOR0xMa1NxU0IiLA0KICAic3ROVFV5TUt0U3hVZjRDZkRKOE9CZlFJc252QlJFaWhUNmVHN0tkaW5tMXZHRGlTSnFKM2VUa29sR3BpN3A0MHBRNFBNOXV4eE9FM0dKQ2ZEZlFJc252QlJFaWhUNmVHN0s0dnVjSlR1emNBdjk2emFZY2c4OFlZcUNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S3NJcUtOZ0NmREo4T0JmUUlzbnZUNmVHN0tDZkRKOE9CZlFJc252VDZlRzdLIjogImVFck5kMDAwQkhXNmRTQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLTXlyR0pIemJOaUNmRGZRSXNudkJSRWloVDZlRzdLS1h4R3VmakFYTDFtT0ZjaWdDZkRKOE9CZlFJc252VDZlRzdLIiwNCiAgInN0TlRVeU1LdFN4VWY0Q2ZESjhPQmZRSXNudkJSRWloVDZlRzdLZGlubTF2R0RpU0pxSjNlVGtvbEdwaTdwNDBwUWNvWU5zUjhCYTk2Q2ZESjhPQmZRSXNudkJSRWloVDZlRzdLcmd0WjBGU0NmRGZRSXNudkJSRWloVDZlRzdLWmJ0ZkNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S2ZpVU9DZkRKOE9CZlFJc252QlJFaWhUNmVHN0tFMjV3SXlkMWVjVkZRQ2ZESjhPQmZRSXNudlQ2ZUc3S0NmREo4T0JmUUlzbnZUNmVHN0siOiAib25WWDhMdHhLTFhzYWJoSG1DbG93Z0NmREo4T0JmUUlzbnZUNmVHN0tDZkRKOE9CZlFJc252VDZlRzdLIiwNCiAgImVYOENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3Szk3aDBYMkdSVGFUU1Z6QVU2T3VScEVpTUtWcjFkTVdWSkg5TmxIcEFtcTRadjJNZ1o4UEVLUGV0TVZnZCI6ICJLQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLem1MTEdoWGhvQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLQ2ZEZlFJc252QlJFaWhUNmVHN0tjbFhmeENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S3NEWXBLWW1td0lGOTQyQ2ZEZlFJc252QlJFaWhUNmVHN0tUQmhyNTRyeWtDZkRKOE9CZlFJc252VDZlRzdLIiwNCiAgImVYOENmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3Szk3aDBYMkdSVGFUU1Z6QVU2T3dvUUxPdXlWNDdXcVJqbW13aWhETnBhcmxPdWl6eVA3Q2ZESjhPQmZRSXNudkJSRWloVDZlRzdLS3gyWE1aRzJwIjogImhTNjRjR3habkptUTNXNHlZQkNmRGZRSXNudkJSRWloVDZlRzdLcDJrdUNmREo4T0JmUUlzbnZCUkVpaFQ2ZUc3S2Nic3BSTTl2SkxsNmJhYmFBQjBHUHEzTUx1aFd6bzh5dWdZQ2ZESjhPQmZRSXNudkJSRWloVDZlRzdLblJNYyIsDQogICJuYmYiOiAxNjQ0NzYzODg1LA0KICAiZXhwIjogMTY0NDc3MTA4NSwNCiAgImF1ZCI6ICJBbnkiDQp9.MYES73taG8aI8pbsi5Lg241bKKKMtGzBziGMdXiPIpo");
                    //await page.SetExtraHttpHeadersAsync(parameters);
                    //var content = await page.GetContentAsync();
                    //HttpUtility.UrlEncode(this.masarUrl + "/assets/static-pages/static-download.html?TId=" + TId + "&AttachId=" + AttachId + "&Anno=" + Anno + "&W=" + Watermark);
                    await page.GoToAsync(this.masarUrl + $"/assets/static-pages/{newHtmlFileName}?id={committeeId}", navigation);
                    await page.PdfAsync(pdf_path, new PdfOptions { PrintBackground = true, Format = PaperFormat.A4 });
                    string non_editable_pdf_path = convertPDfToNonEditable(pdf_path, pdfFileName);
                    return non_editable_pdf_path;
                }
            }
            else
            {
                return "";
            }
        }

        public string ConvertHTMLContentToTIFF(string text, bool clean = true, params string[] styles)
        {
            string html_path = Path.Combine(documents_root, Guid.NewGuid() + ".html");
            string pdf_path = Path.Combine(documents_root, Guid.NewGuid() + ".pdf");
            HelperMethods.WriteFile(html_path, text);
            #region change fonts paths
            if(styles != null)
            {
                string Fonts = File.ReadAllText(styles[0]);
                if (Fonts.Contains("/*{Fonts Modified}*/"))
                {
                    Fonts = Fonts.Replace("../fonts/", this.Fonts_root.Replace("\\", "/"))
                        .Replace("../../images/", this.Images_root.Replace("\\", "/"))
                        .Replace("{Fonts Modified}", "");
                    File.WriteAllText(styles[0], Fonts);
                }
            }
            #endregion
            return ConvertHTMLtoTIFF(html_path, pdf_path, clean, styles);
        }

        public byte[] CreateMSGFile(string Subject, string Body, string Attatchments, string fileName, string RootPath)
        {
            //  List<string> recipients = new List<string>();
            //  recipients.Add("admin@Masar.com");
            ////  Create a new MailItem and set the To, Subject, and Body properties.
            //  Outlook.Application application2 = new Outlook.Application();

            //  var newMail = application2.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;

            // // Set up all the recipients.
            //  foreach (var recipient in recipients)
            //  {
            //      newMail.Recipients.Add(recipient);
            //  }
            //  if (newMail.Recipients.ResolveAll())
            //  {
            //      newMail.Subject = Subject;
            //      newMail.HTMLBody = Body;

            //      foreach (string attachment in Attatchments.Split(","))
            //      {
            //          newMail.Attachments.Add(attachment, Outlook.OlAttachmentType.olByValue);
            //      }
            //  }

            // For complete examples and data files, please go to https://github.com/aspose-email/Aspose.Email-for-.NET
            // Create an instance of the MailMessage class
            Aspose.Email.MailMessage mailMsg = new Aspose.Email.MailMessage();

            // Set from, to, subject and body properties
            mailMsg.From = "sender@domain.com";
            mailMsg.To = "";
            mailMsg.Subject = Subject;
            mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMsg.IsBodyHtml = true;
            mailMsg.HtmlBody = Body;
            if (Attatchments != "" && Attatchments != null || Attatchments != string.Empty)
            {
                foreach (var item in Attatchments.Split(","))
                {
                    mailMsg.Attachments.Add(new Aspose.Email.Attachment(item));
                }

            }
            // Create an instance of the MapiMessage class and pass MailMessage as argument
            MapiMessage outlookMsg = MapiMessage.FromMailMessage(mailMsg);

            // Save the message (MSG) file
            // string strMsgFile = @"CreatingAndSavingOutlookMessages_out.msg";
            var filepath = Path.Combine(RootPath, "Documents\\" + Guid.NewGuid() + ".msg");
            mailMsg.Save(filepath, SaveOptions.DefaultMsg);
            byte[] buff = null;
            FileStream fs = new FileStream(filepath,
                                           FileMode.Open,
                                           FileAccess.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(filepath).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
        public string ConvertHTMLtoTIFF(string html_path, string pdf_path, bool clean = true, params string[] styles)
        {
            lock (ConvertPDFToTiffAndSaveLocker)
            {
                try
                {
                    //html to pdf
                    ConvertHtmlToPDF(html_path, pdf_path, clean, styles);

                    //pdf to tiff
                    return ConvertPDFtoTIFF(pdf_path, clean);
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public string ConvertHTMLtoTIFFOutlook(string html_path, string pdf_path, bool clean = true, string Encoding = "utf-8")
        {
            lock (ConvertPDFToTiffAndSaveLocker)
            {
                try
                {
                    //html to pdf
                    OutLookConvertHtmlToPDF(html_path, pdf_path, clean, Encoding);

                    //pdf to tiff
                    return ConvertPDFtoTIFF(pdf_path, clean);
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public void ConvertHtmlToPDF(string html_path, string pdf_path, bool clean = true, params string[] stylesheets)
        {
            try
            {
                //Add Style Tags And Covert Logical Paths To Physical Paths
                HelperMethods.PreperateHTML(angular_root, html_path, stylesheets);

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //startInfo.FileName = Path.Combine(HtmlToPdfExeName);
                startInfo.FileName = Path.Combine(hosting_root, "exe", HtmlToPdfExeName);
                //startInfo.Arguments = $"-B 0 -L 0 -T 0 --disable-smart-shrinking --encoding utf-8 --page-size A4 \"{string.Join("\" \"", html_path)}\" \"{pdf_path}\"";
                startInfo.Arguments = $"-B 0 -L 0 -R 0 -q -n --page-width 121.5 --page-height 175 --zoom 1 --margin-top 2.5 --encoding utf-8 \"{string.Join("\" \"", html_path)}\" \"{pdf_path}\"";
                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }
                if (clean) try
                    {
                        System.Threading.Tasks.Task.Run(() => File.Delete(html_path));
                    }
                    catch { }
            }
            catch (Exception ex) { throw ex; }
        }

        public void OutLookConvertHtmlToPDF(string html_path, string pdf_path, bool clean = true, string Encoding = "utf-8")
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //startInfo.FileName = Path.Combine(HtmlToPdfExeName);
                startInfo.FileName = Path.Combine(hosting_root, "exe", HtmlToPdfExeName);
                //startInfo.Arguments = $"-B 0 -L 0 -T 0 --disable-smart-shrinking --encoding utf-8 --page-size A4 \"{string.Join("\" \"", html_path)}\" \"{pdf_path}\"";
                startInfo.Arguments = $"-B 0 -L 0 -R 0 -q -n --page-width 121.5 --page-height 179.25 --zoom 1 --margin-top 2.5 --encoding {Encoding} \"{string.Join("\" \"", html_path)}\" \"{pdf_path}\"";
                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }
                if (clean) try { System.Threading.Tasks.Task.Run(() => File.Delete(html_path)); } catch { }
            }
            catch (Exception ex) { throw ex; }
        }

        private static readonly object LockObject = 1;

        public string ConvertPDFtoTIFF(string pdf_path, bool clean = true)
        {
            lock (LockObject)
            {
                PDFConvert converter = new PDFConvert();
                converter.ResolutionX = int.Parse(this.pdfResolution);
                converter.TextAlphaBit = -1;
                converter.RenderingThreads = -1;
                converter.FirstPageToConvert = -1;
                converter.LastPageToConvert = -1;
                converter.OutputToMultipleFile = false;
                converter.JPEGQuality = 1000;
                //converter.DefaultPageSize = "A4";
                //converter.FitPage = true;

                if (this.pdfToColor || IsColoredPdf(pdf_path))
                    converter.OutputFormat = "tiff12nc";
                else
                    converter.OutputFormat = "tifflzw";

                FileInfo input = new FileInfo(pdf_path);
                string output = string.Format("{0}\\{1}{2}", input.Directory, Guid.NewGuid(), ".tiff");
                while (File.Exists(output))
                {
                    output = output.Replace(".tiff", string.Format("{0}{1}", Guid.NewGuid(), ".tiff"));
                }
                string res = converter.Convert(input.FullName, output) == true ? output : "";
                if (clean) try { System.Threading.Tasks.Task.Run(() => File.Delete(pdf_path)); } catch { }
                return res;
            }
        }

        public string ConvertPDFtoTIFF(byte[] pdf_bytes, bool clean = true)
        {
            string pdf_path = Path.Combine(documents_root, Guid.NewGuid() + ".pdf");
            File.WriteAllBytes(pdf_path, pdf_bytes);
            return ConvertPDFtoTIFF(pdf_path, clean);
        }

        public string ConvertHTMLtoTIFF(byte[] html_bytes, bool clean = true)
        {
            string html_path = Path.Combine(documents_root, Guid.NewGuid() + ".HTML");
            string pdf_path = Path.Combine(documents_root, Guid.NewGuid() + ".pdf");
            File.WriteAllBytes(html_path, html_bytes);
            ConvertHtmlToPDF(html_path, pdf_path, true);
            return ConvertPDFtoTIFF(pdf_path, clean);
        }

        private bool IsColoredPdf(string file)
        {
            bool isColored = false;
            try
            {
                GhostscriptProcessor proc = new GhostscriptProcessor();
                GhostscriptPipedOutput gsPipedOutput = new GhostscriptPipedOutput();
                string outputPipeHandle = "%handle%" + int.Parse(gsPipedOutput.ClientHandle).ToString("X2");

                List<string> switches = new List<string>();
                switches.Add("-empty");
                switches.Add("-q");
                switches.Add("-o" + outputPipeHandle);
                switches.Add("-sDEVICE=inkcov");
                switches.Add(file);

                proc.StartProcessing(switches.ToArray(), null);

                string output = System.Text.Encoding.ASCII.GetString(gsPipedOutput.Data);
                string[] lines = output.Split('\n');
                string zero = "0.00000";
                foreach (string line in lines)
                {
                    string[] colors = line.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    if (colors.Count() >= 3 && (colors[0] != zero || colors[1] != zero || colors[2] != zero))
                    {
                        isColored = true;
                        break;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return isColored;
        }

        private string convertPDfToNonEditable(string old_pdf_path, string AttachName)
        {
            string attachDirectory = Path.Combine(documents_root, Guid.NewGuid().ToString());
            if (!Directory.Exists(attachDirectory))
                Directory.CreateDirectory(attachDirectory);

            string pdf_path = Path.Combine(attachDirectory, $"{AttachName}.pdf");

            PdfReader reader = new PdfReader(old_pdf_path);
            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    // add your content
                }
                using (FileStream fs = new FileStream(
                  pdf_path, FileMode.Create, FileAccess.ReadWrite))
                {
                    PdfEncryptor.Encrypt(new PdfReader(ms.ToArray()), fs, null, null, PdfWriter.ALLOW_PRINTING, true);
                }
            }
            return pdf_path;
        }
        // Office Converter
        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }
        public Microsoft.Office.Interop.Excel.Workbook ExcelDocument { get; set; }
        public string ConvertWordToTIFF(byte[] Word_bytes, string extension, bool clean = true)
        {
            string Word_path = Path.Combine(documents_root, Guid.NewGuid() + extension);
            string pdf_path = Path.Combine(documents_root, Guid.NewGuid() + ".pdf");
            File.WriteAllBytes(Word_path, Word_bytes);
            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            wordDocument = appWord.Documents.Open(Word_path);
            wordDocument.ExportAsFixedFormat(Path.Combine(documents_root, pdf_path), WdExportFormat.wdExportFormatPDF);
            wordDocument.Close();

            appWord.Quit();
            //if (wordDocument != null)
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDocument);
            //appWord = null;
            //wordDocument = null;
            //GC.Collect();

            try { System.Threading.Tasks.Task.Run(() => System.IO.File.Delete(Word_path)); } catch { }
            return ConvertPDFtoTIFF(pdf_path, clean);
        }
        public string ConverExcelToTIFF(byte[] Excel_bytes, string extension, bool clean = true)
        {
            string Excel_path = Path.Combine(documents_root, Guid.NewGuid() + extension);
            string pdf_path = Path.Combine(documents_root, Guid.NewGuid() + ".pdf");
            File.WriteAllBytes(Excel_path, Excel_bytes);
            Microsoft.Office.Interop.Excel.Application excelApplication = new Microsoft.Office.Interop.Excel.Application
            {
                ScreenUpdating = false,
                DisplayAlerts = false,
                Visible = false,
            };
            Workbook excelWorkbook = excelApplication.Workbooks.Open(Excel_path);
            ((Microsoft.Office.Interop.Excel._Worksheet)excelWorkbook.ActiveSheet).PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
            excelWorkbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pdf_path);
            excelWorkbook.Close();

            excelApplication.Quit();
            //if (excelWorkbook != null)
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
            //excelWorkbook = null;
            //excelWorkbook = null;
            //GC.Collect();

            try { System.Threading.Tasks.Task.Run(() => System.IO.File.Delete(Excel_path)); } catch { }
            return ConvertPDFtoTIFF(pdf_path, clean);
        }
        public bool DeleteCopiedFiles(string userName)
        {
            if (userName != null)
            {
                string newHtmlFile = Path.Combine(angular_root, $@"assets\static-pages\{userName}.html");
                string newJsFile = Path.Combine(angular_root, $@"assets\static-pages\{userName}.js");

                File.Delete(newHtmlFile);
                File.Delete(newJsFile);

                return true;
            }
            return false;
        }
    }
}
