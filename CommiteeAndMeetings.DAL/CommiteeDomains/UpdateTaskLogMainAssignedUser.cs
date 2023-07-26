using System;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    public class UpdateTaskLogMainAssignedUser : UpdateTaskLog
    {
        public int MainAssignedUserId { get; set; }
        public string Title { get; set; }
        public string TaskDetails { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public bool? IsShared { get; set; }
        public bool? Completed { get; set; }
        public bool ByHangfire { get; set; }
        public int NewMainAssignedUserId { get; set; }
        public string NewFullNameAr { get; set; }
        public string NewFullNameEn { get; set; }


    }
}
