using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteePermissionCategoriesController : _BaseController<CommiteePermissionCategory, CommiteePermissionCategoryDTO>
    {
        private readonly ICommiteePermissionCategoryService _commiteePermissionCategoryService;

        public CommiteePermissionCategoriesController(ICommiteePermissionCategoryService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteePermissionCategoryService = businessService;
        }

    }
}