namespace Models.ProjectionModels
{
    public class UserChatDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }
        public string FullNameAr { get; set; }
        public byte[] ProfileImage { get; set; }
        public string UserName { get; set; }
        public bool State { get; set; }
    }
}
