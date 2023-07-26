namespace Models.ProjectionModels
{
    public class SenderNamesDTO
    {
        public int Id { get; set; }
        public int TransactionActionId { get; set; }
        public int? OrganizationId { get; set; }
        public int? userId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string mainPhoto { get; set; }
        public string StampFile { get; set; }
        public string ProfileImage { get; set; }
    }
}
