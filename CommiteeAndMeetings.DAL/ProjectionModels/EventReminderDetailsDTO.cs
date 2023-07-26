using System;

namespace Models
{
    public class EventReminderDetailsDTO
    {
        private DateTimeOffset? startDateTime;
        private DateTimeOffset? endDateTime;

        public int EventReminderId { get; set; }
        public string Subject { get; set; }
        public string Url { get; set; }

        public DateTimeOffset? StartDateTime
        {
            get
            {
                return startDateTime.Value.ToLocalTime();
            }
            set
            {
                startDateTime = value.Value.ToLocalTime();
            }
        }
        public DateTimeOffset? EndDateTime
        {
            get
            {
                return endDateTime.Value.ToLocalTime();
            }
            set
            {
                endDateTime = value.Value.ToLocalTime();
            }
        }
    }
}