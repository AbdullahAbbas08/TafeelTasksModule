using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        [Key]
        public int NotificationTypeId { get; set; }
        [StringLength(400)]
        public string NotificationTypeCode { get; set; }
        [StringLength(400)]
        public string NotificationTypeNameAr { get; set; }
        [StringLength(400)]
        public string NotificationTypeNameEn { get; set; }
        public string NotificationTypeNameFn { get; set; }

        [InverseProperty(nameof(Notification.NotificationType))]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
