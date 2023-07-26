using CommiteeDatabase.Models.Domains;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    public class Meeting_Meeting_HeaderAndFooter : _BaseEntity
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public int HeaderAndFooterId { get; set; }
        public virtual MeetingHeaderAndFooter HeaderAndFooter { get; set; }
    }
}