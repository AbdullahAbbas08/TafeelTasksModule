using System;

namespace Models
{
    public class EventReminderSummaryDTO
    {
        public int EventReminderId { get; set; }

        private DateTimeOffset? startDateTime;
        private DateTimeOffset? endDateTime;
        public string Subject { get; set; }
        public string Url { get; set; }
        public DateTimeOffset StartDateTime
        {
            get
            {
                return startDateTime.Value.ToLocalTime();
            }
            set
            {
                startDateTime = value.ToLocalTime();
            }
        }
        public DateTimeOffset EndDateTime
        {
            get
            {
                return endDateTime.Value.ToLocalTime();
            }
            set
            {
                endDateTime = value.ToLocalTime();
            }
        }

    }
}