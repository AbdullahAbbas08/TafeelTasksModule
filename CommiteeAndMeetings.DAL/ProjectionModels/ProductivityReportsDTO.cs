namespace Models.ProjectionModels
{
    public class ProductivityReportsDetailsDTO
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNamefn { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }
        public int OrganizationId { get; set; }
        public int roleId { get; set; }
        public string roleNameEn { get; set; }
        public string roleNameAr { get; set; }
        public string roleNameFn { get; set; }
        public int cnt { get; set; }
    }
}
