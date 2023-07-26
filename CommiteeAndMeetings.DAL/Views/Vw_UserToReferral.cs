namespace CommiteeAndMeetings.DAL.Views
{
    public class Vw_UserToReferral
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }

        public string Email { get; set; }
        public int? ProfileImageFileId { get; set; }
        public bool NotificationByMail { get; set; }
        public bool NotificationBySMS { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

    }
}
