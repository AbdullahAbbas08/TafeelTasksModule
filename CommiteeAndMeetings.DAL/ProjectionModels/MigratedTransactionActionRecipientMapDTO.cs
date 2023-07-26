namespace Models.ProjectionModels
{
    public class MigratedTransactionActionRecipientMapDTO
    {
        public long MigratedTransactionActionRecipientId { get; set; }

        public long? MigratedTransactionActionId { get; set; }

        public string DelegationType { get; set; }

        public string ParticipantUserName { get; set; }

        public string ParticipantOrganizationName { get; set; }

        public string ParticipantType { get; set; }

        public string Instructions { get; set; }

        public bool? IsSaved { get; set; }

        public bool? IsCC { get; set; }

        public string ToUserName { get; set; }

        public string ToDeptCode { get; set; }

        public bool? IsPending { get; set; }
    }
}
