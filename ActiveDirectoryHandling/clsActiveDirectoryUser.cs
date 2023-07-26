namespace ActiveDirectoryHandling
{
    public class clsActiveDirectoryUser
    {
        private string _SID = string.Empty;
        private string _samaccountname = string.Empty;
        private string _givenname = string.Empty;
        private string _surename = string.Empty;
        private string _UserGroups = string.Empty;

        public string SID
        {
            get { return _SID; }
            set { _SID = value; }
        }


        public string SamAccountName
        {
            get { return _samaccountname; }
            set { _samaccountname = value; }
        }

        public string GivenName
        {
            get { return _givenname; }
            set { _givenname = value; }
        }

        public string SureName
        {
            get { return _surename; }
            set { _surename = value; }
        }

        public string FullName
        {
            get { return _givenname + " " + _surename; }
        }

        public string UserGroups
        {
            get { return _UserGroups; }
            set { _UserGroups = value; }
        }

    }
}
