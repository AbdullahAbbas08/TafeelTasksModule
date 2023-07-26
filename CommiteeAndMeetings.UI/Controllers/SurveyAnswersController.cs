using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyAnswersController : _BaseController<SurveyAnswer, SurveyAnswerDTO>
    {
        private readonly ISurveyAnswerService _surveyAnswerService;
        public SurveyAnswersController(ISurveyAnswerService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._surveyAnswerService = businessService;
        }
    }
}

