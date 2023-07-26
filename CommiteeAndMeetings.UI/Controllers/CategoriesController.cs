using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : _BaseController<Category, CategoryDTO>
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._categoryService = businessService;
        }
    }
}
