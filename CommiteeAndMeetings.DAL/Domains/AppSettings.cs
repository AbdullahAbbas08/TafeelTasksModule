namespace IHelperServices.Models
{
    public class ConnectionStrings
    {
        public string MainConnectionString { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
    }

    public class SeedData
    {
        public bool SeedDataOnStart { get; set; }
        public string RelativeDirectory { get; set; }
    }

    public class SwaggerToTypeScriptClientGeneratorSettings
    {
        public string SourceDocumentAbsoluteUrl { get; set; }
        public string OutputDocumentRelativePath { get; set; }
    }

    public class FileSettings
    {
        public string RelativeDirectory { get; set; }
    }

    public class DocumentSettings
    {
        public string RelativeDirectory { get; set; }
        public string ECMServer { get; set; }
        public string ECMRepository { get; set; }
        public string ECMUserName { get; set; }
        public string ECMPassword { get; set; }
        public string YearIsHijri { get; set; }
        public string ECMType { get; set; }
        public string OfficeSetting { get; set; }
        public string EnableYearMonths { get; set; }

    }

    public class EmailSetting
    {
        public string SMTPServer { get; set; }
        public string AuthEmailFrom { get; set; }
        public string AuthDomain { get; set; }
        public string EmailFrom { get; set; }
        public int EmailPort { get; set; }
        public string EmailPassword { get; set; }
        public bool EnableSSL { get; set; }
        public string x_uqu_auth { get; set; }
        public string SenderAPIKey { get; set; }
        public string EmailPostActivate { get; set; }
        public string EmailXmlPath { get; set; }
        public string TransNumber { get; set; }
        public string NotificationXsltPath { get; set; }
        public string SendEmailDelegationWithURL { get; set; }
    }

    public class SMSSettings
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }
        public string SmsPostActivate { get; set; }
        public string SenderAPIKey { get; set; }
        public string x_uqu_auth { get; set; }
        public string SMS_SENDER { get; set; }
        public string senderName { get; set; }
        public string apiKey { get; set; }
        public string sendMessageURL { get; set; }

        public string LoginURl { get; set; }
        public string UserName { get; set; }

    }

    public class AzureADSettings
    {
        public int IsEnabled { get; set; }
        public string clientId { get; set; }
        public string tenant { get; set; }
        public string authority { get; set; }
    }

    public class AuditSetting
    {
        public int IsEnabled { get; set; }
        public string BaseUrl { get; set; }
    }
    public class SystemSettingOptions
    {
        public string SystemUsers { get; set; }
        public string FactorAuth { get; set; }
        public string SignatureFactorAuth { get; set; }

        public string SSO_LOGIN { get; set; }
        public string JWTLogOffUQU { get; set; }
        public string Nafaz { get; set; }
        public string SSOURL { get; set; }
        public string SendAttachmentForDelegation { get; set; }
    }
    public class HangFireSettings
    {
        public int IsEnabled { get; set; }
        public string URL { get; set; }
    }
    public class ExternalUserSetting
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
        public SeedData SeedData { get; set; }
        public SwaggerToTypeScriptClientGeneratorSettings SwaggerToTypeScriptClientGeneratorSettings { get; set; }
        public FileSettings FileSettings { get; set; }
        public DocumentSettings DocumentSettings { get; set; }
        public EmailSetting EmailSetting { get; set; }
        public SMSSettings SMSSettings { get; set; }
        public AuditSetting AuditSettings { get; set; }
        public SystemSettingOptions SystemSettingOptions { get; set; }
        public ExternalUserSetting ExternalUserSettings { get; set; }
        public HangFireSettings HangFireSettings { get; set; }
        public AzureADSettings azureADSettings { get; set; }
        public string AbsoluteUrl { get; set; }
        public string MasarURL { get; set; }
    }
}