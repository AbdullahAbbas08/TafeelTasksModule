using System;

namespace Models.ProjectionModels
{
    public class TransactionDetailLogDTO
    {
        public int Id { get; set; }
        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public bool? FromSearch { get; set; }
        public string Organization { get; set; }
    }
}
