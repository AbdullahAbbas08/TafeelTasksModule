namespace Models.ProjectionModels
{
    public class MigratedTransactionActionDTO
    {
        public long MigratedTransactionActionId { get; set; }

        public long? MigratedTransactionId { get; set; }

        public string FromOrganizationName { get; set; }

        public string UrgencyLevel { get; set; }

        public string ImportanceLevel { get; set; }

        public string DelegationDate { get; set; }

        public string Instructions { get; set; }

        public string ApplicationType { get; set; }

        public string FromUserName { get; set; }

        public string FromOrganizationCode { get; set; }


        public string TransactionActionDate { get; set; }

        public string TransactionActionDateHijri { get; set; }
    }
}
