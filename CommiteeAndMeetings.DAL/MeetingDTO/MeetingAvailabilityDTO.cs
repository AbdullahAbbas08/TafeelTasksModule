using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingAvailabilityDTO
    {
        public bool Available { get; set; }
        public List<MeetingDetailsDTO> Meetings { get; set; }
    }
}
