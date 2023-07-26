using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Services.ISevices;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IMeetingAttendeeService : IBusinessService<MeetingAttendee, MeetingAttendeeDTO>
    {
    }
}