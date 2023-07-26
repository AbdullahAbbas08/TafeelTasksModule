using System;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingLookUpDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public bool Repated { get; set; }
        public DateTime Date { get; set; }
        public int ReferenceNumber { get; set; }
        public DateTimeOffset MeetingFromTime { get; set; }
        public DateTimeOffset MeetingToTime { get; set; }
        public int ReminderBeforeMinutes { get; set; }
    }
}
