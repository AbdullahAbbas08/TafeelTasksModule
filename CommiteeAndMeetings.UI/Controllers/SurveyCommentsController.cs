using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyCommentsController : _BaseController<SurveyComment, SurveyCommentDTO>
    {
        private readonly ISurveyCommentService _surveyCommentService;
        public SurveyCommentsController(ISurveyCommentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._surveyCommentService = businessService;
        }
    }
}
