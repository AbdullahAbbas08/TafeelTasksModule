using CommiteeAndMeetings.DAL.Enums;
using Models;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingUserAvailabilityDTO
    {
        public int UserId { get; set; }
        public UserDetailsDTO User { get; set; }
        public bool Available { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public List<MeetingDetailsDTO> Meetings { get; set; }
        public bool? ConfirmeAttendance { get; set; }
        public DateTimeOffset? SendingDate { get; set; }
        public bool? Attended { get; set; }
    }
    public class MeetingUserAttendationDTO
    {
        public int UserId { get; set; }
        public UserType type { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public byte[] ProfileImage { get; set; }
        public virtual UserDetailsDTO UserDelegate { get; set; }



        public bool Attended { get; set; }
    }
}
