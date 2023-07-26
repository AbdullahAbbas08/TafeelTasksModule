namespace CommiteeAndMeetings.DAL.ProjectionModels
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public bool is_ldap_auth { get; set; }
        public bool is_mobile { get; set; }
        public bool is_vip { get; set; }
        public bool is_factor_auth { get; set; }
        public bool IsLocked { get; set; } = false;
        public bool IsTemp { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public string UserTN { get; set; }
        public string UserTP { get; set; }
        public string ResponseMessage { get; set; }
        public bool signature_factor_auth { get; set; }
        public string ERP_EmployeeNumber { get; set; }
        public bool HasRole { get; set; } = true;
    }
}
