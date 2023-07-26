using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_Notifications_CreatedBy")]
    [Index(nameof(DismissedBy), Name = "IX_Notifications_DismissedBy")]
    [Index(nameof(NotificationTypeId), Name = "IX_Notifications_NotificationTypeId")]
    [Index(nameof(OrganizationId), Name = "IX_Notifications_OrganizationId")]
    [Index(nameof(TransactionActionId), Name = "IX_Notifications_TransactionActionId")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_Notifications_TransactionActionRecipientId")]
    [Index(nameof(TransactionId), Name = "IX_Notifications_TransactionId")]
    [Index(nameof(UpdatedBy), Name = "IX_Notifications_UpdatedBy")]
    [Index(nameof(UserId), Name = "IX_Notifications_UserId")]
    public partial class Notification
    {
        [Key]
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
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string ContentFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("NotificationCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DismissedBy))]
        [InverseProperty("NotificationDismissedByNavigations")]
        public virtual User DismissedByNavigation { get; set; }
        [ForeignKey(nameof(NotificationTypeId))]
        [InverseProperty("Notifications")]
        public virtual NotificationType NotificationType { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("Notifications")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("Notifications")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey(nameof(TransactionActionId))]
        [InverseProperty("Notifications")]
        public virtual TransactionAction TransactionAction { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("Notifications")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("NotificationUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("NotificationUsers")]
        public virtual User User { get; set; }
    }
}
