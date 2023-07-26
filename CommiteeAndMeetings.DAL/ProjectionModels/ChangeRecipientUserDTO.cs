namespace Models.ProjectionModels
{
    public class ChangeRecipientUserDTO
    {
        public int TransactionActionRecipientId { get; set; }
        public int Old_UserId { get; set; }
        public int New_UserId { get; set; }
    }
}
