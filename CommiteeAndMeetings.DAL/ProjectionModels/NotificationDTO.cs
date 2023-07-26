using System;

namespace Models.ProjectionModels
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int NotificationTypeId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserId { get; set; }

        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int? DismissedBy { get; set; }
        public DateTimeOffset? DismissedOn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }

    }
}
