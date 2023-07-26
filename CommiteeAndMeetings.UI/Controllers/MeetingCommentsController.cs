using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingCommentsController : _BaseController<MeetingComment, MeetingCommentDTO>
    {
        private readonly IMeetingCommentService _meetingCommentService;
        public MeetingCommentsController(IMeetingCommentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._meetingCommentService = businessService;
        }

        [HttpGet("GetMeetingCommentsByMeetingId")]
        public List<MeetingCommentDTO> GetMeetingCommentsByMeetingId(int meetingId)
        {
            return _meetingCommentService.GetMeetingCommentsByMeetingId(meetingId);
        }

        [HttpPost("InsertMeetingComment")]
        public MeetingCommentDTO InsertMeetingComment(MeetingCommentDTO meetingCommentDTO)
        {
            return _meetingCommentService.InsertMeetingComment(meetingCommentDTO);
        }
        [HttpDelete("deleteMeetingComment")]
        public bool deleteMeetingComment(int id)
        {
            return _meetingCommentService.deleteMeetingComment(id);
        }
    }
}