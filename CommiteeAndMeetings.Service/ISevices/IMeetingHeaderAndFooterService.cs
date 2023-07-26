using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Services.ISevices;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IMeetingHeaderAndFooterService : IBusinessService<MeetingHeaderAndFooter, MeetingHeaderAndFooterDTO>
    {
        List<LookUpDTO> GetListOfMeetingLookup(int take, int skip, string searchText);
    }
}

