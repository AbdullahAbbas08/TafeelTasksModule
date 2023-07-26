namespace CommiteeAndMeetings.DAL.Enums
{
    public enum MeetingState
    {
        Active = 1,
        Finished = 2,
        Closed = 3,
        Canceled = 4
    }

    public enum DisplayMeetingCallType
    {
        FromBody, 
        FromSideBar,
        Other
    }
}
