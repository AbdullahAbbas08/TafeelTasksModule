using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : _BaseController<Comment, CommentDTO>
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commentService = businessService;
        }
    }
}
