namespace Models.ProjectionModels
{
    public class Params
    {
        public auth auth { get; set; }
        public string transactionNumber { get; set; }
        public string hijriYear { get; set; }
        public string transactionTypeCode { get; set; }

    }
    public class ParamsHijri
    {
        public auth auth { get; set; }
        public string transactionNumber { get; set; }
        public string hijriYear { get; set; }

    }
    public class SearchParams
    {
        public auth auth { get; set; }
        public string transactionNumber { get; set; }

    }
    public class OrganizationHasAccessParams
    {
        public auth auth { get; set; }
        public string transactionNumber { get; set; }

        public string OrganizationCode { get; set; }
        public int HasPrefix { get; set; }
    }
    public class UploadFilesParams
    {
        public string LFServerName { get; set; }
        public string LFRepostry { get; set; }
        public string LFUserName { get; set; }
        public string LFUserPassword { get; set; }

        public string FilePath { get; set; }
        public string FolderPath { get; set; }
        public string PathOfLF { get; set; }
    }
    public class DownloadFilesParams
    {
        public string EntryID { get; set; }
        public string FolderPathToSave { get; set; }
        public string LFServerName { get; set; }
        public string LFRepostry { get; set; }

        public string LFUserName { get; set; }
        public string LFUserPassword { get; set; }
    }
    public class StatusResult
    {
        public string EntryIDs { get; set; }
        public string Status { get; set; }
    }
    public class auth
    {
        public string moduleName { get; set; }
        public string password { get; set; }

    }

}
