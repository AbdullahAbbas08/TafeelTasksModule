using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComiteeTaskCategoryController : _BaseController<ComiteeTaskCategory, ComiteeTaskCategoryDTO>
    {
        private readonly IComiteeTaskCategoryService _ICommiteeTaskMultiMissionService;

        public ComiteeTaskCategoryController(IComiteeTaskCategoryService businessService, IHelperServices.ISessionServices sessionSevices)
            : base(businessService, sessionSevices)
        {
            _ICommiteeTaskMultiMissionService = businessService;
        }
    }
}
