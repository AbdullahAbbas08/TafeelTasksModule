using CommiteeAndMeetings.DAL.Enums;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingDetailsDTO
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
        public virtual List<MeetingAttendeeDTO> MeetingAttendees { get; set; }
        public MeetingState MeetingState { get; set; }
        public List<MeetingCoordinatorDTO> MeetingCoordinator { get; set; }
    }
}
