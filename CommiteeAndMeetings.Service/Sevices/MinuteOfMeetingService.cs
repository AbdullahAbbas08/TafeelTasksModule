using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using IHelperServices;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System.Web;
using CommiteeAndMeetings.BLL.Hosting;
using iTextSharp.text.pdf;
using HelperServices;
using CommiteeDatabase.Models.Domains;
using CommiteeAndMeetings.DAL.Domains;
using System.Net.Mail;
using CommiteeAndMeetings.DAL.CommiteeDomains;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class MinuteOfMeetingService : BusinessService<MinuteOfMeeting, MinuteOfMeetingDTO>, IMinuteOfMeetingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailServices _MailServices;
        private readonly IMeetingService _meetingService;
        private readonly ISystemSettingsService _SystemSettingsService;

        public string ChromeExecutablePath;
        private readonly string documents_root = "";
        public string FileName = "";
        ISmsServices smsServices;


        ICommiteeLocalizationService _commiteeLocalizationService;
        IHelperServices.ISessionServices _sessionServices;
        public MinuteOfMeetingService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISmsServices _smsServices, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, IMailServices MailServices, ICommiteeLocalizationService commiteeLocalizationService, IMeetingService meetingService, ISystemSettingsService systemSettingsService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            this._unitOfWork = unitOfWork;
            _MailServices = MailServices;
            _commiteeLocalizationService = commiteeLocalizationService;
            _sessionServices = sessionServices;
            _meetingService = meetingService;
            _SystemSettingsService = systemSettingsService;
            smsServices = _smsServices;
            this.documents_root = Path.Combine(Hosting.RootPath, "Documents\\");

        }

        public void DeleteMomComents(int id)
        {
            var comments = _unitOfWork.GetRepository<MOMComment>().GetAll().Where(c => c.MinuteOfMeetingId == id).ToList();
            foreach (var item in comments)
            {
                _unitOfWork.GetRepository<MOMComment>().Delete(item);
                _unitOfWork.SaveChanges();
            }
        }

        public override IEnumerable<MinuteOfMeetingDTO> Insert(IEnumerable<MinuteOfMeetingDTO> entities)
        {
            var minuteOfMeetings = base.Insert(entities);
            foreach (var minuteOfMeeting in minuteOfMeetings)
            {
                foreach (var item in minuteOfMeeting.Topics)
                {
                    var meetingTopic = _unitOfWork.GetRepository<MeetingTopic>().GetAll().Select(x => new MeetingTopicLookupDTO
                    {
                        Id = x.Id,
                        Points = x.TopicPoints,
                        Title = x.TopicTitle
                    }).FirstOrDefault(c => c.Id == item.MeetingTopicId);
                    item.MeetingTopic = new MeetingTopicLookupDTO
                    {
                        Id = item.MeetingTopicId,
                        Points = meetingTopic.Title,
                        Title = meetingTopic.Title
                    };
                }
            }
            return minuteOfMeetings;
        }
        public async Task<bool> MOMApproval(int meetingId, StringValues token, string masarUrl)
        {
            string pdfToken = token.FirstOrDefault().Replace("Bearer ", "");
            try
            {

                ChromeExecutablePath = _unitOfWork.GetRepository<SystemSetting>().GetAll().Where(s=>s.SystemSettingCode.ToLower()=="ChromeExecutablePath".ToLower()).FirstOrDefault().SystemSettingValue;
                masarUrl = _unitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(s => s.SystemSettingCode.ToLower() == "MasarUrl".ToLower()).FirstOrDefault().SystemSettingValue;
            }
            catch(Exception ex) { }
            var FilepathString = await UrlToPdfHeadless(meetingId, pdfToken, masarUrl);

            var meeting = _unitOfWork.GetRepository<Meeting>().GetById(meetingId);
            var _MinuteOfMeeting = _unitOfWork.GetRepository<MinuteOfMeeting>().GetAll().Where(x => x.MeetingId == meetingId).FirstOrDefault();
            string subject = _commiteeLocalizationService.GetLocaliztionByCode("MOMApprovalSubject", _sessionServices.CultureIsArabic); //"Minute of Meeting  Approval"
            string message = _commiteeLocalizationService.GetLocaliztionByCode("MOMApprovalMessage", _sessionServices.CultureIsArabic);//"Meeting of Meeting  Approval for : " + meeting.Subject

            try
            {
                //if (_MinuteOfMeeting != null)
               // {
                    if (_MinuteOfMeeting.EmailSentInvitation != true)
                    {
                        foreach (var item in meeting.MeetingAttendees)
                        {

                            //_MailServices.SendNotificationEmail(item.Attendee.Email, subject, message.Replace("{Title}", meeting.Title), false, null, "", "", null, FilepathString); ;

                            //Send Sms if attendee have mobile number 
                            var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddMOMApprovalNotificationText");

                            if (item.Attendee.NotificationBySms && !string.IsNullOrEmpty(item.Attendee.Mobile))
                            {
                                smsServices.SendSMS(item.Attendee.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + meeting.Title, null);
                            }

                        }

                        #region Voting on MOM By Email
                        if (meetingId != null)
                        {
                        DAL.CommiteeDTO.SurveyDTO surveyDTO = _meetingService.GetSurviesByMeetingId(meetingId).FirstOrDefault();
                            _meetingService.VotingByEmail(int.Parse(meetingId.ToString()), surveyDTO, FilepathString);


                            _MinuteOfMeeting.EmailSentInvitation = true;
                            _unitOfWork.GetRepository<MinuteOfMeeting>().Update(_MinuteOfMeeting);
                            _unitOfWork.GetRepository<MinuteOfMeeting>().Update();
                        }
                        #endregion
                    }

                //}
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SendMailMoMInvitation(int MeetingId)
        {
            MinuteOfMeeting _MinuteOfMeeting = _unitOfWork.GetRepository<MinuteOfMeeting>().GetAll().Where(x => x.MeetingId == MeetingId).FirstOrDefault();
            if(_MinuteOfMeeting == null)
                return false;
            return _MinuteOfMeeting.EmailSentInvitation;
        }

        public async Task<string> UrlToPdfHeadless(int meetingId, string token, string masarUrl)
        {
            bool status;
            string FileName;
            string Filepath;
            string masarURL;
            string tokenValue;
            string angularRoot;
            //MeetingSummaryDTO meetingSummary = new MeetingSummaryDTO();
            //meetingSummary = GetMeetingData(TId, out status);

            //FileName = meetingSummary.Title + ".pdf";



            FileName = "MinutesOfMeeting";
            masarURL = masarUrl;
            angularRoot = Hosting.AngularRootPath;
            //Filepath = $@"c://{FileName}";

            tokenValue = token;

            if (!string.IsNullOrEmpty(ChromeExecutablePath))
            {
                string attachDirectory = Path.Combine(documents_root, Guid.NewGuid().ToString());
                if (!Directory.Exists(attachDirectory))
                    Directory.CreateDirectory(attachDirectory);

                string pdf_path = Path.Combine(attachDirectory, FileName + ".pdf");
                string static_downloadjs_path = Path.Combine(angularRoot, @"assets\static-pages\static-download.js");
                string static_downloadhtml_path = Path.Combine(angularRoot, @"assets\static-pages\static-download.html");

                #region Copy Static-File
                //FileName = meetingSummary.Title;

                // 1- create new Paths

                string newHtmlFile = Path.Combine(angularRoot, $@"assets\static-pages\{FileName}.html");
                string newJsFile = Path.Combine(angularRoot, $@"assets\static-pages\{FileName}.js");

                // 2- copy static-files 


                try
                {
                    File.Copy(static_downloadjs_path, newJsFile, true);
                    File.Copy(static_downloadhtml_path, newHtmlFile, true);
                }
                catch (IOException iox)
                {
                    Console.WriteLine(iox.Message);
                }

                // 3- replace values in html
                string[] htmlFileLines = File.ReadAllLines(static_downloadhtml_path);
                string newJsFileName = $"./{FileName}.js";
                string newHtmlFileName = $"{FileName}.html";
                for (int i = 0; i < htmlFileLines.Length; i++)
                {
                    if (htmlFileLines[i].Contains(@"./static-download.js"))
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
                    staticdownloadpathcontent = staticdownloadpathcontent.Replace("{hosturl}", masarURL);
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
                    //parameters.Add("Authorization", "Bearer 
                    //await page.SetExtraHttpHeadersAsync(parameters);
                    //var content = await page.GetContentAsync();
                    //HttpUtility.UrlEncode(this.masarUrl + "/assets/static-pages/static-download.html?TId=" + TId + "&AttachId=" + AttachId + "&Anno=" + Anno + "&W=" + Watermark);
                    //await page.GoToAsync("http://www.google.com" , navigation);
                    string routePathe = masarURL + $"/assets/static-pages/{newHtmlFileName}?meetingId=" + meetingId;
                    await page.GoToAsync(routePathe, navigation);

                    await page.PdfAsync(pdf_path, new PdfOptions { PrintBackground = true, Format = PaperFormat.A4 });
                    //string non_editable_pdf_path = convertPDfToNonEditable(pdf_path, FileName + ".pdf");
                    return pdf_path;
                }
            }
            else
            {
                return "";
            }
        }

        private string convertPDfToNonEditable(string old_pdf_path, string AttachName)
        {
            string attachDirectory = Path.Combine(documents_root, Guid.NewGuid().ToString());
            if (!Directory.Exists(attachDirectory))
                Directory.CreateDirectory(attachDirectory);

            string pdf_path = Path.Combine(attachDirectory, AttachName.Contains(".") ? AttachName.Split(".")[0] + ".pdf" : AttachName + ".pdf");

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









        public string PrepareStyle(MeetingSummaryDTO meetingSummary)
        {
            int summaryCount = meetingSummary.MOMSummaries.Count;
            int attendeeCount = meetingSummary.MeetingAttendees.Count;
            int commentCount = meetingSummary.MOMComment.Count;
            string PDF_style = @"
                                        text-align: center;
                                        flex-direction: column;
                                        justify-content: center;
                                        align-items: center;
                                        direction: rtl;

";

            string head_style = @"
                                        text-align: right;
                                        direction: rtl;
                                        margin:10px;
                                        font-weight:bold;
                                        font-size:18px;
direction: rtl;

";

            string head_style2 = @"
                                        text-align: center;
                                        direction: rtl;
                                        margin:10px;
                                        font-weight:bold;
                                        font-size:18px;
direction: rtl;

";

            string table_style1 = @"
                                            justify -content: center;
                                            margin-bottom: 15px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-: separate;
                                            border-color: gray;
                                            width:100%;
                                            
";

            string table_style2 = @"
                                            justify-content: center;
                                            margin-bottom: 15px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-: separate;
                                            border-color: white;
                                            width:100%;

                                            
";

            string tr_style = @" 
                                            white-space: normal;
                                            line-height: normal;
                                            border-bottom: 1px solid #cccccc;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: bold;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            direction: rtl;
                                            font-variant: normal;";


            string td_style = @"
                                            padding: 10px;
                                            font-weight: 600;
                                            text-align:right;
direction: rtl;
                                            
                                            ";

            string td_style2 = @"
                                            padding: 10px;
                                            font-weight: 600;
                                            text-align:right;
                                            border-right: 1px solid #cccccc;
                                            border-bottom: 1px solid #cccccc;
direction: rtl;

                                            ";

            string meta = "<charset='utf-8'>";
            string Startpdf = $@"<div style='{PDF_style}'>";
            string title = $@"<div style='{head_style2}'>{meetingSummary.Title}</div>";
            string top = $@"<table style='{table_style2}>
                                <tr style='{tr_style}'>
                                    <td style='{td_style}' lang='ar'> التاريخ </td >
                                    <td style='{td_style}' lang='ar'> {meetingSummary.Date} </td >
                                    <td style='{td_style}' lang='ar'> من </td >
                                    <td style='{td_style}' lang='ar'> {meetingSummary.MeetingFromTime.Hour}:{meetingSummary.MeetingFromTime.Minute} </td >
                                    <td style='{td_style}' lang='ar'> الى </td >
                                    <td style='{td_style}' lang='ar'> {meetingSummary.MeetingToTime.Hour}:{meetingSummary.MeetingToTime.Minute} </td >
                                 </tr >
                                
                                 <tr style='{tr_style}'>
                                    <td style='{td_style}' colspan='3' lang='ar'> المكان </td >
                                    <td style='{td_style}' colspan='3' lang='ar'> {meetingSummary.PhysicalLocation} </td >
                                 </tr >
                                 <tr style='{tr_style}'>
                                    <td style='{td_style}' colspan='3' lang='ar''> عنوان الاجتماع </td >
                                    <td style='{td_style}' colspan='3' lang='ar'> {meetingSummary.Title} </td >
                                 </tr >
                             </table > ";

            ////////////////////////////////////////////

            string title2 = $@"<div style='{head_style}' lang='ar' >ملخص الاجتماع </div>";


            string summaryTableStart = $@"<table style='{table_style1}>
                                    <tr style='{tr_style}'>
                                        <td style='{td_style2}' lang='ar'> الرقم </td >
                                        <td style='{td_style2}' lang='ar'> الموضوع </td >
                                        <td style='{td_style2}' lang='ar' > المناقشات </td >
                                 </tr >";




            for (int i = 0; i < summaryCount; i++)
            {
                string summaryRow = $@"
                                <tr style='{tr_style}'>
                                    <td style='{td_style2}' lang='ar' > {i + 1} </td >
                                    <td style='{td_style2}' lang='ar' > {meetingSummary.MOMSummaries[i].Title} </td >
                                    <td style='{td_style2}' lang='ar'  > {meetingSummary.MOMSummaries[i].Description} </td >
                                 </tr >
                                 ";

                summaryTableStart = summaryTableStart + summaryRow;
            }

            summaryTableStart = summaryTableStart + $@"</table>";

            //////////////////////



            string title3 = $@"<div style='{head_style}'  >الحاضرين  </div>";
            string AttendeeTableStart = $@"<table style='{table_style1}>
                                    <tr style='{tr_style}'>
                                        <td style='{td_style2}' lang='ar' > الاسم </td >
                                        <td style='{td_style2}' lang='ar' > الوظيفه </td >
                                        <td style='{td_style2}' lang='ar'  > رقم التليفون </td >
                                        <td style='{td_style2}' lang='ar'  > البريد الالكتروني </td >
                                 </tr >";




            for (int i = 0; i < attendeeCount; i++)
            {
                string AttendeRow = $@"
                                <tr style='{tr_style}'>
                                        <td style='{td_style2}' lang='ar' > {meetingSummary.MeetingAttendees[i].Attendee.FullNameAr} </td >
                                        <td style='{td_style2}' lang='ar' > {meetingSummary.MeetingAttendees[i].Attendee.JobTitleId} </td >
                                        <td style='{td_style2}' lang='ar'  > {meetingSummary.MeetingAttendees[i].Attendee.WorkPhoneNumber} </td >
                                        <td style='{td_style2}' lang='ar'  > {meetingSummary.MeetingAttendees[i].Attendee.Email} </td >
                                 </tr >";

                AttendeeTableStart = AttendeeTableStart + AttendeRow;
            }

            AttendeeTableStart = AttendeeTableStart + $@"</table>";

            /////////////////////////////////////////////////////



            string title4 = $@"<div style='{head_style}'  lang='ar'>التوصيات  </div>";
            string CommentTableStart = $@"<table style='{table_style1}>
                                    <tr style='{tr_style}'>
                                        <td style='{td_style2}' lang='ar' > الرقم </td >
                                        <td style='{td_style2}' lang='ar'  > التوصيات </td >
                                 </tr >";



            if (commentCount > 0)
            {
                for (int i = 0; i < commentCount; i++)
                {
                    string CommentRow = $@"
                                <tr style='{tr_style}'>
                                        <td style='{td_style2}' lang='ar'> {i + 1} </td >
                                        <td style='{td_style2}' lang='ar'> {meetingSummary.MOMComment[i].Comment.Text} </td >
                                 </tr >";

                    CommentTableStart = CommentTableStart + CommentRow;
                }

            }
            else
            {
                string CommentRow = $@"
                                <tr style='{tr_style}'>
                                        <td style='{td_style2}' lang='ar'> 0 </td >
                                        <td style='{td_style2}' lang='ar'> لا يوجد توصيات  </td >
                                 </tr >";

                CommentTableStart = CommentTableStart + CommentRow;
            }


            CommentTableStart = CommentTableStart + $@"</table>";











            string Endpdf = $@" </div>";

            string finalpdf = meta + Startpdf + title + top + title2 + summaryTableStart + title3 + AttendeeTableStart + title4 + CommentTableStart + Endpdf;
            //StringBuilder sb = new StringBuilder();
            //sb.Append(meta);
            //sb.Append(Startpdf);
            //sb.Append(title);
            //sb.Append(top);
            //sb.Append(title2);
            //sb.Append(summaryTableStart);
            //sb.Append(title3);
            //sb.Append(AttendeeTableStart);
            //sb.Append(title4);
            //sb.Append(CommentTableStart);
            //sb.Append(Endpdf);
            return finalpdf;
        }

        public MeetingSummaryDTO GetMeetingData(int meetingId, out bool status)
        {
            MeetingSummaryDTO meeting = new MeetingSummaryDTO();
            try
            {
                meeting = _meetingService.GetMeetingSummary(meetingId);
                status = true;
                return meeting;
            }
            catch (Exception ex)
            {
                status = false;
                return null;
            }
        }




    }
}
