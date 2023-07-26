using CommiteeAndMeetings.DAL.Enums;
using Models;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingUserDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual UserDetailsDTO User { get; set; }
        public int MeetingId { get; set; }
        public bool Available { get; set; }
        public AttendeeState State { get; set; }
        public bool ConfirmeAttendance { get; set; }
        public UserType UserType { get; set; }
    }

    public class ListOfMeetingUserDTO
    {
        public List<MeetingUserDTO> UserDTO { get; set; } = new List<MeetingUserDTO>();
    }

    public class AttendeeDTO
    {
        public int Id { get; set; }

        public UserType Type { get; set; }
    }

    public class AttendeesList
    {
        public List<AttendeeDTO> Attendees { get; set; } = new List<AttendeeDTO>();
    }
}
