namespace Models.ProjectionModels
{
    public class UserSpecificOrganizationAndEmployeeDTO
    {
        public int Id { get; set; }
        public int? OrganizationId { get; set; }

        public int? UserId { get; set; }
        public int? SpecificUserId { get; set; }

        public int? SpecificOrganizationId { get; set; }
        public int? GroupId { get; set; }

        public string OrganizationName { get; set; }

        public string UserName { get; set; }

        public string SpecificUserName { get; set; }
        public string SpecificOrganizationName { get; set; }

        public bool IsOrganization { get; set; }


    }
}
