using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Options;
using Models.ProjectionModels;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace EMailIntegration
{
    public class EmailIntegration
    {
        //private MainDbContext _DbContext = new MainDbContext();

        public AppSettings _AppSettings;
        private string SMTPServer { get; set; }
        private string AuthEmailFrom { get; set; }
        private string AuthDomain { get; set; }
        public string EmailFrom { get; set; }
        public int EmailPort { get; set; }
        public string EmailPassword { get; set; }
        public bool EnableSSL { get; set; }
        public string x_uqu_auth { get; set; }
        public string SenderAPIKey { get; set; }
        public string EmailPostActivate { get; set; }
        public string EmailXmlPath { get; set; }
        public string TransNumber { get; set; }
        public string SendEmailDelegationWithURL { get; set; }

        public string NotificationXsltPath { get; set; }
        private readonly string angular_root = "";
        public EmailIntegration(IOptions<AppSettings> appSettings)
        {
            angular_root = angular_root;
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            this.SMTPServer = this._AppSettings.EmailSetting.SMTPServer;
            this.AuthDomain = this._AppSettings.EmailSetting.AuthDomain;
            this.AuthEmailFrom = this._AppSettings.EmailSetting.AuthEmailFrom;
            this.EmailFrom = this._AppSettings.EmailSetting.EmailFrom;
            this.EmailPort = this._AppSettings.EmailSetting.EmailPort;
            this.EmailPassword = this._AppSettings.EmailSetting.EmailPassword;
            this.EnableSSL = this._AppSettings.EmailSetting.EnableSSL;
            this.x_uqu_auth = this._AppSettings.EmailSetting.x_uqu_auth;
            this.SenderAPIKey = this._AppSettings.EmailSetting.SenderAPIKey;
            this.EmailPostActivate = this._AppSettings.EmailSetting.EmailPostActivate;
            this.SendEmailDelegationWithURL = this._AppSettings.EmailSetting.SendEmailDelegationWithURL;
        }

        public void Send(string sender, string[] to, string[] cc, string[] bcc, string title, string body)
        {
            //throw new NotImplementedException();
        }

        public void getMailMessage(Transaction transaction, ref string mailMessage, ref string mailSubject, string nameFrom, string nameFromEn, EmailParamsDTO emailParams)
        {
            try
            {

                if (transaction.Classification == null)
                    mailSubject = "تكليف جديد";
                else
                    mailSubject = true ? transaction.Classification?.ClassificationNameAr : transaction.Classification?.ClassificationNameEn;

                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                string TransactionType = transaction.TransactionType.TransactionTypeNameAr;
                string TransactionTypeEn = transaction.TransactionType.TransactionTypeNameEn;


                mailSubject += "  -  " + transaction.TransactionNumberFormatted + " -" + transaction.Subject;

                string lblTransactionNumber = emailParams.lblTransactionNumber;
                string TransactionNumber = transaction.TransactionNumberFormatted;
                string lblTransactionSubject = emailParams.lblTransactionSubject;
                string Subject = transaction.Subject;
                string lblTransactionType = emailParams.lblTransactionType;
                string lblTransactionDate = emailParams.lblTransactionDate;
                string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                string lblUrl = emailParams.lblTransURL;
                string lblRequiredAction = emailParams.lblRequiredAction;
                string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                string lblUrlEn = emailParams.lblTransURLEn;
                string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                string lblMailHeader = emailParams.lblMailHeader;
                string lblThanks = emailParams.ThanksLocalization;
                string lblThanks2 = emailParams.ThanksLocalization2;
                string incomingOrgNameAr = string.Empty;
                string incomingOrgNameEn = string.Empty;
                string IncomingOrganizationTR = string.Empty;
                string ExternalDelegationOrgsTR = string.Empty;
                string Email_style = @"
                                            text-align: right;
                                            flex-direction: column;
                                            justify-content: center;
                                            align-items: center;";
                string Email_image_style = @" 
                                            margin: auto;
                                            display: center;
                                            justify-content: center;
                                            margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
                                            width: 100%;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: collapse;
                                            border-spacing: 2px;
                                            border-color: grey;";
                string tr_style = @" 
                                            white-space: normal;
                                            line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;

                                            font-variant: normal;";
                string td_style = @"
                                            border: 1px solid #cccccc;
                                            border-bottom: 1px solid #EEE;
                                            padding: 10px;
                                            width: 3px;
                                            margin-bottom: -3px;
                                            font-weight: 600;
                                            margin: -6px;
                                            ";
                string td_style_En = @"
                                            border: 1px solid #cccccc;
                                            border-bottom: 1px solid #EEE;
                                            padding: 10px;
                                            width: 3px;
                                            margin-bottom: -3px;
                                            font-weight: 600;
                                            margin: -6px;
                                            direction: ltr;";
                string w_20 = @"
                                            width: 20%;";
                string w_30 = @"
                                            width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";
                if (transaction.IncomingOrganizationId.HasValue && transaction.IncomingOrganization != null)
                {
                    incomingOrgNameAr = transaction.IncomingOrganization.OrganizationNameAr;
                    incomingOrgNameEn = transaction.IncomingOrganization.OrganizationNameEn;
                    IncomingOrganizationTR = $@"  <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{emailParams.lblIncomingOutgoingOrganizationEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{incomingOrgNameEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{incomingOrgNameAr}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{emailParams.lblIncomingOutgoingOrganization} </td>
                                        </tr>";
                }
                if (emailParams.IsExternalDelegation)
                {
                    ExternalDelegationOrgsTR = $@"  <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{emailParams.lblExternalOrgsDelegateEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{emailParams.OrgNamesEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{emailParams.OrgNamesAr}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{emailParams.lblExternalOrgsDelegate} </td>
                                        </tr>";
                }
                // Add url
                string HtmlString_new = $@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 100%;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                margin: 0;
                                                padding: 0;
                                            '>		
                                        <table style='width: 100%;'>
                                            <tr style='{tr_style}'>
                                                <td colspan='4' style='{td_style}'>
                                                <h3 style='
                                                text-align: center;
                                                background: #13817E;
                                                padding: 9px 0;
                                                font-weight: 900;
                                                margin-bottom: 0px;
                                                color: #fff;'>{lblMailHeader}</h3></td>
                                            </tr>
                                          <tr style='{tr_style}'>
                                            <td style='{td_style_En + ';' + w_20}'>{lblTransactionNumberEn} </ td >   
                                            <td colspan='2' style='{td_style + ';'  }text-align:right'><span style='unicode-bidi: bidi-override;'>{TransactionNumber}</span></td>
                                            <td style='{td_style + ';' + w_20 };text-align:right'>{lblTransactionNumber} </td>
                                          </ tr >  
                                            <tr style='{tr_style}'>
                                              <td style='{td_style_En + ';' + w_20}'>{lblTransactionSubjectEn}</td>
                                              <td colspan='2' style='{td_style + ';' + rtl}'>{Subject}</td>
                                              <td style ='{td_style + ';' + w_20 + rtl}'>{lblTransactionSubject}</td>   
                                          </ tr >     
                                          <tr style='{tr_style}'>
                                          <td style='{td_style_En + ';' + w_20}'>{lblTransactionTypeEn}</td>
                                          <td style='{td_style_En + ';' + w_30}'>{TransactionTypeEn}</td>
                                          <td style='{td_style + ';' + w_30 + rtl}'>{TransactionType}</td>
                                          <td style='{td_style + ';' + w_20 + rtl}'>{lblTransactionType}</td>
                                        </tr>
                                        <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{lblTransactionDateEn}</td>
                                        <td style='{td_style_En + ';' + w_30}'>{CreateDateEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{CreateDate}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{lblTransactionDate}</td>
                                        </tr>
                                        <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{lblDelegationFromOrgEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{nameFromEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{nameFrom}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{lblDelegationFromOrg} </td>
                                        </tr>";
                if (SendEmailDelegationWithURL == "1")
                {
                    HtmlString_new += @$"<tr style='{tr_style}'>
                                                                           <td style = '{td_style_En + ';' + w_20}'>{ lblUrlEn} </td>
                                                                           <td style = '{td_style_En + ';' + w_30}'><p><a href = '{emailParams.transactionActionRecipientToBeReplaceEn}'>{ lblUrlEn}</a></p> </td>
                                                                           <td style = '{td_style + ';' + w_30 + rtl}'><p><a href = '{emailParams.transactionActionRecipientToBeReplace}'>{lblUrl}</a></p> </td>
                                                                           <td style = '{td_style + ';' + w_20 + rtl}'>{ lblUrl} </td>
                                                                           </tr>";
                }
                HtmlString_new += @$"<tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{lblRequiredActionEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{emailParams.RequiredActionToBeReplaceEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{emailParams.RequiredActionToBeReplace}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{lblRequiredAction} </td>
                                        </tr>
                                       <tr style='{tr_style + ';' + tr_display}'>
                                            <td style='{td_style_En + ';' + w_20}'>{emailParams.lblNotesEn} </td>
                                            <td style='{td_style_En + ';' + w_30}'>{emailParams.NotesToBeReplaceEn}</td>
                                            <td style='{td_style + ';' + w_30 + rtl}'>{emailParams.NotesToBeReplace}</td>
                                            <td style='{td_style + ';' + w_20 + rtl}'>{emailParams.lblNotes} </td>
                                       </tr>
                                        {IncomingOrganizationTR}
                                        {ExternalDelegationOrgsTR}
                                      </table>
                                    </div>         
                                    <h2 style='margin-top: 57px;
                                             font-size: 17px;'>{lblThanks}</h2>
                                        <div style='{image_style}'>
                                            <img src='cid:header' >
                                            <img src='cid:footer' >
                                        </div>
                                         <h2 style='margin-top: 57px;
                                             font-size: 17px;'>{lblThanks2}</h2>
                                     </div>";


                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CreateMsgFile(string FromMail, string FromName, string ToMail, string Subject, string Body, string Attatchments, string fileName)
        {
            Spire.Email.MailAddress addressFrom = new Spire.Email.MailAddress(FromMail, FromName);

            Spire.Email.MailAddress addressTo = new Spire.Email.MailAddress(ToMail);

            Spire.Email.MailMessage mail = new Spire.Email.MailMessage(addressFrom, addressTo);
            mail.Subject = Subject;

            string htmlString = Body;
            mail.BodyHtml = htmlString;
            mail.Attachments.Add(new Spire.Email.Attachment("logo.png"));
            mail.Save(fileName, Spire.Email.MailMessageFormat.Msg);
        }
        public void getMailMessageForFollowUp(Transaction transaction, ref string mailMessage, ref string mailSubject, string nameFrom, string nameFromEn, EmailParamsDTO emailParams)
        {
            try
            {

                if (transaction.Classification == null)
                    mailSubject = "تكليف جديد";
                else
                    mailSubject = true ? transaction.Classification?.ClassificationNameAr : transaction.Classification?.ClassificationNameEn;

                string CreateDate = DateTime.Now.ToString("dddd , MMMM, yyyy", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                string TransactionType = transaction.TransactionType.TransactionTypeNameAr;
                string TransactionTypeEn = transaction.TransactionType.TransactionTypeNameEn;


                mailSubject += "  -  " + transaction.TransactionNumberFormatted + " -" + transaction.Subject;

                string lblTransactionNumber = emailParams.lblTransactionNumber;
                string TransactionNumber = transaction.TransactionNumberFormatted;
                string lblTransactionSubject = emailParams.lblTransactionSubject;
                string Subject = transaction.Subject;
                string lblTransactionType = emailParams.lblTransactionType;
                string lblTransactionDate = emailParams.lblTransactionDate;
                string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                string lblUrl = emailParams.lblTransURL;
                string lblRequiredAction = emailParams.lblRequiredAction;
                string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                string lblUrlEn = emailParams.lblTransURLEn;
                string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                string lblMailHeader = emailParams.lblMailHeader;
                string lblThanks = emailParams.ThanksLocalization;
                string incomingOrgNameAr = string.Empty;
                string incomingOrgNameEn = string.Empty;
                string IncomingOrganizationTR = string.Empty;
                string ExternalDelegationOrgsTR = string.Empty;
                string Email_style = @"
                                            text-align: center;
                                            flex-direction: column;
                                            justify-content: center;
                                            align-items: center;";
                string Email_image_style = @" 
                                            margin: auto;
                                            display: center;
                                            justify-content: center;
                                            margin: 32px;";
                string image_style = @"
                                            text-align: center;
                                            width: 90px;
                                            margin: 0;";
                string table_style = @"
                                            width: 810px;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: collapse;
                                            border-spacing: 2px;
                                            border-color: grey;";
                string tr_style = @" 
                                            white-space: normal;
                                            line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;

                                            font-variant: normal;";
                string td_style = @"
                                            border: 1px solid #cccccc;
                                            border-bottom: 1px solid #EEE;
                                            padding: 10px;
                                            width: 3px;
                                            margin-bottom: -3px;
                                            font-weight: 600;
                                            margin: -6px;
                                            ";
                string td_style_En = @"
                                            border: 1px solid #cccccc;
                                            border-bottom: 1px solid #EEE;
                                            padding: 10px;
                                            width: 3px;
                                            margin-bottom: -3px;
                                            font-weight: 600;
                                            margin: -6px;
                                            direction: ltr;";
                string w_20 = @"
                                            width: 20%;";
                string w_30 = @"
                                            width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";
                if (transaction.IncomingOrganizationId.HasValue && transaction.IncomingOrganization != null)
                {
                    incomingOrgNameAr = transaction.IncomingOrganization.OrganizationNameAr;
                    incomingOrgNameEn = transaction.IncomingOrganization.OrganizationNameEn;
                    IncomingOrganizationTR = $@"  <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{emailParams.lblIncomingOutgoingOrganizationEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{incomingOrgNameEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{incomingOrgNameAr}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{emailParams.lblIncomingOutgoingOrganization} </td>
                                        </tr>";
                }
                if (emailParams.IsExternalDelegation)
                {
                    ExternalDelegationOrgsTR = $@"  <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{emailParams.lblExternalOrgsDelegateEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{emailParams.OrgNamesEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{emailParams.OrgNamesAr}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{emailParams.lblExternalOrgsDelegate} </td>
                                        </tr>";
                }
                // Add url
                string HtmlString_new = $@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 100%;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                margin: 0;
                                                padding: 0;
                                            '>		
                                        <table style='width: 100%;'>
                                            <tr style='{tr_style}'>
                                                <td colspan='4' style='{td_style}'>
                                                <h3 style='
                                                text-align: center;
                                                background: #13817E;
                                                padding: 9px 0;
                                                font-weight: 900;
                                                margin-bottom: 0px;
                                                color: #fff;'>{lblMailHeader}</h3></td>
                                            </tr>
                                          <tr style='{tr_style}'>
                                            <td style='{td_style_En + ';' + w_20}'>{lblTransactionNumberEn} </ td >   
                                            <td colspan='2' style='{td_style + ';' + text_center}'><span style='unicode-bidi: bidi-override;'>{TransactionNumber}</span></td>
                                            <td style='{td_style + ';' + w_20 + rtl}'>{lblTransactionNumber} </td>
                                          </ tr >  
                                            <tr style='{tr_style}'>
                                              <td style='{td_style_En + ';' + w_20}'>{lblTransactionSubjectEn}</td>
                                              <td colspan='2' style='{td_style + ';' + text_center}'>{Subject}</td>
                                              <td style ='{td_style + ';' + w_20 + rtl}'>{lblTransactionSubject}</td>   
                                          </ tr >     
                                          <tr style='{tr_style}'>
                                          <td style='{td_style_En + ';' + w_20}'>{lblTransactionTypeEn}</td>
                                          <td style='{td_style_En + ';' + w_30}'>{TransactionTypeEn}</td>
                                          <td style='{td_style + ';' + w_30 + rtl}'>{TransactionType}</td>
                                          <td style='{td_style + ';' + w_20 + rtl}'>{lblTransactionType}</td>
                                        </tr>
                                        <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{lblTransactionDateEn}</td>
                                        <td style='{td_style_En + ';' + w_30}'>{CreateDateEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{CreateDate}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{lblTransactionDate}</td>
                                        </tr>
                                        <tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{lblDelegationFromOrgEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{nameFromEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{nameFrom}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{lblDelegationFromOrg} </td>
                                        </tr>";
                if (SendEmailDelegationWithURL == "1")
                {
                    HtmlString_new += @$"<tr style='{tr_style}'>
                                                  <td style = '{td_style_En + ';' + w_20}'>{ lblUrlEn} </td>
                                                  <td style = '{td_style_En + ';' + w_30}'><p><a href = '{emailParams.transactionActionRecipientToBeReplaceEn}'>{ lblUrlEn}</a></p> </td>
                                                  <td style = '{td_style + ';' + w_30 + rtl}'><p><a href = '{emailParams.transactionActionRecipientToBeReplace}'>{lblUrl}</a></p> </td>
                                                  <td style = '{td_style + ';' + w_20 + rtl}'>{ lblUrl} </td>
                                                  </tr>";
                }

                HtmlString_new += @$"<tr style='{tr_style}'>
                                        <td style='{td_style_En + ';' + w_20}'>{lblRequiredActionEn} </td>
                                        <td style='{td_style_En + ';' + w_30}'>{emailParams.RequiredActionToBeReplaceEn}</td>
                                        <td style='{td_style + ';' + w_30 + rtl}'>{emailParams.RequiredActionToBeReplace}</td>
                                        <td style='{td_style + ';' + w_20 + rtl}'>{lblRequiredAction} </td>
                                        </tr>
                                       <tr style='{tr_style + ';' + tr_display}'>
                                            <td style='{td_style_En + ';' + w_20}'>{emailParams.lblNotesEn} </td>
                                            <td style='{td_style_En + ';' + w_30}'>{emailParams.NotesToBeReplaceEn}</td>
                                            <td style='{td_style + ';' + w_30 + rtl}'>{emailParams.NotesToBeReplace}</td>
                                            <td style='{td_style + ';' + w_20 + rtl}'>{emailParams.lblNotes} </td>
                                       </tr>
                                        {IncomingOrganizationTR}
                                        {ExternalDelegationOrgsTR}
                                      </table>
                                    </div>         
                                    <h2 style='margin-top: 57px;
                                             font-size: 17px;'>{lblThanks}</h2>
                                        <div style='{image_style}'>
                                            <img src='cid:header' >
                                            <img src='cid:footer' >
                                        </div>
                                     </div>";


                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            //Replace('\r', ' ').Replace('\n', ' ');
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]", "\r", "\n" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], " ");
                }
            }
            return str;
        }


















































        public void SendNotificationEmail(string email, string subject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email, string angularRootPath, Transaction transaction, string Attachments = "")
        {
            switch (this.EmailPostActivate)
            {
                case "1":
                    this.SendEmailDefault(email, RemoveSpecialChars(subject), Body, htmlEnabled, htmlView, CC_Email, transaction, Attachments);
                    break;
                case "2":
                    LoadImagesAndXMLForEmail(angularRootPath);
                    this.SendEmailPostUQU(RemoveSpecialChars(subject), Body, email, this.EmailXmlPath, this.TransNumber, this.NotificationXsltPath, htmlEnabled, angularRootPath, transaction);
                    break;
                case "3":
                    this.SendEmailPostALJFS(RemoveSpecialChars(subject), Body, email, htmlView, htmlEnabled, transaction, Attachments);
                    break;
            }
        }

    
        private void SendEmailDefault(string email, string subject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email, Transaction transaction, string Attachments = "")
        {
            try
            {

                SmtpClient EmailSettings = new SmtpClient();

                EmailSettings.Host = SMTPServer;
                EmailSettings.Port = EmailPort;
                EmailSettings.UseDefaultCredentials = false;

                // exchange server have to put "AuthEmailFrom" and "AuthDomain" setting value on appsettings
                if (!string.IsNullOrEmpty(this.AuthDomain))
                    EmailSettings.Credentials = new NetworkCredential(AuthEmailFrom, EmailPassword, AuthDomain);
                else if (!string.IsNullOrEmpty(this.AuthEmailFrom))
                    EmailSettings.Credentials = new NetworkCredential(AuthEmailFrom, EmailPassword);
                else
                    EmailSettings.Credentials = new NetworkCredential(EmailFrom, EmailPassword);

                EmailSettings.EnableSsl = EnableSSL;
                EmailSettings.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(EmailFrom)
                };
                // mailMessage.To.Add(email);
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.IsBodyHtml = htmlEnabled;
                mailMessage.Subject = subject;

                if (!string.IsNullOrEmpty(CC_Email))
                    mailMessage.CC.Add(CC_Email);

                if (htmlEnabled)
                {
                    mailMessage.AlternateViews.Add(htmlView);
                }
                if (!String.IsNullOrEmpty(Body))
                {
                    mailMessage.Body = Body;
                }
                if (Attachments != "" && Attachments != null || Attachments != string.Empty)
                {
                    foreach (var item in Attachments.Split(","))
                    {
                        mailMessage.Attachments.Add(new System.Net.Mail.Attachment(item));
                    }

                }
                EmailSettings.Send(mailMessage);
                // notification log 
                //NotificationLog log = new NotificationLog()
                //{ Subject = mailMessage.Subject, Reciever = email, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction?.TransactionNumberFormatted };
                //_DbContext.NotificationLog.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                //using (var _DbContext = new MasarContext())
                //{
                //    NotificationLog log = new NotificationLog()
                //    { Subject = mailMessage.Subject, Reciever = email, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction?.TransactionNumberFormatted };
                //    _DbContext.NotificationLogs.Add(log);
                //    _DbContext.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("Function is = Send Email " + ex.StackTrace);
                    }
                }
            }
        }





















        private void SendEmailPostALJFS(string subject, string body, string email, AlternateView htmlView, bool htmlEnabled, Transaction transaction, string Attachments = "")
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(EmailFrom);
            msg.Subject = subject;
            msg.To.Add(new MailAddress(email));
            //msg.To.Add(email);
            msg.IsBodyHtml = htmlEnabled;
            SmtpClient client;
            if (!String.IsNullOrEmpty(SMTPServer) && EmailPort != 0)
                client = new SmtpClient(SMTPServer, EmailPort);
            else
                client = new SmtpClient();
            client.UseDefaultCredentials = true;
            NetworkCredential credentials = new NetworkCredential("", "");
            client.Credentials = credentials;
            client.EnableSsl = EnableSSL;
            ServicePointManager.ServerCertificateValidationCallback = null;
            try
            {
                if (htmlEnabled)
                {
                    msg.AlternateViews.Add(htmlView);
                }
                msg.Body = body;
                msg.Body = msg.Body.Replace(Environment.NewLine, "</br>");
                if (Attachments != "" && Attachments != null || Attachments != string.Empty)
                {
                    foreach (var item in Attachments.Split(","))
                    {
                        msg.Attachments.Add(new System.Net.Mail.Attachment(item));
                    }

                }
                client.Send(msg);
                // notification log 
                var TransactionNumber = (transaction == null) ? "" : transaction.TransactionNumberFormatted;
                //NotificationLog log = new NotificationLog()
                //{ Subject = msg.Subject, Reciever = email, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction.TransactionNumberFormatted };
                //_DbContext.NotificationLog.Add(log);
                //_DbContext.SaveChanges();
                // notification log 
                using (var _DbContext = new MasarContext())
                {
                    NotificationLog log = new NotificationLog()
                    { Subject = msg.Subject, Reciever = email, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction?.TransactionNumberFormatted };
                    _DbContext.NotificationLogs.Add(log);
                    _DbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("Function is = Send Email ALJFS");
                        sw.WriteLine("Mes=" + ex.Message);
                        sw.WriteLine("Stack Trace=" + ex.StackTrace);
                    }
                }
            }
        }

        public void Test(string email, string subject, string message, bool htmlEnabled)
        {
            string usserBedawy = "bedotest1993@gmail.com";
            SmtpClient EmailSettings = new SmtpClient();

            EmailSettings.Host = SMTPServer;
            EmailSettings.Port = EmailPort;
            EmailSettings.UseDefaultCredentials = false;
            EmailSettings.Credentials = new NetworkCredential(EmailFrom, EmailPassword);
            EmailSettings.EnableSsl = EnableSSL;
            EmailSettings.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(EmailFrom);
            mailMessage.To.Add(usserBedawy);
            mailMessage.IsBodyHtml = htmlEnabled;
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            try
            {
                EmailSettings.Send(mailMessage);

            }
            catch (Exception ex)
            {
                //string exe = ex.Message;
                //throw;
            }

        }
        private void SendEmailPostUQU(string subject, string message, string to, string emailXmlPath, string transNumber, string notificationXsltPath, bool htmlEnabled, string angularRootPath, Transaction transaction)
        {
            //Uri url = new Uri("");
            try
            {
                //Insert Data in EmailXML file
                //    FillNotificationMessage(emailXmlPath, message, transNumber);

                //Load XSLT file
                XslCompiledTransform XSLTransform = new XslCompiledTransform();
                XSLTransform.Load(notificationXsltPath, new XsltSettings(), new XmlUrlResolver());

                StringBuilder emailbuilder = new StringBuilder();
                XmlTextWriter xmlwriter = new XmlTextWriter(new System.IO.StringWriter(emailbuilder));
                XSLTransform.Transform(emailXmlPath, null, xmlwriter);

                string bodyText;

                XmlDocument xemaildoc = new XmlDocument();
                xemaildoc.LoadXml(emailbuilder.ToString());

                XmlNode bodyNode = xemaildoc.SelectSingleNode("//body");

                bodyText = bodyNode.InnerXml;
                if (bodyText.Length > 0)
                {
                    bodyText = bodyText.Replace("&amp;", "&");

                    bodyText = bodyText.Replace("&lt;", "<");
                    bodyText = bodyText.Replace("&gt;", ">");
                }
                //url = EmailSettings.SMTPServer;
                string RECIEVER = to;
                string MESSAGE = message;
                string SMTPSERVER = this.SMTPServer;
                string SenderAPIKey = this.SenderAPIKey;
                string x_uqu_auth = this.x_uqu_auth;
                string MessageType = "email";
                string MessageTitle = subject;
                Uri url = new Uri(SMTPSERVER + "?SenderAPIKey=" + SenderAPIKey + "&MessageType=" + MessageType + "&MessageContent=" + MESSAGE + "&MessageTitle=" + MessageTitle + "&ReceiversDirect=" + RECIEVER + "&IsDept=1");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["x-uqu-auth"] = x_uqu_auth;
                ServicePointManager.Expect100Continue = true;
                //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("Function is = Send Email UQU");

                        sw.WriteLine("Mes= mail is sent successfully");
                    }
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadLine();
                }
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    // notification log 
                    //NotificationLog log = new NotificationLog()
                    //{ Subject = subject, Reciever = to, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction.TransactionNumberFormatted };
                    //_DbContext.NotificationLog.Add(log);
                    //_DbContext.SaveChanges();
                    // notification log 
                    using (var _DbContext = new MasarContext())
                    {
                        NotificationLog log = new NotificationLog()
                        { Subject = subject, Reciever = to, SendingDate = DateTimeOffset.Now, TransactionNumber = transaction?.TransactionNumberFormatted };
                        _DbContext.NotificationLogs.Add(log);
                        _DbContext.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("Function is = Send Email UQU");
                        sw.WriteLine("Mes=" + ex.Message);
                        sw.WriteLine("Stack Trace=" + ex.StackTrace);
                    }
                }
            }
        }
        private void LoadImagesAndXMLForEmail(string angularRootPath)
        {
            try
            {
                this.EmailXmlPath = angularRootPath + "/assets/images/EmailImages/EmailTemplate/EmailTemplates.xml";
                this.NotificationXsltPath = angularRootPath + "/assets/images/EmailImages/EmailTemplate/EmailNotificationXSLT.xslt";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
