using DbContexts.MasarContext.ProjectionModels;

namespace Models.ProjectionModels
{
    public class importancePeriodDTO
    {
        public int? importancePeriod { get; set; }
        public string transactionNumber { get; set; }
        public OrganizationDetailsDTO followUpOrgaization { get; set; }
        public int TransactionActionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
    }
}
