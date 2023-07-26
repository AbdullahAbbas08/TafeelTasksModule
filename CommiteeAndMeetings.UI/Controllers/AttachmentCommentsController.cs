using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentCommentsController : _BaseController<AttachmentComment, AttachmentCommentDTO>
    {
        private readonly IAttachmentCommentService _attachmentCommentService;
        public AttachmentCommentsController(IAttachmentCommentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._attachmentCommentService = businessService;
        }
    }
}
