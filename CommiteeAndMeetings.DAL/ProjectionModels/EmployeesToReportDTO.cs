namespace Models.ProjectionModels
{
    public class EmployeesToReportDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }
        public string Email { get; set; }
        public bool NotificationByMail { get; set; }
        public bool NotificationBySMS { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public byte[] ProfileImage { get; set; }
    }
}
