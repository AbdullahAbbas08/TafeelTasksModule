using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Services.ISevices;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IMinuteOfMeetingService : IBusinessService<MinuteOfMeeting, MinuteOfMeetingDTO>
    {
        Task <bool> MOMApproval(int meetingId, StringValues token,string MasarURL);
        void DeleteMomComents(int id);
        public MeetingSummaryDTO GetMeetingData(int meetingId, out bool status);
        public bool SendMailMoMInvitation(int MeetingId);
    }
}