namespace Models.ProjectionModels
{
    public class RecipientsNamesDTO
    {
        public int Id { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }
        public bool IsCC { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public string ProfileImage { get; set; }
    }
}
