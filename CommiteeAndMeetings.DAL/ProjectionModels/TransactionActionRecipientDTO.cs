namespace Models.ProjectionModels
{
    public class TransactionActionRecipientDTO
    {
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? RequiredActionId { get; set; }
        public bool IsCC { get; set; }
        public int? CorrespondentUserId { get; set; }
        public bool IsUrgent { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string Notes { get; set; }
        public bool SendNotification { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public byte[] ProfileImage { get; set; }
        public int? recipientStatusId { get; set; }

        public string FullName { get; set; }
        public string Profile_Image { get; set; }
    }
}
