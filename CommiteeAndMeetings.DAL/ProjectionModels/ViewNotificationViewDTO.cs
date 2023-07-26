using System;

namespace Models.ProjectionModels
{
    public class ViewNotificationViewDTO
    {
        public int notificationId { get; set; }
        public long? transactionId { get; set; }
        public int? transactionActionId { get; set; }
        public int? transactionActionRecipientId { get; set; }
        public int? userId { get; set; }
        public int? organizationId { get; set; }
        public string contentAr { get; set; }
        public string contentEn { get; set; }
        public string contentFn { get; set; }

        public int notificationTypeId { get; set; }
        public string notificationTypeNameAr { get; set; }
        public string notificationTypeNameEn { get; set; }
        public string notificationTypeNameFn { get; set; }

        public string fromUserNameAr { get; set; }
        public string fromUserNameEn { get; set; }
        public string fromUserNameFn { get; set; }

        public int IsEmployee { get; set; }
        public int? DismissedBy { get; set; }
        public DateTimeOffset? DismissedOn { get; set; }
        public string fromUserProfileImage { get; set; }
        public bool fromUser { get; set; }

        public string transactionIdEncrypt { get; set; }
        public string transactionActionRecipientIdEncrypt { get; set; }
        public string transactionActionIdEncrypt { get; set; }
    }
}
