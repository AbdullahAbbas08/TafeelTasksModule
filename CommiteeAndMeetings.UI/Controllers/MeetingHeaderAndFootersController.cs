using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingHeaderAndFootersController : _BaseController<MeetingHeaderAndFooter, MeetingHeaderAndFooterDTO>
    {
        private readonly IMeetingHeaderAndFooterService _meetingHeaderAndFooterService;
        public MeetingHeaderAndFootersController(IMeetingHeaderAndFooterService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._meetingHeaderAndFooterService = businessService;
        }
        [HttpGet("GetListOfMeetingLookup")]
        public List<LookUpDTO> GetListOfMeetingLookup(int take, int skip, string searchText)
        {
            return _meetingHeaderAndFooterService.GetListOfMeetingLookup(take, skip, searchText);
        }
    }
}
