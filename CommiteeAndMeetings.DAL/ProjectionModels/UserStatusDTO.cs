namespace Models.ProjectionModels
{
    public class UserStatusDTO
    {
        public int UserId { get; set; }
        public bool Enable { get; set; }
        public string UserName { get; set; }
        public string SSN { get; set; }
        public string JobTitle { get; set; }
    }
}
