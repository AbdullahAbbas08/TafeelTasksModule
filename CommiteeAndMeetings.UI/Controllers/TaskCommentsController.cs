using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCommentsController : _BaseController<TaskComment, TaskCommentDTO>
    {
        private readonly ITaskCommentService _taskCommentService;
        public TaskCommentsController(ITaskCommentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._taskCommentService = businessService;
        }
    }
}
