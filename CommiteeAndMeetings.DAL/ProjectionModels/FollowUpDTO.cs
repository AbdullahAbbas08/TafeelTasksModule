using System;

namespace Models.ProjectionModels
{
    public class FollowUpDTO
    {
        public int FollowUpId { get; set; }
        public bool isEmpolyee { get; set; }
        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public int FollowUpStatusTypeId { get; set; }
        public int? ChangeStatusByUserId { get; set; }
        public DateTimeOffset FollowUpStatusCreatedOn { get; set; }
        public DateTimeOffset ModifiedDateTo { get; set; }
        public DateTimeOffset FinishedDate { get; set; }

        public int importanceLevelId { get; set; }

    }
}
