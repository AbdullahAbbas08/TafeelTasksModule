namespace CommiteeAndMeetings.DAL.Views
{
    public class Vw_OrganizationToReferral
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public bool IsCategory { get; set; }
        public string Code { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public string FullPathAr { get; set; }
        public string FullPathEn { get; set; }
        public string FullPathFn { get; set; }

        public int? Priority { get; set; }
        public bool IsAdminOrganization { get; set; }
    }
}
