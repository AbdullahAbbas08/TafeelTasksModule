namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class Meeting_Meeting_HeaderAndFooterDTO
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string Title { get; set; }
        //  public virtual MeetingDTO Meeting { get; set; }
        public int HeaderAndFooterId { get; set; }
        public virtual MeetingHeaderAndFooterDTO HeaderAndFooter { get; set; }
    }
}
