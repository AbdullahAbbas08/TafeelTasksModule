using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Services.ISevices;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IMeetingCommentService : IBusinessService<MeetingComment, MeetingCommentDTO>
    {
        List<MeetingCommentDTO> GetMeetingCommentsByMeetingId(int meetingId);
        MeetingCommentDTO InsertMeetingComment(MeetingCommentDTO entity);
        bool deleteMeetingComment(int id);
    }
}