using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyAttachmentsController : _BaseController<SurveyAttachment, SurveyAttachmentDTO>
    {
        private readonly ISurveyAttachmentService _surveyAttachmentService;

        public SurveyAttachmentsController(ISurveyAttachmentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._surveyAttachmentService = businessService;
        }
    }
}
