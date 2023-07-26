using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmNotificationView
    {
        public int Id { get; set; }
        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? DismissedBy { get; set; }
        public DateTimeOffset? DismissedOn { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string ContentFn { get; set; }
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public int NotificationTypeId { get; set; }
        [StringLength(50)]
        public string NotificationTypeNameAr { get; set; }
        [StringLength(50)]
        public string NotificationTypeNameEn { get; set; }
        public string NotificationTypeNameFn { get; set; }
        [StringLength(100)]
        public string FromUserNameAr { get; set; }
        [StringLength(100)]
        public string FromUserNameEn { get; set; }
        public string FromUserNameFn { get; set; }
        public byte[] FromUserProfileImage { get; set; }
    }
}
