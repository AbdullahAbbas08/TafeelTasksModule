using System;

namespace Models.ProjectionModels
{
    public class NotificationLogDTO
    {
        public int NotificationLogId { get; set; }
        public DateTimeOffset SendingDate { get; set; }
        public string Subject { get; set; }
        public string Reciever { get; set; }
        public string TransactionNumber { get; set; }
    }
}
