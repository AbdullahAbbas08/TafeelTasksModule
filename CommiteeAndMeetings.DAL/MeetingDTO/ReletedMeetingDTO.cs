using Models;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class ReletedMeetingListDTO
    {
        public List<ReletedMeetingDTO> ReletedMeetings { get; set; }
        public int? CreatedBy { get; set; }
        public UserDetailsDTO CreatedByUser { get; set; }

    }
    public class ReletedMeetingDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset MeetingFromTime { get; set; }
        public DateTimeOffset MeetingToTime { get; set; }
        public int ReferenceNumber { get; set; }
        public ReletedMeetingState MeetingState { get; set; }
        public int? CreatedBy { get; set; }
        public UserDetailsDTO CreatedByUser { get; set; }
        //public bool Colsed { get; set; }
        //public bool IsFinished { get; set; }
    }
    public enum ReletedMeetingState
    {
        New = 1,
        InProgress = 2,
        Finished = 3,
        Colsed = 4
    }
}
