
using CommiteeAndMeetings.DAL.Domains;
using EMailIntegration;
using IHelperServices;
using IHelperServices.Models;
using Microsoft.Extensions.Options;
using Models.ProjectionModels;
using System.Net.Mail;

namespace HelperServices
{
    public class MailServices : _HelperService, IMailServices
    {
        private EmailIntegration EmailIntegration { get; set; }

        public MailServices(IOptions<AppSettings> appSettings)
        {
            this.EmailIntegration = new EmailIntegration(appSettings);
        }

        public void Send(string sender, string[] to, string[] cc, string[] bcc, string title, string body)
        {
            this.EmailIntegration.Send(sender, to, cc, bcc, title, body);
        }

        public void SendNotificationEmail(string email, string subject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email, string angularRootPath, Transaction transaction, string Attachments )
        {
            this.EmailIntegration.SendNotificationEmail(email, subject, Body, htmlEnabled, htmlView, CC_Email, angularRootPath, transaction, Attachments);
        }
        public void Test(string email, string subject, string message, bool htmlEnabled)
        {
            this.EmailIntegration.Test(email, subject, message, htmlEnabled);
        }

        public void getMailMessage(Transaction transaction, ref string message, ref string mailSubject, string nameFrom, string nameFromEn, EmailParamsDTO emailParams)
        {
            this.EmailIntegration.getMailMessage(transaction, ref message, ref mailSubject, nameFrom, nameFromEn, emailParams);

        }
        public void getMailMessageForFollowUp(Transaction transaction, ref string message, ref string mailSubject, string nameFrom, string nameFromEn, EmailParamsDTO emailParams)
        {
            this.EmailIntegration.getMailMessageForFollowUp(transaction, ref message, ref mailSubject, nameFrom, nameFromEn, emailParams);

        }


    }


}