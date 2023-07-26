namespace Models.ProjectionModels
{
    public class OrganizationGroupDTO
    {
        public int OrganizationGroupId { get; set; }
        public string OrganizationGroupNameAr { get; set; }
        public string OrganizationGroupNameEn { get; set; }
        public string OrganizationGroupNameFn { get; set; }

        public int UserId { get; set; }
        public int OrganizationGroupPriority { get; set; }

    }
}
