namespace Models.ProjectionModels
{
    public class UserCorrespondentOrganizationDTO
    {
        public int UserCorrespondentOrganizationId { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }

        //custom
        public string UserName { get; set; }
        public string OrganizationName { get; set; }
    }
}
