using CommiteeAndMeetings.DAL.Domains;
using Models.ProjectionModels;
using System.Net.Mail;

namespace IHelperServices
{
    public interface IMailServices : _IHelperService
    {
        void Send(string sender, string[] to, string[] cc, string[] bcc, string title, string body);
        void SendNotificationEmail(string email, string mailSubject, string Body, bool htmlEnabled, AlternateView htmlView, string CC_Email, string angularRootPath, Transaction transaction, string Attachments = "");
        void Test(string email, string subject, string message, bool htmlEnabled);
        void getMailMessage(Transaction transaction, ref string message, ref string mailSubject, string nameFrom, string nameFromEn, EmailParamsDTO emailParams);
        void getMailMessageForFollowUp(Transaction transaction, ref string message, ref string mailSubject, string nameFrom, string nameFromEn, EmailParamsDTO emailParams);
    }
}